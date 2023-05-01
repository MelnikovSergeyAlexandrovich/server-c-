using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace prac16
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Regex regexIP = new Regex("([0-9]{1,3}).([0-9]{1,3}).([0-9]{1,3}).([0-9]{1,3})$");
        Regex regexLogin = new Regex("(?=.*[A-Za-z0-9]$)[A-Za-z][A-Za-z\\d.-]{0,19}$");
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(InputLogin.Text,regexLogin.ToString()) && Regex.IsMatch(InputIP.Text, regexIP.ToString()))
            {
                UserChatPanel win = new UserChatPanel(InputIP.Text);
                win.Show();
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(InputLogin.Text, regexLogin.ToString()) && Regex.IsMatch(InputIP.Text, regexIP.ToString()))
            {
                AdminChatPanel win = new AdminChatPanel();
                win.Show();
                this.Visibility = Visibility.Collapsed;
            }
        }
    }
}
