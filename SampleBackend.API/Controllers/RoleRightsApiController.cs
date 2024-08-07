using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SampleBackend.API.Logger;
using SampleBackend.Common;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;
using SampleBackend.Service.Services.RoleRights;

namespace SampleBackend.API.Controllers
{
    [Route("api/rolerights")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleRightsApiController : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private IRoleRightsService _rolerightsService;
        private IConfiguration _config;
        private readonly CommonMessages _commonMessages;
        #endregion

        #region Constructor
        public RoleRightsApiController(ILoggerManager logger, IRoleRightsService rolerightsService, IConfiguration config, IOptions<CommonMessages> commonMessages)
        {
            _logger = logger;
            _rolerightsService = rolerightsService;
            _config = config;
            _commonMessages = commonMessages.Value;
        }
        #endregion

        #region Get        
        [HttpGet("GetRoleRightsByRoleId/{roleId}")]
        public async Task<ApiResponse<RoleRightsMasterModel>> GetRoleRightsByRoleId(long roleId)
        {

            ApiResponse<RoleRightsMasterModel> response = new ApiResponse<RoleRightsMasterModel>() { Data = new List<RoleRightsMasterModel>() };
            try
            {
                List<RoleRightsMasterModel> data = await _rolerightsService.GetRoleRightsByRoleId(roleId);
                response.Data = data;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetRoleRightsByRoleId", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region Post
        [HttpPost("SaveRoleRights")]
        public async Task<BaseApiResponse> SaveRoleRights(RoleRightMasterModel model)
        {
            BaseApiResponse response = new BaseApiResponse();
            try
            {
                long result = await _rolerightsService.SaveRoleRightsData(model);
                if (result > 0)
                {
                    response.Message = _commonMessages?.RoleRights?.SaveSuccess;
                    response.Success = true;
                }
                else
                {
                    response.Message = _commonMessages?.RoleRights?.SaveError;
                    response.Success = false;
                }
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("SaveRoleRights", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion
    }
}
