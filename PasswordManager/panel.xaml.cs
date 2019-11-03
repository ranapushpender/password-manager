using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
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
        /*Hotkey*/
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        //private const uint MOD_CONTROL = 0x0100| 0x0040; 

        private IntPtr _windowHandle;
        private HwndSource _source;
        private const int HOTKEY_ID = 9000;
        private const uint VK_OEM_4 = 0xBE;
        private const uint VK_OEM_6 = 0xBF;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_OEM_4); //CTRL + CAPS_LOCK
            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_OEM_6); //CTRL + CAPS_LOCK
        }
        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            
            switch (msg)
            {
                case WM_HOTKEY:
                    
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            Console.WriteLine("Handler exec");
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            WalletEntry wEntry = walletEntries[currentFillEntry];
                            if (vkey == VK_OEM_4)
                            {
                                
                                if (wEntry != null)
                                {
                                    bool changed = false;
                                    if (wEntry.IsEncrypted)
                                    {
                                        wEntry.decrypt();
                                        changed = true;
                                    }

                                    //Console.WriteLine(wEntry.Username);
                                    Thread.Sleep(500);
                                    SendKeys.SendWait(wEntry.Username);
                                    if (changed = true)
                                    {
                                        wEntry.encrypt();
                                    }
                                }
                            }
                            else if (vkey == VK_OEM_6)
                            {
                                bool changed = false;
                                if (wEntry.IsEncrypted)
                                {
                                    wEntry.decrypt();
                                    changed = true;
                                }

                                //Console.WriteLine(wEntry.Username);
                                Thread.Sleep(500);
                                SendKeys.SendWait(wEntry.Password);
                                if (changed = true)
                                {
                                    wEntry.encrypt();
                                }
                            }
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }
        /*Hotkey*/


        User user;
        AddEntry addEntryDialog=null;
        List<WalletEntry> walletEntries;
        int currentFillEntry = -1;
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
            addEntryDialog = AddEntry.newIntance(null,refreshData);
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            WalletEntry obj = ((FrameworkElement)sender).DataContext as WalletEntry;
            addEntryDialog = AddEntry.newIntance(obj.Id,refreshData);
            addEntryDialog.ShowDialog();
        }

        private void Btn_gen_pass_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            WalletEntry obj = (WalletEntry)dataGrid.CurrentCell.Item;
            System.Windows.Clipboard.SetText(obj[dataGrid.CurrentCell.Column.DisplayIndex].ToString());
            



        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            currentFillEntry = walletEntries.IndexOf((WalletEntry)dataGrid.CurrentCell.Item);
            
        }
    }
}
