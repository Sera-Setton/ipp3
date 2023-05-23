using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Form1 : Form
    {
        private const int Port = 8;
        private TcpListener listener;
        private Thread listenThread;


        public Form1()
        {
            InitializeComponent();
        }
       
        private void ServerForm_Load(object sender, EventArgs e)
        {
            listener = new TcpListener(IPAddress.Any, Port);
            listenThread = new Thread(new ThreadStart(ListenForClients));
            listenThread.Start();
            UpdateStatusLabel("Сервер запущений. Очікування з'єднання...");
        }

        private void ListenForClients()
        {
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientCommunication));
                clientThread.Start(client);
            }
        }

        private void HandleClientCommunication(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream networkStream = tcpClient.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
            string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            UpdateStatusLabel("Отримано повідомлення від клієнта: " + message);

            // Виконати дії згідно отриманого повідомлення
            // Наприклад, малювати фігуру або виконати іншу операцію

            byte[] response = Encoding.ASCII.GetBytes("Повідомлення отримано на сервері.");
            networkStream.Write(response, 0, response.Length);

            networkStream.Close();
            tcpClient.Close();
        }

        private void UpdateStatusLabel(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatusLabel), message);
            }
            else
            {
                label1.Text = message;
            }
        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            listener.Stop();
            listenThread.Abort();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
