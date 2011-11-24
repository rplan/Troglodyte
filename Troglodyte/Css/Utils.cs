using System.IO;

namespace Troglodyte.Css
{
    public static class Utils
    {

        public static string GetPhysicalPathFromUrl(string url, string cssFilePath, string siteRootPath)
        {
            var fullPath = "";
            if (url[0] == '/') // absolute path
            {
                fullPath = Path.Combine(siteRootPath, url.Replace('/', '\\').Substring(1));
            }
            else
            {
                fullPath = Path.GetFullPath(Path.Combine(new FileInfo(cssFilePath).DirectoryName, url.Replace('/', '\\')));
            }
            return fullPath;
        }

        public static string GetAbsoluteUrlFromPhysicalPath(string path, string siteRootPath)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            int extraSubstring = 0;
            if (siteRootPath[siteRootPath.Length - 1] == '\\' || siteRootPath[siteRootPath.Length - 1] == '/')
                extraSubstring--;
            return path.Substring(siteRootPath.Length + extraSubstring).Replace('\\', '/');
        }
    }
}