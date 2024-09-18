using SampleBackend.Data.DBRepository.Common;
using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Service.Services.Common
{
 
    public class CommonService: ICommonService
    {
        private readonly ICommonRepository repository;

        public CommonService(ICommonRepository _repository)
        {
            repository = _repository;
        }

        public async Task<IEnumerable<CountryModel>> CountryList()
        { 
            return await repository.CountryList();
        }

        public async Task<IEnumerable<StateModel>> StateList(int CountryId)
        {
            return await repository.StateList(CountryId);
        }

        public async Task<IEnumerable<CityModel>> CityList(int StateId)
        {
            return await repository.CityList(StateId);
        }

    }
}
