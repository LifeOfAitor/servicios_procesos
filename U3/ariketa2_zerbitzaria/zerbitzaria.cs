using ariketa2_zerbitzaria;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ariketa2_zerbitzaria
{
    public class TCPServidor
    {
        private static bool jokuaMartxan = true;
        private static List<BezeroInformazioa> bezeroak = new List<BezeroInformazioa>();
        private static object lockObject = new object();
        private static int zenbakia;
        public static void Main(string[] args)
        {
            string servidor = "127.0.0.1";
            IPAddress ipserver = IPAddress.Parse(servidor);
            int port = 5000;

            TcpListener listener = new TcpListener(ipserver, port);
            Console.WriteLine("Zerbitzaria martxan dago {0}:{1}", servidor, port);
            listener.Start();

            //jokua sortu eta zenbakia gorde
            Jokua jokua = new Jokua();
            zenbakia = jokua.zenbakia;
            Console.WriteLine("Sortutako zenbakia: " + zenbakia);

            while (true)
            {
                TcpClient socketCliente = listener.AcceptTcpClient();
                lock (lockObject)
                {
                    if (bezeroak.Count >= 1)
                    {
                        Console.WriteLine("Bezero gehiegi konektatuta. Ezin da konektatu.");
                        socketCliente.Close();
                        continue;
                    }
                    Console.WriteLine("Bezero bat konektatuta");
                Thread t = new Thread(() => Datuak(socketCliente));
                t.Start();
                }

                
            }
        }

        private static void Datuak(TcpClient socket)
        {
            BezeroInformazioa bezeroInfo = new BezeroInformazioa
            {
                Socket = socket
            };
            try
            {
                using NetworkStream network = socket.GetStream();
                using StreamWriter writer = new StreamWriter(network) 
                { AutoFlush = true };
                using StreamReader reader = new StreamReader(network);

                string mezua = ("Konexioa eginda, 0-100 arteko zenbakia asmatu behar duzu!");
                writer.WriteLine(mezua);

                lock (lockObject)
                {
                    bezeroak.Add(bezeroInfo);
                }

                patidaJolastu(socket, reader, writer, bezeroInfo);

            }
            catch (Exception e)
            {
                Console.WriteLine("Bezeroarekin errorea: " + e.Message);
            }
            finally
            {
                Console.WriteLine($"{bezeroInfo.Socket} bezeroa deskonektatu da.");
                socket.Close();
                lock (lockObject)
                {
                    bezeroak.Remove(bezeroInfo);
                }
            }
        }

        private static void patidaJolastu(TcpClient socket, StreamReader reader, StreamWriter writer, BezeroInformazioa bezeroInfo)
        {
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null) break;
                if (line.ToUpper().Equals("AMAIERA"))
                {
                    writer.WriteLine("Jokoa amaitu da. Eskerrik asko jolasteagatik!");
                    break;
                }
                if (int.TryParse(line, out int guess))
                {
                    lock (lockObject)
                    {
                        if (!jokuaMartxan)
                        {
                            writer.WriteLine("Jokoa amaitu da. Eskerrik asko jolasteagatik!");
                            break;
                        }
                        bezeroInfo.sahiakerak++;
                        if (guess < zenbakia)
                        {
                            writer.WriteLine("Handiagoa");
                        }
                        else if (guess > zenbakia)
                        {
                            writer.WriteLine("Txikiagoa");
                        }
                        else
                        {
                            jokuaMartxan = false;
                            writer.WriteLine($"Zorionak {bezeroInfo.Socket}! Asmatu duzu {bezeroInfo.sahiakerak} saiakeratan.");
                            NotifikatuBesteak($"{bezeroInfo.Socket} irabazi du jokoa!");
                            ErakutsiEstadistikak();
                            break;
                        }
                    }
                        
                }
            }
        }
        private static void ErakutsiEstadistikak()
        {
            Console.WriteLine("Jokoaren amaiera. Bezeroen estatistikak:");
            foreach (var bezero in bezeroak)
            {
                Console.WriteLine($"{bezero.Socket} - Saiakerak: {bezero.sahiakerak}");
            }
        }
        private static void NotifikatuBesteak(string mezua)
        {
            lock (lockObject)
            {
                foreach (var b in bezeroak)
                {
                    try
                    {
                        StreamWriter w = new StreamWriter(b.Socket.GetStream()) { AutoFlush = true };
                        w.WriteLine(mezua);
                    }
                    catch { }
                }
            }
        }
    }
}