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
                //handle bezeroak
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine($"Bezero bat konektatu da: {client.Client.RemoteEndPoint}");
                Task t = Task.Run(() => HandleClient(client));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errorea zerbitzaria abiaraztean: {ex.Message}");
        }
        finally
        {
            listener?.Stop();
        }
    }
    static void HandleClient(TcpClient client)
    {
        NetworkStream ns = null;
        StreamReader reader = null;
        StreamWriter writer = null;

        try
        {
            ns = client.GetStream();
            reader = new StreamReader(ns, Encoding.UTF8);
            writer = new StreamWriter(ns, Encoding.UTF8);
            writer.AutoFlush = true;

            writer.WriteLine("Kaixo! Zerbitzarira konektatuta.");

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine($"[{client.Client.RemoteEndPoint}] => {line}");
                writer.WriteLine($"Echo: {line}");
            }
        }
        catch (IOException)
        {
            Console.WriteLine($"Bezeroa deskonektatu da: {client.Client.RemoteEndPoint}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errorea bezeroarekin: {ex.Message}");
        }
        finally
        {
            // Cerrar en orden inverso a la apertura
            if (writer != null) writer.Close();
            if (reader != null) reader.Close();
            if (ns != null) ns.Close();
            client.Close();
        }
    }

}
