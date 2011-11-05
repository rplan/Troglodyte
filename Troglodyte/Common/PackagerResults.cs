using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Troglodyte.Common
{
    public class PackagerResults
    {
        public PackagerResults()
        {
            Errors = new List<PackagerResultDetail>();
            Warnings = new List<PackagerResultDetail>();
        }
        public bool IsSuccess { get { return Errors != null && !Errors.Any(); } }
        public IEnumerable<PackagerResultDetail> Errors { get; set; }
        public IEnumerable<PackagerResultDetail> Warnings { get; set; }
    }
}