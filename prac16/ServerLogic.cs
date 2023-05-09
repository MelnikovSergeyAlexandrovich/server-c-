using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace prac16
{
    internal class ServerLogic
    {
        public ListBox userBox;
        public ListBox box;
        public Socket socket;
        public List<Socket> clients = new List<Socket>();
        public ServerLogic(Socket socket, ListBox userBox, ListBox box)
        {
            this.socket = socket;
            this.userBox = userBox;
            this.box = box;
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
                if (message.Contains("/username") && !message.Contains("/exit"))
                {
                    username = message.Substring(message.LastIndexOf("/username"));
                    username = username.Remove(0, 11);
                    message = message.Remove(message.LastIndexOf("/username"));
                    message = DateTime.Now.ToString("HH:mm") + "\t" + username + "\n" + message;
                }
                if (message.Contains("/exit /username= ") || message == ("/disconnect"))
                {
                    username = message.Substring(message.LastIndexOf("/username= "));
                    message = DateTime.Now.ToString("HH:mm") + "\t" + username + "\n" + " покинул чятик ;-(";
                    userBox.Items.Remove($"[{username}]");
                }
                if (message.Contains("/connect_username"))
                {
                    string messageWithTag = message;
                    username = message.Substring(message.LastIndexOf("/connect_username"));
                    username = username.Remove(0, 19);
                    message = message.Remove(message.LastIndexOf("/connect_username"));
                    userBox.Items.Add($"[{username}]");
                    message = messageWithTag;
                }
                box.Items.Add($"Time of sending:{DateTime.Now.ToString("HH:mm:ss")}\tsenderIP:{client.RemoteEndPoint} \nmessage sended to clients:\n{message}");
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
