using System;

namespace GameOfLifeConsole
{
    public static class Menu
    {
        public const int OneGameSelected = 1;
        public const int ThousandGamesSelected = 2;
        public const int EightGamesSelected = 3;
        public const int AllGamesSelected = 4;
        public const int LoadGameSelected = 5;

        public static int SelectedOption = 0;

        static string message =
            "Please select an option:   " + Environment.NewLine +
            "1. Start one game   " + Environment.NewLine +
            "2. Start 1000 games   " + Environment.NewLine +
            "3. Start 1000 games and show 8 random of them  " + Environment.NewLine +
            "4. Start 1000 games and show all  " + Environment.NewLine +
            "5. Load game from file" + Environment.NewLine +
            "(Enter the number of selected option) -> ";
        static string errorMessage =
            "You entered incorrect number. Please select the valid number of option from list of options and enter it's number" + Environment.NewLine + Environment.NewLine;

        static string input = String.Empty;

        public static void ShowMenu()
        {
            Console.Write(message);
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    {
                        Console.Clear();
                        SelectedOption = OneGameSelected;
                        RequestGameOptions();
                        GamesHandler.StartOneGame();
                        break;
                    }
                case "2":
                    {
                        Console.Clear();
                        SelectedOption = ThousandGamesSelected;
                        RequestGameOptions();
                        GamesHandler.StartThousandGamesAndShowOne();
                        break;
                    }
                case "3":
                    {
                        Console.Clear();
                        SelectedOption = EightGamesSelected;
                        RequestGameOptions();
                        GamesHandler.StartThousandGamesAndShowEight();
                        break;
                    }
                case "4":
                    {
                        Console.Clear();
                        SelectedOption = AllGamesSelected;
                        RequestGameOptions();
                        GamesHandler.StartThousandGamesAndShowAll();
                        break;
                    }
                case "5":
                    {
                        Console.Clear();
                        SelectedOption = LoadGameSelected;
                        GamesHandler.LoadGameFromFile();
                        break;
                    }
                default:
                    {
                        Console.Clear();
                        Console.Write(errorMessage);
                        ShowMenu();
                        break;
                    }

            }
        }

        public static void RequestGameOptions()
        {
            string inputWidth = string.Empty;
            string inputHeigth = string.Empty;

            Console.Write("Please enter width of field (in range: 1-200)  -> ");
            inputWidth = Console.ReadLine();
            while (!(Int32.TryParse(inputWidth, out Game.Width) && (Game.Width >= 1) && (Game.Width <= 200)))
            {
                Console.Write("\nEntered width is incorrect. Please enter a positive integer number (in range: 1-200) -> ");
                inputWidth = Console.ReadLine();
            }

            Console.Write("Please enter heigth of field (in range: 1-200)  -> ");
            inputHeigth = Console.ReadLine();

            while (!(Int32.TryParse(inputHeigth, out Game.Heigth) && (Game.Heigth >= 1) && (Game.Heigth <= 200)))
            {
                Console.Write("\nEntered heigth is incorrect. Please enter a positive integer number (in range: 1-200) -> ");
                inputHeigth = Console.ReadLine();
            }
            Console.Clear();
        }
    }
}
