using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Service.Services.Chat
{
    public interface IChatService
    {
        Task<int> CreateChat(byte chatType, string chatName, long createdBy);
        Task<int> AddChatParticipant(long chatId, long userId);
        Task<int> SendMessage(long chatId, long senderId, string content);
        Task<List<ChatModel>> GetChatsForUser(long userId, int? pageNumber, int? pageSize);
        Task<List<MessageModel>> GetMessagesForChat(long chatId, long userId, int? pageNumber, int? pageSize);
        Task<long> GetOrCreatePrivateChat(long userId1, long userId2);
        Task<int> CreateGroupChat(string chatName, long createdBy, string memberIds);
    }
}
