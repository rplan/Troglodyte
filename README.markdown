## What is it

Troglodyte is a .Net asset packaging library for Javascript and CSS files. It allows you to:

*    concatenate and compress Javascript files using the Google Closure Compiler
*    concatenate and compress CSS files using the YUI Compressor

It's particularities are:

*    it is currently a library, rather than a keys-in-hand solution
*    all compressors are run in-process (including the Closure Compiler) rather than forking out to java (thanks to [IKVM.Net](http://www.ikvm.net))
*    it can embed images in the compressed CSS files using [data uris](http://en.wikipedia.org/wiki/Data_URI_scheme)

## Usage

For run-time packaging - in global.asax.cs:

            var serverRoot = base.Context.Server.MapPath("/");
            var myCssPackage = new Package
            {
                Name = "MyCss",
                OutputFile =  serverRoot + "\\dynamic\\output.css",
                SiteRoot = serverRoot,
                ComponentFiles = new [] { serverRoot + "\\styles\\first.css", serverRoot + "\\styles\\second.css" }
            };
            var cssPackager = new CssPackager();
            cssPackager.Package(homePageCss, new CssPackagerOptions
                {
                    IsCompressCss = true,
                    CompressionOptions = new CssCompressionOptions { UseDataUris = true }
                });

            // the packaged file is available at /dynamic/output.css


The same code can be used for compile-time or deploy-time packaging.


## Thanks

[IKVM.Net](http://www.ikvm.net), for allowing Java code to run in the CLR
[YUI Compressor.Net](http://yuicompressor.codeplex.com), for converting the YUI Compressor to .Net
