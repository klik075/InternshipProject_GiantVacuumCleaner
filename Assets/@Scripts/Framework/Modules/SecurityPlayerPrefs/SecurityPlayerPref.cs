
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Scripts.Framework.Modules.SecurityPlayerPrefs
{
    public class SecurityPlayerPref
    {
        #region Fields

        protected static readonly string SaltForKey;
        protected const int HashLen = 32;
        private static readonly byte[] Keys;
        private static readonly byte[] Iv;
        private const int KeySize = 256;
        private const int BlockSize = 128;

        #endregion
        
        #region Constructor and Cryptography

        static SecurityPlayerPref()
        {
            // 8 바이트로 하고, 변경해서 쓸것
            byte[] saltBytes = new byte[] { 5, 21, 43, 51, 33, 4, 50, 73 };
            // 길이 상관 없고, 키를 만들기 위한 용도로 씀
            string randomSeedForKey = "dfsdfsdfsdfx2bblocksdf2";
            // 길이 상관 없고, aes에 쓸 key 와 iv 를 만들 용도
            string randomSeedForValue = "vdskljfsdxv2xcx2b0010121";
            Rfc2898DeriveBytes randomSeedKey = new Rfc2898DeriveBytes(randomSeedForKey, saltBytes, 1000);
            Rfc2898DeriveBytes randomSeedValue = new Rfc2898DeriveBytes(randomSeedForValue, saltBytes, 1000);
            SaltForKey = System.Convert.ToBase64String(randomSeedKey.GetBytes(BlockSize / 8));
            Keys = randomSeedValue.GetBytes(KeySize / 8);
            Iv = randomSeedValue.GetBytes(BlockSize / 8);
        }

        protected static string MakeHash(string original)
        {
            using MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(original);
            byte[] hashBytes = md5.ComputeHash(bytes);
            return hashBytes.Aggregate("", (current, t) => current + t.ToString("x2"));
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted)
        {
            using RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Key = Keys;
            aes.IV = Iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using ICryptoTransform ct = aes.CreateEncryptor();
            return ct.TransformFinalBlock(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted)
        {
            using RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = KeySize;
            aes.BlockSize = BlockSize;
            aes.Key = Keys;
            aes.IV = Iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using ICryptoTransform ct = aes.CreateDecryptor();
            return ct.TransformFinalBlock(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
        }

        protected static string Encrypt(string input)
        {
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] bytesEncrypted = Encrypt(bytesToBeEncrypted);
            return System.Convert.ToBase64String(bytesEncrypted);
        }

        protected static string Decrypt(string input)
        {
            byte[] bytesToBeDecrypted = System.Convert.FromBase64String(input);
            byte[] bytesDecrypted = Decrypt(bytesToBeDecrypted);
            return Encoding.UTF8.GetString(bytesDecrypted);
        }

        #endregion
    }
}