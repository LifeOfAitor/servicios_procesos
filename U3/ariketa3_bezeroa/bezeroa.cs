using System.Linq.Expressions;
using System.Net.Sockets;
using System.Text;

class Bezeroa
{
    const string SERVER = "127.0.0.1";
    const int PORT = 3000;

    static void Main()
    {
        try
        {
            using (TcpClient client = new TcpClient(SERVER, PORT))
            using (NetworkStream ns = client.GetStream())
            using (StreamReader reader = new StreamReader(ns, Encoding.UTF8))
            using (StreamWriter writer = new StreamWriter(ns, Encoding.UTF8))
            {
                Console.WriteLine("-----BEZEROA-----");
                Console.WriteLine($"Konektatuta {SERVER}:{PORT}");

                //bezeroa konektatuta dagoen bitartean...
                while (true)
                {
                    string jasotakoMezua = null;
                    //menua erakutsi
                    for (int i = 0; i < 7; i++)
                    {
                        jasotakoMezua+= reader.ReadLine();
                    }
                    if (jasotakoMezua == null)
                    {
                        Console.WriteLine("Zerbitzaria deskonektatu da.");
                        break;
                    }

                    Console.WriteLine($"{jasotakoMezua}");
                    string argazkia = Console.ReadLine();
                    // irten nahi badu erabiltzaileak
                    if (argazkia.Equals("5"))
                    {
                        Console.WriteLine("Irteten...");
                        writer.WriteLine(argazkia);
                        writer.Flush();
                        break;
                    }
                    writer.WriteLine(argazkia);
                    writer.Flush();
                    Console.WriteLine("Argazkiaren path: " + jasotakoMezua);


                }
            }
        }

        catch
        {
            Console.WriteLine("Errorea zerbitzariarekin konektatzen");
        }
    }
}
