using System.Linq;
using NUnit.Framework;
using Troglodyte.Common;

namespace Troglodyte.Tests
{
    [TestFixture]
    public class JsonPackageFileParserTests
    {
        JsonPackageDefinitionParser _parser;

        [SetUp]
        public void SetUp()
        {
            _parser = new JsonPackageDefinitionParser();
        }

        [Test]
        [Ignore]
        public void TestSuccessfulFile()
        {
            var json = @"[{Name: 'hello', ComponentFiles: ['c:\\test.js', '\\test2.js']}, { Name: 'package2', ComponentFiles: ['\\test3.js'] }]";
            var result = _parser.Parse(json);
            Assert.AreEqual(2, result.Count());
            var first = result.ElementAt(0);
            var second = result.ElementAt(1);
            Assert.AreEqual("hello", first.Name);
            Assert.AreEqual("package2", second.Name);
            Assert.AreEqual(2, first.ComponentFiles.Count());
            Assert.AreEqual(1, second.ComponentFiles.Count());
            
            var firstComponentFiles = new [] { @"c:\test.js", @"\test2.js" };
            for(var i = 0; i < first.ComponentFiles.Count(); i++) 
            {
                Assert.AreEqual(firstComponentFiles[i], first.ComponentFiles.ElementAt(i));
            }

            Assert.AreEqual(@"\test3.js", second.ComponentFiles.First());
        }
    }
}
