using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ScreenCaptureTCPClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Process.Start("ScreenCaptureTCPServer.exe");
        }
        private void buttonPicture_Click(object sender, EventArgs e)
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                tcpClient.Connect("192.168.178.34", 11000);

                pictureBox1.Image = new Bitmap(Image.FromStream(tcpClient.GetStream()), pictureBox1.ClientSize);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            finally
            {
                tcpClient.Client.Close();
            }
        }
    }
}
