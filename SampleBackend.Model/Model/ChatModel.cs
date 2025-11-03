using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Model.Model.Model
{
    public class ChatModel
    {
        public long ChatId { get; set; }
        public byte ChatType { get; set; } // 1: Private, 2: Group
        public string? ChatName { get; set; }
        public string? OtherParticipants { get; set; } // Comma-separated names for display
        public DateTime? LastSentAt { get; set; }
        public string? LastMessage { get; set; }
    }

    public class MessageModel
    {
        public long MessageId { get; set; }
        public long ChatId { get; set; }
        public long SenderId { get; set; }
        public string? SenderName { get; set; }
        public string? Content { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
    }

    public class CreateGroupModel : CommonModel
    {
        public string? ChatName { get; set; }
        public List<long>? MemberIds { get; set; }
    }

    public class AddParticipantModel
    {
        public long ChatId { get; set; }
        public long UserId { get; set; }
    }
}
