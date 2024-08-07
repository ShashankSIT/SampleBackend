using System.Text;

namespace SampleBackend.Model.Model
{
    public class CommonMessages
    {
        public string? Error { get; set; }
        public User? User { get; set; }
        public Login? Login { get; set; }
        public RoleRights? RoleRights { get; set; }
        public ForgotPassword? ForgotPassword { get; set; }
        public UpdatePassword? UpdatePassword { get; set; }
        public TwoFactorAuth? TwoFactorAuth { get; set; }
        public Dashboard? Dashboard { get; set; }
        public string CreateCommonMessage(string strmethod, string strData)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine(strmethod);
            s.AppendLine("ERROR");
            s.AppendLine(strData);
            return s.ToString();
        }

    }
    public class User : Messages { }
    public class Login : Messages { }
    public class RoleRights : Messages { }
    public class ForgotPassword : Messages { }
    public class UpdatePassword : Messages { }
    public class TwoFactorAuth : Messages { }
    public class Dashboard : Messages { }
    public class Messages
    {
        public string? SaveSuccess { get; set; }
        public string? SaveError { get; set; }
        public string? DeleteSuccess { get; set; }
        public string? DeleteError { get; set; }
        public string? ExcelSaveError { get; set; }
        public string? AlreadyExists { get; set; }
    }
}
