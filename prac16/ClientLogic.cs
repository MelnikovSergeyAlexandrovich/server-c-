using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace prac16
{
    internal class ClientLogic
    {
        public ListBox Box;
        public ListBox userBox;
        public CancellationTokenSource isWroking;
        public string Login;
        public Socket server;
        public ClientLogic(string Login, Socket server, string IP, ListBox box, ListBox userBox)
        {
            this.Box = box;
            this.userBox = userBox;
            this.Login = Login;
            this.server = server;
            server.Connect(IP, 8888);
            isWroking = new CancellationTokenSource();
        }
        public async Task Receive(CancellationToken Token)
        {
            while (!Token.IsCancellationRequested)
            {
                byte[] bytes = new byte[1024];
                await server.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
                string username = "";
                // Если в сообщении есть /connect_username то тут он убирается
                if (message.Contains("/connect_username"))
                {
                    username = message.Substring(message.LastIndexOf("/connect_username"));
                    username = username.Remove(0, 19);
                    message = message.Remove(message.LastIndexOf("/connect_username"));
                    userBox.Items.Add($"[{username}]");
                }
                if (message.Contains("/exit /username= ") || message == ("/disconnect"))
                {
                    username = message.Substring(message.LastIndexOf("/username= "));
                    message = DateTime.Now.ToString("HH:mm") + "\t" + username + "\n" + " покинул чятик ;-(";
                    userBox.Items.Remove($"[{username}]");
                    isWroking.Cancel();
                }
                Box.Items.Add(message);
            }
        }
        public async Task Send(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await server.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }
    }
}
