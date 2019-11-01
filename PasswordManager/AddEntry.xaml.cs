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

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for AddEntry.xaml
    /// </summary>
    public partial class AddEntry : Window
    {
        Action<bool> callback;
        public AddEntry(Action<bool> callback)
        {
            this.callback = callback;
            InitializeComponent();
        }

        private void Btn_add_entry_submit_Click(object sender, RoutedEventArgs e)
        {
            WalletEntry entry = new WalletEntry(txt_title.Text,txt_url.Text,txt_username.Text,txt_password.Text);
            if(DBHelper.newInstance().save(entry))
            {
                callback(true);
            }
            else
            {
                callback(false);
            }
            
            
            
        }
    }
}
