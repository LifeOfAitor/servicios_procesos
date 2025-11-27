using System;
using System.IO;
using System.Net.Sockets;
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
        private CancellationTokenSource? receiveCts = null;

        public MainWindow()
        {
            InitializeComponent();
            btn_konektatu.IsEnabled = true;
            btn_itxi.IsEnabled = false;
            btn_bidali.IsEnabled = false;
        }

        private async void btn_konektatu_Click(object sender, RoutedEventArgs e)
        {
            await zerbitzariraKonektatuAsync(txt_box_izena.Text.Trim());
        }

        private async Task zerbitzariraKonektatuAsync(string izena)
        {
            if (string.IsNullOrWhiteSpace(izena))
            {
                MessageBox.Show("Sartu izena mesedez.");
                return;
            }

            try
            {
                //zerbitzarira konektatu
                client = new TcpClient();
                await client.ConnectAsync(txt_box_IP.Text.Trim(), portua);
                ns = client.GetStream();
                sr = new StreamReader(ns, Encoding.UTF8);
                sw = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };

                // izena bidali
                await sw.WriteLineAsync(izena);

                // zerbitzariaren mezua irakurri
                var reply = await sr.ReadLineAsync();
                if (reply != null)
                {
                    txt_chat.AppendText(reply + Environment.NewLine);
                    txt_chat.ScrollToEnd();
                }

                // aldatu interfazeko elementuen egoera
                btn_konektatu.IsEnabled = false;
                btn_itxi.IsEnabled = true;
                btn_bidali.IsEnabled = true;
                txt_box_izena.IsEnabled = false;
                txt_box_IP.IsEnabled = false;

                // start receive loop
                receiveCts = new CancellationTokenSource();
                _ = StartReceiveLoopAsync(receiveCts.Token);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ezin izan da konekxioa egin zerbitzariarekin. Errorea: {ex.Message}");
                Cleanup();
            }
        }

        private async Task StartReceiveLoopAsync(CancellationToken token)
        {
            try
            {
                if (sr == null) return;

                while (!token.IsCancellationRequested)
                {
                    // ReadLineAsync is awaited so it does not block the UI thread
                    string? line = await sr.ReadLineAsync().ConfigureAwait(false);
                    if (line == null) break; // server closed connection

                    // marshal to UI thread if needed (await above used ConfigureAwait(false)),
                    // use Dispatcher to be safe
                    await Dispatcher.InvokeAsync(() =>
                    {
                        txt_chat.AppendText(line + Environment.NewLine);
                        txt_chat.ScrollToEnd();
                    });
                }
            }
            catch (Exception ex)
            {
                await Dispatcher.InvokeAsync(() =>
                {
                    txt_chat.AppendText($"Receive error: {ex.Message}{Environment.NewLine}");
                    txt_chat.ScrollToEnd();
                });
            }
            finally
            {
                // ensure UI updated on disconnect
                await Dispatcher.InvokeAsync(() =>
                {
                    btn_konektatu.IsEnabled = true;
                    btn_itxi.IsEnabled = false;
                    btn_bidali.IsEnabled = false;
                    txt_box_izena.IsEnabled = true;
                    txt_box_IP.IsEnabled = true;
                });

                Cleanup();
            }
        }

        private async void btn_bidali_Click(object sender, RoutedEventArgs e)
        {
            if (sw == null) return;
            var text = txt_chat.Text;
            if (string.IsNullOrEmpty(text)) return;

            try
            {
                await sw.WriteLineAsync(text);
                txt_chat.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea bidaltzean: {ex.Message}");
            }
        }

        private void btn_itxi_Click(object sender, RoutedEventArgs e)
        {
            itxi();
        }

        private void itxi()
        {
            receiveCts?.Cancel();
            Cleanup();
        }

        private void Cleanup()
        {
            try { sw?.Close(); } catch { }
            try { sr?.Close(); } catch { }
            try { ns?.Close(); } catch { }
            try { client?.Close(); } catch { }

            sw = null;
            sr = null;
            ns = null;
            client = null;
            receiveCts?.Dispose();
            receiveCts = null;
        }

        private void txt_box_izena_TextChanged(object sender, TextChangedEventArgs e)
        {
            btn_konektatu.IsEnabled = !string.IsNullOrWhiteSpace(txt_box_izena.Text);
        }
    }
}