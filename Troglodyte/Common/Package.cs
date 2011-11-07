using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Troglodyte.Css;
using Troglodyte.Js;

namespace Troglodyte.Common
{
    public class Package
    {
        public string Name { get; set; }
        public IEnumerable<string> ComponentFiles { get; set; }
    }

    public class CompiledPackage : Package
    {
        private string _outputHtmlString;
        private string _componentHtmlString;
        private readonly PackagerOptions _options;
        private readonly string _htmlLink;
        private string _outputUrl;
        private IEnumerable<string> _componentUrls;

        public CompiledPackage(Package package, PackagerOptions options, string htmlLink)
        {
            _options = options;
            _htmlLink = htmlLink;
            Name = package.Name;
            ComponentFiles = package.ComponentFiles;
        }

        public string OutputFile { private get; set; }
        public string SiteRoot { private get; set; }
        public string Variant { get; set; }

        public string GetOutputUrl()
        {
            if (_outputUrl == null)
                _outputUrl = OutputFile.Substring(SiteRoot.Length).Replace('\\', '/');
            return _outputUrl;
        }

        public IEnumerable<string> GetComponentUrls()
        {
            if (_componentUrls == null)
            {
                _componentUrls = ComponentFiles.Select(f => (f[0] == '\\' ? Path.Combine(SiteRoot, f) : f).Substring(SiteRoot.Length).Replace('\\', '/'));
            }
            return _componentUrls;
        }

        public string GetOutputHtmlString()
        {
            if (_outputHtmlString == null)
                _outputHtmlString = string.Format(_htmlLink, _options.OutputCdn, GetOutputUrl());
            return _outputHtmlString;
        }

        public string GetComponentHtmlString()
        {
            if (_componentHtmlString == null)
            {
                var sb = new StringBuilder();
                foreach(var cu in GetComponentUrls())
                {
                    sb.AppendLine(string.Format(_htmlLink, _options.OutputCdn, cu));
                }
                _componentHtmlString = sb.ToString();
            }
            return _componentHtmlString;
        }
    }

    public class PackagedCss : CompiledPackage
    {
        private const string _cssLink = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}/{1}\" />";
        public PackagedCss(Package package, CssPackagerOptions options) : base(package, options, _cssLink) {}
    }

    public class PackagedJs : CompiledPackage
    {
        private const string _jsLink = "<script type=\"text/javascript\" src=\"{0}/{1}\"></script>";
        public PackagedJs(Package package, JsPackagerOptions options) : base(package, options, _jsLink) {}
    }
}