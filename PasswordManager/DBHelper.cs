using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager
{
    class DBHelper
    {
        static DBHelper helper=null;
        MySqlConnection con;
        MySqlCommand cmd;
        AddEntry addEntryDialog;
        private DBHelper()
        {
            con = new MySqlConnection(@"server=localhost;userid=root;password=;database=c_sharp_project");
            con.Open();
            cmd = new MySqlCommand();
            cmd.Connection = con;
        }

        public static DBHelper newInstance()
        {
            if (helper == null)
            {
                helper = new DBHelper();

            }
            return helper;
        }

        public Boolean registerUser(string hash,string encKey)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = "select hash from users where hash=@hashVal";
            cmd.Parameters.AddWithValue("hashVal", hash);
            cmd.Prepare();
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                if (!reader.GetString(0).Equals(hash))
                {
                    return false;
                    
                }
                reader.Close();
                return false;
            }
            else
            {
                reader.Close();
                cmd.Parameters.Clear();
                cmd.CommandText ="insert into users(hash,ekey) values(@hashVal,@keyVal)";
                cmd.Parameters.AddWithValue("hashVal", hash);
                cmd.Parameters.AddWithValue("keyVal", encKey);
                cmd.Prepare();
                int result = cmd.ExecuteNonQuery();
                return true;
            }
        }

        public bool loginUser(string hash,ref string encKey,ref int uid)
        {
            cmd.Parameters.Clear();
            bool retVal = false;
            cmd.CommandText = "select hash,ekey,id from users where hash=@hashVal";
            cmd.Parameters.AddWithValue("hashVal", hash);
            cmd.Prepare();
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                if (reader.GetString(0).Equals(hash))
                {
                    retVal = true;
                    encKey = reader.GetString(1);
                    uid = reader.GetInt32(2);
                }
                else
                {
                    retVal= false;
                }
                
            }
            else
            {
                retVal= false;
            }
            reader.Close();
            return retVal;
        }

        public List<WalletEntry> getWalletEntries()
        {
            
            List<WalletEntry> entries = new List<WalletEntry>();

            MySqlDataReader reader=null;
            try
            {
                cmd.Parameters.Clear();
                cmd.CommandText = "select id,title,url,username,password from entries where uid = @userId";
                cmd.Parameters.AddWithValue("userId",User.CurrentUser.UID);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    WalletEntry.createObjectFromReader(reader, entries);
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return entries;
        }

        public bool save(WalletEntry entry)
        {
            entry.encrypt();
            cmd.Parameters.Clear();
            cmd.CommandText = "insert into entries(title,url,username,password,uid) values(@title,@url,@user,@pass,@uid)";
            cmd.Parameters.AddWithValue("title", entry.Title);
            cmd.Parameters.AddWithValue("url", entry.Url);
            cmd.Parameters.AddWithValue("user", entry.Username);
            cmd.Parameters.AddWithValue("pass", entry.Password);
            cmd.Parameters.AddWithValue("uid", User.CurrentUser.UID);
            if (cmd.ExecuteNonQuery()>0)
            {
                return true;
            }
            return false;
        }

    }
}
