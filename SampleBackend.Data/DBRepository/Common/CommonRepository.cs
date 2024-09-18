using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SampleBackend.Common.Helper;
using SampleBackend.Model.Model;
using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Data.DBRepository.Common
{
    public  class CommonRepository: BaseRepository, ICommonRepository
    {
        private IConfiguration _config;
        public CommonRepository(IConfiguration config, IOptions<ConnectionStrings> dataConfig) : base(dataConfig)
        {
            _config = config;
        }


        public async Task<IEnumerable<CountryModel>> CountryList()
        {
            try
            {
                var param = new DynamicParameters();

                var data = await QueryAsync<CountryModel>(StoreProcedure.CountryList, param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<StateModel>> StateList(int CountryId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@CountryId", CountryId);
                var data = await QueryAsync<StateModel>(StoreProcedure.StateList, param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CityModel>> CityList(int StateId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@stateId", StateId);
                var data = await QueryAsync<CityModel>(StoreProcedure.CityList, param, commandType: CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
