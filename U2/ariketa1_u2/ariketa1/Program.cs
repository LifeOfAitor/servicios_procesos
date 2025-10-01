using System;
using System.Threading;
class Ariketa1
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Ariketa1_U2___HASTEN:");
        Thread aimarHaria = new Thread(Threads.aimar);
        Thread nereaHaria = new Thread(Threads.nerea);
        Thread jurgiHaria = new Thread(Threads.jurgi);

        aimarHaria.Start();
        nereaHaria.Start();
        jurgiHaria.Start();
        aimarHaria.Join();
        nereaHaria.Join();
        jurgiHaria.Join();

        Console.WriteLine("Bukatuta");
    }
}
static class Threads { 
    public static void aimar()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Aimar" + i);
            Thread.Sleep(300);
        }
    }
    public static void nerea()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Nerea" + i);
            Thread.Sleep(1000);
        }
    }
    public static void jurgi()
    {
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Jurgi" + i);
            Thread.Sleep(500);
        }
    }
}
