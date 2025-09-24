using System.IO.Pipes;

class Servidor
{
    static void Main()
    {
        try
        {
            Server();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        
    }

    public static void Server()
    {
        // Creamos el servidor pipe con nombre "pipeando"
        var serverConnect = new NamedPipeServerStream("pipeando");
        
        Console.WriteLine("Servidor esperando conexión...");

        serverConnect.WaitForConnection(); // Esperamos cliente

        Console.WriteLine("Cliente conectado.");

        var sr = new StreamReader(serverConnect);
        var sw = new StreamWriter(serverConnect);
            
        string operacion;

        // Bucle para procesar operaciones mientras el cliente esté conectado
        while (true)
        {
            operacion = sr.ReadLine();
            Console.WriteLine($"Operacion recibida: {operacion}");

            // Parseatu eragiketa
            String[] partes = operacion.Split(' ');

            string operador = partes[0];
            float x = float.Parse(partes[1]);
            float y = float.Parse(partes[2]);
            double resultado = 0;
            bool errorOperacion = false;

            Console.WriteLine("Operacion a realizar: " + x + operador + y);

            // Procesamos la operación
            switch (operador)
            {
                case "+":
                    resultado = x + y;
                    break;
                case "-":
                    resultado = x - y;
                    break;
                case "*":
                    resultado = x * y;
                    break;
                case "/":
                    if (y == 0)
                    {
                        sw.WriteLine("Error: división entre cero.");
                        sw.Flush();
                        errorOperacion = true;
                    }
                    else
                    {
                        resultado = x / y;
                    }
                    break;
                case "^":
                    resultado = Math.Pow(x, y);
                    break;
                default:
                    sw.WriteLine("Error: operador no válido.");
                    sw.Flush();
                    errorOperacion = true;
                    break;
            }

            if (!errorOperacion)
            {
                Console.WriteLine("Resultado a mandar al cliente: " + resultado);
                // Enviamos el resultado al cliente
                sw.WriteLine(resultado);
                sw.Flush();
            }
        }
    }
}



