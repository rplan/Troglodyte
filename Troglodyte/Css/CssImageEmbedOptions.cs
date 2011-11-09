using System;

namespace Troglodyte.Css
{
    public class CssImageEmbedOptions
    {
        public string SiteRoot { get; set; }
        public Func<string, bool> UseDataUrisFor { get; set; }
    }
}
