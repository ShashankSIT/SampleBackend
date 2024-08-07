using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleBackend.Model.Model.Model;

namespace SampleBackend.Service.Services.RoleRights
{
    public interface IRoleRightsService
    {
        #region Get
        Task<List<RoleRightsMasterModel>> GetRoleRightsByRoleId(long roleId);
        #endregion

        #region Post
        Task<long> SaveRoleRightsData(RoleRightMasterModel model);
        #endregion
    }
}
