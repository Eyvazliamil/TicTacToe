using Tic_Tak_Toe.MenuHelper;

namespace Tic_Tak_Toe.Intro
{
    public static class GameIntro
    {
        private static readonly string[] methods = { "START GAME", "HOW TO PLAY", "EXIT" };

        private const string header = """
    ╔══════════════════════════════════════════╗
    ║                                          ║
    ║   ████████╗██╗ ██████╗                   ║
    ║   ╚══██╔══╝██║██╔════╝                   ║
    ║      ██║   ██║██║                        ║
    ║      ██║   ██║██║                        ║
    ║      ██║   ██║╚██████╗                   ║
    ║      ╚═╝   ╚═╝ ╚═════╝                   ║
    ║                                          ║
    ║          T I C   T A C   T O E           ║
    ║                                          ║
    ║             X  |  O  |  X                ║
    ║            ----|-----|----               ║
    ║             O  |  X  |  O                ║
    ║            ----|-----|----               ║
    ║             X  |  O  |  X                ║
    ║                                          ║
    """;

        public static short ShowIntro()
        {
            Console.Title = "Tic Tac Toe";
            return ManageMenu.ShowMenu(methods, header, ConsoleColor.Cyan);
        }
    }
}