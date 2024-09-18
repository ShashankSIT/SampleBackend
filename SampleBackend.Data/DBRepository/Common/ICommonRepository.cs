using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Data.DBRepository.Common
{
    public interface ICommonRepository
    {
        public Task<IEnumerable<CountryModel>> CountryList();

        public Task<IEnumerable<StateModel>> StateList(int CountryId);

        public Task<IEnumerable<CityModel>> CityList(int StateId);
    }
}
