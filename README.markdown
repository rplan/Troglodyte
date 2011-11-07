## What is it

Troglodyte is a .Net asset packaging library for Javascript and CSS files. It allows you to:

*    concatenate and compress Javascript files using the Google Closure Compiler
*    concatenate and compress CSS files using the YUI Compressor

It's particularities are:

*    it is currently a library, rather than a keys-in-hand solution
*    all compressors are run in-process (including the Closure Compiler) rather than forking out to java (thanks to [IKVM.Net](http://www.ikvm.net))
*    it can embed images in the compressed CSS files using [data uris](http://en.wikipedia.org/wiki/Data_URI_scheme)

## Usage

Packages are defined in Javascript, in the following format:

            [
                {
                    "Name": "MyCssPackage"
                    "ComponentFiles": [
                        "/path/to/first/file.css",
                        "/path/to/another/file.css"
                    ]
                }
            ]

For run-time packaging - in global.asax.cs:

            var packageManager = new PackageManager(HttpContext.Current.Server.MapPath("/"));
            packageManager.InitialiseCssPackages("c:\\path\\to\\css-package-definition.js");
            packageManager.InitialiseJsPackages("c:\\path\\to\\js-package-definition.js");
            packageManager.PackageAll(
                CssPackagerOptions.Default("c:\\path\\to\\output-folder"),
                JsPackagerOptions.Default("c:\\path\\to\\output-folder"));
            // the packaged file is now available at c:\\path\\to\\output-folder\\MyCssPackage.css

The &lt;script&gt; tag can then be generated as such:

            var package = packageManager.GetJsPackage("MyCssPackage");
            var htmlScriptTag = package.GetOutputHtmlString();

Compile-time packaging is not currently supported, though it should be fairly straightforward to add.

## Thanks

[IKVM.Net](http://www.ikvm.net), for allowing Java code to run in the CLR  
[YUI Compressor.Net](http://yuicompressor.codeplex.com), for converting the YUI Compressor to .Net
