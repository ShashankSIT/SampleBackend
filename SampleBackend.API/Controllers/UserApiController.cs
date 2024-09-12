using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
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

        [HttpPost("ExportUserList")]
        public async Task<IActionResult> ExportUserList([FromBody] CommonPaginationModel model)
        {
            try
            {
                // Fetch the user list
                List<UserModel> users = await _userService.GetUserList(model);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // Create a new Excel package
                using (var package = new ExcelPackage())
                {
                    // Add a worksheet
                    var worksheet = package.Workbook.Worksheets.Add("Users");

                    // Define styles
                    var headerStyle = worksheet.Cells[1, 1, 1, 5].Style;
                    headerStyle.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headerStyle.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#253652"));
                    headerStyle.Font.Color.SetColor(System.Drawing.Color.White);
                    headerStyle.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    // Add headers with styling
                    worksheet.Cells[1, 1].Value = "UserId";
                    worksheet.Cells[1, 2].Value = "FirstName";
                    worksheet.Cells[1, 3].Value = "LastName";
                    worksheet.Cells[1, 4].Value = "Email";
                    worksheet.Cells[1, 5].Value = "RoleName";

                    // Add data
                    int row = 2;
                    foreach (var user in users)
                    {
                        worksheet.Cells[row, 1].Value = user.UserId;
                        worksheet.Cells[row, 2].Value = user.FirstName;
                        worksheet.Cells[row, 3].Value = user.LastName;
                        worksheet.Cells[row, 4].Value = user.Email;
                        worksheet.Cells[row, 5].Value = user.RoleName;
                        row++;
                    }

                    // Center align text for all cells
                    var dataStyle = worksheet.Cells[2, 1, row - 1, 5].Style;
                    dataStyle.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    // Set column widths
                    worksheet.Cells.AutoFitColumns();

                    // Convert to byte array
                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    var content = stream.ToArray();
                    var fileName = $"UserList_{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx";

                    // Return the file
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.Information(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
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
