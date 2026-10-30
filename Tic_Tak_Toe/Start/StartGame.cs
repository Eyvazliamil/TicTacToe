
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tic_Tak_Toe.Start
{
    public class StartGame
    {
        private const int Port = 28001;

        private static char[,] board =
        {
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' }
        };

        private static bool IsWinner(char player)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == player &&
                    board[i, 1] == player &&
                    board[i, 2] == player)
                    return true;

                if (board[0, i] == player &&
                    board[1, i] == player &&
                    board[2, i] == player)
                    return true;
            }

            return
                (board[0, 0] == player &&
                 board[1, 1] == player &&
                 board[2, 2] == player)
                ||
                (board[0, 2] == player &&
                 board[1, 1] == player &&
                 board[2, 0] == player);
        }

        private static bool IsDraw()
        {
            foreach (char cell in board)
            {
                if (cell == ' ')
                    return false;
            }

            return true;
        }

        private static bool MakeMove(int index, char player)
        {
            if (index < 1 || index > 9)
                return false;

            int row = (index - 1) / 3;
            int col = (index - 1) % 3;

            if (board[row, col] != ' ')
                return false;

            board[row, col] = player;

            return true;
        }

        private static string GetBoard()
        {
            string result = "";

            foreach (char cell in board)
                result += cell;

            return result;
        }

        private static void PrintBoard()
        {
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine(
                    $" {board[i, 0]} | {board[i, 1]} | {board[i, 2]} ");

                if (i < 2)
                    Console.WriteLine("---+---+---");
            }

            Console.WriteLine();
        }

        private static void ResetBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    board[i, j] = ' ';
            }
        }

        private static async Task Send(
            StreamWriter writer, string message)
        {
            await writer.WriteLineAsync(message);
        }

        public static async Task CreateServer()
        {
            ResetBoard();

            using TcpListener listener =
                new TcpListener(IPAddress.Any, Port);

            TcpClient? clientX = null;
            TcpClient? clientO = null;

            try
            {
                listener.Start();

                Console.WriteLine("Server started on port 28001...");
                Console.WriteLine("Waiting for players...");

                clientX = await listener.AcceptTcpClientAsync();

                Console.WriteLine("Player X connected!");

                clientO = await listener.AcceptTcpClientAsync();

                Console.WriteLine("Player O connected!");

                using NetworkStream streamX = clientX.GetStream();
                using NetworkStream streamO = clientO.GetStream();

                using StreamReader readerX = new StreamReader(streamX);
                using StreamWriter writerX = new StreamWriter(streamX)
                {
                    AutoFlush = true
                };

                using StreamReader readerO = new StreamReader(streamO);
                using StreamWriter writerO = new StreamWriter(streamO)
                {
                    AutoFlush = true
                };

                await Send(writerX, "CONNECTED:X");
                await Send(writerO, "CONNECTED:O");

                char currentPlayer = 'X';

                while (true)
                {
                    StreamReader currentReader =
                        currentPlayer == 'X' ? readerX : readerO;

                    StreamWriter currentWriter =
                        currentPlayer == 'X' ? writerX : writerO;

                    StreamWriter waitingWriter =
                        currentPlayer == 'X' ? writerO : writerX;

                    string waitingPlayer =
                        currentPlayer == 'X' ? "O" : "X";

                    try
                    {
                        await Send(currentWriter, "YOUR_TURN");
                        await Send(waitingWriter, "WAIT");

                        Console.WriteLine(
                            $"Waiting for Player {currentPlayer}...");

                        string? input =
                            await currentReader.ReadLineAsync();

                        if (input == null)
                        {
                            Console.WriteLine(
                                $"Player {currentPlayer} disconnected.");

                            break;
                        }

                        if (!int.TryParse(input, out int index))
                        {
                            await Send(currentWriter, "INVALID");
                            continue;
                        }

                        if (!MakeMove(index, currentPlayer))
                        {
                            await Send(currentWriter, "INVALID");
                            continue;
                        }

                        Console.WriteLine(
                            $"Player {currentPlayer} selected {index}");

                        PrintBoard();

                        string boardData = GetBoard();

                        // Send updated board to X
                        await Send(writerX, $"BOARD:{boardData}");

                        // Send updated board to O
                        await Send(writerO, $"BOARD:{boardData}");

                        if (IsWinner(currentPlayer))
                        {
                            await Send(writerX, $"WINNER:{currentPlayer}");
                            await Send(writerO, $"WINNER:{currentPlayer}");

                            Console.WriteLine(
                                $"Player {currentPlayer} wins!");

                            break;
                        }

                        if (IsDraw())
                        {
                            await Send(writerX, "DRAW");
                            await Send(writerO, "DRAW");

                            Console.WriteLine("Game ended in a draw!");

                            break;
                        }

                        currentPlayer =
                            currentPlayer == 'X' ? 'O' : 'X';
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(
                            $"Communication error with Player {currentPlayer} " +
                            $"(other player: {waitingPlayer}): {ex.Message}");

                        break;
                    }
                    catch (ObjectDisposedException ex)
                    {
                        Console.WriteLine(
                            $"Connection was closed: {ex.Message}");

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Server error: {ex}");
                Console.ResetColor();
            }
            finally
            {
                clientX?.Close();
                clientO?.Close();

                Console.WriteLine("\nGame finished.");
            }
        }
    }
}
