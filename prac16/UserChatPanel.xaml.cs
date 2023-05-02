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
        public UserChatPanel(string IP, string Login)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Client = new ClientLogic(Login, socket, IP);
            InitializeComponent();
            MessageInput.Focus();
            Client.Receive();
            Client.Send($"{Login} подключился к чятику ^_^ /connect_username= {Login}");
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(MessageInput.Text))
                Client.Send(MessageInput.Text + " /username= " + Client.Login);
            MessageInput.Focus();
            MessageInput.Text = null;
            Thread.Sleep(500);
            Display.ItemsSource = "";
            Display.ItemsSource = ClientLogic.messages;
            DisplayUsers.ItemsSource = "";
            DisplayUsers.ItemsSource = ServerLogic.usersnames;
        }
    }
}