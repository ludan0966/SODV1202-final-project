using System;

namespace ConnectFourGame
{
    // Define abstract Player Class
    public abstract class Player
    {
        public string Name { get; }

        protected Player(string name)
        {
            Name = name;
        }

        // Abstract method for choosing column
        public abstract int ChooseColumn(Board board);
    }

    // Define HumanPlayer Class inheriting from Player
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name) : base(name) { }

        public override int ChooseColumn(Board board)
        {
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
                        return col - 1;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid column. Please choose a valid column.");
                }
            }
        }
    }

    // Define AIPlayer Class inheriting from Player
    public class AIPlayer : Player
    {
        public AIPlayer(string name) : base(name) { }

        public override int ChooseColumn(Board board)
        {
            // Implement AI logic to choose column
            // For simplicity, let's just choose a random column
            Random random = new Random();
            int col;
            do
            {
                col = random.Next(7); // Random column from 0 to 6
            } while (board.IsColumnFull(col));

            Console.WriteLine($"AI {Name} chooses column: {col + 1}");
            return col;
        }
    }

    // Define Board Class
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

        // Check if there is a winner
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

        // Check if board is full
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
        private Player player1;
        private Player player2;
        private Board board;
        private Player currentPlayer;

        public ConnectFourGame()
        {
            board = new Board();
        }

        // Start Game Method
        public void StartGame()
        {
            Console.WriteLine("Welcome to Connect Four Game!");
            while (true)
            {
                Console.WriteLine("Select game mode:");
                Console.WriteLine("1. Player vs Player");
                Console.WriteLine("2. Player vs AI");
                Console.Write("Enter your choice (1 or 2): ");
                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
                {
                    Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                }

                if (choice == 1)
                {
                    Console.Write("Enter player 1 name: ");
                    string player1Name = Console.ReadLine();
                    Console.Write("Enter player 2 name: ");
                    string player2Name = Console.ReadLine();
                    player1 = new HumanPlayer(player1Name);
                    player2 = new HumanPlayer(player2Name);
                }
                else
                {
                    Console.Write("Enter your name: ");
                    string playerName = Console.ReadLine();
                    player1 = new HumanPlayer(playerName);
                    player2 = new AIPlayer("AI");
                }

                currentPlayer = player1;
                PlayGame();

                Console.Write("Restart? Yes(1) No(0): ");
                string playAgain = Console.ReadLine().ToLower();
                if (playAgain != "1")
                {
                    break; // Exit the game
                }
            }
        }

        // Play Game Method
        private void PlayGame()
        {
            board = new Board();
            while (true)
            {
                board.PrintBoard();
                Console.WriteLine($"Player {currentPlayer.Name}'s turn.");

                int col = currentPlayer.ChooseColumn(board);

                char token = currentPlayer == player1 ? 'X' : 'O';
                if (!board.PlaceToken(col, token))
                {
                    Console.WriteLine("This column is already full. Please choose another column.");
                    continue; // ask the player to choose another column
                }

                if (board.CheckWin(token))
                {
                    board.PrintBoard();
                    Console.WriteLine($"It's Connect 4!  {currentPlayer.Name} wins!");
                    break;
                }

                if (board.IsBoardFull())
                {
                    board.PrintBoard();
                    Console.WriteLine("It's a draw!");
                    break;
                }

                currentPlayer = (currentPlayer == player1) ? player2 : player1;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConnectFourGame game = new ConnectFourGame();
            game.StartGame();
        }
    }
}
