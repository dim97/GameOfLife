using System;

namespace GameOfLifeConsole
{
    public static class Menu
    {
        static string message =
            "Please select an option:   " + Environment.NewLine +
            "1. Start one game   " + Environment.NewLine +
            "2. Start 1000 games   " + Environment.NewLine +
            "3. Start 1000 games and show 8 random of them  " + Environment.NewLine +
            "4. Load game from file" + Environment.NewLine +
            "(Enter the number of selected option) -> ";

        static string input = String.Empty;

        public static void ShowMenu()
        {
            Console.Write(message);
            input = Console.ReadLine();
            Console.Clear();

            switch (input)
            {
                case "1":
                    {
                        RequestGameOptions();
                        break;
                    }
                case "2":
                    {
                        RequestGameOptions();
                        GamesHandler.StartThousandGames();
                        break;
                    }
                case "3":
                    {
                        RequestGameOptions();
                        GamesHandler.DrawEightGames();
                        break;
                    }
                case "4":
                    {
                        GamesHandler.LoadGameFromFile();
                        break;
                    }
                default:
                    {
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
