using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SampleBackend.API.Logger;
using SampleBackend.Common;
using System.Text.RegularExpressions;
using System.Web;
using SampleBackend.Model.Model;
using static SampleBackend.Common.EmailNotification;
using static SampleBackend.Common.EncryptionDecryption;
using SampleBackend.Service.Services.Login;
using SampleBackend.Service.Services.User;
using SampleBackend.Model.Model.Model;

namespace SampleBackend.API.Controllers
{
    [Route("api/login")]
    [ApiController]

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LoginApiController(
        ILoggerManager logger,
        IConfiguration config,
        IOptions<CommonMessages> commonMessages,
        IOptions<DataConfig> dataConfig,
        IOptions<ApplicationSettings> appSettings,
        IHttpContextAccessor httpContextAccessor,
        ILoginService loginService,
        IUserService userService) : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger = logger;
        private readonly IConfiguration _config = config;
        private readonly CommonMessages _commonMessages = commonMessages.Value;
        private readonly DataConfig _dataConfig = dataConfig.Value;
        private readonly ApplicationSettings _appSettings = appSettings.Value;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILoginService _loginService = loginService;
        private readonly IUserService _userService = userService;
        #endregion

        #region Post
        [HttpPost("LoginUser")]
        [AllowAnonymous]
        public async Task<ApiPostResponse<LoginModel>> LoginUser([FromBody] LoginModel model)
        {
            ApiPostResponse<LoginModel> response = new();
            try
            {   
                model.Password = GetEncrypt(model.Password ?? string.Empty);
                LoginModel result = await _loginService.LoginUser(model);
                if (result != null)
                {
                    if (string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        if (!string.Equals(result.RoleName, RoleName.SuperAdmin) && result.IsBlocked == true)
                        {
                            response.Message = result.ErrorMessage;
                            response.Success = false;
                        }
                        else
                        {
                            response.Success = true;
                            string? host = _httpContextAccessor?.HttpContext?.Request?.Host.Value;
                            string? scheme = _httpContextAccessor?.HttpContext?.Request?.Scheme;
                            string hosturl = scheme + "://" + host;
                            result.UserPhoto = Path.Combine(hosturl, _dataConfig.UserProfile ?? string.Empty, result.UserPhoto ?? string.Empty);
                            string UserName = result.FirstName + " " + result.LastName;
                            result.JWTToken = JWTToken.GenerateJSONWebToken(result.Email ?? string.Empty, GetEncrypt(result.UserId.ToString()), result.RoleId.ToString() ?? string.Empty, _appSettings.JWT_Secret ?? string.Empty);
                            response.Data = result;
                            response.Message = _commonMessages?.Login?.SaveSuccess;
                            response.Data.EncryptedUserId = response.Success && (result.IsFirstLogin == true || result.Is2FARequired == true) ? HttpUtility.UrlEncode(GetEncrypt(Convert.ToString(result.UserId))) : string.Empty;
                            if (response.Success && result.Is2FARequired == true)
                            {
                                // TFA Code
                                UserAuthModel userAuthModel = new()
                                {
                                    UserId = result.UserId,
                                    Is2FARequired = result.Is2FARequired
                                };
                                AuthResponse resetCodeResult = await ResetCode(userAuthModel);
                                result.VerifyUser = resetCodeResult.TAID;
                                response.Message = resetCodeResult.Message;
                            }
                            result.UserId = 0;
                        }
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = result.ErrorMessage;
                        if (!string.IsNullOrEmpty(result?.ErrorMessage) && result.UserId > 0) // For wrong credentials
                        {
                            UserLoginTrackModel loginResult = await _loginService.SaveLoginUserTrack(model);
                            if (loginResult.IsSuperAdmin == false)
                            {
                                int remainingAttempts = 5 - loginResult.WrongAttemptCount ?? 0;
                                response.Message = remainingAttempts <= 0 ? "Your account is locked due to too many wrong attempts, please contact your administrator!" : "Please enter valid credentials.  You have " + remainingAttempts + " attempts left!";
                            }
                        }
                    }
                }
                else
                {
                    response.Message = result?.ErrorMessage;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<BaseApiResponse> ChangePassword([FromBody] UserAuthModel model)
        {
            BaseApiResponse response = new();
            try
            {
                model.UserId = model.LoggedInUserId;
                model.CurrentPassword = GetEncrypt(model.CurrentPassword ?? string.Empty);
                model.Password = GetEncrypt(model.Password ?? string.Empty);
                string result = await _loginService.UpdatePassword(model);
                if (string.IsNullOrEmpty(result))
                {
                    response.Message = _commonMessages.UpdatePassword?.SaveSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = result;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<BaseApiResponse> ForgotPassword([FromBody] UserAuthModel model)
        {
            BaseApiResponse response = new();
            try
            {
                string VerifyUser = GeneratePassword(35, true);
                UserAuthModel AuthModel = new()
                {
                    Email = model.Email,
                    VerifyUser = VerifyUser
                };
                long UserId = await _loginService.ValidateUserEmail(AuthModel);
                if (UserId > 0)
                {
                    CommonPaginationModel PaginationModel = new()
                    {
                        Id = UserId
                    };
                    List<UserModel> UserData = await _userService.GetUserList(PaginationModel);
                    if (UserData != null && UserData.Count > 0)
                    {
                        UserModel UserDetail = UserData[0];
                        string UserName = UserDetail.FirstName + " " + UserDetail.LastName;
                        string EncryptedUserId = HttpUtility.UrlEncode(GetEncrypt(Convert.ToString(UserId)));

                        string Subject = "Forgot Password";
                        string EmailHTML = string.Empty;
                        EmailSetting setting = GetEmailSettingObj(Convert.ToBoolean(_appSettings.EmailEnableSsl), _appSettings.EmailHostName ?? string.Empty, _appSettings.EmailAppPassword ?? string.Empty, _appSettings.EmailAppPassword ?? string.Empty, Convert.ToInt32(_appSettings.EmailPort), _appSettings.EmailUsername ?? string.Empty, _appSettings.FromEmail ?? string.Empty, _appSettings.FromName ?? string.Empty);

                        string BasePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates");

                        if (!Directory.Exists(BasePath))
                        {
                            Directory.CreateDirectory(BasePath);
                        }
                        using StreamReader reader = new(Path.Combine(BasePath, "ForgotPassword.html"));
                        string EmailBody = reader.ReadToEnd();
                        string Client_URL = _dataConfig.WebAppURL ?? string.Empty;
                        EmailBody = EmailBody.Replace("##UserName##", UserName);
                        EmailBody = EmailBody.Replace("##LogoURL##", string.Concat(Client_URL, _dataConfig.LogoPath ?? string.Empty, "/veltuff-logo.svg"));
                        EmailBody = EmailBody.Replace("##ResetPasswordLink##", Path.Combine(Client_URL, "auth/reset-password?userId=" + EncryptedUserId + "&auth=" + VerifyUser));
                        bool IsSuccess = SendMailMessage(model.Email ?? string.Empty, string.Empty, string.Empty, Subject, EmailBody, setting, string.Empty);

                        //Email Log History
                        Task EmailLog = new(async () =>
                        {
                            EmailNotificationLogDetailModel notification = new()
                            {
                                NotificationType = EmailNotificationType.ForgotPassword,
                                FromEmail = setting.FromEmail,
                                ToEmail = model.Email,
                                EmailSubject = Subject,
                                EmailBody = EmailBody,
                                IsSuccess = IsSuccess,
                                RecipientType = UserDetail.RoleName == RoleName.SuperAdmin ? RoleName.SuperAdmin : UserDetail.RoleName == RoleName.Admin ? RoleName.Admin : RoleName.MoveManager
                            };
                            //long Notification = await _notificationService.SaveEmailNotificationLogDetail(notification);
                        });

                        if (IsSuccess)
                        {
                            response.Message = _commonMessages.ForgotPassword?.SaveSuccess;
                            response.Success = true;
                        }
                        else
                        {
                            response.Message = _commonMessages.Error;
                            response.Success = false;
                        }
                    }
                }
                else
                {
                    response.Message = _commonMessages.ForgotPassword?.SaveError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("ResetPassword")]
        public async Task<BaseApiResponse> ResetPassword([FromBody] UserAuthModel model)
        {
            BaseApiResponse response = new();
            try
            {
                if (!string.IsNullOrEmpty(model.EncryptedUserId))
                {
                    model.EncryptedUserId = HttpUtility.UrlDecode(model.EncryptedUserId);
                    model.UserId = Convert.ToInt64(GetDecrypt(model.EncryptedUserId));
                }
                else
                {
                    model.UserId = model.LoggedInUserId;
                }
                bool IsAllowReset = await _loginService.ValidateResetPassword(model);
                if (IsAllowReset)
                {
                    model.Password = GetEncrypt(model.Password ?? string.Empty);
                    string result = await _loginService.UpdatePassword(model);
                    if (string.IsNullOrEmpty(result))
                    {
                        response.Message = _commonMessages.UpdatePassword?.SaveSuccess;
                        response.Success = true;
                    }
                    else
                    {
                        response.Message = result;
                        response.Success = false;
                    }
                }
                else
                {
                    response.Message = _commonMessages.UpdatePassword?.DeleteError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("ValidateResetPassword")]
        public async Task<AuthResponse> ValidateResetPassword([FromBody] UserAuthModel model)
        {
            AuthResponse response = new();
            try
            {
                if (!string.IsNullOrEmpty(model.EncryptedUserId))
                {
                    model.EncryptedUserId = HttpUtility.UrlDecode(model.EncryptedUserId);
                    model.UserId = Convert.ToInt64(GetDecrypt(model.EncryptedUserId));
                    bool result = await _loginService.ValidateResetPassword(model);
                    if (result)
                    {
                        response.Success = true;
                        response.Message = "Valid user.";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = _commonMessages.Error;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = _commonMessages.Error;
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("ValidateTwoFactorCodee")]
        [AllowAnonymous]
        public async Task<ApiPostResponse<LoginModel>> ValidateTwoFactorCodee([FromBody] UserAuthModel model)
        {
            ApiPostResponse<LoginModel> response = new();
            try
            {
                if (!string.IsNullOrEmpty(model.EncryptedUserId))
                {
                    model.EncryptedUserId = HttpUtility.UrlDecode(model.EncryptedUserId);
                    model.UserId = Convert.ToInt64(GetDecrypt(model.EncryptedUserId));
                }
                if (model.UserId > 0)
                {
                    LoginModel result = await _loginService.ValidateTwoFactorCode(model.UserId, model.TwoFactorCode ?? string.Empty, model.VerifyUser ?? string.Empty);
                    if (result != null && string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        if (!string.IsNullOrEmpty(model.VerifyUser))
                        {
                            response.Success = true;
                        }
                        else
                        {
                            string? host = _httpContextAccessor?.HttpContext?.Request.Host.Value;
                            string? scheme = _httpContextAccessor?.HttpContext?.Request.Scheme;
                            string hosturl = scheme + "://" + host;
                            result.UserPhoto = Path.Combine(hosturl, _dataConfig.UserProfile ?? string.Empty, result.UserPhoto ?? string.Empty);
                            result.JWTToken = JWTToken.GenerateJSONWebToken(result.Email ?? string.Empty, GetEncrypt(result.UserId.ToString()), result.RoleId.ToString() ?? string.Empty, _appSettings.JWT_Secret ?? string.Empty);
                            result.UserId = 0;
                            response.Data = result;
                            response.Success = true;
                            response.Message = _commonMessages.Login?.SaveSuccess;
                        }
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = result?.ErrorMessage;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = _commonMessages.Error;
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("ResetCode")]
        public async Task<AuthResponse> ResetCode([FromBody] UserAuthModel model)
        {
            AuthResponse response = new();
            try
            {
                if (!string.IsNullOrEmpty(model.auth))
                {
                    model.auth = HttpUtility.UrlDecode(model.auth);
                    model.UserId = Convert.ToInt64(GetDecrypt(model.auth));
                }
                CommonPaginationModel UserModel = new()
                {
                    Id = model.UserId
                };
                List<UserModel> UserData = await _userService.GetUserList(UserModel);
                if (UserData != null && UserData.Count > 0)
                {
                    UserModel UserDetail = UserData[0];
                    Random generator = new();
                    model.TwoFactorCode = generator.Next(0, 1000000).ToString("D6");
                    if (model.Is2FARequired == true)
                    {
                        model.VerifyUser = GeneratePassword(35);
                    }
                    model.Email = UserDetail.Email;
                    long UserId = await _loginService.ValidateUserEmail(model);
                    if (UserId > 0)
                    {
                        EmailSetting setting = GetEmailSettingObj(Convert.ToBoolean(_appSettings.EmailEnableSsl), _appSettings.EmailHostName ?? string.Empty, _appSettings.EmailAppPassword ?? string.Empty, _appSettings.EmailAppPassword ?? string.Empty, Convert.ToInt32(_appSettings.EmailPort), _appSettings.EmailUsername ?? string.Empty, _appSettings.FromEmail ?? string.Empty, _appSettings.FromName ?? string.Empty);
                        string Subject = "Two Factor Authentication Code";

                        string BasePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates");
                        if (!Directory.Exists(BasePath))
                        {
                            Directory.CreateDirectory(BasePath);
                        }
                        using StreamReader reader = new(Path.Combine(BasePath, "TwoFactorAuthentication.html"));
                        string EmailBody = reader.ReadToEnd();
                        string WebAppURL = _dataConfig.WebAppURL ?? string.Empty;
                        EmailBody = EmailBody.Replace("##LogoURL##", Path.Combine(WebAppURL, _dataConfig.LogoPath ?? string.Empty, "veltuff-logo.svg"));
                        EmailBody = EmailBody.Replace("##UserName##", string.Concat(UserDetail.FirstName, " ", UserDetail.LastName));
                        EmailBody = EmailBody.Replace("##TwoFactorCode##", model.TwoFactorCode);
                        bool IsSuccess = SendMailMessage(UserDetail.Email ?? string.Empty, string.Empty, string.Empty, Subject, EmailBody, setting, string.Empty);

                        //Email Log History
                        Task EmailLog = new(async () =>
                        {
                            EmailNotificationLogDetailModel notification = new()
                            {
                                NotificationType = EmailNotificationType.ForgotPassword,
                                FromEmail = setting.FromEmail,
                                ToEmail = model.Email,
                                EmailSubject = Subject,
                                EmailBody = EmailBody,
                                IsSuccess = IsSuccess,
                                RecipientType = UserDetail.RoleName == RoleName.SuperAdmin ? RoleName.SuperAdmin : UserDetail.RoleName == RoleName.Admin ? RoleName.Admin : RoleName.MoveManager
                            };
                            notification.EmailBody = Regex.Replace(notification.EmailBody, Regex.Escape(model.TwoFactorCode), new string('*', model.TwoFactorCode.Length));
                            //long Notification = await _notificationService.SaveEmailNotificationLogDetail(notification);
                        });
                        response.Message = IsSuccess ? _commonMessages.TwoFactorAuth?.SaveSuccess : _commonMessages.TwoFactorAuth?.SaveError;
                        response.Success = true;
                        if (model.Is2FARequired == true)
                        {
                            response.TAID = model.VerifyUser;
                        }
                    }
                    else
                    {
                        response.Message = _commonMessages.Error;
                        response.Success = false;
                    }
                }
                else
                {
                    response.Message = _commonMessages.Error;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

    }
}
