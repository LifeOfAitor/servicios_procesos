using System;
using System.IO.Pipes;

class ClientClass
{
    public static void Main(String[] args)
    {
        var cliente = new NamedPipeClientStream("pipeando");
        cliente.Connect();

        StreamReader sr = new StreamReader(cliente);
        StreamWriter sw = new StreamWriter(cliente);

        String input;
        do
        {
            Console.WriteLine("Operacion a realizar:");
            Console.WriteLine("1 - suma");
            Console.WriteLine("2 - resta");
            Console.WriteLine("3 - multiplicar");
            Console.WriteLine("4 - dividir");
            Console.WriteLine("5 - potencia");
            Console.WriteLine("-1 - salir");
            Console.Write("Introduce la operacion:");

            input = Console.ReadLine();

            switch (input)
            {
                case "-1":
                    break;
                case "1":
                    var(x, y) = conseguir_operadores(Double.Parse(input));
                    sw.WriteLine("1 "+x+" "+y);//le mandamos al servidor la operacion
                    sw.Flush();
                    break;
                case "2":
                    (x, y) = conseguir_operadores(Double.Parse(input));
                    sw.WriteLine("2 "+x+ " " + y);
                    sw.Flush();
                    break;
                case "3":
                    (x, y) = conseguir_operadores(Double.Parse(input));
                    sw.WriteLine("3 "+x+ " " + y);
                    sw.Flush();
                    break;
                case "4":
                    (x, y) = conseguir_operadores(Double.Parse(input));
                    sw.WriteLine("4 "+x+ " " + y);
                    sw.Flush();
                    break;
                case "5":
                    (x, y) = conseguir_operadores(Double.Parse(input));
                    sw.WriteLine("5 "+x+ " " + y);
                    sw.Flush();
                    break;
                default:
                    Console.WriteLine("Introduce un valor válido");
                    break;
            }

        } while (input != "-1");
    }
    static (double, double) conseguir_operadores(double operacion)
    {
        Double primero;
        Double segundo;
        do
        {
            Console.WriteLine("Introduce el primer operando");
            primero = Double.Parse(Console.ReadLine());
            Console.WriteLine("Introduce el segundo operando");
            segundo = Double.Parse(Console.ReadLine());
        } while ((primero == null) || (segundo == null));

        return (primero, segundo);
    }
}
