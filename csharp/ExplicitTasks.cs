using System.Threading;
using System.Threading.Tasks;

public class ExplicitTasks {
    public static async Task Go() {
        const int THREADS = 100;
        const int INCS_PER_THREAD = 1000;

        Counter c = new Counter();
        Console.WriteLine("Counter is {0}", c.Count);

        List<Task> tasks = new List<Task>();

        // define each task and add it to the list
        for (var i = 0; i<THREADS; i++) {
            tasks.Add(Task.Run(() => {
                for (int j=0; j<INCS_PER_THREAD; j++) {
                    c.Increment();
                }
            }));
        }

        // ... and now, we wait
        await Task.WhenAll(tasks);

        Console.WriteLine("Counter is {0}", c.Count);
    }
}