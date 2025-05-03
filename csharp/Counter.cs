public interface Counter {
    uint Count { get; }
    void Increment();
}

class UnlockedCounter : Counter
{
    private uint count = 0;

    public uint Count => count;

    public void Increment()
    {
        count += 1;
    }
}

class LockedCounter : Counter {
    private uint count = 0;
    private Object _lock = new Object();

    public uint Count {
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

class InterlockedCounter : Counter {
    private uint count = 0;

    public uint Count => count;

    public void Increment() { Interlocked.Increment(ref count); }
}
