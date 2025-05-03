# C# Example
This directory contains examples of how to launch 100 threads, each of which increments a shared integer value by 1 in a loop that runs 1000 times. 

Note that C# here is simply a preferential choice; we could have used F# or even Visual Basic to write any of these, with a change only to syntax. (That is to say, there would be little to no change in semantics.)

## QuickStart
Assuming you have `dotnet` installed on your machine, you can drop into this directory and run the program with `dotnet run`.

## Explanation
The code in this directory is designed to demonstrate how C# code compares against the following C++ code:

```
// Can be run at: https://www.onlinegdb.com/ using C++ 23 Language option
// https://onlinegdb.com/feC8QlqVW

#include <mutex>
#include <print>
#include <ranges>
#include <thread>
#include <vector>

// Mutex to protect the shared resource
std::mutex mtx;

// Number of increments per thread
const int INCS_PER_THREAD = 1000;

// Number of threads to run
const int THREADS = 100;

// Increments the dynamically allocated shared variable
void increment(int* shared_var, int increments) {
    for (int i = 0; i < increments; ++i) {
        std::lock_guard<std::mutex> lock(mtx);
        ++(*shared_var);
    }
}

int main() {
    // Dynamically allocate memory for the shared variable
    int* shared_var = new int(0);
    
    // Create a vector to store the thread handles
    std::vector<std::jthread> threads;

    std::println(
        "Spawning {} threads to increment heap-allocated 'count' {} times each...",
        THREADS,
        INCS_PER_THREAD
    );

    // Create threads to increment the shared variable
    for (int i : std::ranges::iota_view(0, THREADS)) {
        threads.emplace_back(increment, shared_var, INCS_PER_THREAD);
    }

    // Wait for threads to finish
    for (auto &t : threads) {
        t.join();
    }

    // Output the final value of the shared variable
    std::println("Expected total count: {}; Actual count: {}",
        THREADS * INCS_PER_THREAD,
        *shared_var
    );

    // Free the dynamically allocated memory
    delete shared_var;

    return 0;
}
```

There's several different ways to do concurrency in the .NET platform; this directory contains a single .NET project that in turn is made up of a "host" that calls into one of each of the different implementations, each contained in separate C# files.

### Counter
The `Counter` class is a very simple `lock`-wrapped integer; in fact, we could use some built-in locks (such as the `Monitor.Enter`/`Monitor.Exit` pair if we wanted lower-level control, or the `System.Threading.Mutex` class if we wanted something slightly higher-level) to manage this. In fact, we could even use the `System.Threading.Interlocked` class to do the increment, rather than managing the lock ourselves.

### RawThreads
The code in the `RawThreads.cs` file is the most direct comparison to the C++ code above: It creates an array of 100 `Thread` objects each with an anonymous method to iterate 1000 times, incrementing the shared counter (a `Counter` instance) each time. The array is then launched, and each of the threads is then `Joined` to wait for its completion.

### ThreadPooling
More idiomatically (of C# 1.0, anyway) is the use of the build-in "thread pool" mechanism inside of the CLR. In the `ThreadPooling` file, we see the use of `QueueUserWorkItem` which assignes the lambda for execution by an available thread in the pool, or queues it for the first available thread in the pool. The thread pool is sized on startup, and may grow up to a CPU-based limit. We can change this limit by calling `SetMaxThreads` but generally the CLR tunes this pool's size pretty well.

Because the threads in the thread pool are all background threads by nature, they will not keep the CLR alive on their own, so we `Wait` on a signal for each thread to `Signal` when they are finished with their loop.

### ExplicitTasks
This uses the Task-based Asynchronous Programming (TAP) approach in C#, exemplified by the `Task` type, to abstract away the threading behavior entirely. Although it's not strictly necessary, TAP usually brings in the use of the C# `async` and `await` keywords, which help make it easier to invoke these to run in parallel (even though it doesn't make a whole lot of sense in this simple example).

### ImplicitTasks
We can also do some "abstract away the Tasks" by using some of the `Parallel` methods.

