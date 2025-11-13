using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

class Bezeroa
{
    const string SERVER = "127.0.0.1";
    const int PORT = 5000;

    static void Main()
    {
        try
        {
            using (TcpClient client = new TcpClient(SERVER, PORT))
            using (NetworkStream ns = client.GetStream())
            using (StreamReader reader = new StreamReader(ns, Encoding.UTF8))
            using (StreamWriter writer = new StreamWriter(ns, Encoding.UTF8) { AutoFlush = true })
            {
                Console.WriteLine("-----BEZEROA-----");
                Console.WriteLine($"Konektatuta {SERVER}:{PORT}");

                // Lehen mezua zerbitzaritik
                Console.WriteLine(reader.ReadLine());

                while (true)
                {
                    Console.Write("Sartu zenbakia (edo 'AMAIERA'): ");
                    string line = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    writer.WriteLine(line);

                    string response = reader.ReadLine();
                    if (response == null)
                    {
                        Console.WriteLine("Zerbitzaria deskonektatu da.");
                        break;
                    }

                    Console.WriteLine("Zerbitzaria: " + response);

                    if (line.ToUpper().Equals("AMAIERA") || response.Contains("irabazi"))
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errorea: " + ex.Message);
        }
    }
}
