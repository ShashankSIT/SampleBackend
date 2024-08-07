using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleBackend.Common.Helper;
using SampleBackend.Model.Model.Model;
using SampleBackend.Model.Model;

namespace SampleBackend.Data.DBRepository.RoleRights
{
    public class RoleRightsRepository : BaseRepository, IRoleRightsRepository
    {
        #region Fields
        private IConfiguration _config;
        #endregion

        #region Constructor
        public RoleRightsRepository(IConfiguration config, IOptions<ConnectionStrings> dataConfig) : base(dataConfig)
        {
            _config = config;
        }
        #endregion

        #region Get
        public async Task<List<RoleRightsMasterModel>> GetRoleRightsByRoleId(long roleId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@roleId", roleId);
                var data = await QueryAsync<RoleRightsMasterModel>(StoreProcedure.RoleRightsGetById, param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Post
        public async Task<long> SaveRoleRightsData(RoleRightMasterModel model)
        {
            try
            {
                DataTable dtRoleRights = new DataTable("tbl_RoleRights");
                dtRoleRights.Columns.Add("menuId", typeof(long));
                dtRoleRights.Columns.Add("isAdd", typeof(bool));
                dtRoleRights.Columns.Add("isEdit", typeof(bool));
                dtRoleRights.Columns.Add("isDelete", typeof(bool));
                dtRoleRights.Columns.Add("isView", typeof(bool));

                if (model.RoleRights.Count > 0)
                {
                    foreach (var item in model.RoleRights)
                    {
                        DataRow dtRow = dtRoleRights.NewRow();
                        dtRow["menuId"] = item.MenuId;
                        dtRow["isAdd"] = item.IsAdd;
                        dtRow["isEdit"] = item.IsEdit;
                        dtRow["isDelete"] = item.IsDelete;
                        dtRow["isView"] = item.IsView;
                        dtRoleRights.Rows.Add(dtRow);
                    }
                }

                var param = new DynamicParameters();
                param.Add("@userId", model.LoggedInUserId);
                param.Add("@roleId", model.RoleId);
                param.Add("@roleRights", dtRoleRights.AsTableValuedParameter("[dbo].[tbl_RoleRights]"));
                return await QueryFirstOrDefaultAsync<long>(StoreProcedure.RoleRightsAddUpdate, param, commandType: CommandType.StoredProcedure);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
