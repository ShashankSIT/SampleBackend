using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Model.Model.Model
{
    public class BranchModel : CommonModel
    {
        public long BranchId { get; set; }
        public string? BranchName { get; set; }
        public long CompanyId { get; set; }
        public string? CompanyBranchName { get; set; }
        public long? TotalFilteredCount { get; set; }

    }
}
