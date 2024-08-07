using SampleBackend.Model.Model.Model;
using SampleBackend.Data.DBRepository.User;

namespace SampleBackend.Service.Services.User
{
    public class UserService(IUserRepository repository) : IUserService
    {
        #region Fields
        private readonly IUserRepository _repository = repository;

        #endregion

        #region Delete
        public async Task<bool> DeleteUser(CommonIdModel model)
        {
            return await _repository.DeleteUser(model);
        }


        #endregion

        #region Get
        public async Task<List<UserModel>> GetRoleList()
        {
            return await _repository.GetRoleList();
        }

        public async Task<List<UserModel>> GetUserDropdownList(CommonPaginationModel model)
        {
            return await _repository.GetUserDropdownList(model);
        }

        public async Task<List<BranchModel>> GetBranchDropdownList(CommonPaginationModel model)
        {
            return await _repository.GetBranchDropdownList(model);
        }

        public async Task<List<CompanyModel>> GetCompanyDropdownList(CommonPaginationModel model)
        {
            return await _repository.GetCompanyDropdownList(model);
        }
        #endregion

        #region Post
        public async Task<List<UserModel>> GetUserList(CommonPaginationModel model)
        {
            return await _repository.GetUserList(model);
        }

        public async Task<UserModel> SaveUser(UserModel model)
        {
            return await _repository.SaveUser(model);
        }
        #endregion
    }
}
