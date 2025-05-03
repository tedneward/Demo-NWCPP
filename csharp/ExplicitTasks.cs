using System.Threading;
using System.Threading.Tasks;

public class ExplicitTasks {
    public static async Task Go(Counter c) {
        const int THREADS = 100;
        const int INCS_PER_THREAD = 1000;

        List<Task> tasks = new List<Task>();
        for (var i = 0; i<THREADS; i++) {
            tasks.Add(Task.Run(() => {
                for (int j=0; j<INCS_PER_THREAD; j++) {
                    c.Increment();
                }
            }));
        }

        // ... and now, we wait
        await Task.WhenAll(tasks);
    }
}