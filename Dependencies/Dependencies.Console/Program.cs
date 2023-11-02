using Dependencies.Console;
using Dependencies.Console.Processor;
using Dependencies.Contract.Services;
using Dependencies.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// DI registration
HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddScoped<IDependencyService, DependencyService>();
builder.Services.AddScoped<IDependencyProcessor, DependencyProcessor>();
using IHost host = builder.Build();
using IServiceScope serviceScope = host.Services.CreateScope();

// Reading instructions
Console.WriteLine("Hello, Dependecies!");
Console.WriteLine("Select option: 1 - Provider code by console, 2 - Read code from file");
var option = Console.ReadKey();
Console.WriteLine();

var instructions = new List<string>();
 switch (option.Key)
{
    case ConsoleKey.D1:
    case ConsoleKey.NumPad1:
        ReadConsoleInstructions.Process(instructions);
        break;
    case ConsoleKey.D2:
    case ConsoleKey.NumPad2:
        ReadFileInstructions.Process(instructions);
        break;
    default:
        Console.WriteLine("Wrong option");
        break;
}

// Processing dependencies
var processor = serviceScope.ServiceProvider.GetRequiredService<IDependencyProcessor>();
processor.Process(instructions.ToArray());

