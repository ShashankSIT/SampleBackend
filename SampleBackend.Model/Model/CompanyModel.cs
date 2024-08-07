using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Model.Model.Model
{
    public class CompanyModel : CommonModel
    {
        public long CompanyId { get; set; }
        public string? CompanyName { get; set; }
    }

}
