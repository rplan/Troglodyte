using System.IO;
using System.Reflection;
using NUnit.Framework;
using Troglodyte.Common;
using Troglodyte.Css;

namespace Troglodyte.Tests
{
    [TestFixture]
    public class CssPackagerTests
    {
        private string _currentFolder;
        private CssPackager _packager;
        [SetUp]
        public void SetUp()
        {
            _currentFolder = Path.GetDirectoryName(Assembly.GetAssembly(GetType()).Location);
            _packager = new CssPackager();
        }

        [Test]
        public void Package_should_work_as_expected()
        {
            var inputFile = Path.Combine(_currentFolder, "TestAssets\\test1.css");
            var outputFile = Path.Combine(_currentFolder, "output.css");
            if (File.Exists(outputFile))
                File.Delete(outputFile);
            var package = new Package
                              {
                                  Name = "Test",
                                  ComponentFiles = new[] { inputFile },
                              };
            var packagerOptions  = new CssPackagerOptions { 
                OutputFolder = Path.Combine(_currentFolder, "TestAssets"),
                SiteRoot = _currentFolder,
                CompressOutput = true, 
                CompressionOptions = new CssCompressionOptions { UseDataUris = true },
                OutputNaming = OutputNamings.CustomPath(outputFile)
            };
            var result = _packager.Package(package, packagerOptions);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(File.Exists(outputFile));
            //not if data URIs are used Assert.IsTrue(File.ReadAllBytes(outputFile).Length < File.ReadAllBytes(inputFile).Length);
        }

        [Test]
        public void Package_should_compress_output_file()
        {
            var inputFile = Path.Combine(_currentFolder, "TestAssets\\test1.css");
            var outputFile = Path.Combine(_currentFolder, "output.css");
            if (File.Exists(outputFile))
                File.Delete(outputFile);
            var package = new Package
                              {
                                  Name = "Test",
                                  ComponentFiles = new[] { inputFile },
                              };
            var packagerOptions  = new CssPackagerOptions { 
                OutputFolder = Path.Combine(_currentFolder, "TestAssets"),
                SiteRoot = _currentFolder,
                CompressOutput = true, 
                CompressionOptions = new CssCompressionOptions { UseDataUris = false },
                OutputNaming = OutputNamings.CustomPath(outputFile)
            };
            var result = _packager.Package(package, packagerOptions);
            Assert.IsTrue(result.IsSuccess);
            Assert.IsTrue(File.Exists(outputFile));
            Assert.IsTrue(File.ReadAllBytes(outputFile).Length < File.ReadAllBytes(inputFile).Length);
        }
    }
}