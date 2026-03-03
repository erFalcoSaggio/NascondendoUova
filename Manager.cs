using System;
using System.CodeDom.Compiler;
using System.Drawing;
using System.Security.Cryptography;

namespace Nascondendo_Uova;

public class Manager
{
    public Queue<Egg> eggs;
    public Queue<Egg> toColor;
    public SemaphoreSlim mutexQueue;
    public SemaphoreSlim mutexToColorQueue;
    public Manager()
    {
        eggs = new Queue<Egg>();
        mutexQueue = new SemaphoreSlim(1);
    }
    public async Task SendEgg()
    {
        try
        {
            while (true)
            {
                //prima di generare un nuovo uovo controlli
                await mutexToColorQueue.WaitAsync();
                try
                {
                    if (toColor.Count > 0)
                    {
                        toColor.Peek().colors = (GenerateColor(Random.Shared.Next(6)), GenerateColor(Random.Shared.Next(6)));
                        eggs.Enqueue(toColor.Dequeue());
                    }
                }
                finally
                {
                    mutexToColorQueue.Release();
                }
                await mutexQueue.WaitAsync(); //aspetto per la queue
                Egg egg = new Egg((GenerateColor(Random.Shared.Next(6)), GenerateColor(Random.Shared.Next(6)))); //uovo
                eggs.Enqueue(egg);
            }
        } catch (OperationCanceledException)
        {
            
        }
    }
    private Color GenerateColor(int n)
    {
       switch (n)
        {
            case 0: return Color.Green;
            case 1: return Color.Magenta;
            case 2: return Color.LightBlue;
            case 3: return Color.Yellow;
            case 4: return Color.Orange;
            case 5: return Color.MistyRose;
            case 6: return Color.Violet;
            default: Console.WriteLine("ERROR: Generation Color function."); return Color.White;
        }
    }
}
