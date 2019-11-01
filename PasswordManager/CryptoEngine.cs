using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    
    class CryptoEngine
    {
        byte[] encKey;
        TripleDESCryptoServiceProvider tripleDES;
        ICryptoTransform cTransformDecryptor;
        ICryptoTransform cTransformEncryptor;
        static CryptoEngine engine = null;
        private CryptoEngine(byte[] key)
        {
            encKey = key;
            Console.WriteLine(ConvertHelper.getStringFromBytes(key));
            tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = encKey;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            cTransformDecryptor = tripleDES.CreateDecryptor();
            cTransformEncryptor = tripleDES.CreateEncryptor();

        }

        public static CryptoEngine newInstance(byte[] key)
        {
            if (engine == null)
            {
                engine = new CryptoEngine(key);
            }
            return engine;
        }
        public static CryptoEngine getInstance()
        {
            return engine;
        }

        public Dictionary<string,string> encrypt(Dictionary<string,string> data)
        {
            IEnumerable<string> keys = data.Keys;
            for (int i = 0; i < keys.Count(); i++)
            {
                string key = keys.ElementAt(i);
                
                byte[] value = Encoding.UTF8.GetBytes(data[key]);
                
                data[key] = Convert.ToBase64String(cTransformEncryptor.TransformFinalBlock(value, 0, value.Length));
               // Console.WriteLine("Encrypted " + key + " = " + data[key]);
            }
            return data;
        }

        public Dictionary<string,string> decrypt(Dictionary<string,string> data)
        {
            IEnumerable<string> keys = data.Keys;
            for(int i=0;i<keys.Count();i++)
            {
                string key = keys.ElementAt(i);
                byte[] byteValue = Convert.FromBase64String(data[key].Replace(" ",""));
                byte[] value = cTransformDecryptor.TransformFinalBlock(byteValue,0,byteValue.Length);
                data[key] = Encoding.UTF8.GetString(value);
                //Console.WriteLine("Decrypted " + key + " = " + data[key]);
                
            }
            return data;
        }
    }
}
