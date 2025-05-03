class Counter {
    private Int32 count = 0;
    private Object _lock = new Object();

    public int Count {
        get { 
            // Since the CLR guarantees 32-bit atomicity,
            // this is probably unnecessary but their documentation
            // insists on it so why not.
            lock(_lock) { 
                return count; 
            }
        }
    }

    public void Increment() {
        // Because we are doing multi-step operations in the
        // += expression below, we require the lock.
        lock (_lock) {
            count += 1;
        }
    }
}
