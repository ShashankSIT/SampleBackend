using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Service.Services.Common
{
    public interface ICommonService
    {

        public Task<IEnumerable<CountryModel>> CountryList();

        public Task<IEnumerable<StateModel>> StateList(int CountryId);

        public Task<IEnumerable<CityModel>> CityList(int StateId);
    }
}
