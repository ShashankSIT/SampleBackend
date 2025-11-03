using SampleBackend.Data.DBRepository.Chat;
using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Service.Services.Chat
{
    public class ChatService(IChatRepository repository) : IChatService
    {
        #region Fields
        private readonly IChatRepository _repository = repository;

        #endregion
        public async Task<int> AddChatParticipant(long chatId, long userId)
        {
            return await _repository.AddChatParticipant(chatId, userId);
        }

        public async Task<int> CreateChat(byte chatType, string chatName, long createdBy)
        {
            return await _repository.CreateChat(chatType, chatName, createdBy);
        }

        public async Task<int> CreateGroupChat(string chatName, long createdBy, string memberIds)
        {
            return await _repository.CreateGroupChat(chatName, createdBy, memberIds);
        }

        public async Task<List<ChatModel>> GetChatsForUser(long userId, int? pageNumber, int? pageSize)
        {
            return await _repository.GetChatsForUser(userId, pageNumber, pageSize);
        }

        public async Task<List<MessageModel>> GetMessagesForChat(long chatId, long userId, int? pageNumber, int? pageSize)
        {
            return await _repository.GetMessagesForChat(chatId, userId, pageNumber, pageSize);
        }

        public async Task<long> GetOrCreatePrivateChat(long userId1, long userId2)
        {
            return await _repository.GetOrCreatePrivateChat(userId1, userId2);
        }

        public async Task<int> SendMessage(long chatId, long senderId, string content)
        {
            return await _repository.SendMessage(chatId, senderId, content);
        }
    }
}
