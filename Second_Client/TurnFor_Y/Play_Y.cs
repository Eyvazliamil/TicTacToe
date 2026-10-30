
using System;
using System.IO;
using System.Threading.Tasks;

namespace Second_Client.TurnFor_Y
{
    public static class Play_Y
    {
        public static async Task PlayY(
            StreamReader reader,
            StreamWriter writer)
        {
            try
            {
                while (true)
                {
                    string? message = await reader.ReadLineAsync();

                    if (message == null)
                    {
                        Console.WriteLine("Server disconnected.");
                        break;
                    }

                    switch (message)
                    {
                        case "CONNECTED:O":
                            Console.WriteLine("Connected as Player O!");
                            break;

                        case "YOUR_TURN":
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\nYour turn! (O)");
                            Console.ResetColor();

                            Console.Write("Enter index (1-9): ");

                            string? input = Console.ReadLine();

                            await writer.WriteLineAsync(input ?? "");
                            break;

                        case "WAIT":
                            Console.WriteLine("\nWaiting for Player X...");
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
                                Console.WriteLine(
                                    $"\nPlayer {message.Substring(7)} wins!");

                                return;
                            }

                            break;
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
        }

        private static void PrintBoard(string data)
        {
            if (data.Length != 9)
                return;

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
