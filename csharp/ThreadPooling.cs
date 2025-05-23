public class ThreadPooling {
    public static void Go(Counter c) {
        const int THREADS = 100;
        const int INCS_PER_THREAD = 1000;

        // We need a "done" signal that each thread will flip
        // when it is done counting
        CountdownEvent signal = new(THREADS);
        for (int i=0; i<THREADS; i++) {
            ThreadPool.QueueUserWorkItem(_ => {
                for (int j=0; j<INCS_PER_THREAD; j++) {
                    c.Increment();
                }
                signal.Signal();
            });
        }

        // Wait for the queued work to all finish
        signal.Wait();
    }
}
