using SampleBackend.Data.DBRepository.User;
using SampleBackend.Data.DBRepository.Login;
using SampleBackend.Data.DBRepository.RoleRights;
using SampleBackend.Data.DBRepository.Chat;

namespace SampleBackend.Data
{
    public static class DataRegister
    {
        public static Dictionary<Type, Type> GetTypes()
        {
            var dataDictionary = new Dictionary<Type, Type>
            {
                { typeof(ILoginRepository), typeof(LoginRepository) },
                { typeof(IUserRepository), typeof(UserRepository) },
                { typeof(IChatRepository), typeof(ChatRepository) },
                { typeof(IRoleRightsRepository), typeof(RoleRightsRepository) }
            };
            return dataDictionary;
        }
    }
}
