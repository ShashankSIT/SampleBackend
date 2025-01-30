using SampleBackend.Model.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Service.Services.PostgraseSQL
{
    public interface IPostgraseSQLService
    {
        Task<LoginModel> LoginUser(LoginModel model);
        Task<bool> UpsertUser(UserModel model);
        Task<string> GetRoleName(int roleId);

    }
}
