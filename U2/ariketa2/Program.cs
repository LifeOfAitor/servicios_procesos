using System;
using System.Threading;

public static class ariketa2
{
    public static void Main(string[] args)
    {
        Console.WriteLine("U2-2.ariketa");

        Thread batu = new Thread(new ParameterizedThreadStart(Eragiketak.batuketa));
        Thread biderkatu = new Thread(new ParameterizedThreadStart(Eragiketak.biderketa));
        Thread batu_1 = new Thread(new ParameterizedThreadStart(Eragiketak.batuketa));

        batu.Start(new int[] { 3, 5 });
        
        biderkatu.Start(new int[] { 4, 6 });
        batu.Join();
        biderkatu.Join();
        batu_1.Start(new int[] { 7, 8 });
        batu_1.Join();

        Console.WriteLine("Bukatuta");
    }
}

class Eragiketak
{
    public static void batuketa(object parametro)
    {
        int[] arrayInt = (int[])parametro;
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"{arrayInt[0]} + {arrayInt[1]} = {arrayInt[0] + arrayInt[1]}");
            Thread.Sleep(300);
        }
    }

    public static void biderketa(object parametro)
    {
        int[] arrayInt = (int[])parametro;
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"{arrayInt[0]} * {arrayInt[1]} = {arrayInt[0] * arrayInt[1]}");
            Thread.Sleep(1000);
        }
    }
}
