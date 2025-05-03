public class ImplicitTasks {
    public static void Go(Counter c) {
        const int THREADS = 100;
        const int INCS_PER_THREAD = 1000;

        Parallel.For(0, THREADS, i => {
            for (int j=0; j<INCS_PER_THREAD; j++) {
                c.Increment();
            }
        });
    }
}
