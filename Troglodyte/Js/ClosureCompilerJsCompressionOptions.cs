﻿namespace Troglodyte.Js
{
    public class ClosureCompilerJsCompressionOptions : JsCompressionOptions
    {
        public ClosureCompilerJsCompressionOptions() : base()
        {
            FailOnCompilerWarnings = false;
        }

        public ClosureCompressionLevel CompressionLevel { get; set; }
        public bool FailOnCompilerErrors { get; set; }
        public bool FailOnCompilerWarnings { get; set; }
    }
}