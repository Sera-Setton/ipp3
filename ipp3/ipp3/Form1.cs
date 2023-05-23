using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ipp3
{
    public partial class Form1 : Form
    {
        private const int Port = 8;
        private const string ServerIP = "127.0.0.1";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();

            try
            {
                client.Connect(ServerIP, Port);
                NetworkStream networkStream = client.GetStream();

                string message = textBox1.Text;

                byte[] data = Encoding.ASCII.GetBytes(message);
                networkStream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = networkStream.Read(buffer, 0, buffer.Length);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                MessageBox.Show(response);

                networkStream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при з'єднанні з сервером: " + ex.Message);
            }
        }
    }
}
