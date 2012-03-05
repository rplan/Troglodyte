using System;
using System.Collections.Generic;
using System.Linq;

namespace Troglodyte.Common
{
    [Serializable]
    public static class FileMatchers
    {
        /// <summary>
        /// matches any file path
        /// </summary>
        public static Func<string, bool> All
        {
            get
            {
                return s => true;
            }
        }

        /// <summary>
        /// matches no file paths
        /// </summary>
        public static Func<string, bool> None
        {
            get
            {
                return s => false;
            }
        }

        /// <summary>
        /// matches any file path
        /// </summary>
        public static Func<string, bool> Whitelist(IList<string> whitelist)
        {
            return path => whitelist.Any(x => path.ToLower().EndsWith(x.ToLower()));
        }
    }
}