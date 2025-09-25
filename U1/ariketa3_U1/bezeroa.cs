
using System.IO.Pipes;

public class Bezeroa
{

    public static void Main(string[] args)
    {
        try
        {
            // pipearekin konektatu
            var bezeroa = new NamedPipeClientStream("pipe_ariketa3");
            bezeroa.Connect();

            Console.WriteLine("Zerbitzarira konexioa eginda. " + DateTime.Now.ToShortTimeString());

            //irakurtzeko eta idazteko
            var read = new StreamReader(bezeroa);
            var write = new StreamWriter(bezeroa);

            mezuaSortu();

            var input = Console.ReadLine();
            while (input != "-1")
            {
                String aldea1;
                Boolean egin = true;
                switch (input)
                {
                    case "1":
                        Console.WriteLine("Erabaki aldearen luzeera");
                        aldea1 = Console.ReadLine();
                        write.WriteLine("karratua " + aldea1 +" " + aldea1);
                        write.Flush();
                        break;
                    case "2":
                        Console.WriteLine("Erabaki aldearen luzeera");
                        String aldea2;
                        aldea1 = Console.ReadLine();
                        Console.WriteLine("Erabaki bigarren aldearen luzeera");
                        aldea2 = Console.ReadLine();
                        write.WriteLine("laukizuzena " + aldea1 + " " + aldea2);
                        write.Flush();
                        break;
                    case "3":
                        Console.WriteLine("Erabaki aldearen luzeera");
                        aldea1 = Console.ReadLine();
                        write.WriteLine("triangelua " + aldea1 + " " + aldea1);
                        write.Flush();
                        break;
                    case "4":
                        Console.WriteLine("Erabaki aldearen luzeera");
                        aldea1 = Console.ReadLine();
                        write.WriteLine("pentagonoa " + aldea1 + " " + aldea1);
                        write.Flush();
                        break;
                    default:
                        Console.WriteLine("Ezin da horrelakorik egin, sartu balio egokua");
                        egin = false;
                        break;
                }
                if (egin)
                {
                    Console.WriteLine(read.ReadLine());
                }
                mezuaSortu();
                input = Console.ReadLine();
            }
            Console.WriteLine("Ateratzen...");
            read.Close();
            write.Close();
            bezeroa.Close();
        }
        
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public static void mezuaSortu()
    {
        Console.WriteLine("Figura ezberdinen azalera kalkulatzen dut");
        Console.WriteLine("Erabaki figura:");
        Console.WriteLine("1 - karratua");
        Console.WriteLine("2 - laukizuzena");
        Console.WriteLine("3 - triangelua");
        Console.WriteLine("4 - pentagonoa");
        Console.WriteLine("-1 - atera");
        Console.WriteLine("Sartu figura: ");
    }
}