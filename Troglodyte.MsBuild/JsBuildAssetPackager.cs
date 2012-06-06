using Troglodyte.Common;
using Troglodyte.Js;

namespace Troglodyte.MsBuild
{
	public class JsBuildAssetPackager : BaseBuildAssetPackager
	{
		public string OutputCdn
		{
			get;
			set;
		}
		public override bool Execute()
		{
			JsPackagerOptions jsPackagerOptions = new JsPackagerOptions();
			jsPackagerOptions.CompressOutput = true;
			jsPackagerOptions.OutputNaming = GetOutputNaming();
			jsPackagerOptions.OutputFolder = OutputFolder;
			jsPackagerOptions.CompressionOptions = new ClosureCompilerJsCompressionOptions();
			jsPackagerOptions.OutputCdn = OutputCdn;
            jsPackagerOptions.IsCreatePackage = IsCreatePackage;
			JsPackagerOptions options = jsPackagerOptions;
			PackageManager.PackageManager packageManager = new PackageManager.PackageManager(SiteRoot);
			packageManager.InitialiseJsPackages(PackageDefinition);
			if (!string.IsNullOrEmpty(PackageName))
			{
				Log.LogMessage("JsBuildPackager: packaging " + PackageName + ((Variant == null) ? "" : ("(" + Variant + ")")), new object[0]);
				Package(PackageName, packageManager, options);
			}
			else
			{
				foreach (Package current in packageManager._jsPackages)
				{
					Log.LogMessage("JsBuildPackager: packaging " + current.Name + ((Variant == null) ? "" : ("(" + Variant + ")")), new object[0]);
					Package(current.Name, packageManager, options);
				}
			}
			return true;
		}
		private void Package(string packageName, PackageManager.PackageManager packageManager, JsPackagerOptions options)
		{
			packageManager.PackageJs(packageName, options);
			PackagedJs jsPackage = packageManager.GetJsPackage(packageName, Variant);
			jsPackage.SerializeTo(OutputFolder);
		}
	}
}
