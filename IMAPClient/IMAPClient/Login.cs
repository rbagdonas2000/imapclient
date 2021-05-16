using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace IMAPClient
{
    public partial class Login : Form
    {
        Socket socket = null;
 
        public Login()
        {
            InitializeComponent();
            passwordText.PasswordChar = '*';
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            var hostname = addressText.Text;
            var port = int.Parse(portText.Text); 
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(hostname);
                foreach (IPAddress address in hostEntry.AddressList)
                {
                    IPEndPoint ipe = new IPEndPoint(address, port);
                    Socket tempSocket =
                        new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    tempSocket.Connect(ipe);

                    if (tempSocket.Connected)
                    {
                        socket = tempSocket;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if(IsSocketConnected(socket)) connLabel.Visible = true;
            }
            catch(Exception ex)
            {
                var error = ex.ToString();
                MessageBox.Show(error, "Connection failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Byte[] bytesReceived = new Byte[10000];
            int bytes = 0;
            string mess = "";
            do
            {
                bytes = socket.Receive(bytesReceived);
                mess += Encoding.ASCII.GetString(bytesReceived, 0, bytes);
            }
            while (bytes > 0);
            commandLine.AppendText(mess + "\r\n");
        }

        bool IsSocketConnected(Socket s) => !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (IsSocketConnected(socket))
            {
                commandLine.AppendText("Still connected\r\n");
                var message = "A1 login " + usernameText.Text + " " + passwordText.Text;
                Byte[] bytesSent = Encoding.ASCII.GetBytes(message);
                socket.Send(bytesSent, bytesSent.Length, 0);


                Byte[] bytesReceived = new Byte[256];
                int bytes = 0;
                string mess = "";
                do
                {
                    bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
                    mess += Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                }
                while (bytes > 0);
                commandLine.AppendText(mess + "\r\n");
            }
            else commandLine.AppendText("Not connected\r\n");
            //Hide();
            //MainScreen main = new MainScreen(socket, usernameText.Text, passwordText.Text);
            //main.Show();
        }
    }
}
