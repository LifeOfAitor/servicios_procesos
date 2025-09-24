using System.Diagnostics;
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

namespace ariketa1_U1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Process ftpProcess; //para guardar ftp.exe aqui
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ftpBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ftpProcess = new Process();
                ftpProcess.StartInfo.FileName = "ftp.exe";
                ftpProcess.Start();

                lblProcessId.Content = ("Proceso FTP lanzado con ID: " + ftpProcess.Id);
            }
            catch (Exception ex)
            {
                lblProcessId.Content = ("Error al lanzar el proceso: " + ex.Message);
            }
        }

        private void KillProcessBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ftpProcess != null && !ftpProcess.HasExited)
            {
                int id = ftpProcess.Id;
                ftpProcess.Kill();
                ftpProcess.WaitForExit();

                lblProcessId.Content = "Proceso eliminado: " + id;
            }
            else
            {
                lblProcessId.Content = "No hay proceso FTP activo.";
            }
        }

        private void ShowProcess_Click(object sender, RoutedEventArgs e)
        {
            comboProcess.Items.Clear();

            Process[] processes = Process.GetProcesses();

            foreach (Process p in processes)
            {
                comboProcess.Items.Add(p.ProcessName);
            }
        }

    }
}