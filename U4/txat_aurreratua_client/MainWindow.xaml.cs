using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace txat_aurreratua_client
{
    public partial class MainWindow : Window
    {
        private const int portua = 5000;
        private NetworkStream? ns = null;
        private StreamReader? sr = null;
        private StreamWriter? sw = null;
        private TcpClient? client = null;

        public MainWindow()
        {
            InitializeComponent();
            btn_konektatu.IsEnabled = true;
            btn_itxi.IsEnabled = false;
            btn_bidali.IsEnabled = false;
        }

        private void btn_konektatu_Click(object sender, RoutedEventArgs e)
        {
            zerbitzariraKonektatu(txt_box_izena.Text.Trim());
        }

        private void zerbitzariraKonektatu(string izena)
        {
            if (string.IsNullOrWhiteSpace(izena))
            {
                MessageBox.Show("Sartu izena mesedez.");
                return;
            }

            try
            {
                //zerbitzarira konektatu
                client = new TcpClient("127.0.0.1", portua);
                ns = client.GetStream();
                sr = new StreamReader(ns, Encoding.UTF8);
                sw = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };

                // izena bidali
                sw.WriteLine(izena);

                // zerbitzariaren mezua irakurri
                var reply = sr.ReadLine();
                if (reply != null)
                {
                    txt_chat.AppendText(reply +" \n");
                }

                // aldatu interfazeko elementuen egoera
                btn_konektatu.IsEnabled = false;
                btn_itxi.IsEnabled = true;
                btn_bidali.IsEnabled = true;
                txt_box_izena.IsEnabled = false;
                txt_box_IP.IsEnabled = false;

                // Haria erantzunak irakurtzeko
                Thread t = new Thread(() =>
                {
                    try
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                txt_chat.Text += line + "\n";
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            txt_chat.Text += "Errorea: " + ex.Message + "\n";
                        });
                    }
                });
                t.IsBackground = true;
                t.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ezin izan da konekxioa egin zerbitzariarekin.");
                bezeroaItxi();
            }
        }

        private void btn_bidali_Click(object sender, RoutedEventArgs e)
        {
            if (sw == null) return;
            var text = txt_mensaje.Text;
            if (string.IsNullOrEmpty(text)) return;

            // zerbitzariari mezua bidali
            try
            {
                sw.WriteLine(text);
                txt_mensaje.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea bidaltzean: {ex.Message}");
            }
        }

        private void btn_itxi_Click(object sender, RoutedEventArgs e)
        {
            bezeroaItxi();
        }

        private void bezeroaItxi()
        {
            sw?.Close();
            sr?.Close();
            ns?.Close();
            client?.Close();

            sw = null;
            sr = null;
            ns = null;
            client = null;

            // aldatu interfazeko elementuen egoera
            btn_konektatu.IsEnabled = true;
            btn_itxi.IsEnabled = false;
            btn_bidali.IsEnabled = false;
            txt_box_izena.IsEnabled = true;
            txt_box_IP.IsEnabled = true;
        }

        private void txt_box_izena_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_konektatu.IsEnabled = !string.IsNullOrWhiteSpace(txt_box_izena.Text);
        }
    }
}