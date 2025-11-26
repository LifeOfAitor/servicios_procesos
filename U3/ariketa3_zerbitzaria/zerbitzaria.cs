using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public static class Zerbitzaria
{
    public static object lockObject = new object(); // lock objetua
    public static int bezeroak = 0;

    public static void Main(string[] args)
    {
        string zerbitzaria = "127.0.0.1";
        IPAddress ipserver = IPAddress.Parse(zerbitzaria);
        int port = 3000;

        TcpListener listener = new TcpListener(ipserver, port);
        Console.WriteLine($"Zerbitzaria martxan dago {zerbitzaria}:{port}");
        listener.Start(5);

        // bezeroak kudeatu (max 5)
        while (true)
        {
            TcpClient socketBezeroa = listener.AcceptTcpClient();
            lock (lockObject)
            {
                if (bezeroak >= 5)
                {
                    Console.WriteLine("Bezero gehiegi konektatuta. Ezin da konektatu.");
                    socketBezeroa.Close();
                    continue;
                }
                bezeroak++;
            }

            Console.WriteLine("Bezero bat konektatuta");
            Thread t = new Thread(() => Irudiak(socketBezeroa));
            t.Start();
        }
    }

    private static void Irudiak(TcpClient socketBezeroa)
    {
        try
        {
            using NetworkStream network = socketBezeroa.GetStream();
            using StreamWriter writer = new StreamWriter(network, Encoding.UTF8) { AutoFlush = true };
            using StreamReader reader = new StreamReader(network, Encoding.UTF8);

            string argazkienMezua = """
                Zein argazki nahi duzu? 
                (Erantzun zenbakiarekin)
                1- irudia_1
                2- irudia_2
                3- irudia_3
                4- irudia_4
                5- irten
                """;

            // bidali menua
            writer.WriteLine(argazkienMezua);

            string mezua;
            while ((mezua = reader.ReadLine()) != null)
            {
                if (mezua.Equals("5"))
                {
                    writer.WriteLine("Agur!");
                    break;
                }

                Console.WriteLine("Bezeroak eskatutako argazkia: " + mezua);
                string ruta= mezua switch
                {
                    "1" => @"D:\servicios_procesos\U3\irudiak\irudia_1.jpg",
                    "2" => @"D:\servicios_procesos\U3\irudiak\irudia_2.jpg",
                    "3" => @"D:\servicios_procesos\U3\irudiak\irudia_3.jpg",
                    "4" => @"D:\servicios_procesos\U3\irudiak\irudia_4.jpg",
                    _ => null
                };

                if (ruta != null)
                    writer.WriteLine($"Hemen daukazu irudiaren ruta: {ruta}");
                else
                    writer.WriteLine("Zenbaki bat bidali behar duzu (1–5).");

                // Optional: reprint menu again
                writer.WriteLine(argazkienMezua);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Errorea bezeroarekin: " + e.Message);
        }
        finally
        {
            socketBezeroa.Close();
            lock (lockObject) bezeroak--;
            Console.WriteLine("Bezeroa deskonektatuta.");
        }
    }
}
