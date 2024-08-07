using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Model.Model.Model
{
    public class RoleRightMasterModel : CommonModel
    {
        public long RoleId { get; set; }
        public List<RoleRightsMasterModel>? RoleRights { get; set; }
    }

    public class RoleRightsMasterModel
    {
        public long? ParentMenuId { get; set; }
        public long MenuId { get; set; }
        public bool? IsAdd { get; set; }
        public bool? IsEdit { get; set; }
        public bool? IsView { get; set; }
        public bool? IsDelete { get; set; }
        public string? MenuIcon { get; set; }
        public string? MenuUrl { get; set; }
        public string? MenuName { get; set; }
        public string? SortOrder { get; set; }
    }
}
