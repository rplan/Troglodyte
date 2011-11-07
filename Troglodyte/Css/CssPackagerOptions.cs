using Troglodyte.Common;

namespace Troglodyte.Css
{
    public class CssPackagerOptions : PackagerOptions
    {
        /// <summary>
        /// The default JS Packager options:
        /// <ul>
        /// <li>IsCompressCss = true</li>
        /// <li>CompressionOptions.UseDataUris = true</li>
        /// </ul>
        /// </summary>
        public static CssPackagerOptions Default(string outputFolder)
        {
            return new CssPackagerOptions
                       {
                           OutputNaming = OutputNamings.PackageNamePrefixedMd5,
                           CompressionOptions = new CssCompressionOptions
                                                    {
                                                        UseDataUris = true,
                                                    },
                            OutputFolder = outputFolder
                       };
        }

        public CssCompressionOptions CompressionOptions { get; set; }
    }
}