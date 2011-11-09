using System;

namespace Troglodyte.Common
{
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
    }
}