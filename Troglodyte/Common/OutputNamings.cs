using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Troglodyte.Common
{
    public static class OutputNamings
    {
        private static readonly MD5 Md5 = MD5.Create();

        /// <summary>
        /// This function will return the package name suffixed with the MD5 hash of the output. This guarantees
        /// that the same output will generate the same filename, which means that all contents can be set with an 
        /// infinite expiry header.
        /// <br></br>
        /// <result><code>&lt;Package.Name&gt;_&lt;Md5(Output)&gt;.&lt;suffix&gt;</code>, e.g. <code>MyPackage_hoenthuk93no9i8oe9u9....9oe8u0oeu0.js</code></result>
        /// </summary>
        public static Func<OutputNamingParameters, string> PackageNamePrefixedMd5
        {
            get
            {
                return parameters => {
                   var md5Bytes = Md5.ComputeHash(new MemoryStream(Encoding.UTF8.GetBytes(parameters.PackagedOutput)));
                   var sb = new StringBuilder();
                   for (var i = 0; i < md5Bytes.Length; i++)
                       sb.Append (md5Bytes[i].ToString ("x2"));
                   var md5 = sb.ToString();
                   return parameters.Package.Name + (parameters.PackagerOptions.Variant != null ? '_' + parameters.PackagerOptions.Variant : "") + '_' + md5 + '.' + parameters.OutputFilenameSuffix;
                };
            }
        }

        /// <summary>
        /// 
        /// <result><code>&lt;Package.Name&gt;.&lt;suffix&gt;</code>, e.g. <code>MyPackage.js</code></result>
        /// </summary>
        public static Func<OutputNamingParameters, string> PackageName
        {
            get
            {
                return parameters => parameters.Package.Name + (parameters.PackagerOptions.Variant != null ? '_' + parameters.PackagerOptions.Variant : "") + '.' + parameters.OutputFilenameSuffix;
            }
        }

        /// <summary>
        /// Returns the path passed into this method as a parameter
        /// </summary>
        public static Func<OutputNamingParameters, string> CustomPath(string customPath)
        {
            return parameters => customPath;
        }
    }
}