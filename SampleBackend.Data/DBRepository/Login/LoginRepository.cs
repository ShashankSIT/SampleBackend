using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleBackend.Common.Helper;
using System.Data;
using SampleBackend.Model.Model.Model;
using SampleBackend.Model.Model;
using SampleBackend.Data;

namespace SampleBackend.Data.DBRepository.Login
{
    public class LoginRepository(IConfiguration config, IOptions<ConnectionStrings> connectionString) : BaseRepository(connectionString), ILoginRepository
    {
        #region Fields
        private readonly IConfiguration _config = config;
        #endregion

        #region Post
        public async Task<LoginModel> LoginUser(LoginModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Email", model.Email);
                param.Add("@Password", model.Password);
                return await QueryFirstOrDefaultAsync<LoginModel>(StoreProcedure.LoginUser, param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<LoginModel> LoginWithoutPassword(LoginModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@TemporaryPassword", model.TemporaryPassword);
                return await QueryFirstOrDefaultAsync<LoginModel>(StoreProcedure.LoginWithoutPassword, param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserLoginTrackModel> SaveLoginUserTrack(LoginModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Email", model.Email);
                return await QueryFirstOrDefaultAsync<UserLoginTrackModel>(StoreProcedure.UserLogInTrack_AddUpdate, param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<long> ValidateUserEmail(UserAuthModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Email", model.Email);
                param.Add("@TwoFactorCode", model.TwoFactorCode);
                param.Add("@VerifyUser", model.VerifyUser);
                param.Add("@TemporaryPassword", model.TemporaryPassword);
                return await QueryFirstOrDefaultAsync<long>(StoreProcedure.UserMaster_ValidateEmail, param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> UpdatePassword(UserAuthModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@Password", model.Password);
                param.Add("@CurrentPassword", model.CurrentPassword);
                return await QueryFirstOrDefaultAsync<string>(StoreProcedure.UserMaster_UpdatePassword, param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> ValidateResetPassword(UserAuthModel model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", model.UserId);
                param.Add("@VerifyUser", model.VerifyUser);
                return await QueryFirstOrDefaultAsync<bool>(StoreProcedure.UserMaster_ValidateResetPassword, param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<LoginModel> ValidateTwoFactorCode(long UserId, string TwoFactorCode, string VerifyUser)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                param.Add("@TwoFactorCode", TwoFactorCode);
                param.Add("@VerifyUser", VerifyUser);
                return await QueryFirstOrDefaultAsync<LoginModel>(StoreProcedure.UserMaster_ValidateTFACode, param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
