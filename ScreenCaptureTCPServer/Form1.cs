using System.Net.Sockets;
using System.Net;
using ScreenShotExample;
using System.Drawing.Imaging;

namespace ScreenCaptureTCPServer
{
    public partial class Form1 : Form
    {
        Task? serverTask = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("192.168.178.34"), 11000);
            if (serverTask == null)
            {
                serverTask = Task.Run(() =>
                {
                    try
                    {
                        Text = "Server is working...";
                        tcpListener.Start(5);
                        do
                        {
                            if (tcpListener.Pending())
                            {
                                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                                NetworkStream ns = tcpClient.GetStream();

                                ns.Write(ReceiveScreen());
                                
                                tcpClient.Client.Shutdown(SocketShutdown.Send);
                            }
                        } while (true);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}");
                    }
                    finally
                    {
                        tcpListener.Stop();
                    }
                });
            }
        }

        private byte[] ReceiveScreen()
        {
            ScreenCapture capture = new ScreenCapture();
            Image image = capture.CaptureScreen();

            byte[] buff;

            using(MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                buff = ms.ToArray();
            }

            return buff;
        }
    }
}
