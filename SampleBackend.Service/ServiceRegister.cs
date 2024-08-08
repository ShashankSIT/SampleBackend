using SampleBackend.Service.Services.User;
using SampleBackend.Service.Services.Login;
using SampleBackend.Service.Services.RoleRights;

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
                { typeof(IRoleRightsService), typeof(RoleRightsService) }
            };
            return serviceDictonary;
        }
    }
}
