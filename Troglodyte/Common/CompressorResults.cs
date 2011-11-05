using System.Collections.Generic;
using System.Linq;

namespace Troglodyte.Common
{
    public class CompressorResults
    {
        public CompressorResults()
        {
            Errors = new List<PackagerResultDetail>();
            Warnings = new List<PackagerResultDetail>();
        }
        public string Output { get; set; }
        public IList<PackagerResultDetail> Errors { get; set; }
        public IList<PackagerResultDetail> Warnings { get; set; }
        public bool IsSuccess { get { return Errors != null && !Errors.Any(); } }
    }
}