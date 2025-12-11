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
        private bool konektatuta = false;

        public MainWindow()
        {
            InitializeComponent();
            // btn_konektatu.IsEnabled = true;
            // btn_itxi.IsEnabled = false;
            // btn_bidali.IsEnabled = false;
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
                    txt_chat.Text += reply +" \n"; // {izena} konektatu da zerbitzarira
                    konektatuta = true;
                }

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
                        Dispatcher.Invoke(() =>
                        {
                            txt_chat.Text += "Zerbitzaria itxi da. Konexioa galdu da.\n";
                        });
                    }
                    catch (Exception ex)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            txt_chat.Text += $"Konexio errorea: {ex.Message}\n";
                        });
                    }
                    finally
                    {
                        bezeroaItxi();
                    }
                });
                t.IsBackground = true;
                t.Start();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ezin izan da zerbitzarira konektatu: {ex.Message}");
                bezeroaItxi();
            }
        }

        private void btn_bidali_Click(object sender, RoutedEventArgs e)
        {
            if (sw != null && konektatuta)
            {
                string mezua = txt_mensaje.Text;
                if (string.IsNullOrEmpty(mezua)) return;

                // zerbitzariari mezua bidali
                try
                {
                    sw.WriteLine(mezua);
                    txt_mensaje.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errorea bidaltzean: {ex.Message}");
                    bezeroaItxi();
                }
            }
        }

        private void btn_itxi_Click(object sender, RoutedEventArgs e)
        {
            if (konektatuta)
            {
                bezeroaItxi();
            }
        }

        private void bezeroaItxi()
        {
            if (!konektatuta) return;
            
            konektatuta = false;
            
            try
            {
                sw?.Close();
                sr?.Close();
                ns?.Close();
                client?.Close();
                Dispatcher.Invoke(() =>
                {
                    txt_chat.Text += "Zerbitzaritik deskonektatuta \n";
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    txt_chat.Text += "Errorea deskonektatzean: " + ex.Message + "\n";
                });
            }
        }

    }
}