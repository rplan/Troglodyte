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

        public string OutputFile { protected get; set; }
        public string SiteRoot { protected get; set; }
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

    }
}