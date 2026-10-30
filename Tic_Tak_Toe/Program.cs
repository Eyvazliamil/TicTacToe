
using System;
using System.Threading.Tasks;
using Tic_Tak_Toe.Intro;
using Tic_Tak_Toe.Start;

class Program
{
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.CursorVisible = false;

            short selected = GameIntro.ShowIntro();

            switch (selected)
            {
                case 0:
                    Console.Clear();

                    await StartGame.CreateServer();

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey(true);
                    break;

                case 1:
                    Console.Clear();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("How to play?");
                    Console.ResetColor();

                    Console.WriteLine("Get 3 X's or O's in a row to win!");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ResetColor();

                    Console.ReadKey(true);
                    break;

                case 2:
                    Console.Clear();
                    Console.WriteLine("Goodbye!");
                    return;
            }
        }
    }
}
