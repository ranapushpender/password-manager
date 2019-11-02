using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager
{
    public class WalletEntry
    {
        int id;
        string title;
        string url;
        string username;
        string password;
        bool isEncrypted;

        public WalletEntry(int id,string title,string url,string username,string password)
        {
            this.id = id;
            this.title = title;
            this.url = url;
            this.username = username;
            this.password = password;
            
        }

        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return id;
                    case 1:
                        return title;
                    case 2:
                        return url;
                    case 3:
                        return username;
                    case 4:
                        return password;
                    default:
                        return 1;
                }
            }
            
        }
        public bool IsEncrypted {
            get { return isEncrypted; }
            set { isEncrypted = value; }
        }
        public WalletEntry( string title, string url, string username, string password)
        {
            this.title = title;
            this.url = url;
            this.username = username;
            this.password = password;
        }
        public int Id
        {
            get { return id; }
            set { this.id = value; }
        }
        public string Title { get => title; set => title = value; }
        public string Url { get => url; set => url = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }

        public static List<WalletEntry> createObjectFromReader(MySqlDataReader reader,List<WalletEntry> entries)
        {
            while (reader.Read())
            {
                Console.WriteLine(reader.GetInt32(0));
                WalletEntry entry = new WalletEntry(reader.GetInt32(0), reader.GetString(1),reader.GetString(2),reader.GetString(3),reader.GetString(4));
                entry.IsEncrypted = true;
                entries.Add(entry);
            }
            return entries;
        }

        public void encrypt()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            
            data["url"] = url;
            data["password"] = password;
            data["username"] = username;
            CryptoEngine.getInstance().encrypt(data);
            
            this.url = data["url"];
            this.username = data["username"];
            this.password = data["password"];
            isEncrypted = true;
        }

        public void decrypt()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            
            data["url"] = url;
            data["password"] = password;
            data["username"] = username;
            CryptoEngine.getInstance().decrypt(data);
            
            this.url = data["url"];
            this.username = data["username"];
            this.password = data["password"];
            isEncrypted = false;
        }
    }
}
