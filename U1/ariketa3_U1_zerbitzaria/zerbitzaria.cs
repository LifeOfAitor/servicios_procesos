
using System;
using System.IO.Pipes;

public class Zerbitzaria
{
    public static void Main(String[] args)
    {
        try
        {
            var zerbitzaria = new NamedPipeServerStream("pipe_ariketa3");

            Console.WriteLine("Konexioa itxaroten");

            zerbitzaria.WaitForConnection();

            Console.WriteLine("Konexioa eginda: " + DateTime.Now.ToShortTimeString());

            var read = new StreamReader(zerbitzaria);
            var write = new StreamWriter(zerbitzaria);
            while (true)
            {
                String katea = read.ReadLine();
                if (katea == null)
                {
                    Console.WriteLine("Bezeroak programa itxi du");
                    Task.Delay(5000).Wait();
                    read.Close();
                    write.Close();
                    zerbitzaria.Close();
                    break;
                }
                //Figura eta aldeak lortu
                String[] figura = katea.Split(" ");

                //azalera kalkulatu
                Double emaitza = eragiketa_egin(figura[0], Double.Parse(figura[1]), Double.Parse(figura[2]));
                Console.WriteLine("Bezeroarentzako emaitza: " + emaitza);
                write.WriteLine("Eragiketaren emaitza: "+emaitza);
                write.Flush();
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        
    }

    public static Double eragiketa_egin(String figura, Double lado1, Double lado2)
    {
        Double azalera = 0;
        switch (figura)
        {
            case "triangelua":
                Console.WriteLine("Egingo den figura: " + figura + " aldea: "+ lado1);
                azalera = (Math.Sqrt(3) / 4) * lado1 * lado1;
                break;
            case "karratua":
                Console.WriteLine("Egingo den figura: " + figura + " aldea: " + lado1);
                azalera = lado1 * lado1;
                break;
            case "laukizuzena":
                Console.WriteLine("Egingo den figura: " + figura + " aldeak: " + lado1+ " eta "+ lado2);
                azalera = lado1 * lado2;
                break;
            case "pentagonoa":
                Console.WriteLine("Egingo den figura: " + figura + " aldea: " + lado1);
                azalera = (5.0 / 4.0) * lado1 * lado1 * (1 / Math.Tan(Math.PI / 5));
                break;
            default:
                Console.WriteLine("Ezin da figura hori kalkulatu");
                azalera = 0;
                break;
        }
        return azalera;
    }
}