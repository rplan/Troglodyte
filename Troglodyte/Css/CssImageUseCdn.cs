using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Troglodyte.Common;
using Troglodyte.Js;

namespace Troglodyte.Css
{
        public class CssImageUseCdn
    {
        private readonly CssImageUseCdnOptions _options;

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
        
        public CssImageUseCdn(CssImageUseCdnOptions options)
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
                var replacementString = _options.GetCdnImagePath(path.Value);
                if (!string.IsNullOrEmpty(replacementString))
                {
                    replacementString = " url('" + replacementString + "')";
                    modifiedCss += css.Substring(lastMatch, match.Index - lastMatch - 1) + replacementString;
                    lastMatch = match.Index + match.Length;
                }
            }
            modifiedCss += css.Substring(lastMatch, css.Length - lastMatch);
            result.Output = modifiedCss;
            return result;
        }
    }
}