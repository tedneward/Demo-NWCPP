# C# Example
This directory contains examples of how to launch 100 threads, each of which increments a shared integer value by 1 in a loop that runs 1000 times. 

Note that C# here is simply a preferential choice; we could have used F# or even Visual Basic to write any of these, with a change only to syntax. (That is to say, there would be little to no change in semantics.)

## QuickStart
This project was created via a `dotnet new console`. You can download all the dependencies for the project by `dotnet restore`, then run the project via `dotnet run`. Two subdirectories will be created, `bin` and `obj`; the compiled binaries will be in `bin`, but are typically not executed directly. (Technically the dotnet CLI will create DLLs, not EXEs, so they're not directly executable--one uses the `dotnet` command to load and run them, just as Java uses `java` to do the same.)

Assuming you have `dotnet` installed on your machine, you can drop into this directory and run the program with `dotnet run`.

## Explanation
There's several different ways to do concurrency in the .NET platform; this directory contains a single .NET project that in turn is made up of a "host" that calls into one of each of the different implementations, each contained in separate C# files:

### Counter
The `Counter` class is a very simple `lock`-wrapped integer; in fact, we could use some built-in locks (such as the `Monitor.Enter`/`Monitor.Exit` pair if we wanted lower-level control, or the `System.Threading.Mutex` class if we wanted something slightly higher-level) to manage this. In fact, we could even use the `System.Threading.Interlocked` class to do the increment, rather than managing the lock ourselves.

### RawThreads
The code in the `RawThreads.cs` file is the most direct comparison to the C++ code above: It creates an array of 100 `Thread` objects each with an anonymous method to iterate 1000 times, incrementing the shared counter (a `Counter` instance) each time. The array is then launched, and each of the threads is then `Joined` to wait for its completion.

### ThreadPooling
More idiomatically (of C# 1.0, anyway) is the use of the build-in "thread pool" mechanism inside of the CLR. In the `ThreadPooling` file, we see the use of `QueueUserWorkItem` which assignes the lambda for execution by an available thread in the pool, or queues it for the first available thread in the pool. The thread pool is sized on startup, and may grow up to a CPU-based limit. We can change this limit by calling `SetMaxThreads` but generally the CLR tunes this pool's size pretty well.

Because the threads in the thread pool are all background threads by nature, they will not keep the CLR alive on their own, so we `Wait` on a signal for each thread to `Signal` when they are finished with their loop.

### ExplicitTasks
This uses the Task-based Asynchronous Programming (TAP) approach in C#, exemplified by the `Task` type, to abstract away the threading behavior entirely. Although it's not strictly necessary, TAP usually brings in the use of the C# `async` and `await` keywords, which help make it easier to invoke these to run in parallel (even though it doesn't make a whole lot of sense in this simple example). Unfortunately, `async`/`await` also "pollutes" the codebase, requiring their use all up the call stack unless you take extra steps to "block" it higher up. C# folks have basically gotten to the point where they are advocating "everything should be async/await", which I find ludicrous and horrible, but hey, here we are.

### ImplicitTasks
We can also do some "abstract away the Tasks" by using some of the `Parallel` methods. This starts to get into PLINQ, which are the parallel extensions to the LINQ library, and heavily underused (and unknown).

