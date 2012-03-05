using System;

namespace Troglodyte.Common
{
    [Serializable]
    public abstract class PackagerOptions
    {
        public bool CompressOutput { get; set; }

        /// <summary>
        /// The Variant is prefixed to the packaged output filename, and can be used e.g. to generate browser-specific versions of the output.
        /// </summary>
        public string Variant { get; set; }

        /// <summary>
        /// The Output CDN is prefixed to the packaged output, and can be used when using a passthrough CDN to deliver the files.
        /// </summary>
        public string OutputCdn { get; set; }

        /// <summary>
        /// The folder that the output will be written to
        /// </summary>
        public string OutputFolder { get; set; }

        internal string SiteRoot { get; set; }

        /// <summary>
        /// The naming function is used to generate the name of the packaged output.
        /// </summary>
        public Func<OutputNamingParameters, string> OutputNaming { get; set; }
    }
}