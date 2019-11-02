using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for AddEntry.xaml
    /// </summary>
    /// 
    public delegate void operation();
    public partial class AddEntry : Window
    {
        Action<bool> callback;
        WalletEntry entry;
        operation op;
        
        private AddEntry(Action<bool> callback)
        {
            this.callback = callback;
            InitializeComponent();
        }

        public static AddEntry newIntance(Nullable<int> id,Action<bool> callback)
        {
            if (id.HasValue)
            {
                AddEntry add = new AddEntry(callback);
                add.entry = DBHelper.newInstance().getWalletEntry(id.Value);
                if (add.entry == null)
                {
                    return null;
                }
                add.fillData();
                add.op = add.updateEntry;
                add.btn_add_entry_submit.Content = "UPDATE";
                return add;
            }
            else
            {
                AddEntry add = new AddEntry(callback);
                add.op = add.saveEntry;
                add.btn_add_entry_submit.Content = "ADD";
                return add;
            }
        }

        private void Btn_add_entry_submit_Click(object sender, RoutedEventArgs e)
        {
            op();
        }

        private void saveEntry() {
            entry = new WalletEntry(txt_title.Text, txt_url.Text, txt_username.Text, txt_password.Text);
            if (DBHelper.newInstance().save(entry))
            {
                callback(true);
            }
            else
            {
                callback(false);
            }
        }

        private void updateEntry()
        {
            entry.Title = txt_title.Text;
            entry.Username = txt_username.Text;
            entry.Password = txt_password.Text;
            entry.Url = txt_url.Text;

            if (DBHelper.newInstance().update(entry))
            {
                callback(true);
            }
            else
            {
                callback(false);
            }
        }

        private void fillData() {
            if (entry != null)
            {
                if (entry.IsEncrypted == true)
                {
                    entry.decrypt();
                    entry.IsEncrypted = false;
                }
                txt_title.Text = entry.Title;
                txt_url.Text = entry.Url;
                txt_username.Text = entry.Username;
                txt_password.Text = entry.Password;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] b = new byte[16];
            provider.GetBytes(b);
            txt_password.Text = Convert.ToBase64String(b);
        }
    }
}
