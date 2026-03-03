using System;

namespace Nascondendo_Uova;

public class Rabbit
{
    string name;
    Manager manager;
    public List<Egg> lawn;
    bool first;
    public Rabbit(string name_, Manager manager_) { name = name_; manager = manager_; first = true; lawn = new List<Egg>(); }
    public async Task GetEgg()
    {
        await Task.Delay(500);
        await manager.mutexToColorQueue.WaitAsync();
        try 
        { 
            Egg currentEgg = null;
            await manager.mutexQueue.WaitAsync();
            try
            {
                if (manager.eggs.Count > 0)
                {
                    currentEgg = manager.eggs.Dequeue();
                }
            }
            finally { manager.mutexQueue.Release(); }

            if (currentEgg == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ANSELMO] There aren't eggs.");
                Console.ForegroundColor = ConsoleColor.White;
                return;
            }

            if (first) 
            { 
                lawn.Add(currentEgg); 
                first = false; 
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[ANSELMO] Hidden the first egg.");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (Matches(currentEgg, lawn[^1])) 
            { 
                lawn.Add(currentEgg);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("[ANSELMO] Hidden an egg.");
                Console.ForegroundColor = ConsoleColor.White;
            } 
            else 
            { 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ANSELMO] Can't hide this egg. Sent back to be recolored.");
                Console.ForegroundColor = ConsoleColor.White;
                manager.toColor.Enqueue(currentEgg); 
            }    
        }
        finally { manager.mutexToColorQueue.Release(); }
    }

    public bool Matches(Egg newEgg, Egg lastEgg)
    {
        return newEgg.colors.Item1 == lastEgg.colors.Item1 ||
               newEgg.colors.Item1 == lastEgg.colors.Item2 ||
               newEgg.colors.Item2 == lastEgg.colors.Item1 ||
               newEgg.colors.Item2 == lastEgg.colors.Item2;
    }
}
