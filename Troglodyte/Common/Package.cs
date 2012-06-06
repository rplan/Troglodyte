using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Troglodyte.Css;
using Troglodyte.Js;

namespace Troglodyte.Common
{
    [Serializable]
    public class Package
    {
        public string Name { get; set; }
        public IEnumerable<string> ComponentFiles { get; set; }
    }

    [Serializable]
    public class CompiledPackage : Package
    {
        private string _outputHtmlString;
        private string _componentHtmlString;
        protected readonly PackagerOptions Options;
        private readonly string _htmlLink;
        private string _outputUrl;
        private IEnumerable<string> _componentUrls;

        public CompiledPackage(Package package, PackagerOptions options, string htmlLink)
        {
            Options = options;
            _htmlLink = htmlLink;
            Name = package.Name;
            ComponentFiles = package.ComponentFiles;
        }

        public string OutputFile { protected get; set; }
        public string SiteRoot { protected get; set; }
        public string Variant { get; set; }
        public bool IsPackaged { get; set; }

        public string GetOutputUrl()
        {
            if (_outputUrl == null)
                _outputUrl = OutputFile.Substring(SiteRoot.Length).Replace('\\', '/');
            if (_outputUrl.Length > 0 && _outputUrl[0] == '/')
                _outputUrl = _outputUrl.Substring(1);
            return _outputUrl;
        }

        public IEnumerable<string> GetComponentUrls()
        {
            if (_componentUrls == null)
            {
                _componentUrls = ComponentFiles.Select(f =>
                {
                    var u = (f[0] == '\\' ? Path.Combine(SiteRoot, f) : f).Substring(SiteRoot.Length).Replace('\\', '/');
                    if (u.Length > 0 && u[0] == '/')
                        u = u.Substring(1);
                    return u;
                });
            }
            return _componentUrls;
        }

        public string GetOutputHtmlString()
        {
            if (_outputHtmlString == null)
            {
                var outputUrl = GetOutputUrl();
                _outputHtmlString = string.Format(_htmlLink, Options.OutputCdn, outputUrl);
            }
            return _outputHtmlString;
        }

        public string GetComponentHtmlString()
        {
            if (_componentHtmlString == null)
            {
                var sb = new StringBuilder();
                foreach(var cu in GetComponentUrls())
                {
                    sb.AppendLine(string.Format(_htmlLink, Options.OutputCdn, cu));
                }
                _componentHtmlString = sb.ToString();
            }
            return _componentHtmlString;
        }

        public static T DeserializeFrom<T>(string pathSerialisedPackage)
        {
            using (var stream = new FileStream(pathSerialisedPackage, FileMode.Open))
                //return (T) new DataContractSerializer(typeof (T)).ReadObject(stream);
                return (T) new BinaryFormatter().Deserialize(stream);
        }
    }

    [Serializable]
    public class PackagedCss : CompiledPackage
    {
        private const string CssLink = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}/{1}\" />";
        public PackagedCss(Package package, CssPackagerOptions options) : base(package, options, CssLink) {}
        public void SerializeTo(string outputFolder)
        {
            using(var stream = new FileStream(Path.Combine(outputFolder, Name + (Variant == null ? "" : "_" + Variant) + ".css.bin"), FileMode.Create))
                new BinaryFormatter().Serialize(stream, this);
                //new DataContractSerializer(GetType(), new List<Type> { typeof(CssCompressionOptions), typeof(CssPackagerOptions)}).WriteObject(stream, this);
        }

        public void SetRuntimeOptions(CssPackagerOptions options)
        {
            if (options != null)
            {
                Options.OutputCdn = options.OutputCdn;
            }
        }

    }

    [Serializable]
    public class PackagedJs : CompiledPackage
    {
        private const string JsLink = "<script type=\"text/javascript\" src=\"{0}/{1}\"></script>";
        public PackagedJs(Package package, JsPackagerOptions options) : base(package, options, JsLink) {}
        public void SerializeTo(string outputFolder)
        {
            using(var stream = new FileStream(Path.Combine(outputFolder, Name + (Variant == null ? "" : "_" + Variant) + ".js.bin"), FileMode.Create))
                new BinaryFormatter().Serialize(stream, this);
                //new DataContractSerializer(GetType()).WriteObject(stream, this);
        }

        public void SetRuntimeOptions(JsPackagerOptions options)
        {
            if (options != null)
            {
                Options.OutputCdn = options.OutputCdn;
            }
        }

    }
}