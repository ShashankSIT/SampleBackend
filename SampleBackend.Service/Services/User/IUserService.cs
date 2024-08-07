using SampleBackend.Model.Model.Model;

namespace SampleBackend.Service.Services.User
{
    public interface IUserService
    {
        Task<List<UserModel>> GetUserList(CommonPaginationModel model);
        Task<List<UserModel>> GetUserDropdownList(CommonPaginationModel model);
        Task<List<BranchModel>> GetBranchDropdownList(CommonPaginationModel model);
        Task<List<CompanyModel>> GetCompanyDropdownList(CommonPaginationModel model);
        Task<List<UserModel>> GetRoleList();
        Task<UserModel> SaveUser(UserModel model);
        Task<bool> DeleteUser(CommonIdModel model);

    }
}
