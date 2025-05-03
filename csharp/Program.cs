Console.WriteLine("Hello, threaded world!");

static void Go(Action<Counter> goFunc) {
    var name = goFunc.Method.DeclaringType?.Name;
    var counter = new LockedCounter();
    Console.WriteLine("{0}... go!", name);
    Console.WriteLine("{1}: Counter is {0}", counter.Count, name);
    goFunc(counter);
    Console.WriteLine("{1}: Counter is {0}", counter.Count, name);
}
static async void AGo(Func<Counter, Task> goFunc) {
    var name = goFunc.Method.DeclaringType?.Name;
    var counter = new LockedCounter();
    Console.WriteLine("{0}... go!", name);
    Console.WriteLine("{1}: Counter is {0}", counter.Count, name);
    await goFunc(counter);
    Console.WriteLine("{1}: Counter is {0}", counter.Count, name);
}

Go(RawThreads.Go);
Go(ThreadPooling.Go);
AGo(ExplicitTasks.Go); // this one will interleave with the one that follows
Go(ImplicitTasks.Go);

Console.WriteLine("Explicit IL assembly... go!");
Console.WriteLine("Hah, just kidding. This concludes our demo");

