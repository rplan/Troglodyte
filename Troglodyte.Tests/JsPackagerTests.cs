using System.IO;
using System.Reflection;
using NUnit.Framework;
using Troglodyte.Common;
using Troglodyte.Js;

namespace Troglodyte.Tests
{
    [TestFixture]
    public class JsPackagerTests
    {
        private string _currentFolder;
        private JsPackager _packager;
        [SetUp]
        public void SetUp()
        {
            _currentFolder = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location);
            _packager = new JsPackager();
        }

        [Test]
        public void Package_should_compress_output_file()
        {
            var inputFile = Path.Combine(_currentFolder, "TestAssets\\jquery-1.6.1.js");
            var outputFile = Path.Combine(_currentFolder, "output.js");
            if (File.Exists(outputFile))
                File.Delete(outputFile);
            var package = new Package
                              {
                                  Name = "Test",
                                  ComponentFiles = new[] { inputFile },
                              };
            var packagerOptions = new JsPackagerOptions { 
                OutputFolder = Path.Combine(_currentFolder, "TestAssets"),
                SiteRoot = _currentFolder,
                CompressOutput = true, 
                CompressionOptions = new ClosureCompilerJsCompressionOptions { CompressionLevel = ClosureCompressionLevel.SimpleOptimizations},
                OutputNaming = OutputNamings.CustomPath(outputFile)
            };
            var result = _packager.Package(package, packagerOptions);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(File.Exists(outputFile));
            Assert.IsTrue(File.ReadAllBytes(outputFile).Length < File.ReadAllBytes(inputFile).Length, "The output file is not smaller than the input file - did compression occur?");
        }
    }
}