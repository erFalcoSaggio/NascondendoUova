using System;

namespace Nascondendo_Uova;

public class Rabbit
{
    string name;
    Manager manager;
    List<Egg> lawn;
    bool first;
    public Rabbit(string name_, Manager manager_) { name = name_; manager = manager_; first = true; lawn = new List<Egg>(); }
    public async Task GetEgg()
    {
        await manager.mutexQueue.WaitAsync();
        //siamo al primo
        if (first) { lawn.Add(manager.eggs.Dequeue()); first = !first; }
        //secondo in poi
        await manager.mutexToColorQueue.WaitAsync();
        try
        {
            //if (Matches(manager.eggs.Peek(), lawn[lawn.Count - 1])) { lawn.Add(manager.eggs.Dequeue()); } 
            if (lawn.Count > 0 && Matches(manager.eggs.Peek(), lawn[^1])); //^1 prendo l'ultimo elemento
            //lo metto nella coda dei ToColor
            else { 
                manager.toColor.Enqueue(manager.eggs.Dequeue()); 
            }    
        }
        finally { manager.mutexToColorQueue.Release(); }
        
    }
    public bool Matches(Egg first, Egg second)
{
    return first.colors.Item1 == second.colors.Item1 ||
           first.colors.Item1 == second.colors.Item2;
}
}
