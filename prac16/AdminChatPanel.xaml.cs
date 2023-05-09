using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

    public partial class AdminChatPanel : Window
    {
        ServerLogic Server;
        ClientLogic Client;
        MainWindow MainWindow;
        public AdminChatPanel(string Login, MainWindow mainWindow)
        {
            InitializeComponent();
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Server = new ServerLogic(socket, DisplayUsers, Display);

            var socketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Client = new ClientLogic(Login, socketClient, "127.0.0.1", Display, DisplayUsers);

            DisplayUsers.Items.Add($"[{Login}]");
            Server.ListenToClients();
            MainWindow = mainWindow;
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            Send(MessageInput.Text + " /username= " + Client.Login);
            Thread.Sleep(5);
        }
        private async Task Send(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await Client.server.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }
    }
}
