using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.Clear();
        string os = RuntimeInformation.OSDescription.Trim();
        string host = Environment.MachineName;
        string user = Environment.UserName;
        string kernel = RuntimeInformation.OSArchitecture.ToString();
        
        TimeSpan uptimeSpan = TimeSpan.FromMilliseconds(Environment.TickCount64);
        string uptime = $"{uptimeSpan.Days}d {uptimeSpan.Hours}h {uptimeSpan.Minutes}m";

        string cpuCores = $"{Environment.ProcessorCount} threads";
        
        long totalMemoryMB = 0;
        try
        {
            var gcMemoryInfo = GC.GetGCMemoryInfo();
            totalMemoryMB = gcMemoryInfo.TotalAvailableMemoryBytes / (1024 * 1024);
        }
        catch { totalMemoryMB = 0; }

        string memory = totalMemoryMB > 0 ? $"{totalMemoryMB / 1024} GB" : "Unknown";

        string localIp = "127.0.0.1";
        try
        {
            var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in hostEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                    break;
                }
            }
        }
        catch { }

        long processMemoryMB = GC.GetTotalMemory(false) / (1024 * 1024);

        string[] art = new string[]
        {
            "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
            "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣠⣴⣶⣿⣿⣿⣀⣼⣿⡆⠀⠀⠀⠀⠀⠀⠀⠀",
            "⠀⠀⠀⠀⠀⠀⠀⠀⠀⣰⣿⣿⡿⠋⠀⠀⠀⣀⣠⣤⣤⣶⡖⢲⣦⣄⠀⠀⠀",
            "⠀⠀⠀⠀⠀⠀⠀⠀⢰⣿⣿⣿⣇⠀⠀⣰⣾⡿⠟⠛⠛⠛⠳⠾⠟⠛⠀⠀⠀",
            "⠀⠀⠀⠀⠀⠀⠀⠀⠈⣿⣿⣿⣿⣦⣸⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀",
            "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⢿⣿⣿⣿⣿⣿⣆⠀⣤⣾⣿⣿⣷⣦⡀⠀⠀⠀⠀",
            "⠀⠀⠀⠀⠀⠀⣀⠀⠀⠀⠈⠻⣿⣿⣿⣿⣿⣿⣿⠋⠀⠀⠈⢻⣿⡦⠤⣄⡀",
            "⠀⠀⣠⣶⣿⣿⣿⣿⣷⣦⡀⠀⠙⢿⣿⣿⣿⣿⣿⣷⣄⠀⠀⠀⠻⠧⠴⠿⠿",
            "⠀⣼⣿⣿⣿⣿⣿⣿⣿⣿⣿⡄⠀⠈⠻⣿⣿⣿⣿⣿⣿⣷⡄⠀⠀⠀⠀⠀⠀",
            "⠸⣿⿿⠛⠛⢻⣿⣿⣿⣿⣿⡇⠀⠀⠀⠙⣿⣿⣿⣿⣿⣿⣿⣆⠀",
            "⠀⠀⠀⠀⣠⣾⣿⣿⣿⣿⣿⠃⠀⠀⠀⠀⢸⣿⣿⣿⣿⣿⣿⣿⣆⠀⠀",
            "⠀⠀⠀⣴⣿⣿⣿⣿⣿⣿⣿⣶⣶⣶⣶⣾⣿⣿⣿⣿⣿⣿⣿⣿⣿⠀⠀⠀⠀",
            "⠀⠀⢸⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⡿⠀⠀⠀⠀",
            "⠀⠀⠈⢿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⣿⠟⠀⠀⠀⠀⠀",
            "⠀⠀⠀⠀⠈⠙⠛⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠟⠋⠁⠀⠀⠀⠀⠀⠀"
        };

        string[] info = new string[]
        {
            $"│ OS: {os}",
            $"│ Host: {host}",
            $"│ User: {user}",
            $"│ Architecture: {kernel}",
            $"│ Uptime: {uptime}",
            $"│ CPU Cores: {cpuCores}",
            $"│ RAM Total: {memory}",
            $"│ Process RAM: {processMemoryMB} MB",
            $"│ Local IP: {localIp}",
            $"│ Runtime .NET: {Environment.Version}",
            $"│ System Dir: {Environment.SystemDirectory}",
            "",
            "          .88888.  oo       dP                             ",
            "         d8'   `88          88                             ",
            ".d888b.  88        dP .d888b88 88d888b. .d8888b. ",
            "88ooood8 88   YP88 88 88'  `88 88'  `88 88'  `88 ",
            "88.  ... Y8.   .88 88 88.  .88 88       88.  .88 ",
            "`88888P'  `88888'  dP `88888P8 dP       `88888P8 "
        };

        int artWidth = art.Max(a => a.Length); 
        int maxLines = Math.Max(art.Length, info.Length);

        for (int i = 0; i < maxLines; i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string currentArt = i < art.Length ? art[i] : "";
            Console.Write(currentArt.PadRight(artWidth));

            Console.Write("    "); 

            string currentInfo = i < info.Length ? info[i] : "";

            if (i >= 12 && i < info.Length)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if (!string.IsNullOrEmpty(currentInfo))
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine(currentInfo);
        }
        Console.ReadLine();
        Console.ResetColor();
    }
}