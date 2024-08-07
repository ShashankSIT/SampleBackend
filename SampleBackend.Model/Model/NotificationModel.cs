namespace SampleBackend.Model.Model.Model
{
    public class NotificationModel
    {
    }

    public class EmailNotificationLogDetailModel : CommonPaginationResponse
    {
        public long EmailNotificationId { get; set; }
        public string? NotificationType { get; set; }
        public string? FromEmail { get; set; }
        public string? ToEmail { get; set; }
        public string? CCEmail { get; set; }
        public string? BCCEmail { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailBody { get; set; }
        public bool? IsSuccess { get; set; }
        public long? CreatedBy { get; set; }
        public string? CreatedOn { get; set; }
        public string? RecipientType { get; set; }
    }
}
