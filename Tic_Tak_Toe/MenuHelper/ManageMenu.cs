namespace Tic_Tak_Toe.MenuHelper
{
    public class ManageMenu
    {
        public static short ShowMenu(string[] items, string header = "", ConsoleColor boxColor = ConsoleColor.Gray, int width = 42)
        {
            short selected = 0;
            while (true)
            {
                Console.Clear();

                Console.ForegroundColor = boxColor;
                if (header != "")
                    Console.WriteLine(header);

                for (short i = 0; i < items.Length; i++)
                {
                    string text = i == selected ? $">> {items[i]} <<" : items[i];
                    int pad = width - text.Length;
                    int left = pad / 2;
                    int right = pad - left;

                    Console.ForegroundColor = boxColor;
                    Console.Write("║" + new string(' ', left));

                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.Write(text);
                    Console.ResetColor();

                    Console.ForegroundColor = boxColor;
                    Console.WriteLine(new string(' ', right) + "║");
                }

                Console.ForegroundColor = boxColor;
                Console.WriteLine("║" + new string(' ', width) + "║");
                Console.WriteLine("╚" + new string('═', width) + "╝");
                Console.ResetColor();

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.UpArrow && selected > 0) selected--;
                if (key.Key == ConsoleKey.DownArrow && selected < items.Length - 1) selected++;
                if (key.Key == ConsoleKey.Enter) return selected;
            }
        }
    }
}