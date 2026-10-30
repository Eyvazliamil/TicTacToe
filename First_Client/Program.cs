
using First_Client.TurnFor_X;
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
            using TcpClient client =
                new TcpClient("127.0.0.1", 28001);

            Console.WriteLine("First Client connected!");

            using NetworkStream stream = client.GetStream();

            using StreamReader reader = new StreamReader(stream);

            using StreamWriter writer = new StreamWriter(stream)
            {
                AutoFlush = true
            };

            await Play_X.PlayX(reader, writer);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"First Client error: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey(true);
    }
}
