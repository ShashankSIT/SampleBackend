using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SampleBackend.API.Logger;
using SampleBackend.Common;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;
using SampleBackend.Service.Services.PostgraseSQL;
using SampleBackend.Service.Services.User;

namespace SampleBackend.API.Controllers
{
    [Route("api/postgreSQL")]
    [ApiController]
    public class PostgraseSQLController(
        ILoggerManager logger,
        IOptions<CommonMessages> commonMessages,
        IPostgraseSQLService service) : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger = logger;
        private readonly IPostgraseSQLService _service = service;
        private readonly CommonMessages _commonMessages = commonMessages.Value;
        #endregion

        #region Post
        [HttpPost("LoginUser")]
        public async Task<ApiPostResponse<LoginModel>> LoginUser(LoginModel model)
        {
            ApiPostResponse<LoginModel> response = new();
            try
            {
                LoginModel users = await _service.LoginUser(model);

                response.Data = users;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("LoginUser", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpPost("GetRoleName")]
        public async Task<ApiPostResponse<string>> GetRoleName(int roleId)
        {
            ApiPostResponse<string> response = new();
            try
            {
                string roleName = await _service.GetRoleName(roleId);
                response.Data = roleName;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetRoleName", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("UpsertUser")]
        public async Task<ApiPostResponse<bool>> UpsertUser(UserModel model)
        {
            ApiPostResponse<bool> response = new();
            try
            {
                bool data = await _service.UpsertUser(model);

                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("UpsertUser", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

    }
}
