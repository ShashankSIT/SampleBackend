using SampleBackend.Model.Model.Model;
using SampleBackend.Data.DBRepository.Login;

namespace SampleBackend.Service.Services.Login
{
    public class LoginService(ILoginRepository repository) : ILoginService
    {
        #region Fields
        private readonly ILoginRepository _repository = repository;
        #endregion

        #region Post
        public async Task<LoginModel> LoginUser(LoginModel model)
        {
            return await _repository.LoginUser(model);
        }

        public async Task<UserLoginTrackModel> SaveLoginUserTrack(LoginModel model)
        {
            return await _repository.SaveLoginUserTrack(model);
        }

        public async Task<long> ValidateUserEmail(UserAuthModel model)
        {
            return await _repository.ValidateUserEmail(model);
        }

        public async Task<string> UpdatePassword(UserAuthModel model)
        {
            return await _repository.UpdatePassword(model);
        }

        public async Task<bool> ValidateResetPassword(UserAuthModel model)
        {
            return await _repository.ValidateResetPassword(model);
        }

        public async Task<LoginModel> ValidateTwoFactorCode(long UserId, string TwoFactorCode, string VerifyUser)
        {
            return await _repository.ValidateTwoFactorCode(UserId, TwoFactorCode, VerifyUser);
        }

        public async Task<LoginModel> LoginWithoutPassword(LoginModel model)
        {
            return await _repository.LoginWithoutPassword(model);
        }
        #endregion
    }
}
