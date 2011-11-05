using System;
using System.IO;
using System.Text;
using Troglodyte.Common;

namespace Troglodyte.Js
{
    public class JsPackager
    {
        public PackagerResults Package(Package package, JsPackagerOptions packagerOptions)
        {
            var packagerResult = new PackagerResults();
            // concatenate files
            var sb = new StringBuilder();
            foreach (var file in package.ComponentFiles)
            {
                if (!File.Exists(file))
                    throw new ArgumentException("File '" + file + "' doesn't exist! (in package " + package.Name + ")");
                // data uris
                var css = File.ReadAllText(file);
                sb.AppendLine(css);
            }

            // closure compressor
            string output;
            if (packagerOptions.JsCompressionOptions != null 
                && packagerOptions.JsCompressionOptions.CompressJs
                && packagerOptions.JsCompressionOptions is ClosureCompilerJsCompressionOptions)
            {
                var result = new ClosureCompilerJsCompressor().Compress(sb.ToString(), "compressed.js", (ClosureCompilerJsCompressionOptions)packagerOptions.JsCompressionOptions);
                packagerResult.Errors = result.Errors;
                packagerResult.Warnings = result.Warnings;
                if (result.IsSuccess)
                {
                    output = result.Output;
                }
                else
                {
                    output = sb.ToString(); // input string
                }
            }
            else
            {
                output = sb.ToString();
            }

            File.WriteAllText(package.OutputFile, output);
            return packagerResult;
        }
    }
}