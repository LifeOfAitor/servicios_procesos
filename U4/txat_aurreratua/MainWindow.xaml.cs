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
                new Thread(() => hasiZerbitzaria()).Start();
                server_log_textblock.Text += "Zerbitzaria martxan jartzen ari da... \n";
                btn_martxan.IsEnabled = false;
            }
            catch (Exception ex)
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
        public void hasiZerbitzaria()
        {
            server = new TcpListener(IPAddress.Parse(ipadress), portua);
            server.Start();
            server_log_textblock.Dispatcher.Invoke(() => {
                server_log_textblock.Text += "Zerbitzaria martxan dago. \n";
            });
            btn_itzali.Dispatcher.Invoke(() => btn_itzali.IsEnabled = true);
            
            while (true)
            {
                try
                {
                    TcpClient c = server.AcceptTcpClient();
                    lock (lockObj)
                    {
                        if (bezeroak.Count >= 5)
                        {
                            c.Close();
                            server_log_textblock.Dispatcher.Invoke(() =>
                            {
                                server_log_textblock.Text += "Bezero limitea iritsita, ezin da gehiago onartu";
                            });
                            continue;
                        }
                    }
                    new Task(() => BezeroarekinGauzakEgin(c)).Start();
                }
                catch (SocketException ex)
                {
                    // zerbitzaria gelditzen denean
                    server_log_textblock.Dispatcher.Invoke(() =>
                    {
                        server_log_textblock.Text += "Zerbitzaria gelditu da. \n";
                    });
                    break;
                }
            }
        }
        public void itxiZerbitzaria()
        {
            server.Stop(); // honek hasiZerbitzariaren catch blokea exekutatuko du
            btn_martxan.IsEnabled = true;
            btn_itzali.IsEnabled = false;
        }

        public void BezeroarekinGauzakEgin(TcpClient bezeroarenTCP)
        {
            using NetworkStream ns = bezeroarenTCP.GetStream();
            using StreamWriter sw = new StreamWriter(ns) { AutoFlush = true };
            using StreamReader sr = new StreamReader(ns);
            // bezero berria (Objetua) sortu
            string izena = sr.ReadLine();
            BezeroObjetua bezeroa = new BezeroObjetua(izena, bezeroarenTCP);
            lock (lockObj)
            {
                bezeroak.Add(bezeroa);
            }
            string mezua = $"{bezeroa.izena} konektatu da zerbitzarira";
            sw.WriteLine(mezua);
            Console.WriteLine(mezua);

            try
            {
                while (true)
                {
                    //bezerotik jasotzeko mezua
                    string jasotakoMezua = sr.ReadLine();
                    chat_log_textblock.Text += $"{bezeroa.izena} mezua: {jasotakoMezua} \n";
                    sw.WriteLine("Proba"); // bezeroari bidali mezua
                }
            }
            catch (Exception ex)
            {
                ns.Close();
                sw.Close();
                sr.Close();
                bezeroarenTCP.Close();
                server_log_textblock.Text += $"{bezeroa.izena} deskonektatu da \n";
                lock (lockObj)
                {
                    bezeroak.Remove(bezeroa);
                }
            }
        }
    }
}