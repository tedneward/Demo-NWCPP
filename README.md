# Demo-NWCPP
Some demos for NW C++ Users Group

The original C++ code used is this:

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

## cpp
I attempted to get the C++ code to compile on my machine; unfortunately, the latest version of GCC I have on my machine (g++ (Ubuntu 13.3.0-6ubuntu2~24.04) 13.3.0) doesn't seem to support the `<print>` header or `std::println` functionality, so I had to make a few adjustments in this directory to compile and run the C++ version.

This project makes use of the world's simplest Makefile, which has three targets: "run", which builds "sample"; "sample", which compiles `sample.cpp`; and "clean`, which deletes the compiled executable. I maybe could've used CMake or some other tools, but hey, simplest possible thing, yeah?

## csharp
This project was created via a `dotnet new console`. You can download all the dependencies for the project by `dotnet restore`, then run the project via `dotnet run`. Two subdirectories will be created, `bin` and `obj`; the compiled binaries will be in `bin`, but are typically not executed directly. (Technically the dotnet CLI will create DLLs, not EXEs, so they're not directly executable--one uses the `dotnet` command to load and run them, just as Java uses `java` to do the same.)

## mojo
This project was created via `magic init --format mojoproject`. That in turn created a hidden `.magic` directory that contains the "environment" for the project. This is updated/downloaded via `magic update`, which downloads and installs a LOT of stuff.

Run the sample code by *either*:

* Dropping into a "magic shell" by running:

    * `magic shell` (which will actually download some stuff on first run)
    * `mojo sample.mojo`
    * `exit` (to exit the shell)

* Or by running:

    * `magic run mojo sample.mojo`

