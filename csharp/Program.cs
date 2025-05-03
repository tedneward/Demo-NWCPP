Console.WriteLine("Hello, World!");

Console.WriteLine("RawThreads... go!");
RawThreads.Go();
Console.WriteLine("ThreadPool... go!");
ThreadPooling.Go();
Console.WriteLine("ExplicitTasks... go!");
await ExplicitTasks.Go();

Console.WriteLine("Explicit IL assembly... go!");
Console.WriteLine("Hah, just kidding.");
