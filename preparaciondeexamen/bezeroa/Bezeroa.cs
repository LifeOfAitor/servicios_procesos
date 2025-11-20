using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace examen_prestatu_zerbitzaria
{
    public class Bezeroa
    {
        private const string ipadress = "127.0.0.1";
        private const int portua = 5000;
        private static NetworkStream ns = null;
        private static StreamReader sr = null;
        private static StreamWriter sw = null;
        private static TcpClient client = null;

        public static void Main()
        {
            zerbitzariraKonektatu();
            try
            {
                while (true)
                {
                    Console.WriteLine("Zerbitzariari bidaltzeko: ");
                    string bidaltzeko;
                    bidaltzeko = Console.ReadLine();
                    while (string.IsNullOrEmpty(bidaltzeko))
                    {
                        bidaltzeko = Console.ReadLine();
                    }
                    sw.WriteLine(bidaltzeko); // zerbitzariai bidali mezua

                    //zerbitzaritik jasotzeko mezua
                    Console.WriteLine();
                    string jasotakoMezua = sr.ReadLine();
                    Console.WriteLine($"Zerbitzariaren mezua: {jasotakoMezua}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void zerbitzariraKonektatu()
        {
            //izena ezarri
            Console.WriteLine("Sartu izena konektatzeko:");
            string izena = Console.ReadLine();

            while (string.IsNullOrEmpty(izena))
            {
                izena = Console.ReadLine();
            }
            try
            {
                client = new TcpClient(ipadress, portua);
                ns = client.GetStream();
                sr = new StreamReader(ns, Encoding.UTF8);
                sw = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };

                //bidali mezua zerbitzarira
                sw.WriteLine(izena);

                string mezua = sr.ReadLine();
                Console.WriteLine(mezua);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                itxi();
            }
            
        }

        public static void itxi()
        {
            ns.Close();
            sw.Close();
            sr.Close();
            client.Close();
        }
    }
}
