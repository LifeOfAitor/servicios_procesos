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
        private object lockObj = new object();
        private const int portua = 5000;
        private List<BezeroObjetua> bezeroak = new();
        private TcpListener server = null;
        private bool zerbitzariaMartxan = false;


        public MainWindow()
        {
            InitializeComponent();
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
            zerbitzariaMartxan = true;

            idatziServerMezuak("Zerbitzaria martxan dago. \n");

            btn_itzali.Dispatcher.Invoke(() => btn_itzali.IsEnabled = true);

            Thread t = new Thread(LehenHaria);
            t.IsBackground = true;
            t.Start();
        }

        private void LehenHaria()
        {
            try
            {
                while (zerbitzariaMartxan)
                {
                    TcpClient c = server.AcceptTcpClient();
                    lock (lockObj)
                    {
                        if (bezeroak.Count >= 5)
                        {
                            c.Close();
                            idatziServerMezuak("Bezero limitea iritsita, ezin da gehiago onartu \n");
                            continue;
                        }
                    }
                    new Thread(() => BezeroarekinGauzakEgin(c)).Start();
                }
            }
            catch (SocketException)
            {
                // Normala da Stop() deitzen dugunean
                idatziServerMezuak("Zerbitzaria gelditu da \n");
            }
        }
        public void itxiZerbitzaria()
        {
            server.Stop();
            zerbitzariaMartxan = false;
            btn_martxan.IsEnabled = true;
            btn_itzali.IsEnabled = false;
        }

        public void BezeroarekinGauzakEgin(TcpClient bezeroarenTCP)
        {
            BezeroObjetua bezeroa = new BezeroObjetua(bezeroarenTCP);
            lock (lockObj)
            {
                bezeroak.Add(bezeroa);
            }
            // bezero berria (Objetua) sortu
            string izena = bezeroa.sr.ReadLine();
            bezeroa.setIzena(izena);
            string mezua = $"{bezeroa.izena} konektatu da zerbitzarira";
            bezeroa.sw.WriteLine(mezua);
            idatziServerMezuak(mezua);

            try
            {
                while (zerbitzariaMartxan)
                {
                    //bezerotik jasotzeko mezua
                    string jasotakoMezua = bezeroa.sr.ReadLine();
                    mezua = $"{bezeroa.izena}: {jasotakoMezua}";
                    idatziTxatMezuak(mezua);
                    bezeroeiBidali(mezua);
                }
            }
            catch (Exception ex)
            {
                bezeroa.ns.Close();
                bezeroa.sw.Close();
                bezeroa.sr.Close();
                bezeroarenTCP.Close();
                idatziServerMezuak($"{bezeroa.izena} deskonektatu da");
                lock (lockObj)
                {
                    bezeroak.Remove(bezeroa);
                }
            }
        }

        private void bezeroeiBidali(string mezua)
        {
            foreach (var bezeroa in bezeroak)
            {
                using NetworkStream ns = bezeroa.tcpClient.GetStream();
                bezeroa.sw.WriteLine(mezua);
            }
        }

        private void idatziTxatMezuak(string mezua)
        {
            chat_log_textblock.Dispatcher.Invoke(() =>
            {
                chat_log_textblock.Text += $"{mezua} \n";
            });
        }

        private void idatziServerMezuak(string mezua)
        {
            server_log_textblock.Dispatcher.Invoke(() =>
            {
                server_log_textblock.Text += $"{mezua} \n";
            });
        }

        private void chat_log_textblock_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void list_clientes_textblock_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void server_log_textblock_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}