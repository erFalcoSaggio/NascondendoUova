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
        toColor = new Queue<Egg>();
        mutexQueue = new SemaphoreSlim(1);
        mutexToColorQueue = new SemaphoreSlim(1);
    }
    public async Task SendEgg(CancellationToken token)
    {
        await Task.Delay(500);
        try
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(500);
                //prima di generare un nuovo uovo controlli
                await mutexToColorQueue.WaitAsync();
                try
                {
                    if (toColor.Count > 0)
                    {
                        Egg oldEgg = toColor.Dequeue();
                        (Color, Color) oldColors = oldEgg.colors;
                        Color color1, color2;
                        do
                        {
                            color1 = GenerateColor(Random.Shared.Next(6));
                            color2 = GenerateColor(Random.Shared.Next(6));
                        }
                        while (
                            color1 == oldColors.Item1 ||
                            color1 == oldColors.Item2 ||
                            color2 == oldColors.Item1 ||
                            color2 == oldColors.Item2
                        );
                        Egg newEgg = new Egg((color1, color2));
                        await mutexQueue.WaitAsync();
                        try { eggs.Enqueue(newEgg); }
                        finally { mutexQueue.Release(); }
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[MANAGER] Egg colors are changed. Sent to Anselmo");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                finally
                {
                    mutexToColorQueue.Release();
                }

                Egg egg = new Egg((GenerateColor(Random.Shared.Next(6)), GenerateColor(Random.Shared.Next(6)))); //uovo
                await mutexQueue.WaitAsync();
                try { eggs.Enqueue(egg); }
                finally { mutexQueue.Release(); }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[MANAGER] Added an egg to the Queue.");
                Console.ForegroundColor = ConsoleColor.White;
            }
        } 
        catch (OperationCanceledException)
        {
            Console.WriteLine("Stopped.");
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
