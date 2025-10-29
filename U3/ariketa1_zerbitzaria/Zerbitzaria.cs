using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Zerbitzaria
{
    const int PORT = 5000;

    static void Main()
    {
        TcpListener listener = null;

        try
        {
            listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            Console.WriteLine("-----ZERBITZARIA-----");
            Console.WriteLine($"Zerbitzaria entzuten dago: port {PORT}");

            while (true)
            {
                TcpClient client = null;
                NetworkStream ns = null;
                StreamReader reader = null;
                StreamWriter writer = null;

                try
                {
                    client = listener.AcceptTcpClient();
                    ns = client.GetStream();
                    reader = new StreamReader(ns, Encoding.UTF8);
                    writer = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true };

                    Console.WriteLine($"Bezero bat konektatu da: {client.Client.RemoteEndPoint}");

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine($"Jasotako mezua: {line}");

                        if (line.ToUpper().Equals("AMAIERA"))
                        {
                            writer.WriteLine("Agur!");
                            break;
                        }
                        else
                        {
                            int vokalak = KalkulatuBokalak(line);
                            writer.WriteLine($"Jasotako mezua: {line} | Vokal kopurua: {vokalak}");
                        }

                            
                    }

                    Console.WriteLine("Bezeroarekin konexioa amaitu da.");
                }
                catch (Exception exClient)
                {
                    Console.WriteLine($"Bezeroaren errorea: {exClient.Message}");
                }
                finally
                {
                    writer?.Close();
                    reader?.Close();
                    ns?.Close();
                    client?.Close();
                    Console.WriteLine("Bezeroaren baliabideak askatu dira.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errorea: {ex.Message}");
        }
        finally
        {
            listener?.Stop();
            Console.WriteLine("Zerbitzaria gelditu da.");
        }
    }

    private static int KalkulatuBokalak(string mezua)
    {
        int vokalKopurua = 0;
        foreach (char karaktere in mezua)
        {
            if ("aeiouAEIOU".Contains(karaktere))
            {
                vokalKopurua++;
            }
        }
        return vokalKopurua;
    }
}
