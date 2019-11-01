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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = new User(text_user.Text, text_pass.Password);
            if (user.loginUser())
            {
                panel userPanel = new panel();
                userPanel.Show();
                userPanel.Activate();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login Failed");
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            User user = new User(text_user.Text, text_pass.Password);
            if(user.registerUser())
            {
                MessageBox.Show("User Created Successfully");
            }
            else
            {
                MessageBox.Show("User Creation failed");
            }

        }
    }
}
