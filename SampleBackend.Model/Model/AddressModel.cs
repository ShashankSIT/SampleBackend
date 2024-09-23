using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Model.Model.Model
{
    public class AddressModel
    {
        public int? AddressId { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }

        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public long UserDetailId { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public string? CityName { get; set;}
        public bool? IsDelete { get; set; }

    }

    public class AddressResponse
    {
        public string? Success { get; set; }

        public string? ErrorMessage {  get; set; }
    }

}
