using SampleBackend.Model.Model.Model;

namespace SampleBackend.Data.DBRepository.Login
{
    public interface ILoginRepository
    {
        #region Post
        Task<LoginModel> LoginUser(LoginModel model);
        Task<LoginModel> LoginWithoutPassword(LoginModel model);
        Task<UserLoginTrackModel> SaveLoginUserTrack(LoginModel model);
        Task<long> ValidateUserEmail(UserAuthModel model);
        Task<string> UpdatePassword(UserAuthModel model);
        Task<bool> ValidateResetPassword(UserAuthModel model);
        Task<LoginModel> ValidateTwoFactorCode(long UserId, string TwoFactorCode, string VerifyUser);
        #endregion
    }
}
