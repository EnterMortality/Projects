using System;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using Figgle;
using Figgle.Fonts;

class Program
{
    private const string TaagUrl = "https://patorjk.com/software/taag/";
    private static readonly HttpClient HttpClient = CreateHttpClient();

    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            ShowAcsii();
            ShowMenu();
            if (!await InputAsync())
            {
                return;
            }
        }
    }

    private static HttpClient CreateHttpClient()
    {
        var client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 Chrome/131.0.0.0 Safari/537.36");
        return client;
    }

    static void ShowAcsii()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(@"                                
▄█████     ███  ██ ██▄  ▄██ ▄█████ 
██     ▄▄▄ ██ ▀▄██ ██ ▀▀ ██ ██     
▀█████     ██   ██ ██    ██ ▀█████ 

v1.0.0");
        Console.ResetColor();
    }

    static void ShowMenu()
    {
        WriteMenuOption("1", "Create ASCII art");
        WriteMenuOption("2", "Exit");
        WriteMenuOption("3", "About");
        WriteMenuOption("4", "TAAG font list");
    }

    static void ShowAcsiiArt()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Font name (for example, Graffiti): ");
        string? fontName = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(fontName))
        {
            WriteMessage("Font name cannot be empty.", ConsoleColor.Yellow);
            WaitForMenu();
            return;
        }

        var normalizedFontName = NormalizeFontName(fontName);
        var fontProperty = typeof(FiggleFonts)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .FirstOrDefault(property =>
                NormalizeFontName(property.Name).Equals(
                    normalizedFontName,
                    StringComparison.OrdinalIgnoreCase));
        var font = fontProperty?.GetValue(null) as FiggleFont
            ?? FiggleFonts.TryGetByName(fontName.ToLowerInvariant())
            ?? FiggleFonts.TryGetByName(normalizedFontName.ToLowerInvariant());

        if (font is null)
        {
            WriteMessage(
                $"Font \"{fontName}\" is not supported. Check the spelling or choose another font.",
                ConsoleColor.Red);
            WaitForMenu();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nEnter a word or text: ");
        Console.ForegroundColor = ConsoleColor.White;
        string? inputText = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(inputText))
        {
            WriteMessage("Text cannot be empty.", ConsoleColor.Yellow);
        }
        else
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(font.Render(inputText));
            Console.ResetColor();
        }

        WaitForMenu();
    }

    private static string NormalizeFontName(string name) =>
        Regex.Replace(name, @"[^a-zA-Z0-9]", "");

    private static void WritePanelTitle(string title)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.ResetColor();
    }

    private static void WriteMenuOption(string number, string label)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"[{number}] ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(label);
        Console.ResetColor();
    }

    private static void WriteMessage(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"\n{message}");
        Console.ResetColor();
    }

    private static void WaitForMenu()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("\nPress any key to return to the menu...");
        Console.ResetColor();
        Console.ReadKey(true);
    }

    static void ShowReadme()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("This program turns your text into ASCII art.");
        Console.WriteLine("Choose a font, enter a word, and the result will appear in the console.");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Developed by: @pilocy.");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("\nPress any key to return to the menu...");
        Console.ReadKey(true);
    }

    private static async Task ShowTaagFontsAsync()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Loading the font list from TAAG...");

        try
        {
            var fonts = await FetchTaagFontsAsync();
            WritePanelTitle($"Fonts found: {fonts.Length}");
            foreach (var font in fonts)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("  • ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(font);
            }
            Console.ResetColor();
        }
        catch (HttpRequestException exception)
        {
            WriteMessage($"Could not load the TAAG font list: {exception.Message}", ConsoleColor.Red);
        }
        catch (TaskCanceledException exception)
        {
            WriteMessage($"Timed out while waiting for TAAG: {exception.Message}", ConsoleColor.Red);
        }
        catch (InvalidDataException exception)
        {
            WriteMessage($"Could not read the font list: {exception.Message}", ConsoleColor.Red);
        }

        WaitForMenu();
    }

    private static async Task<string[]> FetchTaagFontsAsync()
    {
        var page = await HttpClient.GetStringAsync(TaagUrl);
        var scriptMatch = Regex.Match(
            page,
            "<script[^>]+src=[\"']([^\"']+\\.js)[\"']",
            RegexOptions.IgnoreCase);
        if (!scriptMatch.Success)
        {
            throw new InvalidDataException("TAAG's font catalog script was not found.");
        }

        var scriptUri = new Uri(new Uri(TaagUrl), scriptMatch.Groups[1].Value);
        var script = await HttpClient.GetStringAsync(scriptUri);
        var fontMatches = Regex.Matches(script, @"\{label:""([^""]+)"",value:""([^""]+)""");
        var fonts = fontMatches
            .Select(match => match.Groups[2].Value)
            .Distinct(StringComparer.CurrentCultureIgnoreCase)
            .OrderBy(font => font, StringComparer.CurrentCultureIgnoreCase)
            .ToArray();

        if (fonts.Length == 0)
        {
            throw new InvalidDataException("No fonts were found in TAAG's catalog.");
        }

        return fonts;
    }

    static async Task<bool> InputAsync()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("\nYour choice: ");
        Console.ResetColor();
        ConsoleKeyInfo key = Console.ReadKey(true);
        switch (key.Key)
        {
            case ConsoleKey.D1:
                ShowAcsiiArt();
                return true;
            case ConsoleKey.D2:
                WriteMessage("Goodbye!", ConsoleColor.Magenta);
                return false;
            case ConsoleKey.D3:
                ShowReadme();
                return true;
            case ConsoleKey.D4:
                await ShowTaagFontsAsync();
                return true;
            default:
                WriteMessage("Invalid option. Choose 1, 2, 3, or 4.", ConsoleColor.Yellow);
                return true;
        }
    }
}