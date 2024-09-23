using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleBackend.Common;
using SampleBackend.Model.Model.Model;
using SampleBackend.Model.Model;
using Microsoft.Extensions.Options;
using SampleBackend.API.Logger;
using SampleBackend.Service.Services.UserProfile;
using Newtonsoft.Json;
using System.Diagnostics;

namespace SampleBackend.API.Controllers
{
    [Route("api/userprofile")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserProfileApiController(
        ILoggerManager logger,
        IOptions<CommonMessages> commonMessages,
        IUserProfileService userProfileService,
        IWebHostEnvironment environment
        ) : ControllerBase
    {

        #region Fields
        private readonly ILoggerManager _logger = logger;
        private readonly CommonMessages _commonMessages = commonMessages.Value;
        private readonly IUserProfileService _userProfileService = userProfileService;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly string defaultPhoto = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRLMI5YxZE03Vnj-s-sth2_JxlPd30Zy7yEGg&s";
        #endregion

        #region Get
        [HttpGet("GetLanguageList")]
        public async Task<ApiResponse<LanguageModel>> GetLanguageList()
        {
            ApiResponse<LanguageModel> response = new();
            try
            {
                List<LanguageModel> languages = await _userProfileService.GetLanguageList();
                response.Data = languages;
                response.Success = true;
            }
            catch (Exception ex)
            {

                string st = _commonMessages.CreateCommonMessage("GetLanguageList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
        #endregion

        #region Post
        [HttpPost("GetUserDetailById")]
        public async Task<ApiPostResponse<UserDetailModel>> GetUserDetailById(UserDetailModel model)
        {
            ApiPostResponse<UserDetailModel> response = new();
            try
            {
                UserDetailModel userDetail = await _userProfileService.GetUserDetailById(model.LoggedInUserId);
                if (userDetail.Languages != null)
                {
                    userDetail.FormatLanguages = await SetFormatLanguages(userDetail?.Languages);
                }
                userDetail.UserPhoto = userDetail.UserPhoto != null ? $"http://localhost:48400/Uploads/{userDetail.UserPhoto}" : defaultPhoto;
                response.Data = userDetail;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetUserDetailById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        [HttpPost("SaveUserProfileDetail")]
        public async Task<ApiPostResponse<UserDetailModel>> SaveUserProfileDetail([FromForm] SaveUserDetailModel model)
        {
            ApiPostResponse<UserDetailModel> response = new();
            try
            {

                UserDetailModel ProfileData = JsonConvert.DeserializeObject<UserDetailModel>(model?.UserDetailData);
                ProfileData.LoggedInUserId = model.LoggedInUserId;
                



                if (model.ProfileData == null && ProfileData.UserPhoto != null)
                {
                    ProfileData.UserPhoto = null;
                }

                if (model.ProfileData != null)
                {
                    // for the save image in the local backend
                    var guid = Guid.NewGuid();
                    var fileName = guid.ToString() + "-" + model.ProfileData.FileName; // Keep the file extension
                                                                                       // Define the path where the image will be saved
                    var path = Path.Combine(_environment.WebRootPath, "Uploads", fileName);

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(Path.Combine(_environment.WebRootPath, "Uploads")))
                    {
                        Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "Uploads"));
                    }

                    // Save the image to the server
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ProfileData.CopyToAsync(stream);
                    }

                    ProfileData.UserPhoto = fileName;
                }

                ProfileData = await _userProfileService.SaveUserProfileDetail(ProfileData);

                if (ProfileData.UserPhoto != null)
                {
                    ProfileData.UserPhoto = $"http://localhost:48400/Uploads/{ProfileData.UserPhoto}";
                }
                response.Data = ProfileData;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("SaveUserProfileDetail", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }


        [HttpGet("GetCountryList")]
        public async Task<ApiResponse<CountryModel>> GetCountryList()
        {
            ApiResponse<CountryModel> response = new();
            try
            {
                List<CountryModel> countries = await _userProfileService.GetCountryList();
                response.Data = countries;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetCountryList", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;

            }

            return response;
        }


        [HttpPost("GetStateListById")]
        public async Task<ApiResponse<StateModel>> GetStateListById(CountryModel country)
        {
            ApiResponse<StateModel> response = new();
            try
            {
                List<StateModel> states = await _userProfileService.GetStateListById(country.CountryId);
                response.Data = states;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetStateListById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;
        }

        [HttpPost("GetCityListById")]
        public async Task<ApiResponse<CityModel>> GetCityListById(StateModel state)
        {
            ApiResponse<CityModel> response = new();
            try
            {
                List<CityModel> cities = await _userProfileService.GetCityListById(state.StateId);
                response.Data = cities;
                response.Success = true;
            }
            catch (Exception ex)
            {
                string st = _commonMessages.CreateCommonMessage("GetCityListById", ex.ToString());
                _logger.Information(st.ToString());
                response.Success = false;
                response.Message = ex.Message;

            }
            return response;
        }

        private async Task<string[]> SetFormatLanguages(string langStr)
        {

            string[] Ids = langStr?.Split(',');
            List<LanguageModel> Languages = await _userProfileService.GetLanguageList();
            var FormatLanguages = Languages?.Where(lang => Ids.Contains(lang.Id.ToString()))?.Select(lang => lang.Language).ToArray();


            return FormatLanguages;
        }
    }
}
