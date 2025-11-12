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
        TcpClient client = null;
        NetworkStream ns = null;
        StreamReader reader = null;
        StreamWriter writer = null;

        try
        {
            client = new TcpClient();
            client.Connect(SERVER, PORT);

            ns = client.GetStream();
            reader = new StreamReader(ns, Encoding.UTF8);
            writer = new StreamWriter(ns, Encoding.UTF8);
            writer.AutoFlush = true;

            Console.WriteLine("-----BEZEROA-----");
            Console.WriteLine($"Konektatuta {SERVER}:{PORT}");
            while (true)
            {
                Console.WriteLine("Idatzi zenbaki bat 1-100");
                string input = Console.ReadLine();
                writer.WriteLine(input);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errorea konektatzean: {ex.Message}");
            client?.Close();
            return;
        }
    }
}