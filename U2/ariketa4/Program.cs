using System;
using System.Threading;
using System.Threading.Tasks;

class Edurnezuri
{
    public bool paseatzen;
    public bool zerbitzatuta;

    public void JanariaEman()
    {
        Thread.Sleep(500); // Janaria emateko denbora
    }
    public void paseatu()
    {
        Random random = new Random();
        Console.WriteLine("Edurnezurik paseatzen du.");
        Thread.Sleep(random.Next(1000, 2000));
    }
}

class Ipotxa
{
    public bool eserita;
    public int Id;

    public Ipotxa(int Id)
    {
        this.Id = Id;
        this.eserita = false;
    }
    public void jan()
    {
        Random random = new Random();
        Console.WriteLine($"{Id} jaten ari da.");
        Thread.Sleep(random.Next(1000, 2000));
    }
    public void lanean()
    {
        Random random = new Random();
        Console.WriteLine($"{Id} lanean ari da.");
        Thread.Sleep(random.Next(2000, 4000));
    }
}

class Aulkiak
{
    public static readonly int aulkiaKopurua = 4;
    public static int aulkiakErabiliak = 0;

    public static bool aulkiakLibre()
    {
        return aulkiakErabiliak < aulkiaKopurua;
    }
    public static bool esanEserita()
    {
        return aulkiakErabiliak > 0;
    }

    public static void hartuAulkia()
    {
        aulkiakErabiliak++;
    }

    public static void askatuAulkia()
    {
        aulkiakErabiliak--;
    }

    public static int erabilitaKopurua()
    {
        return aulkiakErabiliak;
    }
}

class ProgramaNagusia
{
    static object lockObject = new object();
    static Edurnezuri edurnezuri = new Edurnezuri();
    static Ipotxa[] ipotxak;

    public static void Main(string[] args)
    {
        ipotxak = new Ipotxa[7];
        for (int i = 0; i < ipotxak.Length; i++)
        {
            ipotxak[i] = new Ipotxa(i + 1);
        }

        Thread edurnezuriThread = new Thread(edurnezuriFuntzioa);
        edurnezuriThread.Start();

        foreach (var ipotxa in ipotxak)
        {
            Thread ipotxaThread = new Thread(ipotxakFuntzioa);
            ipotxaThread.Start(ipotxa);
        }
    }
    static void ipotxakFuntzioa(object obj)
    {
        //egin behar da casting thread-ek object mota eskatzen duelako nahitanahiez
        Ipotxa ipotxa = (Ipotxa)obj;
        while(true)
        {
            //lanean dago
            ipotxa.lanean();
            lock (lockObject)
            {
                //aulki librerik ez egon arte itxaroten egongo da
                while (!Aulkiak.aulkiakLibre())
                {
                    Console.WriteLine($"{ipotxa.Id} itxaroten...");
                    Monitor.Wait(lockObject);
                }
                ipotxa.eserita = true;
                Aulkiak.hartuAulkia();
                Console.WriteLine($"{ipotxa.Id} 'Eserita nago, mesedez eman janaria'");

                //edurnezuriri eserita dagoela esaten dio
                Monitor.PulseAll(lockObject);

                //janaria prestatu arte itxaroten du
                while (!edurnezuri.zerbitzatuta)
                {
                    Monitor.Wait(lockObject);
                }
            }
            //janaria jaten du
            ipotxa.jan();
            lock (lockObject)
            {
                //aulkitik altxatzen da eta aulkia libre dagoela dio
                ipotxa.eserita = false;
                Aulkiak.askatuAulkia();
                Console.WriteLine($"{ipotxa.Id} altxatu da. Aulkiak erabilita: {Aulkiak.erabilitaKopurua()}");

                //Edurnezuriri aulkia libre dagoela esaten dio
                Monitor.PulseAll(lockObject);
            }
        }
    }
    static void edurnezuriFuntzioa()
    {
        while (true)
        {
            lock (lockObject)
            {
                // Paseatzen aulki guztiak hutsik dauden bitartean
                while (Aulkiak.erabilitaKopurua() == 0)
                {
                    edurnezuri.paseatzen = true;
                    edurnezuri.paseatu();
                    Monitor.Wait(lockObject);
                }

                edurnezuri.paseatzen = false;
                Console.WriteLine("Edurnezuri paseatzetik bueltatu da.");

                // Janaria eman eserita dauden ipotxei
                for (int i = 0; i < ipotxak.Length; i++)
                {
                    if (ipotxak[i].eserita)
                    {
                        Console.WriteLine($"Edurnezurik janaria eman dio {ipotxak[i].Id} ipotxari.");
                        edurnezuri.JanariaEman();
                    }
                }

                // Janaria zerbitzatu da guztiei
                edurnezuri.zerbitzatuta = true;
                Console.WriteLine("Janaria zerbitzatuta dago. Jaten hasi zaitezkete.");
                Monitor.PulseAll(lockObject);

                // Itxaron ipotx guztiak altxa arte (aulkiak hutsik)
                while (Aulkiak.erabilitaKopurua() > 0)
                {
                    Monitor.Wait(lockObject);
                }

                if (Aulkiak.erabilitaKopurua() > 0)
                {
                    continue; // Volver al inicio del bucle sin pasear todavía
                }

                Console.WriteLine("Ipotx guztiak altxatu dira. Edurnezuri paseatzera doa berriro.");
                edurnezuri.zerbitzatuta = false;
                Monitor.PulseAll(lockObject);
            }
        }
    }

}
