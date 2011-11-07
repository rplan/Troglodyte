using Troglodyte.Common;

namespace Troglodyte.Js
{
    public class JsPackagerOptions : PackagerOptions
    {
        /// <summary>
        /// The default JS Packager options:
        /// <ul>
        /// <li>CompressOutput = true</li>
        /// <li>OutputNaming = OutputNamings.PackageNamePrefixedMd5</li>
        /// </ul>
        /// </summary>
        public static JsPackagerOptions Default(string outputFolder)
        {
            return new JsPackagerOptions
            {
                CompressOutput = true,
                OutputNaming = OutputNamings.PackageNamePrefixedMd5,
                CompressionOptions = new JsCompressionOptions(),
                OutputFolder = outputFolder
            };
        }

        public JsCompressionOptions CompressionOptions { get; set; }
    }
}