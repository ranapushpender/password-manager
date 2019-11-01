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
        List<WalletEntry> walletEntries;
        public panel()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            walletEntries = DBHelper.newInstance().getWalletEntries();
            dataGrid.ItemsSource = walletEntries;
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
            walletEntries = DBHelper.newInstance().getWalletEntries();
            dataGrid.ItemsSource = walletEntries;
        }

        private void Btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            refreshData(true);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            WalletEntry obj = ((FrameworkElement)sender).DataContext as WalletEntry;
            if (obj.IsEncrypted)
            {
                obj.decrypt();
            }
            else
            {
                obj.encrypt();
            }
            dataGrid.ItemsSource = walletEntries;
            dataGrid.Items.Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WalletEntry obj = ((FrameworkElement)sender).DataContext as WalletEntry;
            DBHelper.newInstance().deleteEntry(obj.Id);
            walletEntries.Remove(obj);
            dataGrid.ItemsSource = walletEntries;
            dataGrid.Items.Refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            User.CurrentUser.Password = "";
            User.CurrentUser.Username = "";
            User.CurrentUser.UID = -1;
            User.CurrentUser = null;
            walletEntries = null;
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
