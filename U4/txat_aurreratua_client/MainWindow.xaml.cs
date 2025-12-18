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
        private CancellationTokenSource? cancellationTokenSource = null;
        private readonly object lockObj = new object();

        public MainWindow()
        {
            InitializeComponent();
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
                client = new TcpClient("127.0.0.1", portua);
                ns = client.GetStream();
                sr = new StreamReader(ns, Encoding.UTF8);
                sw = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };

                sw.WriteLine(izena);

                var reply = sr.ReadLine();
                if (reply != null)
                {
                    txt_chat.Text += reply + " \n";
                    lock (lockObj)
                    {
                        konektatuta = true;
                    }
                }

                /*
                 * sortuko dugu cancellationTokenSource haria modu seguruan geldiarazteko.
                 * --------SUPER INTERESGARRIA ERRONKARAKO----------
                 *User clicks "Disconnect"
                 *         ↓
                 *bezeroaItxi() calls cancellationTokenSource?.Cancel()
                 *         ↓
                 *The CancellationToken inside IrakurriErantzunak detects the signal
                 *         ↓
                 *The while loop condition fails and the thread exits cleanly
                 */
                cancellationTokenSource = new CancellationTokenSource();
                Thread t = new Thread(() => IrakurriErantzunak(cancellationTokenSource.Token))
                {
                    IsBackground = true
                };
                t.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ezin izan da zerbitzarira konektatu: {ex.Message}");
                bezeroaItxi();
            }
        }

        private void IrakurriErantzunak(CancellationToken cancellationToken)
        {
            try
            {
                string? line;
                while (!cancellationToken.IsCancellationRequested && sr != null && (line = sr.ReadLine()) != "itxi")
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
            catch (IOException)
            {
                Dispatcher.Invoke(() =>
                {
                        txt_chat.Text += "Konexioa galdu da.\n";
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
        }

        private void btn_bidali_Click(object sender, RoutedEventArgs e)
        {
            lock (lockObj)
            {
                if (sw != null && konektatuta)
                {
                    string mezua = txt_mensaje.Text;
                    if (string.IsNullOrEmpty(mezua)) return;

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
        }

        private void btn_itxi_Click(object sender, RoutedEventArgs e)
        {
            bezeroaItxi();
        }

        private void bezeroaItxi()
        {
            lock (lockObj)
            {
                if (!konektatuta) return;
                konektatuta = false;
            }

            try
            {
                cancellationTokenSource?.Cancel();
                sw?.Close();
                sr?.Close();
                ns?.Close();
                client?.Close();
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    txt_chat.Text += "Errorea deskonektatzean: " + ex.Message + "\n";
                });
            }
            finally
            {
                sw?.Dispose();
                sr?.Dispose();
                ns?.Dispose();
                client?.Dispose();

                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
                ns = null;
                sr = null;
                sw = null;
                client = null;

                Dispatcher.Invoke(() =>
                {
                    txt_chat.Text += "Zerbitzaritik deskonektatuta \n";
                });
            }
        }
    }
}