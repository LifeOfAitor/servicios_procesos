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
            Console.WriteLine("Idatzi testua bidaltzeko. 'AMAIERA' idatziz amaitzen da.");

            while (true)
            {
                string line = Console.ReadLine();
                if (line == null) break;

                writer.WriteLine(line);
                string response = reader.ReadLine();
                if (response == null) break;

                Console.WriteLine("Server-etik: " + response);

                if (line.ToUpper().Equals("AMAIERA"))
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errorea: " + ex.Message);
        }

        // Itxi dena eskuz
        if (writer != null) writer.Close();
        if (reader != null) reader.Close();
        if (ns != null) ns.Close();
        if (client != null) client.Close();
    }
}
