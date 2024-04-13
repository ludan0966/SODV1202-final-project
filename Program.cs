using System;
using System.Collections.Generic;

namespace ConnectFourGame
{
    // Define Player Class
    public class Player
    {
        public string Name { get; }

        public Player(string name)
        {
            Name = name;
        }
    }

    // define Board Class
    public class Board
    {
        private const int Rows = 6;
        private const int Cols = 7;
        private readonly char[,] grid;

        public Board()
        {
            grid = new char[Rows, Cols];
            InitializeBoard();
        }

        // Initialize Board
        private void InitializeBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    grid[row, col] = '#'; // mark empty grid with #
                }
            }
        }

        // Print Board
        public void PrintBoard()
        {
            
            for (int row = 0; row < Rows; row++)
            {
                Console.Write("| ");
                for (int col = 0; col < Cols; col++)
                {
                    Console.Write(grid[row, col] + " ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("  1 2 3 4 5 6 7");
        }

        // Check if column is full
        public bool IsColumnFull(int col)
        {
            return grid[0, col] != '#';
        }

        
        public bool PlaceToken(int col, char token)
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (grid[row, col] == '#')
                {
                    grid[row, col] = token;
                    return true;
                }
            }
            return false;
        }

        // Check if there is winner
        public bool CheckWin(char token)
        {
            // Check if there are four-in-a-row of the same symbol
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols - 3; col++)
                {
                    if (grid[row, col] == token &&
                        grid[row, col + 1] == token &&
                        grid[row, col + 2] == token &&
                        grid[row, col + 3] == token)
                    {
                        return true;
                    }
                }
            }

            // Check if there are four-in-a-column of the same symbol

            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    if (grid[row, col] == token &&
                        grid[row + 1, col] == token &&
                        grid[row + 2, col] == token &&
                        grid[row + 3, col] == token)
                    {
                        return true;
                    }
                }
            }

            // Check if there are four-in-a-diagonal of the same symbol(left-top to right-bottom)
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 0; col < Cols - 3; col++)
                {
                    if (grid[row, col] == token &&
                        grid[row + 1, col + 1] == token &&
                        grid[row + 2, col + 2] == token &&
                        grid[row + 3, col + 3] == token)
                    {
                        return true;
                    }
                }
            }

            // Check if there are four-in-a-diagonal of the same symbol(right-top to left-bottom)
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 3; col < Cols; col++)
                {
                    if (grid[row, col] == token &&
                        grid[row + 1, col - 1] == token &&
                        grid[row + 2, col - 2] == token &&
                        grid[row + 3, col - 3] == token)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Check if board if full
        public bool IsBoardFull()
        {
            for (int col = 0; col < Cols; col++)
            {
                if (!IsColumnFull(col))
                {
                    return false;
                }
            }
            return true;
        }
    }

    // Define ConnectFourGame Class
    public class ConnectFourGame
    {
        private readonly Player[] players;
        private Board board;
        private int currentPlayerIndex;

        public ConnectFourGame(string player1Name, string player2Name)
        {
            players = new Player[2];
            players[0] = new Player(player1Name);
            players[1] = new Player(player2Name);
        }

        // Play Game Method
        public void PlayGame()
        {
            while (true)
            {
                board = new Board();
                currentPlayerIndex = new Random().Next(2); // Randomly pick a palyer to drop the piece first

                while (true)
                {
                    board.PrintBoard();
                    Console.WriteLine($"Player {players[currentPlayerIndex].Name}'s turn.");

                    int col;
                    while (true)
                    {
                        Console.Write("Enter column (1-7): ");
                        if (int.TryParse(Console.ReadLine(), out col) && col >= 1 && col <= 7)
                        {
                            if (board.IsColumnFull(col - 1))
                            {
                                Console.WriteLine("This column is already full. Please choose another column.");
                            }
                            else
                            {
                                break; 
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid column. Please choose a valid column.");
                        }
                    }

                    char token = currentPlayerIndex == 0 ? 'X' : 'O';
                    if (!board.PlaceToken(col - 1, token))
                    {
                        Console.WriteLine("This column is already full. Please choose another column.");
                        continue; // ask the player to choose another column
                    }

                    if (board.CheckWin(token))
                    {
                        board.PrintBoard();
                        Console.WriteLine($"It's Connect 4!  {players[currentPlayerIndex].Name} wins!");
                        break;
                    }

                    if (board.IsBoardFull())
                    {
                        board.PrintBoard();
                        Console.WriteLine("It's a draw!");
                        break;
                    }

                    currentPlayerIndex = (currentPlayerIndex + 1) % 2;
                }

                Console.Write("Restart? Yes(1) N0(0):");
                string playAgain = Console.ReadLine().ToLower();
                if (playAgain != "1")
                {
                    break; // if player inputs 2, exit the game
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Connect Four Game!");
            Console.Write("Enter player 1 name: ");
            string player1Name = Console.ReadLine();
            Console.Write("Enter player 2 name: ");
            string player2Name = Console.ReadLine();

            ConnectFourGame game = new ConnectFourGame(player1Name, player2Name);
            game.PlayGame();
        }
    }
}
