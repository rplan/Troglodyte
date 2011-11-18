using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Troglodyte.Css;

namespace Troglodyte.Tests
{
    [TestFixture]
    public class CssImageUseCdnTests
    {
        private CssImageUseCdn _compressor;
        private string currentFolder;

        [SetUp]
        public void SetUp()
        {
            currentFolder = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location);
            _compressor = new CssImageUseCdn(new CssImageUseCdnOptions { GetCdnImagePath = (path) =>
                                                                                               {
                                                                                                   if (path.StartsWith("//"))
                                                                                                       return null;
                                                                                                   if (path.StartsWith("http"))
                                                                                                       return null;
                                                                                                   return "//mycdn"  + (path.StartsWith("/") ? "" : "/") + path;
                                                                                               }});
        }

        [Test]
        public void Compress_ShouldWorkAsExpected()
        {
            const string expectedOutput = @"/*
* a multiline comment
*/
.test 
{
    font-size: 13pt;
    background-image: url('//mycdn/day.jpg');
    color: #fff;
}

a 
{
   /* background-image: url('//mycdn/a/day.jpg'); */
   background-image: url('//mycdn/a/1/day.jpg'); 
}";
            var cssFile = Path.Combine(currentFolder, "TestAssets\\test1.css");
            var output = _compressor.Compress(File.ReadAllText(cssFile), cssFile);
            Console.WriteLine("output:");
            Console.WriteLine(output.Output);
            Assert.AreEqual(expectedOutput, output.Output);
        }
    }
}
