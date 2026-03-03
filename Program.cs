using QueueLibrary;
using StackLibrary;
namespace Nascondendo_Uova;

class Program
{
    static async Task Main(string[] args)
    {
        Manager manager = new Manager();
        Rabbit rabbit = new Rabbit("Anselmo", manager);
        Console.WriteLine("Eggs!");
        using CancellationTokenSource cts = new CancellationTokenSource();
        //task manager
        Task factoryTask = Task.Run(async () => await manager.SendEgg(cts.Token));

        //task Anselmo
        Task rabbitTask = Task.Run(async () =>
        {
            while (rabbit.lawn.Count < 10)
                await rabbit.GetEgg();
            //ferma il manager
            cts.Cancel();
        });
        await Task.WhenAll(factoryTask, rabbitTask);

        Console.WriteLine("[PROGRAM] At least 10 eggs hidden!");
        Console.WriteLine("[PROGRAM] Sequence of hidden eggs:");
        foreach (var egg in rabbit.lawn)
        {
            Console.WriteLine(egg.colors.Item1 + " " + egg.colors.Item2);
        }
    }
}
