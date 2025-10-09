using System;
using System.Threading;
using System.Threading.Tasks;

class Edurnezuri
{
    public bool paseatzen;
    public bool zerbitzatuta;

    public void JanariaEman()
    {
        Console.WriteLine("Janaria eman diot ipotxari");
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
        Console.WriteLine($"{Id} jaten ari da.");
    }
    public void lanean()
    {
        Console.WriteLine($"{Id} lanean ari da.");
    }
}

static class Aulkiak
{
    public static int aulkiaKopurua = 4;
    public static int aulkiakErabiliak = 0;

    public static bool aulkiakLibre()
    {
        return aulkiakErabiliak < aulkiaKopurua;
    }
}

class ProgramaNagusia
{
    static void Main(string[] args)
    {
        object lockObject = new object();
        Edurnezuri edurnezuri = new Edurnezuri();
        Ipotxa[] ipotxak = {
            new Ipotxa(1), new Ipotxa(2), new Ipotxa(3),
            new Ipotxa(4), new Ipotxa(5), new Ipotxa(6), new Ipotxa(7)
        };

        Console.WriteLine("Simulazioa hasi da.");

        // Task Edurnezuri
        Task.Run(() =>
        {
            while (true)
            {
                lock (lockObject)
                {
                    //paseatzen egongo da aulkiak libre dauden bitartean
                    while (Aulkiak.aulkiakLibre())
                    {
                        edurnezuri.paseatzen = true;
                        Console.WriteLine("Edurnezuri paseatzen");
                        Monitor.Wait(lockObject);
                    }

                    edurnezuri.paseatzen = false;
                    Console.WriteLine("Edurnezuri ez dago paseatzen");

                    // Janaria ematen die eserita dauden ipotxei
                    for (int i = 0; i < Aulkiak.aulkiakErabiliak; i++)
                    {
                        edurnezuri.JanariaEman();
                    }

                    edurnezuri.zerbitzatuta = true;
                    Console.WriteLine("Janaria zerbitzatuta dago");

                    Monitor.PulseAll(lockObject); // Ipotxei janaria prest dagoela esaten die

                    // Jaten bukatzen duten harte itxaroten du
                    while (Aulkiak.aulkiakErabiliak > 0)
                    {
                        Monitor.Wait(lockObject);
                    }

                    edurnezuri.zerbitzatuta = false;
                    Monitor.PulseAll(lockObject);
                }
            }
        });

        // Hilo Ipotxak, bat bakoitzeko
        Task.Run(() =>
        {
            while (true)
            {
                lock (lockObject)
                {
                    // Aulki librea itxaroten du
                    while (!Aulkiak.aulkiakLibre())
                    {
                        Monitor.Wait(lockObject);
                    }

                    // Eseritzen da
                    ipotxak[0].eserita = true;
                    Aulkiak.aulkiakErabiliak++;
                    Console.WriteLine($"Ipotxa {ipotxak[0].Id} eserita.");

                    Monitor.PulseAll(lockObject); // Eserita dagoela esaten dio Edurnezuriri

                    // Janaria zerbitzatuta egon arte itxaroten du
                    while (!edurnezuri.zerbitzatuta)
                    {
                        Monitor.Wait(lockObject);
                    }

                    // Jan
                    ipotxak[0].jan();

                    // altxatu elta lanera doa
                    ipotxak[0].eserita = false;
                    Aulkiak.aulkiakErabiliak--;
                    Console.WriteLine($"Ipotxa {ipotxak[0].Id} altxatu da.");

                    ipotxak[0].lanean();

                    Monitor.PulseAll(lockObject); // aulkia libre dagoela esaten dio Edurnezuriri
                }
            }
        });
        Task.Run(() =>
        {
            while (true)
            {
                lock (lockObject)
                {
                        // Aulki librea itxaroten du
                        while (!Aulkiak.aulkiakLibre())
                        {
                            Monitor.Wait(lockObject);
                        }

                    // Eseritzen da
                        ipotxak[1].eserita = true;
                        Aulkiak.aulkiakErabiliak++;
                        Console.WriteLine($"Ipotxa {ipotxak[1].Id} eserita.");

                        Monitor.PulseAll(lockObject); // Eserita dagoela esaten dio Edurnezuriri

                        // Janaria zerbitzatuta egon arte itxaroten du
                        while (!edurnezuri.zerbitzatuta)
                        {
                            Monitor.Wait(lockObject);
                        }

                        // Jan
                        ipotxak[1].jan();

                        // altxatu elta lanera doa
                        ipotxak[1].eserita = false;
                        Aulkiak.aulkiakErabiliak--;
                        Console.WriteLine($"Ipotxa {ipotxak[1].Id} altxatu da.");

                        ipotxak[1].lanean();

                        Monitor.PulseAll(lockObject); // aulkia libre dagoela esaten dio Edurnezuriri
                }
            }
        });
        Task.Run(() =>
        {
            while (true)
            {
                lock (lockObject)
                {
                        // Aulki librea itxaroten du
                        while (!Aulkiak.aulkiakLibre())
                        {
                            Monitor.Wait(lockObject);
                        }

                    // Eseritzen da
                        ipotxak[2].eserita = true;
                        Aulkiak.aulkiakErabiliak++;
                        Console.WriteLine($"Ipotxa {ipotxak[2].Id} eserita.");

                        Monitor.PulseAll(lockObject); // Eserita dagoela esaten dio Edurnezuriri

                        // Janaria zerbitzatuta egon arte itxaroten du
                        while (!edurnezuri.zerbitzatuta)
                        {
                            Monitor.Wait(lockObject);
                        }

                        // Jan
                        ipotxak[2].jan();

                        // altxatu elta lanera doa
                        ipotxak[2].eserita = false;
                        Aulkiak.aulkiakErabiliak--;
                        Console.WriteLine($"Ipotxa {ipotxak[2].Id} altxatu da.");

                        ipotxak[2].lanean();

                        Monitor.PulseAll(lockObject); // aulkia libre dagoela esaten dio Edurnezuriri
                }
            }
        });
        Task.Run(() =>
        {
            while (true)
            {
                lock (lockObject)
                {
                        // Aulki librea itxaroten du
                        while (!Aulkiak.aulkiakLibre())
                        {
                            Monitor.Wait(lockObject);
                        }

                    // Eseritzen da
                        ipotxak[3].eserita = true;
                        Aulkiak.aulkiakErabiliak++;
                        Console.WriteLine($"Ipotxa {ipotxak[3].Id} eserita.");

                        Monitor.PulseAll(lockObject); // Eserita dagoela esaten dio Edurnezuriri

                        // Janaria zerbitzatuta egon arte itxaroten du
                        while (!edurnezuri.zerbitzatuta)
                        {
                            Monitor.Wait(lockObject);
                        }

                        // Jan
                        ipotxak[3].jan();

                        // altxatu elta lanera doa
                        ipotxak[3].eserita = false;
                        Aulkiak.aulkiakErabiliak--;
                        Console.WriteLine($"Ipotxa {ipotxak[3].Id} altxatu da.");

                        ipotxak[3].lanean();

                        Monitor.PulseAll(lockObject); // aulkia libre dagoela esaten dio Edurnezuriri
                }
            }
        });
        Task.Run(() =>
        {
            while (true)
            {
                lock (lockObject)
                {
                        // Aulki librea itxaroten du
                        while (!Aulkiak.aulkiakLibre())
                        {
                            Monitor.Wait(lockObject);
                        }

                    // Eseritzen da
                        ipotxak[4].eserita = true;
                        Aulkiak.aulkiakErabiliak++;
                        Console.WriteLine($"Ipotxa {ipotxak[4].Id} eserita.");

                        Monitor.PulseAll(lockObject); // Eserita dagoela esaten dio Edurnezuriri

                        // Janaria zerbitzatuta egon arte itxaroten du
                        while (!edurnezuri.zerbitzatuta)
                        {
                            Monitor.Wait(lockObject);
                        }

                        // Jan
                        ipotxak[4].jan();

                        // altxatu elta lanera doa
                        ipotxak[4].eserita = false;
                        Aulkiak.aulkiakErabiliak--;
                        Console.WriteLine($"Ipotxa {ipotxak[4].Id} altxatu da.");

                        ipotxak[4].lanean();

                        Monitor.PulseAll(lockObject); // aulkia libre dagoela esaten dio Edurnezuriri
                }
            }
        });
        Task.Run(() =>
        {
            while (true)
            {
                lock (lockObject)
                {
                        // Aulki librea itxaroten du
                        while (!Aulkiak.aulkiakLibre())
                        {
                            Monitor.Wait(lockObject);
                        }

                        // Eseritzen da
                        ipotxak[5].eserita = true;
                        Aulkiak.aulkiakErabiliak++;
                        Console.WriteLine($"Ipotxa {ipotxak[5].Id} eserita.");

                        Monitor.PulseAll(lockObject); // Eserita dagoela esaten dio Edurnezuriri

                        // Janaria zerbitzatuta egon arte itxaroten du
                        while (!edurnezuri.zerbitzatuta)
                        {
                            Monitor.Wait(lockObject);
                        }

                        // Jan
                        ipotxak[5].jan();

                        // altxatu elta lanera doa
                        ipotxak[5].eserita = false;
                        Aulkiak.aulkiakErabiliak--;
                        Console.WriteLine($"Ipotxa {ipotxak[5].Id} altxatu da.");

                        ipotxak[5].lanean();

                        Monitor.PulseAll(lockObject); // aulkia libre dagoela esaten dio Edurnezuriri
                }
            }
        });
        Task.Run(() =>
        {
            while (true)
            {
                lock (lockObject)
                {
                        // Aulki librea itxaroten du
                        while (!Aulkiak.aulkiakLibre())
                        {
                            Monitor.Wait(lockObject);
                        }

                    // Eseritzen da
                        ipotxak[6].eserita = true;
                        Aulkiak.aulkiakErabiliak++;
                        Console.WriteLine($"Ipotxa {ipotxak[6].Id} eserita.");

                        Monitor.PulseAll(lockObject); // Eserita dagoela esaten dio Edurnezuriri

                        // Janaria zerbitzatuta egon arte itxaroten du
                        while (!edurnezuri.zerbitzatuta)
                        {
                            Monitor.Wait(lockObject);
                        }

                        // Jan
                        ipotxak[6].jan();

                        // altxatu elta lanera doa
                        ipotxak[6].eserita = false;
                        Aulkiak.aulkiakErabiliak--;
                        Console.WriteLine($"Ipotxa {ipotxak[6].Id} altxatu da.");

                        ipotxak[6].lanean();

                        Monitor.PulseAll(lockObject); // aulkia libre dagoela esaten dio Edurnezuriri
                }
            }
        });
        Task.Run(() =>
        {
            while (true)
            {
                lock (lockObject)
                {
                        // Aulki librea itxaroten du
                        while (!Aulkiak.aulkiakLibre())
                        {
                            Monitor.Wait(lockObject);
                        }

                    // Eseritzen da
                        ipotxak[0].eserita = true;
                        Aulkiak.aulkiakErabiliak++;
                        Console.WriteLine($"Ipotxa {ipotxak[0].Id} eserita.");

                        Monitor.PulseAll(lockObject); // Eserita dagoela esaten dio Edurnezuriri

                        // Janaria zerbitzatuta egon arte itxaroten du
                        while (!edurnezuri.zerbitzatuta)
                        {
                            Monitor.Wait(lockObject);
                        }

                        // Jan
                        ipotxak[0].jan();

                        // altxatu elta lanera doa
                        ipotxak[0].eserita = false;
                        Aulkiak.aulkiakErabiliak--;
                        Console.WriteLine($"Ipotxa {ipotxak[0].Id} altxatu da.");

                        ipotxak[0].lanean();

                        Monitor.PulseAll(lockObject); // aulkia libre dagoela esaten dio Edurnezuriri
                }
            }
        });
    }
}
