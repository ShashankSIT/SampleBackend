using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Model.Model.Model
{
    public class UserDetailModel : CommonModel
    {
        public long? UserDetailId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNo { get; set; }
        public string? UserPhoto { get; set; }
        public string? Gender { get; set; }
        public string? DOB { get; set; }
        public string? Address { get; set; }
        public string? Languages { get; set; }
        public string[]? FormatLanguages { get; set; }
    }

    public class RequestProfileModel : CommonModel
    {
        public IFormFile? ProfileData { get; set; }
        public string? UserPhoto {  get; set; }
    }
    public class SaveUserDetailModel :CommonModel
    { 
        public string? UserDetailData { get; set; }

       public IFormFile? ProfileData { get; set; }
    }


}
