﻿using System;

namespace Troglodyte.Css
{
    public class CssCompressionOptions
    {
        public Func<string, bool> UseDataUrisFor { get; set; }
        public Func<string, string> GetCdnImagePath { get; set; }
    }
}