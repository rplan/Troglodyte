using Microsoft.Build.Framework;
using System;
using System.Linq;
using Troglodyte.Common;
using Troglodyte.Css;

namespace Troglodyte.MsBuild
{
	public class CssBuildAssetPackager : BaseBuildAssetPackager
	{
		public string OutputCdn
		{
			get;
			set;
		}
		public bool CompressionOptionsUseDataUris
		{
			get;
			set;
		}
		public bool CompressionOptionsUseCdn
		{
			get;
			set;
		}

	    private string _compressionOptionsCdnPrefix;

	    public string CompressionOptionsCdnPrefix
	    {
	        get { return _compressionOptionsCdnPrefix; }
            set
            {
                _compressionOptionsCdnPrefix = value;
                CompressionOptionsUseCdn = !string.IsNullOrEmpty(value);
            }
	    }

	    public ITaskItem[] CompressionOptionsDataUriWhitelist
		{
			get;
			set;
		}
		public override bool Execute()
		{
			CssPackagerOptions cssPackagerOptions = new CssPackagerOptions();
			cssPackagerOptions.CompressOutput = CompressOutput;
			cssPackagerOptions.OutputNaming = GetOutputNaming();
			cssPackagerOptions.OutputFolder = OutputFolder;
			cssPackagerOptions.OutputCdn = OutputCdn;
			cssPackagerOptions.Variant = Variant;
            cssPackagerOptions.IsCreatePackage = IsCreatePackage;
			CssPackagerOptions arg_BD_0 = cssPackagerOptions;
			CssCompressionOptions cssCompressionOptions = new CssCompressionOptions();
			CssCompressionOptions arg_9E_0 = cssCompressionOptions;
			Func<string, bool> arg_9E_1;
			if (!CompressionOptionsUseDataUris)
			{
				arg_9E_1 = FileMatchers.None;
			}
			else
			{
				if (CompressionOptionsDataUriWhitelist.Length != 0)
				{
					arg_9E_1 = FileMatchers.Whitelist(Enumerable.ToList<string>(Enumerable.Select<ITaskItem, string>(CompressionOptionsDataUriWhitelist, (ITaskItem ti) => ti.ItemSpec)));
				}
				else
				{
					arg_9E_1 = FileMatchers.All;
				}
			}
			arg_9E_0.UseDataUrisFor = arg_9E_1;
			cssCompressionOptions.GetCdnImagePath = CdnImagePath.AllIfEnabled(CompressionOptionsUseCdn, this.CompressionOptionsCdnPrefix);
			arg_BD_0.CompressionOptions = cssCompressionOptions;
			CssPackagerOptions options = cssPackagerOptions;
			PackageManager.PackageManager packageManager = new PackageManager.PackageManager(SiteRoot);
			packageManager.InitialiseCssPackages(PackageDefinition);
			if (!string.IsNullOrEmpty(PackageName))
			{
				Log.LogMessage("CssBuildPackager: packaging " + base.PackageName + ((Variant == null) ? "" : ("(" + Variant + ")")), new object[0]);
				Package(PackageName, packageManager, options);
			}
			else
			{
				foreach (Package current in packageManager._cssPackages)
				{
					Log.LogMessage("CssBuildPackager: packaging " + current.Name + ((Variant == null) ? "" : ("(" + Variant + ")")), new object[0]);
					Package(current.Name, packageManager, options);
				}
			}
			return true;
		}
		private void Package(string packageName, PackageManager.PackageManager packageManager, CssPackagerOptions options)
		{
			packageManager.PackageCss(packageName, options);
			PackagedCss cssPackage = packageManager.GetCssPackage(packageName, Variant);
			cssPackage.SerializeTo(OutputFolder);
		}
	}
}
