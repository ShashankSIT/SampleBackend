using SampleBackend.Data.DBRepository.Login;
using SampleBackend.Data.DBRepository.PostgraseSQL;
using SampleBackend.Model.Model.Model;
using SampleBackend.Service.Services.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackend.Service.Services.PostgraseSQL
{
    public class PostgraseSQLService(IPostgraseSQLRepository repository) : IPostgraseSQLService
    {
        #region Fields
        private readonly IPostgraseSQLRepository _repository = repository;
        #endregion

        public async Task<string> GetRoleName(int roleId)
        {
            return await _repository.GetRoleName(roleId);
        }

        public async Task<LoginModel> LoginUser(LoginModel model)
        {
            return await _repository.LoginUser(model);
        }

        public async Task<bool> UpsertUser(UserModel model)
        {
            return await _repository.UpsertUser(model);
        }
    }
}
