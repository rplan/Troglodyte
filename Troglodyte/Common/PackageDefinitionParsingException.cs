using System;

namespace Troglodyte.Common
{
    public class PackageDefinitionParsingException : Exception
    {
        public PackageDefinitionParsingException(string filename, Exception e) : base(string.Format("Error parsing package definition '{0}'", filename), e)
        {
        }
    }
}