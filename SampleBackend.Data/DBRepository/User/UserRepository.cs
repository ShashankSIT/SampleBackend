using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleBackend.Common.Helper;
using System.Data;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;

namespace SampleBackend.Data.DBRepository.User
{
    public class UserRepository(IConfiguration config, IOptions<ConnectionStrings> connectionString) : BaseRepository(connectionString), IUserRepository
    {
        #region Fields
        private readonly IConfiguration _config = config;
        #endregion

        #region Post
        public async Task<List<UserModel>> GetUserList(CommonPaginationModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.Id);
                param.Add("@PageNumber", model.PageNumber);
                param.Add("@PageSize", model.PageSize);
                param.Add("@SortOrder", model.SortOrder);
                param.Add("@SortColumn", model.SortColumn);
                param.Add("@StrSearch", model.StrSearch);
                var data = await QueryAsync<UserModel>(StoreProcedure.UserGetList, param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<UserModel>> GetUserDropdownList(CommonPaginationModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PageNumber", model.PageNumber);
                param.Add("@PageSize", model.PageSize);
                param.Add("@StrSearch", model.StrSearch);
                var data = await QueryAsync<UserModel>(StoreProcedure.UserDropDownList, param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<UserModel> SaveUser(UserModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@FirstName", model.FirstName);
                param.Add("@LastName", model.LastName);
                param.Add("@Email", model.Email);
                param.Add("@Password", model.Password);
                param.Add("@RoleId", model.RoleId);
                param.Add("@CompanyId", model.CompanyId);
                param.Add("@BranchId", model.BranchId);
                param.Add("@PhoneNo", model.PhoneNo);
                param.Add("@CreatedBy", model.LoggedInUserId);
                return await QueryFirstOrDefaultAsync<UserModel>(StoreProcedure.SaveUser, param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get
        public async Task<List<UserModel>> GetRoleList()
        {
            try
            {
                var data = await QueryAsync<UserModel>(StoreProcedure.GetRoleList, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteUser(CommonIdModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.Id);
                string? result = await QueryFirstOrDefaultAsync<string>(StoreProcedure.DeleteUser, param, commandType: CommandType.StoredProcedure);
                if (string.IsNullOrEmpty(result))
                {
                    return true;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        #endregion
    }
}
