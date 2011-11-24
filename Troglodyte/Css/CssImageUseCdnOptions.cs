using System;

namespace Troglodyte.Css
{
    public class CssImageUseCdnOptions
    {
        public Func<string, string> GetCdnImagePath { get; set; }
        public string SiteRoot { get; set; }
    }
}
