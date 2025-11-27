using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace txat_aurreratua
{
    public partial class MainWindow : Window
    {
        private const string ipadress = "127.0.0.1";
        private static object lockObj = new object();
        private const int portua = 5000;
        private static List<BezeroObjetua> bezeroak = new();
        private static TcpListener server = null;

        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btn_martxan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task.Run(async () => await hasiZerbitzaria());
                server_log_textblock.Text += "Zerbitzaria martxan jartzen ari da... \n";
                btn_martxan.IsEnabled = false;
            }
            catch (Exception)
            {
                if (server != null)
                {
                    server.Stop();
                    server_log_textblock.Text += "Errorea: Zerbitzaria itxi da. \n";
                }
            }
            
        }

        private void btn_itzali_Click(object sender, RoutedEventArgs e)
        {
            itxiZerbitzaria();
        }

        ////////////Zerbitzariaren funtzioak////////////////
        public async Task hasiZerbitzaria()
        {
            server = new TcpListener(IPAddress.Parse(ipadress), portua);
            server.Start();
            server_log_textblock.Dispatcher.BeginInvoke(() =>
                server_log_textblock.Text += "Zerbitzaria martxan dago. \n");
            btn_itzali.Dispatcher.BeginInvoke(() => btn_itzali.IsEnabled = true);

            while (true)
            {
                TcpClient c;
                try
                {
                    c = await server.AcceptTcpClientAsync();
                }
                catch (SocketException)
                {
                    server_log_textblock.Dispatcher.BeginInvoke(() =>
                        server_log_textblock.Text += "Zerbitzaria gelditu da. \n");
                    break;
                }

                lock (lockObj)
                {
                    if (bezeroak.Count >= 5)
                    {
                        c.Close();
                        server_log_textblock.Dispatcher.BeginInvoke(() =>
                            server_log_textblock.Text += "Bezero limitea iritsita, ezin da gehiago onartu");
                        continue;
                    }
                }

                _ = Task.Run(() => BezeroarekinGauzakEgin(c));
            }
        }
        public void itxiZerbitzaria()
        {
            server.Stop(); // honek hasiZerbitzariaren catch blokea exekutatuko du
            btn_martxan.IsEnabled = true;
            btn_itzali.IsEnabled = false;
        }

        public async Task BezeroarekinGauzakEgin(TcpClient bezeroarenTCP)
        {
            using NetworkStream ns = bezeroarenTCP.GetStream();
            using StreamWriter sw = new StreamWriter(ns) { AutoFlush = true };
            using StreamReader sr = new StreamReader(ns);

            string izena = await sr.ReadLineAsync();
            BezeroObjetua bezeroa = new BezeroObjetua(izena, bezeroarenTCP);
            lock (lockObj) bezeroak.Add(bezeroa);

            string mezua = $"{bezeroa.izena} konektatu da zerbitzarira";
            await sw.WriteLineAsync(mezua);
            chat_log_textblock.Dispatcher.BeginInvoke(() =>
                        server_log_textblock.Text += $"{mezua} \n");

            try
            {
                while (true)
                {
                    string jasotakoMezua = await sr.ReadLineAsync();
                    if (jasotakoMezua == null) throw new IOException("Client disconnected");

                    chat_log_textblock.Dispatcher.BeginInvoke(() =>
                        chat_log_textblock.Text += $"{bezeroa.izena} mezua: {jasotakoMezua} \n");

                    await sw.WriteLineAsync("Proba");
                }
            }
            catch (Exception)
            {
                ns.Close();
                bezeroarenTCP.Close();

                server_log_textblock.Dispatcher.BeginInvoke(() =>
                    server_log_textblock.Text += $"{bezeroa.izena} deskonektatu da \n");

                lock (lockObj) bezeroak.Remove(bezeroa);
            }
        }

        private void server_log_textblock_TextChanged(object sender, TextChangedEventArgs e)
        {
            server_log_textblock.ScrollToEnd();
        }

        private void chat_log_textblock_TextChanged(object sender, TextChangedEventArgs e)
        {
            chat_log_textblock.ScrollToEnd();
        }
    }
}