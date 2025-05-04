# Demo-NWCPP
Some demos for NW C++ Users Group

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

