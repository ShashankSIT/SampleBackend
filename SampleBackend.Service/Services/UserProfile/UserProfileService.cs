using SampleBackend.Data.DBRepository.User;
using SampleBackend.Data.DBRepository.UserProfile;
using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Service.Services.UserProfile
{
    public class UserProfileService(IUserProfileRepository repository) : IUserProfileService
    {

        #region Fields
        private readonly IUserProfileRepository _repository = repository;
        #endregion
        public async Task<List<CityModel>> GetCityListById(int StateId)
        {
            return await _repository.GetCityListById(StateId);
        }

        public async Task<List<CountryModel>> GetCountryList()
        {
            return await _repository.GetCountryList();
        }
        public async Task<List<LanguageModel>> GetLanguageList()
        {
            return await _repository.GetLanguageList();
        }

        public async Task<List<StateModel>> GetStateListById(int CountryId)
        {
            return await _repository.GetStateListById(CountryId);
        }

        public async Task<UserDetailModel> GetUserDetailById(long UserId)
        {
            return await _repository.GetUserDetailById(UserId);
        }

        public async Task<UserDetailModel> SaveUserProfileDetail(UserDetailModel model)
        {
            return await _repository.SaveUserProfileDetail(model);
        }


    }
}