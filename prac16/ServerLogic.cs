using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace prac16
{
    internal class ServerLogic
    {
        public Socket socket;
        public List<Socket> clients = new List<Socket>();
        public static List<string> usersnames = new List<string>();
        public ServerLogic(Socket socket)
        {
            this.socket = socket;
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 8888);
            socket.Bind(ipPoint);
            socket.Listen(1000);
        }
        public async Task ListenToClients()
        {
            while (true)
            {
                var client = await this.socket.AcceptAsync();
                this.clients.Add(client);
                this.RecieveMessage(client);
            }
        }
        public async Task RecieveMessage(Socket client)
        {
            while (true)
            {
                byte[] bytes = new byte[1024];
                await client.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                string message = Encoding.UTF8.GetString(bytes);
                string username = "";
                if (message.Contains("/username"))
                {
                    username = message.Substring(message.LastIndexOf("/username"));
                    username = username.Remove(0, 11);
                    message = message.Remove(message.LastIndexOf("/username"));
                    message = DateTime.Now.ToString("HH:mm") + "\t" + username + "\n" + message;
                }
                if (message.Contains("/connect_username"))
                {
                    string messageWithTag = message;
                    username = message.Substring(message.LastIndexOf("/connect_username"));
                    username = username.Remove(0, 19);
                    message = message.Remove(message.LastIndexOf("/connect_username"));
                    usersnames.Add("[username]");
                    message = messageWithTag;
                }
                ClientLogic.messages.Add($"Time of sending:{DateTime.Now.ToString("HH:mm:ss")}\tsenderIP:{client.RemoteEndPoint} \nmessage sended to clients:\n{message}");
                foreach (var item in clients)
                    SendMessage(item, message);
            }

        }
        public async Task SendMessage(Socket client, string message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
        }
    }
}
