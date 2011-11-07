using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Troglodyte.Common;
using Troglodyte.Css;
using Troglodyte.Js;

namespace Troglodyte.PackageManager
{
    public class PackageManager
    {
        private readonly string _siteRoot;
        private readonly List<Package> _cssPackages = new List<Package>();
        private readonly List<Package> _jsPackages = new List<Package>();
        private readonly List<PackagedCss> _cssCompiledPackages = new List<PackagedCss>();
        private readonly List<PackagedJs> _jsCompiledPackages = new List<PackagedJs>();
        private readonly JsPackager _jsPackager = new JsPackager();
        private readonly CssPackager _cssPackager = new CssPackager();
        private readonly IPackageDefinitionParser _packageDefinitionParser = new JsonPackageDefinitionParser();
        
        public PackageManager(string siteRoot)
        {
            _siteRoot = siteRoot;
        }

        public PackageManager(PackageManagerOptions options, string siteRoot)
        {
            _siteRoot = siteRoot;
            _packageDefinitionParser = options.PackageDefinitionParser ?? _packageDefinitionParser;
        }

        public PackageManager InitialiseCssPackages(string cssPackageDefinitionFilename)
        {
            AddPackages(_cssPackages, cssPackageDefinitionFilename);
            return this;
        }

        public PackageManager InitialiseJsPackages(string jsPackageDefinitionFilename)
        {
            AddPackages(_jsPackages, jsPackageDefinitionFilename);
            return this;
        }

        public PackageManager PackageAll(CssPackagerOptions cssPackagerOptions = null, JsPackagerOptions jsPackagerOptions = null)
        {
            PackageAllCss(cssPackagerOptions);
            PackageAllJs(jsPackagerOptions);
            return this;
        }

        public PackageManager PackageAllJs(JsPackagerOptions packagerOptions = null)
        {
            foreach (var package in _jsPackages)
                PackageJs(package.Name, packagerOptions);
            return this;
        }

        public PackageManager PackageAllCss(CssPackagerOptions packagerOptions = null)
        {
            foreach (var package in _cssPackages)
                PackageCss(package.Name, packagerOptions);
            return this;
        }

        public PackageManager PackageCss(string packageName, CssPackagerOptions packagerOptions = null)
        {
            var package = _cssPackages.FirstOrDefault(x => x.Name == packageName);
            if (package == null)
                throw new ArgumentException("CSS package " + packageName + " doesn't exist");
            if (package.ComponentFiles == null || !package.ComponentFiles.Any())
                throw new ArgumentException("CSS package " + packageName + " doesn't have any components");
            if (packagerOptions == null)
                packagerOptions = CssPackagerOptions.Default(Path.GetDirectoryName(package.ComponentFiles.First()));
            packagerOptions.SiteRoot = _siteRoot;
            var res = _cssPackager.Package(package, packagerOptions);
            res.CompiledPackage.Variant = packagerOptions.Variant;
            _cssCompiledPackages.Add(res.CompiledPackage as PackagedCss);
            return this;
        }

        public PackageManager PackageJs(string packageName, JsPackagerOptions packagerOptions = null)
        {
            var package = _jsPackages.FirstOrDefault(x => x.Name == packageName);
            if (package == null)
                throw new ArgumentException("js package " + packageName + " doesn't exist");
            if (package.ComponentFiles == null || !package.ComponentFiles.Any())
                throw new ArgumentException("js package " + packageName + " doesn't have any components");
            if (packagerOptions == null)
                packagerOptions = JsPackagerOptions.Default(Path.GetDirectoryName(package.ComponentFiles.First()));
            packagerOptions.SiteRoot = _siteRoot;
            var res = _jsPackager.Package(package, packagerOptions);
            res.CompiledPackage.Variant = packagerOptions.Variant;
            _jsCompiledPackages.Add(res.CompiledPackage as PackagedJs);
            return this;
        }

        public PackagedJs GetJsPackage(string name, string variant = null)
        {
            return _jsCompiledPackages.FirstOrDefault(x => x.Name == name && x.Variant == variant);
        }

        public PackagedCss GetCssPackage(string name, string variant = null)
        {
            return _cssCompiledPackages.FirstOrDefault(x => x.Name == name && x.Variant == variant);
        }

        private void AddPackages(List<Package> existingPackages, string packageFilename)
        {
            if (!File.Exists(packageFilename))
                throw new ArgumentException(packageFilename + "  doesn't exist!");
            var packages =_packageDefinitionParser.Parse(packageFilename);
            if (packages == null || !packages.Any())
                throw new ArgumentException("Package file '" + packageFilename + "' doesn't contain any valid packages");
            foreach (var package in packages)
            {
                var newComponentFiles = new List<string>();
                foreach (var componentFile in package.ComponentFiles)
                    if (string.IsNullOrWhiteSpace(componentFile))
                        continue;
                    else if (componentFile[0] == '\\')
                        newComponentFiles.Add(Path.Combine(_siteRoot, componentFile.Substring(1)));
                    else if (componentFile[0] == '/')
                        newComponentFiles.Add(Path.Combine(_siteRoot, componentFile.Substring(1).Replace('/', '\\')));
                    else
                        newComponentFiles.Add(componentFile);
                package.ComponentFiles = newComponentFiles;
            }
            existingPackages.AddRange(packages);
        }
    }
}
