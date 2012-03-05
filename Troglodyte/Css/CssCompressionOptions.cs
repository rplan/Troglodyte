using System;

namespace Troglodyte.Css
{
    [Serializable]
    public class CssCompressionOptions
    {
        [NonSerialized]
        private Func<string, bool> _useDataUrisFor;
        public Func<string, bool> UseDataUrisFor
        {
            get { return _useDataUrisFor; }
            set { _useDataUrisFor = value; }
        }

        [NonSerialized]
        private Func<string, string> _getCdnImagePath;

        public Func<string, string> GetCdnImagePath
        {
            get { return _getCdnImagePath; }
            set { _getCdnImagePath = value; }
        }
    }

    [Serializable]
    public static class CdnImagePath
    {
        public static Func<string, string> All(string cdnPrefix)
        {
            return path =>
                       {

                           if (path.StartsWith("//"))
                               return null;
                           if (path.StartsWith("http"))
                               return null;
                           return cdnPrefix + (path.StartsWith("/") ? "" : "/") + path;
                       };
        }

        public static Func<string, string> AllIfEnabled(bool isEnabled, string cdnPrefix)
        {
            return path =>
                       {
                           if (!isEnabled)
                               return null;
                           if (path.StartsWith("//"))
                               return null;
                           if (path.StartsWith("http"))
                               return null;
                           return cdnPrefix + (path.StartsWith("/") ? "" : "/") + path;
                       };
        }
    }
}