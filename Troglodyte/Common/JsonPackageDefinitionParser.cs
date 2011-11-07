using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace Troglodyte.Common
{
    public class JsonPackageDefinitionParser : IPackageDefinitionParser
    {
        public IEnumerable<Package> Parse(string packageDefinitionFilename)
        {
            try
            {
                return new JavaScriptSerializer().Deserialize<IEnumerable<Package>>(File.ReadAllText(packageDefinitionFilename));
            }
            catch (Exception e)
            {
                throw new PackageDefinitionParsingException(packageDefinitionFilename, e);
            }
        }
    }
}
