using System.Security.Cryptography;
using System.Text;

namespace SampleBackend.Common
{
    public class EncryptionDecryption
    {
        #region Variable Declaration

        /// <summary>
        /// key String
        /// </summary>
        //private static string keyString = "09TSITUS-AARH-JMBM-2BOB-26OVN1983BYE";
        private static readonly string keyString = "PI34FF3E-9KF2-4177-B59Q-WS81D9564426";

        #endregion

        #region Methods/Functions

        /// <summary>
        /// Get Encrypted Value of Passed value
        /// </summary>
        /// <param name="value">value to Encrypted</param>
        /// <returns>encrypted string</returns>
        public static string GetEncrypt(string value)
        {
            return SHA512Encrypt(keyString, value);
        }

        /// <summary>
        /// Get Decrypted value of passed encrypted string
        /// </summary>
        /// <param name="value">value to Decrypted</param>
        /// <returns>Decrypted string</returns>
        public static string GetDecrypt(string value)
        {
            return SHA512Decrypt(keyString, value);
        }

        public static string SHA512Encrypt(string EncryptionKey, string value)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(value);
            string cipherText = string.Empty;
            using (SHA512 encryptor = SHA512.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                //encryptor.Key = pdb.GetBytes(32);
                //encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new())
                {
                    using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    cipherText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return cipherText;
        }

        public static string SHA512Decrypt(string EncryptionKey, string value)
        {
            string clearText = string.Empty;
            byte[] cipherBytes = Convert.FromBase64String(value);
            using (SHA512 encryptor = SHA512.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                //encryptor.Key = pdb.GetBytes(32);
                //encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new())
                {
                    using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    clearText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string GeneratePassword(int lengthOfPassword, bool isReset = false)
        {
            Random random = new();
            string specialCharacters = isReset ? "!@*$" : "!@*&$";
            string[] categories = { "ABCDEFGHIJKLMNPQRSTUVWXYZ", "abcdefghijklmnpqrstuvwxyz", specialCharacters, "0123456789" };

            List<char> chars = new(lengthOfPassword);
            foreach (string cat in categories)
            {
                chars.Add(cat[random.Next(cat.Length)]);
            }
            string all = string.Concat(categories);
            while (chars.Count < lengthOfPassword)
            {
                chars.Add(all[random.Next(all.Length)]);
            }
            char[]? password = [.. chars.OrderBy(c => random.NextDouble())];
            return string.Join(null, password);
        }
        #endregion
    }
}
