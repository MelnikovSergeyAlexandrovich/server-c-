using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
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

namespace prac16
{
    /// <summary>
    /// Логика взаимодействия для UserChatPanel.xaml
    /// </summary>
    public partial class UserChatPanel : Window
    {
        private Socket server;
        public UserChatPanel(string IP)
        {
            InitializeComponent();

            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.ConnectAsync(IP, 8888);
            Receive();
        }
        private async Task Receive()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await server.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                Display.Items.Add(Encoding.UTF8.GetString(bytes));
                DisplayUsers.Items.Add(server.RemoteEndPoint);
            }
        }
        private async Task Send(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await server.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            Send(MessageInput.Text);
        }
    }
}
