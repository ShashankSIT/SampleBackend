using SampleBackend.Model.Model.Model;
using System.Reflection;

namespace SampleBackend.Service.Services.User
{
    public interface IUserService
    {
        Task<List<UserModel>> GetUserList(CommonPaginationModel model);
        Task<List<UserDetailsMasterModel>> GetUserDetailsList(CommonPaginationModel model);
        Task<List<UserModel>> GetUserDropdownList(CommonPaginationModel model);
        Task<List<UserModel>> GetRoleList();
        Task<UserModel> SaveUser(UserModel model);
        Task<bool> DeleteUser(CommonIdModel model);
        Task<bool> DeleteMultipleRecords(CommonDeleteModel model);
        Task<bool> DeleteAllUser(CommonModel model);

    }
}
