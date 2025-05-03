using System.Collections.Generic;
using System.Threading;

public class RawThreads {
    public static void Go() {
        const int THREADS = 100;
        const int INCS_PER_THREAD = 1000;

        Counter c = new Counter();
        Console.WriteLine("Counter is {0}", c.Count);

        List<Thread> threads = new List<Thread>();
        for (int i=0; i<THREADS; i++) {
            Thread t = new Thread(() => {
                for (int j=0; j<INCS_PER_THREAD; j++) {
                    c.Increment();
                }
            });
            threads.Add(t);
        }
        foreach (var thread in threads) {
            thread.Start();
        }
        foreach (var thread in threads) {
            thread.Join();
        }

        Console.WriteLine("Counter is {0}", c.Count);
    }
}