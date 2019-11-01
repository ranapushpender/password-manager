using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    class User
    {
        int uid;
        string username;
        string password;
        CryptoEngine crypto;
        private static User currentUser;
        public User(string username,string password)
        {
            this.username = username;
            this.password = password;

        }

        public CryptoEngine Crypto
        {
            get { return crypto; }
        }
        public static User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        public string Password
        {
            get { return password; }
            set { this.password = value; }
        }
        public int UID
        {
            get { return uid; }
            set { uid = value; }
        }
        public Boolean loginUser()
        {
            SHA256 sha = SHA256.Create();
            byte[] userHash = sha.ComputeHash(Encoding.UTF8.GetBytes(username));
            byte[] passHash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] combined = new byte[userHash.Length + passHash.Length];
            Array.Copy(userHash, 0, combined, 0, userHash.Length);
            Array.Copy(passHash, 0, combined, userHash.Length, passHash.Length);
            byte[] final = sha.ComputeHash(combined);
            StringBuilder builder = new StringBuilder();
            foreach (byte b in final)
            {
                builder.Append(b.ToString());
            }
            DBHelper db = DBHelper.newInstance();
            string encKey = null;
            bool result = db.loginUser(builder.ToString(),ref encKey,ref uid);
            if (result)
            {
                MD5 md5 = MD5.Create();
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(username+password));
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                CryptoEngine.newInstance(cTransform.TransformFinalBlock(Convert.FromBase64String(encKey), 0, Convert.FromBase64String(encKey).Length));
                CurrentUser = this;
            }
            password = "";
            return result;
            
        }

        public bool registerUser()
        {
            SHA256 sha = SHA256.Create();
            MD5 md5 = MD5.Create();
            byte[] userHash = sha.ComputeHash(Encoding.UTF8.GetBytes(username));
            byte[] passHash = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            byte[] combined = new byte[userHash.Length+passHash.Length];
            Array.Copy(userHash,0,combined,0,userHash.Length);
            Array.Copy(passHash, 0, combined, userHash.Length, passHash.Length);
            byte[] final = sha.ComputeHash(combined);
            StringBuilder builder = new StringBuilder();
            foreach (byte b in final)
            {
                builder.Append(b.ToString());
            }

            //Creating encryption key

            byte[] encKey = new byte[16];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(encKey);
            encKey = md5.ComputeHash(encKey);

            //Dumping original key
            
            Console.WriteLine("Original key: "+ConvertHelper.getStringFromBytes(encKey));
            
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(username+password));
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] encKeyResultArray = cTransform.TransformFinalBlock(encKey, 0, encKey.Length);
            
            
            DBHelper db = DBHelper.newInstance();
            password = "";
            return db.registerUser(builder.ToString(),Convert.ToBase64String(encKeyResultArray,0,encKeyResultArray.Length));
            
        }
    }
}
