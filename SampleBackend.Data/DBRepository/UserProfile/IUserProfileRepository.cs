using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Data.DBRepository.UserProfile
{
    public interface IUserProfileRepository
    {
        Task<List<LanguageModel>> GetLanguageList();

        Task<UserDetailModel> GetUserDetailById(long UserId);

        Task<UserDetailModel> SaveUserProfileDetail(UserDetailModel model);

        Task<List<CountryModel>> GetCountryList();

        Task<List<StateModel>> GetStateListById(int CountryId);

        Task<List<CityModel>> GetCityListById(int StateId); 
    }
}
