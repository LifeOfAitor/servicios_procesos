using System.IO.Pipes;

class Servidor
{
    static void Main()
    {
        Server();
    }

    static void Server()
    {
        // Creamos el servidor pipe con nombre "pipeando"
        using (var serverConnect = new NamedPipeServerStream("pipeando"))
        {
            Console.WriteLine("Servidor esperando conexión...");

            serverConnect.WaitForConnection(); // Esperamos cliente

            Console.WriteLine("Cliente conectado.");

            using (var sr = new StreamReader(serverConnect))
            using (var sw = new StreamWriter(serverConnect))
            {
                string operacion;

                // Bucle para procesar operaciones mientras el cliente esté conectado
                while ((operacion = sr.ReadLine()) != null)
                {
                    Console.WriteLine($"Operacion recibida: {operacion}");

                    // Se espera formato: operador x y (ejemplo: "+ 4 5")
                    string[] partes = operacion.Split(' ');

                    if (partes.Length != 3)
                    {
                        sw.WriteLine("Error: formato incorrecto.");
                        sw.Flush();
                        continue;
                    }

                    string operador = partes[0];
                    if (!double.TryParse(partes[1], out double x) || !double.TryParse(partes[2], out double y))
                    {
                        sw.WriteLine("Error: operandos invalidos.");
                        sw.Flush();
                        continue;
                    }

                    double resultado = 0;
                    bool errorOperacion = false;

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
                        // Enviamos el resultado al cliente
                        sw.WriteLine(resultado);
                        sw.Flush();
                    }
                }
            }
        }
    }
}



