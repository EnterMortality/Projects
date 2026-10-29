using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    private static readonly string[] Prefixes =
    {
        "val", "sol", "aer", "lun", "zen", "mar", "ver", "cor", "kal", "ast",
        "nov", "pyr", "syl", "vok", "mir", "cel", "or", "el", "al", "zeph"
    };

    private static readonly string[] Middles =
    {
        "an", "ar", "is", "or", "en", "el", "im", "at", "os", "um",
        "d", "r", "s", "t", "l", "m", "n", "v", "z"
    };

    private static readonly string[] Suffixes =
    {
        "is", "us", "a", "os", "ia", "or", "um", "ex", "is", "en",
        "on", "ix", "eth", "al", "as", "is", "ys", "ar"
    };

    static void Main(string[] args)
    {
        Console.Clear();
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine(@"
  _   _                                             ____                           _             
 | | | |___  ___ _ __ _ __   __ _ _ __ ___   ___   / ___| ___ _ __   ___ _ __ __ _| |_ ___  _ __ 
 | | | / __|/ _ \ '__| '_ \ / _` | '_ ` _ \ / _ \ | |  _ / _ \ '_ \ / _ \ '__/ _` | __/ _ \| '__|
 | |_| \__ \  __/ |  | | | | (_| | | | | | |  __/ | |_| |  __/ | | |  __/ | | (_| | || (_) | |   
  \___/|___/\___|_|  |_| |_|\__,_|_| |_| |_|\___|  \____|\___|_| |_|\___|_|  \__,_|\__\___/|_|   
  
                    -- Creator: @policy | Telegram channel: @policychannel --");

        Random random = new Random();

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nGeneric normal usernames:");
            List<string> usernames = GenerateRandomUsernames(random, 12);

            foreach (var user in usernames)
            {
                Console.WriteLine($"@{user}");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nclick <Enter> to contunie generic partys / or <q> leave.");
            Console.ResetColor();
            Console.Write("Enter: ");
            string input = Console.ReadLine();

            if (input?.Trim().ToLower() == "q")
                break;
            Console.Clear();
        }
    }

    public static List<string> GenerateRandomUsernames (Random random, int count)
    {
        HashSet<string> result = new HashSet<string>();
        int maxAttempts = 200;
        int attempts = 0;

        while (result.Count < count && attempts < maxAttempts)
        {
            attempts ++;
            string username = GenerateWord(random);

            if (username.Length >= 5 && username.Length <= 12 && username.All(char.IsLetter))
            {
                string formatted = char.ToUpper(username[0]) + username.Substring(1).ToLower();
                result.Add(formatted);
            }
        }

        return result.ToList();
    }

    private static string GenerateWord(Random random)
    {
        int pattern = random.Next(3);

        string prefix = Prefixes[random.Next(Prefixes.Length)];
        string middle = Middles[random.Next(Middles.Length)];
        string suffix = Suffixes[random.Next(Suffixes.Length)];


        Console.ForegroundColor = ConsoleColor.Red;
        return pattern switch
        {
            0 => prefix + suffix,
            1 => prefix + middle + suffix,
            2 => prefix + suffix,
        };
    }
}