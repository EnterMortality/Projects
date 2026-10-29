using System;
using SharpPcap;
using PacketDotNet;

class Program
{
    static void Main()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("-" + new string('-', 60) + "-");
        Console.WriteLine("Поиск сетевых устройств.");
        Console.WriteLine("-" + new string('-', 60) + "-");

        var devices = CaptureDeviceList.Instance;

        if (devices.Count < 1)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[!] Network interface not found.");
            return;
        }

        for (int i = 0; i < devices.Count; i++)
        {
            Console.WriteLine($"[{i}] {devices[i].Description}");
        }

        Console.WriteLine();
        int selectedIndex = -1;

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"Введите номер интерфейса (0 - {devices.Count - 1}): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out selectedIndex) && selectedIndex >= 0 && selectedIndex < devices.Count)
            {
                break;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("[!] Неверный ввод. Пожалуйста, введите корректный номер из списка.\n");
        }

        ICaptureDevice device = devices[selectedIndex];

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n[+] Выбран интерфейс: {device.Description}\n");

        device.OnPacketArrival += device_OnPacketArrival;
        
        int readTimeoutMS = 1000;
        device.Open(DeviceModes.Promiscuous, readTimeoutMS);

        device.StartCapture();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Traffic monitoring... Нажмите Enter для остановки.\n");

        Console.ReadLine();

        device.StopCapture();
        device.Close();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n[+] Monitoring stopped.");
    }

    private static void device_OnPacketArrival(object sender, PacketCapture e)
    {
        var rawPacket = e.GetPacket();
        var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
        var ipPacket = packet.Extract<IPPacket>();

        if (ipPacket != null)
        {
            System.Net.IPAddress srcIp = ipPacket.SourceAddress;
            System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
            string protocol = ipPacket.Protocol.ToString();

            int length = ipPacket.TotalLength;

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {srcIp} -> {dstIp} | Protocol: {protocol} | Length: {length}");
        }
    }
}