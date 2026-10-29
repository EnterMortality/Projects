using System;

class Program
{
    static void Main()
    {
        AcsiiArtShow();
        ShowMenu();
    }

    static void AcsiiArtShow()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine(@"
  ..|'''.|                 '||     '||              '||                              '||'  '|'        
.|'     '  .. .. ..      .. ||      || ..     ....   ||  ... ...    ....  ... ..      '|.  .'      /| 
||          || || ||   .'  '||      ||' ||  .|...||  ||   ||'  || .|...||  ||' ''      ||  |      /|| 
'|.      .  || || ||   |.   ||      ||  ||  ||       ||   ||    | ||       ||           |||        || 
 ''|....'  .|| || ||.  '|..'||.    .||. ||.  '|...' .||.  ||...'   '|...' .||.           |         || 
                                                          ||                                       || 
                                                         ''''                                     ,/-'");
        Console.ForegroundColor = ConsoleColor.Cyan;

    }

    static void ShowMenu()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("[1] Generate password");
        Console.WriteLine("[2] Clear browser history");
        Console.WriteLine("[3] Exit");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("\n┌─[cmd-helper]─[~]\n└──╼ ");
        int choice = int.Parse(Console.ReadLine() ?? "");

        switch (choice)
        {
            case 1:
                PasswordGenerator();
                break;
            case 2:
                BrowserHistoryCleaner();
                break;
            case 3:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n[+] Exiting...");
                System.Threading.Thread.Sleep(1000);
                Environment.Exit(0);
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Неверный выбор. Пожалуйста, выберите снова.");
                ShowMenu();
                break;
        }
    }

    static void PasswordGenerator()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n[+] Генерация пароля...");
        // Логика генерации пароля
    }

    static void BrowserHistoryCleaner()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n[+] Очистка истории браузера...");
        // Логика очистки истории браузера
    }
}