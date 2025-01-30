using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Data.DBRepository.PostgraseSQL
{
    public interface IPostgraseSQLRepository
    {
        Task<LoginModel> LoginUser(LoginModel model);
        Task<string> GetRoleName(int roleId);
        Task<bool> UpsertUser(UserModel model);
    }
}
