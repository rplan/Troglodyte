using System;
using Troglodyte.Common;

namespace Troglodyte.Css
{
    [Serializable]
    public class CssPackagerOptions : PackagerOptions
    {
        /// <summary>
        /// The default JS Packager options:
        /// <ul>
        /// <li>IsCompressCss = true</li>
        /// <li>CompressionOptions.UseDataUrisFor = <see cref="FileMatchers.None" /></li>
        /// </ul>
        /// </summary>
        public static CssPackagerOptions Default(string outputFolder)
        {
            return new CssPackagerOptions
                       {
                           OutputNaming = OutputNamings.PackageNamePrefixedMd5,
                           CompressionOptions = new CssCompressionOptions
                                                    {
                                                        UseDataUrisFor = FileMatchers.None,
                                                    },
                            OutputFolder = outputFolder
                       };
        }

        public CssCompressionOptions CompressionOptions { get; set; }
    }
}