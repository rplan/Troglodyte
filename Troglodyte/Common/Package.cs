using System.Collections.Generic;

namespace Troglodyte.Common
{
    public class Package
    {
        public string Name { get; set; }
        public string OutputFile { get; set; }
        public string SiteRoot { get; set; }
        public IEnumerable<string> ComponentFiles { get; set; }
    }
}