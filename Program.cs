using System;
using System.Threading;

class Filosofo
{
    private int id;
    private SemaphoreSlim garfoEsquerdo;
    private SemaphoreSlim garfoDireito;
    private SemaphoreSlim mutex;
    private Random rand = new Random();
    //faça um getter/setter nessa linguagem 

    public Filosofo(int id, SemaphoreSlim garfoEsquerdo, SemaphoreSlim garfoDireito, SemaphoreSlim mutex)
    {
        this.id = id;
        this.garfoEsquerdo = garfoEsquerdo;
        this.garfoDireito = garfoDireito;
        this.mutex = mutex;
    }

    public void Viver()
    {
        while (true)
        {
            Pensar();

            mutex.Wait(); 

            garfoEsquerdo.Wait();
            garfoDireito.Wait();

            Comer();

            garfoDireito.Release();
            garfoEsquerdo.Release();

            mutex.Release();
        }
    }

    public void Start()
    {
        new Thread(Viver).Start();
    }

    private void Pensar()
    {
        Console.WriteLine($"Filósofo {id + 1} está pensando.");
        Thread.Sleep(rand.Next(1000, 2000));
    }

    private void Comer()
    {
        Console.WriteLine($"Filósofo {id + 1} está comendo.");
        Thread.Sleep(rand.Next(1000, 2000));
        Console.WriteLine($"Filósofo {id + 1} terminou de comer.");
    }
}

class Program
{
    static void Main()
    {
        int numFilosofos = 5;
        SemaphoreSlim[] garfos = new SemaphoreSlim[numFilosofos];
        Filosofo[] filosofos = new Filosofo[numFilosofos];
        SemaphoreSlim mutex = new SemaphoreSlim(numFilosofos - 1); 

        for (int i = 0; i < numFilosofos; i++)
        {
            garfos[i] = new SemaphoreSlim(1, 1);
        }

        for (int i = 0; i < numFilosofos; i++)
        {
            SemaphoreSlim garfoEsquerdo = garfos[i];
            SemaphoreSlim garfoDireito = garfos[(i + 1) % numFilosofos];
            filosofos[i] = new Filosofo(i, garfoEsquerdo, garfoDireito, mutex);
            filosofos[i].Start();
        }

        Console.ReadLine(); 
}
}
