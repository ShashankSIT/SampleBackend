using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleBackend.Model.Model.Model;
using SampleBackend.Data.DBRepository.RoleRights;

namespace SampleBackend.Service.Services.RoleRights
{
    public class RoleRightsService : IRoleRightsService
    {
        #region Fields
        private readonly IRoleRightsRepository _repository;
        #endregion

        #region Construtor
        public RoleRightsService(IRoleRightsRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Get
        public async Task<List<RoleRightsMasterModel>> GetRoleRightsByRoleId(long roleId)
        {
            return await _repository.GetRoleRightsByRoleId(roleId);
        }
        #endregion

        #region Post
        public async Task<long> SaveRoleRightsData(RoleRightMasterModel model)
        {
            return await _repository.SaveRoleRightsData(model);
        }
        #endregion

    }
}
