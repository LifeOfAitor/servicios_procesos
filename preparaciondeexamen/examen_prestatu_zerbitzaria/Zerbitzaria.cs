using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace examen_prestatu_zerbitzaria
{
    public class Zerbitzaria
    {
        private const string ipadress = "127.0.0.1";
        private static object lockObj = new object();
        private const int portua = 5000;
        private static List<BezeroObjetua> bezeroak = new();
        private static TcpListener server = null;

        public static void Main()
        {
            hasiZerbitzaria();
            while (true)
            {
                TcpClient c = server.AcceptTcpClient();
                lock (lockObj)
                {
                    if (bezeroak.Count >= 5) // bezero limitea superatzean bota egiten du
                    {
                        c.Close();
                        Console.WriteLine("Bezero limitea isitsita, ezin da gehiago onartu");
                        continue;
                    }
                }
                new Task( () => BezeroarekinGauzakEgin(c)).Start();
            }


        }

        public static void hasiZerbitzaria()
        {
            server = new TcpListener(IPAddress.Parse(ipadress), portua);
            server.Start();
            Console.WriteLine("Zerbitzaria martxan dago");
            Console.WriteLine("-----------------------");
        }

        public static void BezeroarekinGauzakEgin(TcpClient bezeroarenTCP)
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
                    Console.WriteLine($"{bezeroa.izena} mezua: {jasotakoMezua}");
                    sw.WriteLine("Proba"); // bezeroari bidali mezua
                }
            }
            catch (Exception ex)
            {
                ns.Close();
                sw.Close();
                sr.Close();
                bezeroarenTCP.Close();
                Console.WriteLine($"{bezeroa.izena} deskonektatu da");
                lock (lockObj)
                {
                    bezeroak.Remove(bezeroa);
                }
            }
        }
    }
}
