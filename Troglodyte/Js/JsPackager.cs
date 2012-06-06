using System;
using System.IO;
using System.Text;
using Troglodyte.Common;

namespace Troglodyte.Js
{
    public class JsPackager
    {
        public PackagerResults Package(Package package, JsPackagerOptions options)
        {
            var packagerResult = new PackagerResults();
            string outputPath = null;
            if (options.IsCreatePackage)
            {
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
                if (options.CompressionOptions != null
                    && options.CompressOutput
                    && options.CompressionOptions is ClosureCompilerJsCompressionOptions)
                {
                    var result = new ClosureCompilerJsCompressor().Compress(sb.ToString(), "compressed.js", (ClosureCompilerJsCompressionOptions)options.CompressionOptions);
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

                var outputNamingParameters = new OutputNamingParameters
                {
                    Package = package,
                    PackagedOutput = output,
                    PackagerOptions = options,
                    OutputFilenameSuffix = "js"
                };

                var outputFilename = options.OutputNaming(outputNamingParameters);
                outputPath = Path.Combine(options.OutputFolder, outputFilename);
                File.WriteAllText(outputPath, output);
            }

            packagerResult.CompiledPackage = new PackagedJs(package, options)
            {
                OutputFile = outputPath,
                IsPackaged = options.IsCreatePackage,
                SiteRoot = options.SiteRoot
            };


            //File.WriteAllText(package.OutputFile, output);
            return packagerResult;
        }
    }
}