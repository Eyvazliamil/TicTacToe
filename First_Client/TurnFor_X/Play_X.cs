
using System;
using System.IO;
using System.Threading.Tasks;

namespace First_Client.TurnFor_X
{
    public static class Play_X
    {
        public static async Task PlayX(
            StreamReader reader,
            StreamWriter writer)
        {
            while (true)
            {
                string? message = await reader.ReadLineAsync();

                if (message == null)
                    break;

                switch (message)
                {
                    case "CONNECTED:X":
                        Console.WriteLine("Connected as Player X!");
                        break;

                    case "YOUR_TURN":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\nYour turn! (X)");
                        Console.ResetColor();

                        Console.Write("Enter index (1-9): ");

                        string? input = Console.ReadLine();

                        await writer.WriteLineAsync(input ?? "");
                        break;

                    case "WAIT":
                        Console.WriteLine("\nWaiting for Player O...");
                        break;

                    case "INVALID":
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid position! Try again.");
                        Console.ResetColor();
                        break;

                    case "DRAW":
                        Console.WriteLine("\nThe game is a draw!");
                        return;

                    default:
                        if (message.StartsWith("BOARD:"))
                        {
                            PrintBoard(message.Substring(6));
                        }
                        else if (message.StartsWith("WINNER:"))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(
                                $"\nPlayer {message.Substring(7)} wins!");
                            Console.ResetColor();

                            return;
                        }

                        break;
                }
            }

            Console.WriteLine("Disconnected from server.");
        }

        private static void PrintBoard(string data)
        {
            Console.WriteLine("\nCurrent Board:");

            for (int i = 0; i < 9; i += 3)
            {
                Console.WriteLine(
                    $" {data[i]} | {data[i + 1]} | {data[i + 2]} ");

                if (i < 6)
                    Console.WriteLine("---+---+---");
            }
        }
    }
}
