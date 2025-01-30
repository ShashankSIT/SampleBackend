using SampleBackend.Service.Services.User;
using SampleBackend.Service.Services.Login;
using SampleBackend.Service.Services.RoleRights;
using SampleBackend.Service.Services.PostgraseSQL;

namespace SampleBackend.Service
{
    public static class ServiceRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var serviceDictonary = new Dictionary<Type, Type>
            {
                { typeof(ILoginService), typeof(LoginService) },
                { typeof(IUserService), typeof(UserService) },
                { typeof(IRoleRightsService), typeof(RoleRightsService) },
                { typeof(IPostgraseSQLService), typeof(PostgraseSQLService) }
            };
            return serviceDictonary;
        }
    }
}
