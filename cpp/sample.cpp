// Can be run at: https://www.onlinegdb.com/ using C++ 23 Language option
// https://onlinegdb.com/feC8QlqVW

#include <mutex>
#include <iostream> //#include <print>
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

    //std::println(
    //    "Spawning {} threads to increment heap-allocated 'count' {} times each...",
    //    THREADS,
    //    INCS_PER_THREAD
    //);
    std::cout << 
        "Spawning " << THREADS << " threads to increment heap-allocated 'count' " << 
        INCS_PER_THREAD << " times each." << std::endl;

    // Create threads to increment the shared variable
    for (int i : std::ranges::iota_view(0, THREADS)) {
        threads.emplace_back(increment, shared_var, INCS_PER_THREAD);
    }

    // Wait for threads to finish
    for (auto &t : threads) {
        t.join();
    }

    // Output the final value of the shared variable
    // std::println("Expected total count: {}; Actual count: {}",
    //     THREADS * INCS_PER_THREAD,
    //     *shared_var
    // );
    std::cout << "Expected total count: " << (THREADS * INCS_PER_THREAD) << 
        ", Actual count: " << (*shared_var) << std::endl;

    // Free the dynamically allocated memory
    delete shared_var;

    return 0;
}
