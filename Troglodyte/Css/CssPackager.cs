using System;
using System.IO;
using System.Text;
using Troglodyte.Common;

namespace Troglodyte.Css
{
    /// <summary>
    /// Uses the CSS Compressor from http://yuicompressor.codeplex.com/
    /// </summary>
    public class CssPackager
    {
        // TODO: prefix CDN url to CSS images
        private CssImageEmbed _imageEmbedder;
        public PackagerResults Package(Package package, CssPackagerOptions options)
        {
            _imageEmbedder = new CssImageEmbed(new CssImageEmbedOptions{ SiteRoot = package.SiteRoot});
            var packagerResult = new PackagerResults();

            // concatenate files
            var sb = new StringBuilder();
            foreach (var file in package.ComponentFiles)
            {
                if (!File.Exists(file))
                    throw new ArgumentException("File '" + file + "' doesn't exist! (in package " + package.Name + ")");
                // data uris
                var css = File.ReadAllText(file);
                if (options.CompressionOptions != null && options.CompressionOptions.UseDataUris)
                {
                    var embedderResult = _imageEmbedder.Compress(css, file);
                    css = embedderResult.Output;
                    packagerResult.Errors = embedderResult.Errors;
                    packagerResult.Warnings = embedderResult.Warnings;
                }
                sb.AppendLine(css);
            }

            // YUI compressor
            string output;
            if (options.IsCompressCss)
                output = Yahoo.Yui.Compressor.CssCompressor.Compress(sb.ToString());
            else
                output = sb.ToString();

            File.WriteAllText(package.OutputFile, output);
            return packagerResult;
        }
    }
}