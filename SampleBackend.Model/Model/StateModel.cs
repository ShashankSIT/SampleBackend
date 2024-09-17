using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Model.Model.Model
{
    public  class StateModel
    {
        public int StateId { get; set; }
        public string? StateName { get; set; }    
        public int? CountryId { get; set; }

    }
}
