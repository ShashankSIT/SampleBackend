using System.ComponentModel;

namespace SampleBackend.Common
{
    public class Enums
    {


    }
    public static class EnumExtension
    {
        /// <summary>
        /// The get description.
        /// </summary>
        /// <param name="element">
        /// The element.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetDescription(this Enum element)
        {
            var type = element.GetType();
            var memberInfo = type.GetMember(Convert.ToString(element));
            if (memberInfo.Length > 0)
            {
                var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            return Convert.ToString(element);
        }
    }

    public static class RoleName
    {
        public const string MoveManager = "Move Manager";
        public const string Admin = "Admin";
        public const string SuperAdmin = "Super Admin";
    }

    public static class EmailNotificationType
    {
        public const string NewUser = "New User";
        public const string ForgotPassword = "Forgot Password";
        public const string TwoFactorAuth = "Two Factor Authentication";
    }
}
