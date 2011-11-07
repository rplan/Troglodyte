namespace Troglodyte.Common
{
    public class OutputNamingParameters
    {
        public Package Package { get; set; }
        public string PackagedOutput { get; set; }
        public PackagerOptions PackagerOptions { get; set; }
        public string OutputFilenameSuffix { get; set; }
    }
}