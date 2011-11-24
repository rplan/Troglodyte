using System.IO;
using System.Reflection;
using NUnit.Framework;
using Troglodyte.Css;

namespace Troglodyte.Tests
{
    [TestFixture]
    public class UtilsTests
    {
        private string currentFolder;

        [SetUp]
        public void SetUp()
        {
            currentFolder = Path.GetDirectoryName(Assembly.GetAssembly(this.GetType()).Location);
        }

        [TestCase("../a/day.jpg", "TestAssets\\stylesheets\\test.css", "TestAssets\\a\\day.jpg")]
        [TestCase("../../a/day.jpg", "TestAssets\\stylesheets\\subfolder\\test.css", "TestAssets\\a\\day.jpg")]
        [TestCase("../a/1/day.jpg", "TestAssets\\stylesheets\\test.css", "TestAssets\\a\\1\\day.jpg")]
        [TestCase("/TestAssets/a/1/day.jpg", "test.css", "TestAssets\\a\\1\\day.jpg")]
        public void GetFileLocation_Should_Return_Correct_Path(string css, string cssPath, string expectedPath)
        {
            cssPath = Path.Combine(currentFolder, cssPath);
            expectedPath = Path.Combine(currentFolder, expectedPath);
            Assert.AreEqual(expectedPath.ToLower(), Utils.GetPhysicalPathFromUrl(css, cssPath, currentFolder).ToLower());
        }

        [TestCase("c:\\test\\a\\path\\image.png", "c:\\test", "/a/path/image.png")]
        [TestCase("c:\\test\\a\\path\\image.png", "c:\\test\\", "/a/path/image.png")]
        public void GetAbsoluteUrlFromPhysicalPath_Should_work_as_expected(string path, string siteRoot, string expected)
        {
            Assert.AreEqual(expected, Utils.GetAbsoluteUrlFromPhysicalPath(path, siteRoot));
        }

    }
}