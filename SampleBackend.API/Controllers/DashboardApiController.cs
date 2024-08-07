using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SampleBackend.API.Logger;
using SampleBackend.Common;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;
using SampleBackend.Service.Services.Dashboard;

namespace SampleBackend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardApiController(
        ILoggerManager logger,
        IConfiguration config,
        IOptions<CommonMessages> commonMessages,
        IOptions<DataConfig> dataConfig,
        IOptions<ApplicationSettings> appSettings,
        IHttpContextAccessor httpContextAccessor,
        IDashboardService dashboardService) : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger = logger;
        private readonly IConfiguration _config = config;
        private readonly CommonMessages _commonMessages = commonMessages.Value;
        private readonly DataConfig _dataConfig = dataConfig.Value;
        private readonly ApplicationSettings _appSettings = appSettings.Value;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IDashboardService _dashboardService = dashboardService;
        #endregion

        #region Get
        [HttpPost("GetDashboardMasterDetail")]
        public async Task<ApiPostResponse<DashboardModel>> GetDashboardMasterDetail(UserModel model)
        {
            ApiPostResponse<DashboardModel> response = new();
            try
            {
                DashboardModel Result = await _dashboardService.GetDashboardMasterDetail(model);
                response.Success = true;
                response.Data = Result;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetDashboardMasterDetail", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("GetGRSMaterialPurchasesDetail")]
        public async Task<ApiResponse<GRSMaterialPurchaseDetailModel>> GetGRSMaterialPurchasesDetail(UserModel model)
        {
            ApiResponse<GRSMaterialPurchaseDetailModel> response = new() { Data = [] };
            try
            {
                List<GRSMaterialPurchaseDetailModel> Result = await _dashboardService.GetGRSMaterialPurchasesDetail(model);
                response.Success = true;
                response.Data = Result;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetGRSMaterialPurchasesDetail", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("GetCarbonReductionChartMasterDetail")]
        public async Task<ApiResponse<CarbonReductionModel>> GetCarbonReductionChartMasterDetail(UserModel model)
        {
            ApiResponse<CarbonReductionModel> response = new() { Data = [] };
            try
            {
                List<CarbonReductionModel> Result = await _dashboardService.GetCarbonReductionChartMasterDetail(model);
                response.Success = true;
                response.Data = Result;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetCarbonReductionChartMasterDetail", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion
        #region Post

        [HttpPost("SaveDashboardMaster")]
        public async Task<BaseApiResponse> SaveDashboardMaster(DashboardModel dashboardModel)
        {
            BaseApiResponse response = new();
            try
            {
                List<GRSMaterialPurchaseDetailModel>? GrsMaterialsList = dashboardModel?.grsMaterials?.ToList();
                if (GrsMaterialsList != null && GrsMaterialsList.Count > 0)
                {
                    var DuplicateDetails = GrsMaterialsList.GroupBy(d => d.MaterialNumber).Where(d => d.Count() > 1);
                    if (DuplicateDetails.Any())
                    {
                        response.Success = false;
                        response.Message = "There are dupliate material numbers in GRS details. Please try again!";
                        return response;
                    }
                }

                long Result = await _dashboardService.SaveDashboardMaster(dashboardModel);

                DashboardModel GrpResult = await _dashboardService.SaveGRSMaterialsMaster(dashboardModel);

                DashboardModel carbonReductionResult = await _dashboardService.SaveCarbonReductionMaster(dashboardModel);

                if (Result > 0 && GrpResult.Result == "" && carbonReductionResult.Result == "")
                {
                    response.Success = true;
                    response.Message = _commonMessages.Dashboard?.SaveSuccess;
                }
                else
                {
                    response.Success = false;
                    response.Message = _commonMessages.Dashboard?.SaveError;
                }
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("SaveDashboardMaster", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion
    }
}
