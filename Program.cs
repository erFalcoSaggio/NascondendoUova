using QueueLibrary;
using StackLibrary;
namespace Nascondendo_Uova;

class Program
{
    static async Task Main(string[] args)
    {
        Manager manager = new Manager();
        await manager.SendEgg();
        Console.WriteLine("Eggs!");
    }
}
