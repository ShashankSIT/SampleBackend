using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SampleBackend.API.Logger;
using SampleBackend.Common;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;
using SampleBackend.Service.Services.User;
using static SampleBackend.Common.EncryptionDecryption;

namespace SampleBackend.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserApiController(
        ILoggerManager logger,
        IOptions<CommonMessages> commonMessages,
        IUserService userService) : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger = logger;
        private readonly IUserService _userService = userService;
        private readonly CommonMessages _commonMessages = commonMessages.Value;
        #endregion

        #region Get
        [HttpGet("GetRolesList")]
        public async Task<ApiResponse<UserModel>> GetRolesList()
        {
            ApiResponse<UserModel> response = new();
            try
            {
                var Result = await _userService.GetRoleList();
                response.Success = true;
                response.Data = Result;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetRolesList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion


        #region Post

        [HttpPost("GetUserList")]
        public async Task<ApiResponse<UserModel>> GetUserList(CommonPaginationModel model)
        {
            ApiResponse<UserModel> response = new() { Data = [] };
            try
            {
                List<UserModel> users = await _userService.GetUserList(model);
                if (model.Id > 0)
                {
                    foreach (var item in users)
                    {
                        item.Password = GetDecrypt(item.Password ?? string.Empty);
                    }
                }

                response.Data = users;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetUserList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("GetUserDropdownList")]
        public async Task<ApiResponse<UserModel>> GetUserDropdownList(CommonPaginationModel model)
        {
            ApiResponse<UserModel> response = new() { Data = [] };
            try
            {
                List<UserModel> users = await _userService.GetUserDropdownList(model);

                response.Data = users;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetUserDropdownList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("GetBranchDropdownList")]
        public async Task<ApiResponse<BranchModel>> GetBranchDropdownList(CommonPaginationModel model)
        {
            ApiResponse<BranchModel> response = new() { Data = [] };
            try
            {
                List<BranchModel> branches = await _userService.GetBranchDropdownList(model);

                response.Data = branches;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetBranchDropdownList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }


        [HttpPost("GetCompanyDropdownList")]
        public async Task<ApiResponse<CompanyModel>> GetCompanyDropdownList(CommonPaginationModel model)
        {
            ApiResponse<CompanyModel> response = new() { Data = [] };
            try
            {
                List<CompanyModel> companies = await _userService.GetCompanyDropdownList(model);

                response.Data = companies;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetCompanyDropdownList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }



        [HttpPost("SaveUser")]
        public async Task<ApiResponse<long>> SaveUser(UserModel model)
        {
            ApiResponse<long> response = new();

            try
            {
                model.Password = GetEncrypt(model.Password ?? string.Empty);

                UserModel result = await _userService.SaveUser(model);

                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    response.Success = false;
                    response.Message = result.ErrorMessage;
                }
                else if (result.UserId > 0)
                {
                    response.Success = true;
                    response.Message = _commonMessages?.User?.SaveSuccess;
                }
                else
                {
                    response.Success = false;
                    response.Message = _commonMessages?.User?.SaveError;
                }
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("SaveUser", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        #endregion

        #region Delete

        [HttpDelete("DeleteUser")]
        public async Task<BaseApiResponse> DeleteUser(CommonIdModel model)
        {
            BaseApiResponse response = new();
            try
            {
                bool result = await _userService.DeleteUser(model);
                if (result)
                {
                    response.Message = _commonMessages?.User?.DeleteSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = _commonMessages?.User?.DeleteError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("DeleteUser", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;

            #endregion

        }
    }
}
