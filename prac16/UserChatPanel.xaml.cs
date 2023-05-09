using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace prac16
{
    /// <summary>
    /// Логика взаимодействия для UserChatPanel.xaml
    /// </summary>
    public partial class UserChatPanel : Window
    {
        ClientLogic Client;
        MainWindow MainWindow;
        public UserChatPanel(string IP, string Login, MainWindow MainWindow)
        {
            InitializeComponent();
            this.MainWindow = MainWindow;
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Client = new ClientLogic(Login, socket, IP, this.Display, this.DisplayUsers);
            MessageInput.Focus();
            Client.Receive(Client.isWroking.Token);
            Client.Send($"{Login} подключился к чятику ^_^ /connect_username= {Login}");
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(MessageInput.Text))
                Client.Send(MessageInput.Text + " /username= " + Client.Login);
            MessageInput.Focus();
            MessageInput.Text = null;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Client.Send($"/exit /username= {Client.Login}");
            Environment.Exit(0);
        }

    }
}