using SampleBackend.Model.Model.Model;

namespace SampleBackend.Data.DBRepository.User
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetUserList(CommonPaginationModel model);
        Task<List<UserModel>> GetUserDropdownList(CommonPaginationModel model);
        Task<List<UserModel>> GetRoleList();
        Task<UserModel> SaveUser(UserModel model);
        Task<bool> DeleteUser(CommonIdModel model);
        Task<bool> DeleteMultipleRecords(CommonDeleteModel model);
        Task<bool> DeleteAllUser(CommonModel model);

    }
}
