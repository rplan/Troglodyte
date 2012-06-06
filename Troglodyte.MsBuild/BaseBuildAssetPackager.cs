using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using Troglodyte.Common;
namespace Troglodyte.MsBuild
{
	public abstract class BaseBuildAssetPackager : Task
	{
        public BaseBuildAssetPackager()
        {
            IsCreatePackage = true;
        }

        [Required]
		public bool IsCreatePackage
		{
			get;
			set;
		}
		[Required]
		public string PackageDefinition
		{
			get;
			set;
		}
		public string PackageName
		{
			get;
			set;
		}
		[Required]
		public string OutputFolder
		{
			get;
			set;
		}
		[Required]
		public bool CompressOutput
		{
			get;
			set;
		}
		[Required]
		public string SiteRoot
		{
			get;
			set;
		}
		public string OutputNaming
		{
			get;
			set;
		}
		public string Variant
		{
			get;
			set;
		}
		protected Func<OutputNamingParameters, string> GetOutputNaming()
		{
			string outputNaming;
			if ((outputNaming = this.OutputNaming) != null)
			{
				if (outputNaming == "Md5")
				{
					return OutputNamings.PackageNamePrefixedMd5;
				}
				if (outputNaming == "Package")
				{
					return OutputNamings.PackageName;
				}
			}
			return OutputNamings.PackageNamePrefixedMd5;
		}
	}
}
