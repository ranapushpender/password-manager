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
    /// Interaction logic for panel.xaml
    /// </summary>
    public partial class panel : Window
    {
        User user;
        AddEntry addEntryDialog=null;
        public panel()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = DBHelper.newInstance().getWalletEntries();
            lbl_username.Content= "Welcome, " + User.CurrentUser.Username;

            //WalletEntry entry = new WalletEntry("T1","url1","user1","pass1");
            //entry.encrypt();
           // entry.decrypt();
        }

        private void Btn_add_entry_Click(object sender, RoutedEventArgs e)
        {
            addEntryDialog = new AddEntry(refreshData);
            addEntryDialog.ShowDialog();
        }

        public void refreshData(bool b)
        {
            if (addEntryDialog != null)
            {
                addEntryDialog.Hide();
                addEntryDialog = null;
            }
            dataGrid.ItemsSource = DBHelper.newInstance().getWalletEntries();
        }

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            refreshData(true);
        }
    }
}
