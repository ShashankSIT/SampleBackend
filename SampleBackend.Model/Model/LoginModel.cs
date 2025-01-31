namespace SampleBackend.Model.Model.Model
{
    public class LoginModel : CommonModel
    {
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public long? RoleId { get; set; }
        public string? PhoneNo { get; set; }
        public string? UserPhoto { get; set; }
        public bool? IsFirstLogin { get; set; }
        public string? RoleName { get; set; }
        public string? ErrorMessage { get; set; }
        public bool? IsBlocked { get; set; }
        public string? JWTToken { get; set; }
        public string? EncryptedUserId { get; set; }
        public bool? Is2FARequired { get; set; }
        public string? VerifyUser { get; set; }
        public string? TemporaryPassword { get; set; }
    }

    public class UserLoginTrackModel
    {
        public int? WrongAttemptCount { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public string? StrMessage { get; set; }
    }

    public class UserAuthModel : LoginModel
    {
        public string? TwoFactorCode { get; set; }
        public string? auth { get; set; }
        public string? CurrentPassword { get; set; }
    }

}
