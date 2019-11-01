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

        public WalletEntry(int id,string title,string url,string username,string password)
        {
            this.id = id;
            this.title = title;
            this.url = url;
            this.username = username;
            this.password = password;
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
        }
    }
}
