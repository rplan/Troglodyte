using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Troglodyte.Common;
using Troglodyte.Js;

namespace Troglodyte.Css
{
    // based on the code in Jammit http://documentcloud.github.com/jammit/
    public class CssImageEmbed
    {
        private readonly CssImageEmbedOptions _options;

        private static Dictionary<string, string> EMBED_MIME_TYPES = new Dictionary<string, string> (){
                                                                                                          {".png","image/png"},
                                                                                                          {".jpg", "image/jpeg"},
                                                                                                          {".jpeg", "image/jpeg"},
                                                                                                          {".gif", "image/gif"},
                                                                                                          {".tif", "image/tiff"},
                                                                                                          {".tiff", "image/tiff"},
                                                                                                          {".ttf", "font/truetype"},
                                                                                                          {".otf", "font/opentype"},
                                                                                                          {".woff", "font/wof"}
                                                                                                      };

        // CSS asset-embedding regexes for URL rewriting.
        private static Regex EMBED_DETECTOR  = new Regex("url\\(['\"]?([^\\s)]+\\.[a-z]+)(\\?\\d+)?['\"]?\\)");
        private static Regex EMBEDDABLE      = new Regex(" [^\\/]embed\\/");
        private static Regex EMBED_REPLACER  = new Regex(" url\\(__EMBED__(.+?)(\\?\\d+)?\\)");

        // MHTML file constants.
        private const string MHTML_START     = "/*\r\nContent-Type: multipart/related; boundary=\"MHTML_MARK\"\r\n\r\n";
        private const string MHTML_SEPARATOR = "--MHTML_MARK\r\n";
        private const string MHTML_END       = "\r\n--MHTML_MARK--\r\n*/\r\n";

        //(32k - padding) maximum length for data-uri assets (an IE8 limitation).
        private const int MAX_IMAGE_SIZE = 32700;

        public CssImageEmbed(CssImageEmbedOptions options)
        {
            _options = options;
        }

        public CompressorResults Compress(string css, string cssPath)
        {
            var result = new CompressorResults();
            var modifiedCss = "";
            var lastMatch = 0;
            foreach (Match match in EMBED_DETECTOR.Matches(css))
            {
                var path = match.Groups[1];
                if (path.Value.StartsWith("http"))
                    continue; //ignore any non-local images
                var fullPath = Utils.GetPhysicalPathFromUrl(path.Value, cssPath, _options.SiteRoot);
                if (!File.Exists(fullPath))
                {
                    result.Warnings.Add(new PackagerResultDetail
                    {
                        Filename = cssPath,
                        Description = fullPath + " doesn't exist! (resolved from " + path.Value + " in " + cssPath + ")"
                    });
                }
                else if (_options.UseDataUrisFor(fullPath))
                {
                    var replacementString = GetEncodedAssetUrl(fullPath, path.Value);
                    modifiedCss += css.Substring(lastMatch, match.Index - lastMatch - 1) + replacementString;
                    lastMatch = match.Index + match.Length;
                }
            }
            modifiedCss += css.Substring(lastMatch, css.Length - lastMatch);
            result.Output = modifiedCss;
            return result;
        }

        private string GetEncodedAssetUrl(string fullPath, string originalPath)
        {
            var extension = Path.GetExtension(fullPath).ToLower();
            if (!EMBED_MIME_TYPES.ContainsKey(extension))
                return string.Format("url({0})", originalPath);
            var mimeType = EMBED_MIME_TYPES[extension];
            var bytes = File.ReadAllBytes(fullPath);
            if (bytes.Length > MAX_IMAGE_SIZE)
            {
                return string.Format("url({0})", originalPath);
            }
            var base64 = Convert.ToBase64String(bytes);
            return string.Format("url(\"data:{0};charset=utf-8;base64,{1}\")", mimeType, base64);
        }
    }
}