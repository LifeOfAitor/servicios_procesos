using System;
using System.IO.Pipes;
using System.Runtime.InteropServices;

class ClientClass
{
    public static void Main(String[] args)
    {
        try
        {
            //egin serbidorearekin konexioa
            var cliente = new NamedPipeClientStream("pipeando");
            cliente.Connect();

            //reader eta writerrak sortu komunukatzeko
            var sr = new StreamReader(cliente);
            var sw = new StreamWriter(cliente);

            String input;
            while (true)
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
                double x = 0;
                double y = 0;

                switch (input)
                {
                    case "-1":
                        Console.WriteLine("Saliendo del programa");
                        break;
                    case "1":
                        (x, y) = conseguir_operadores();
                        sw.WriteLine("+ " + x + " " + y);//zerbitzariari bidali eragiketa
                        sw.Flush();
                        break;
                    case "2":
                        (x, y) = conseguir_operadores();
                        sw.WriteLine("- " + x + " " + y);
                        sw.Flush();
                        break;
                    case "3":
                        (x, y) = conseguir_operadores();
                        sw.WriteLine("* " + x + " " + y);
                        sw.Flush();
                        break;
                    case "4":
                        (x, y) = conseguir_operadores();
                        sw.WriteLine("/ " + x + " " + y);
                        sw.Flush();
                        break;
                    case "5":
                        (x, y) = conseguir_operadores();
                        sw.WriteLine("^ " + x + " " + y);
                        sw.Flush();
                        break;
                    default:
                        Console.WriteLine("Introduce un valor válido");
                        continue;
                }
                //behin eragiketa lortuta
                Console.WriteLine("Resultado de la operacion: " + sr.ReadLine());

            }
        }catch (Exception ex) 
        {
            Console.WriteLine(ex.ToString()); 
        }
    }
    public static (double, double) conseguir_operadores()
    {
        Double primero;
        Double segundo;
        do
        {
            Console.Write("Introduce el primer operando: ");
            primero = Double.Parse(Console.ReadLine());
            Console.Write("Introduce el segundo operando: ");
            segundo = Double.Parse(Console.ReadLine());
        } while ((primero == null) || (segundo == null));

        return (primero, segundo);
    }
}
