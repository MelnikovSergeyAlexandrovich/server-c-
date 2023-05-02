using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace prac16
{
    internal class ClientLogic
    {
        public static List<string> messages = new List<string>();
        public string Login;
        public Socket server;
        public ClientLogic(string Login, Socket server, string IP)
        {
            this.Login = Login;
            this.server = server;
            server.Connect(IP,8888);
        }
        public async Task Receive()
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await server.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
                string username = "";
                // Если в сообщении есть коннект и /юзернейм то тут он убирается
                if (message.Contains("/connect_username"))
                {
                    username = message.Substring(message.LastIndexOf("/connect_username"));
                    username = username.Remove(0, 19);
                    message = message.Remove(message.LastIndexOf("/connect_username"));
                    messages.Add($"[{username}]");
                }
                messages.Add(message);
            }
        }
        public async Task Send(string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await server.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }
    }
}
