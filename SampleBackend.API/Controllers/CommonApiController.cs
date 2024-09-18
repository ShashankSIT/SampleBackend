using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleBackend.Common;
using SampleBackend.Model.Model.Model;
using SampleBackend.Model.Model;
using Microsoft.Extensions.Options;
using SampleBackend.API.Logger;
using SampleBackend.Service.Services.Profile;
using SampleBackend.Service.Services.User;
using SampleBackend.Service.Services.Common;

namespace SampleBackend.API.Controllers
{
    [Route("api/Common")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommonApiController(
        ILoggerManager logger,
        IOptions<CommonMessages> commonMessages,
        ICommonService commonService) : ControllerBase
    {
        private readonly ILoggerManager _logger = logger;
        
        private readonly CommonMessages _commonMessages = commonMessages.Value;
        
        private readonly ICommonService _commonService = commonService;


        [HttpGet("GetCountryList")]
        public async Task<ApiPostResponse<IEnumerable<CountryModel>>> GetCountryList()
        {
            ApiPostResponse<IEnumerable<CountryModel>> response = new();
            try
            {
                var result = await _commonService.CountryList();
                response.Success = true;
                response.Data = result;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetUserProfileById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet("GetStateList")]
        public async Task<ApiPostResponse<IEnumerable<StateModel>>> GetStateList(int CountryId)
        {
            ApiPostResponse<IEnumerable<StateModel>> response = new();
            try
            {
                var result = await _commonService.StateList(CountryId);
                response.Success = true;
                response.Data = result;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetUserProfileById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        [HttpGet("GetCityList")]
        public async Task<ApiPostResponse<IEnumerable<CityModel>>> GetCityList(int StateId)
        {
            ApiPostResponse<IEnumerable<CityModel>> response = new();
            try
            {
                var result = await _commonService.CityList(StateId);
                response.Success = true;
                response.Data = result;
                response.Message = "Success";
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetUserProfileById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
