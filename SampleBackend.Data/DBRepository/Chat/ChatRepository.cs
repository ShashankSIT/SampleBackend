using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleBackend.Common.Helper;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Data.DBRepository.Chat
{
    public class ChatRepository(IConfiguration config, IOptions<ConnectionStrings> connectionString) : BaseRepository(connectionString), IChatRepository
    {
        #region Fields
        private readonly IConfiguration _config = config;
        #endregion

        public async Task<int> AddChatParticipant(long chatId, long userId)
        {
            var param = new DynamicParameters();
            param.Add("@ChatId", chatId);
            param.Add("@UserId", userId);
            return await QueryFirstOrDefaultAsync<int>(StoreProcedure.AddChatParticipant, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CreateChat(byte chatType, string chatName, long createdBy)
        {
            var param = new DynamicParameters();
            param.Add("@ChatType", chatType);
            param.Add("@ChatName", chatName);
            param.Add("@CreatedBy", createdBy);
            param.Add("@ChatId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            return await QueryFirstOrDefaultAsync<int>(StoreProcedure.CreateChat, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> CreateGroupChat(string chatName, long createdBy, string memberIds)
        {
            var param = new DynamicParameters();
            param.Add("@ChatName", chatName);
            param.Add("@CreatedBy", createdBy);
            param.Add("@MemberIds", memberIds);
            param.Add("@ChatId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            return await QueryFirstOrDefaultAsync<int>(StoreProcedure.CreateGroupChat, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<List<ChatModel>> GetChatsForUser(long userId, int? pageNumber, int? pageSize)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@PageNumber", pageNumber);
            param.Add("@PageSize", pageSize);
            var data = await QueryAsync<ChatModel>(StoreProcedure.GetChatsForUser, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<List<MessageModel>> GetMessagesForChat(long chatId, long userId, int? pageNumber, int? pageSize)
        {
            var param = new DynamicParameters();
            param.Add("@ChatId", chatId);
            param.Add("@UserId", userId);
            param.Add("@PageNumber", pageNumber);
            param.Add("@PageSize", pageSize);
            var data = await QueryAsync<MessageModel>(StoreProcedure.GetMessagesForChat, param, commandType: CommandType.StoredProcedure);
            return data.ToList();
        }

        public async Task<long> GetOrCreatePrivateChat(long userId1, long userId2)
        {
            var param = new DynamicParameters();
            param.Add("@UserId1", userId1);
            param.Add("@UserId2", userId2);
            param.Add("@ChatId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            return await QueryFirstOrDefaultAsync<long>(StoreProcedure.GetOrCreatePrivateChat, param, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SendMessage(long chatId, long senderId, string content)
        {
            var param = new DynamicParameters();
            param.Add("@ChatId", chatId);
            param.Add("@SenderId", senderId);
            param.Add("@Content", content);
            param.Add("@MessageId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            return await QueryFirstOrDefaultAsync<int>(StoreProcedure.SendMessage, param, commandType: CommandType.StoredProcedure);
        }
    }
}
