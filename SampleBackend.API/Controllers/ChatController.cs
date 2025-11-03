using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SampleBackend.API.Logger;
using SampleBackend.Common;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;
using SampleBackend.Service.Services.Chat;
using SampleBackend.Service.Services.User;

namespace SampleBackend.API.Controllers
{
    [Route("api/chat")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatController(ILoggerManager logger,
        IOptions<CommonMessages> commonMessages,
        IChatService chatService, IUserService userService) : ControllerBase
    {
        #region Fields
        private readonly ILoggerManager _logger = logger;
        private readonly IChatService _chatService = chatService;
        private readonly IUserService _userService = userService;
        private readonly CommonMessages _commonMessages = commonMessages.Value;
        #endregion

        [HttpPost("users")]
        public async Task<ApiResponse<UserModel>> GetUsers(CommonPaginationModel model)
        {
            ApiResponse<UserModel> response = new() { Data = [] };
            try
            {
                List<UserModel> users = await _userService.GetUserList(model);
                response.Data = users;
                response.Success = true;
                response.Message = "Users retrieved successfully";
            }
            catch (Exception ex)
            {
                string error = _commonMessages.CreateCommonMessage("GetUsers", ex.ToString());
                _logger.Information(error);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("chats")]
        public async Task<ApiResponse<ChatModel>> GetMyChats(CommonPaginationModel model)
        {
            ApiResponse<ChatModel> response = new() { Data = [] };
            try
            {
                List<ChatModel> chats = await _chatService.GetChatsForUser(model.LoggedInUserId, model.PageNumber, model.PageSize);
                response.Data = chats;
                response.Success = true;
                response.Message = "Chats retrieved successfully";
            }
            catch (Exception ex)
            {
                string error = _commonMessages.CreateCommonMessage("GetMyChats", ex.ToString());
                _logger.Information(error);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("messages/{chatId}")]
        public async Task<ApiResponse<MessageModel>> GetMessages(int chatId, CommonPaginationModel model)
        {
            ApiResponse<MessageModel> response = new() { Data = [] };
            try
            {
                List<MessageModel> messages = await _chatService.GetMessagesForChat(chatId, model.LoggedInUserId, model.PageNumber, model.PageSize);
                response.Data = messages;
                response.Success = true;
                response.Message = "Messages retrieved successfully";
            }
            catch (Exception ex)
            {
                string error = _commonMessages.CreateCommonMessage("GetMessages", ex.ToString());
                _logger.Information(error);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("private/{targetUserId}")]
        public async Task<ApiPostResponse<long>> StartPrivateChat(CommonIdModel model, int targetUserId)
        {
            ApiPostResponse<long> response = new();
            try
            {
                long chatId = await _chatService.GetOrCreatePrivateChat(model.LoggedInUserId, targetUserId);
                response.Data = chatId;
                response.Success = true;
                response.Message = "Private chat started successfully";
            }
            catch (Exception ex)
            {
                string error = _commonMessages.CreateCommonMessage("StartPrivateChat", ex.ToString());
                _logger.Information(error);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("group")]
        public async Task<ApiPostResponse<long>> CreateGroup(CreateGroupModel model)
        {
            ApiPostResponse<long> response = new();
            try
            {
                string memberIdsStr = string.Join(",", model.MemberIds);
                int chatId = await _chatService.CreateGroupChat(model.ChatName, model.LoggedInUserId, memberIdsStr);
                response.Data = chatId;
                response.Success = true;
                response.Message = "Group created successfully";
            }
            catch (Exception ex)
            {
                string error = _commonMessages.CreateCommonMessage("CreateGroup", ex.ToString());
                _logger.Information(error);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("add-participant")]
        public async Task<ApiResponse<object>> AddParticipant(AddParticipantModel model)
        {
            ApiResponse<object> response = new();
            try
            {
                await _chatService.AddChatParticipant(model.ChatId, model.UserId);
                response.Success = true;
                response.Message = "Participant added successfully";
            }
            catch (Exception ex)
            {
                string error = _commonMessages.CreateCommonMessage("AddParticipant", ex.ToString());
                _logger.Information(error);
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
