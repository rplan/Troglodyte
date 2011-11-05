using System.Linq;
using com.google.javascript.jscomp;
using Troglodyte.Common;

namespace Troglodyte.Js
{
    public class ClosureCompilerJsCompressor
    {
        public CompressorResults Compress(string js, string filename, ClosureCompilerJsCompressionOptions packagerOptions)
        {
            var compressorResult = new CompressorResults();
            var options = new CompilerOptions();
            CompilationLevel compilationLevel;
            switch (packagerOptions.CompressionLevel)
            {
                case ClosureCompressionLevel.SimpleOptimizations:
                    compilationLevel = CompilationLevel.SIMPLE_OPTIMIZATIONS;
                    break;
                case ClosureCompressionLevel.AdvancedOptimizations:
                    compilationLevel = CompilationLevel.ADVANCED_OPTIMIZATIONS;
                    break;
                default:
                    compilationLevel = CompilationLevel.WHITESPACE_ONLY;
                    break;
            }
            compilationLevel.setOptionsForCompilationLevel(options);

            var compiler = new Compiler();
            var dummy = JSSourceFile.fromCode("externs.js", "");
            var source = JSSourceFile.fromCode(filename, js);

            var result = compiler.compile(dummy, source, options);

            if (!result.success && packagerOptions.FailOnCompilerErrors)
            {
                if (packagerOptions.FailOnCompilerErrors)
                {
                    throw new ClosureCompilerErrorsException(result.warnings);
                }
                compressorResult.Errors = (from error in result.errors
                                           select new PackagerResultDetail { Line = error.lineNumber, Char = error.getCharno(), Description = error.description, Filename = error.sourceName }).ToList();
            }
            if (result.warnings.Length > 0)
            {
                if (packagerOptions.FailOnCompilerWarnings)
                {
                    throw new ClosureCompilerWarningsException(result.warnings);
                }
                compressorResult.Warnings = (from warning in result.warnings
                                            select new PackagerResultDetail { Line = warning.lineNumber, Char = warning.getCharno(), Description = warning.description, Filename = warning.sourceName }).ToList();
            }

            compressorResult.Output = compiler.toSource();
            return compressorResult;
        }
    }
}