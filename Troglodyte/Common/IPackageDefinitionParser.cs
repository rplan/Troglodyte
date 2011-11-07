using System.Collections.Generic;

namespace Troglodyte.Common
{
    public interface IPackageDefinitionParser
    {
        IEnumerable<Package> Parse(string packageDefinitionFilename);
    }
}