using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleBackend.Common;
using SampleBackend.Data.DBRepository.Login;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Data.DBRepository.PostgraseSQL
{
    public class PostgraseSQLRepository(IConfiguration config, IOptions<ConnectionStrings> connectionString) : BaseRepository(connectionString), IPostgraseSQLRepository
    {
        #region Fields
        private readonly IConfiguration _config = config;
        #endregion

        #region Post
        public async Task<LoginModel> LoginUser(LoginModel model)
        {
            var param = new DynamicParameters();
            param.Add("@p_email", model.Email);
            param.Add("@p_password", EncryptionDecryption.GetEncrypt(model.Password ?? string.Empty));
            var data = await PostgreQueryFirstOrDefaultAsync<LoginModel>("SELECT * FROM sp_temp_login(@p_email, @p_password)", param, commandType: CommandType.Text);
            return data;
        }
        
        public async Task<string> GetRoleName(int roleId)
        {
            var param = new DynamicParameters();
            param.Add("@p_roleid", roleId); 
            param.Add("@p_rolename", dbType: DbType.String, direction: ParameterDirection.Output);
            await PostgreQueryFirstOrDefaultAsync2<LoginModel>("sp_get_role_name_by_id", param, commandType: CommandType.StoredProcedure);
            var roleName = param.Get<string>("@p_rolename");
            return roleName;
        }

        public async Task<bool> UpsertUser(UserModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("p_userid", model.UserId); // Pass 0 for insert, or UserId for update
                param.Add("p_firstname", model.FirstName);
                param.Add("p_lastname", model.LastName);
                param.Add("p_email", model.Email);
                param.Add("p_password", EncryptionDecryption.GetEncrypt(model.Password ?? string.Empty));
                param.Add("p_roleid", model.RoleId);
                param.Add("p_phoneno", model.PhoneNo);
                param.Add("p_userphoto", model.UserPhoto);
                param.Add("p_isfirstlogin", true);

                // Call the stored procedure using PostgreExecuteAsync
                int result = await PostgreExecuteAsync<int>("sp_upsert_user", param);

                // If the execution completes without an exception, return true
                return result > 0;
            }
            catch (Exception)
            {
                // Log or handle the error as needed
                return false; // Indicate failure
            }
        }

        #endregion


    }
}
