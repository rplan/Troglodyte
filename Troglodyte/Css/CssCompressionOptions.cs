using System;

namespace Troglodyte.Css
{
    public class CssCompressionOptions
    {
        public Func<string, bool> UseDataUrisFor { get; set; }
    }
}