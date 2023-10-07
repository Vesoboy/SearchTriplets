using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        string filePath = Console.ReadLine();

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не существует.");
            return;
        }

        var timeStart = DateTime.Now;

        string text = File.ReadAllText(filePath);

        var triplets = GetTopTriplets(text, 10);
        var timeStop = DateTime.Now;
        Console.WriteLine("10 самых часто встречающихся триплетов:");
        Console.WriteLine(string.Join(", ", triplets));
        
        var time = (timeStop - timeStart).TotalMilliseconds;
        Console.WriteLine($"Выполнено за: {time} миллисекунд");
    }

    static string[] GetTopTriplets(string text, int topCount)
    {
        var tripletCounts = new ConcurrentDictionary<string, int>();

        string[] lines = text.Split('\n');

        Parallel.ForEach(lines, line =>
        {
            for (int i = 0; i < line.Length - 2; i++)
            {
                string triplet = line.Substring(i, 3);
                tripletCounts.AddOrUpdate(triplet, 1, (_, count) => count + 1);
            }
        });

        var topTriplets = tripletCounts.OrderByDescending(r => r.Value)
                                       .Take(topCount)
                                       .Select(r => r.Key)
                                       .ToArray();

        return topTriplets;
    }
}