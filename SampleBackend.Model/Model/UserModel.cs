namespace SampleBackend.Model.Model.Model
{
    public class UserModel : CommonModel
    {
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public long? RoleId { get; set; }
        public string? PhoneNo { get; set; }
        public string? UserPhoto { get; set; }
        public string? RoleName { get; set; }
        public string? ErrorMessage { get; set; }
        public long? CompanyId { get; set; }
    }

    public class UserDetailsMasterModel : UserModel
    {
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public int? CountryId { get; set; }
        public string? AboutMe { get; set; }
        public string? CoverPhoto { get; set; }
    }
}
