using System.Windows.Forms;
using System.Net.Sockets;

namespace IMAPClient
{
    public partial class MainScreen : Form
    {
        private readonly string _username;
        private readonly string _password;
        private readonly Socket _socket;
        public MainScreen()
        {
            InitializeComponent();
        }
        public MainScreen(Socket socket, string username, string password)
        {
            InitializeComponent();
            _socket = socket;
            _username = username;
            _password = password;
        }
    }
}
