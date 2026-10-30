
using Second_Client.TurnFor_Y;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        try
        {
            using TcpClient client = new TcpClient();

            await client.ConnectAsync("127.0.0.1", 28001);

            Console.WriteLine("Second Client connected!");

            using NetworkStream stream = client.GetStream();

            using StreamReader reader = new StreamReader(stream);

            using StreamWriter writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };

            await Play_Y.PlayY(reader, writer);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Second Client error: {ex}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey(true);
    }
}
