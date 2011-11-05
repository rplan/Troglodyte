namespace Troglodyte.Css
{
    public class CssPackagerOptions
    {
        public bool IsCompressCss { get; set; }
        public CssCompressionOptions CompressionOptions { get; set; }
    }


    public class CssCompressionOptions
    {
        public bool UseDataUris { get; set; }
    }
}