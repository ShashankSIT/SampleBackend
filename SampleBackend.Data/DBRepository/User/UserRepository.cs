using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleBackend.Common.Helper;
using System.Data;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;
using SampleBackend.Common;
using System.Data.SqlClient;

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

                // Create a table-valued parameter for ColumnFilters
                var filterTable = new DataTable();
                filterTable.Columns.Add("ColumnName", typeof(string));
                filterTable.Columns.Add("FilterValue", typeof(string));

                if (model.ColumnFilters != null && model.ColumnFilters.Any())
                {
                    foreach (var filter in model.ColumnFilters)
                    {
                        filterTable.Rows.Add(filter.ColumnName, filter.FilterValue);
                    }
                }

                param.Add("@ColumnFilters", filterTable.AsTableValuedParameter("dbo.ColumnFilterType"));

                var data = await QueryAsync<UserModel>(StoreProcedure.UserGetList, param, commandType: CommandType.StoredProcedure);
                return data.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<UserDetailsMasterModel>> GetUserDetailsList(CommonPaginationModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.LoggedInUserId);
                param.Add("@PageNumber", model.PageNumber);
                param.Add("@PageSize", model.PageSize);
                param.Add("@SortOrder", model.SortOrder);
                param.Add("@SortColumn", model.SortColumn);
                param.Add("@StrSearch", model.StrSearch);

                // Create a table-valued parameter for ColumnFilters
                var filterTable = new DataTable();
                filterTable.Columns.Add("ColumnName", typeof(string));
                filterTable.Columns.Add("FilterValue", typeof(string));

                if (model.ColumnFilters != null && model.ColumnFilters.Any())
                {
                    foreach (var filter in model.ColumnFilters)
                    {
                        filterTable.Rows.Add(filter.ColumnName, filter.FilterValue);
                    }
                }

                param.Add("@ColumnFilters", filterTable.AsTableValuedParameter("dbo.ColumnFilterType"));

                var data = await QueryAsync<UserDetailsMasterModel>(StoreProcedure.UserDetailsGetList, param, commandType: CommandType.StoredProcedure);
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
        
        public async Task<UserModel> SaveUserDetails(UserModel model)
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

        public async Task<bool> DeleteMultipleRecords(CommonDeleteModel model)
        {
            ApiResponse<bool> response = new ApiResponse<bool>();

            try
            {
                // Convert the list of IDs to a comma-separated string
                var idString = string.Join(",", model.Ids);

                var param = new DynamicParameters();
                param.Add("@Ids", idString);
                param.Add("@AffectedRows", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await QueryFirstOrDefaultAsync<bool>(StoreProcedure.DeleteUserMultiple, param, commandType: CommandType.StoredProcedure);


                int affectedRows = param.Get<int>("@AffectedRows");

                if (affectedRows > 0)
                {
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No records were deleted.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response.Success;
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
        public async Task<bool> DeleteAllUser(CommonModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UpdatedBy", model.LoggedInUserId);
                param.Add("@AffectedRows", dbType: DbType.Int32, direction: ParameterDirection.Output);

                await QueryFirstOrDefaultAsync<string>(StoreProcedure.DeleteAllUser, param, commandType: CommandType.StoredProcedure);

                int affectedRows = param.Get<int>("@AffectedRows");

                if (affectedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
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
