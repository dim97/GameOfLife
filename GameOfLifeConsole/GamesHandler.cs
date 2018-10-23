using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GameOfLifeConsole
{
    public static class GamesHandler
    {
        private static int gamesMaxCount = 1000;

        public static int currentLoadedGamesCount;
        public static int currentDrawingGame = 0;

        public static List<Game> games;
        public static List<Thread> threads;

        public static void Draw()
        {
            int lastDrawedGeneration = 0;
            while (true)
            {
                Game gameToDraw = games[currentDrawingGame];
                if ((!Game.Pause) && (gameToDraw.Generation != lastDrawedGeneration))
                {
                    lastDrawedGeneration = gameToDraw.Generation;
                    Console.Clear();

                    Console.WriteLine(gameToDraw.GetGameData());
                    gameToDraw.Field.DrawFieldToConsole();

                }
            }
        }

        public static void Start()
        {
            currentLoadedGamesCount = 0;
            RequestGameOptions();

            Thread globalKeyHandler = new Thread(GlobalKeyHandler.HandleKeys);
            globalKeyHandler.Start();
            CreateManyGames(1000);
            Draw();
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

        public static void SaveAllGamesToFile(string fileName)
        {
            string output = JsonConvert.SerializeObject(games);

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(output);
            }
        }
        public static void SaveCurrentGameToFile(string fileName, int gameNumber)
        {
            string output = JsonConvert.SerializeObject(games[gameNumber]);

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(output);
            }
        }

        public static void LoadGameFromFile(string fileName)
        {
            currentDrawingGame = 0;

            string input;

            StreamReader sr = new StreamReader(fileName);

            input = sr.ReadLine();
            sr.Close();

            Game gameToAdd = JsonConvert.DeserializeObject<Game>(input);
            Game.Width = gameToAdd.Cells[gameToAdd.Cells.Count - 1].Location.X + 1;
            Game.Heigth = gameToAdd.Cells[gameToAdd.Cells.Count - 1].Location.Y + 1;
            gameToAdd.Field = new Field(Game.Width, Game.Heigth);

            gameToAdd.Generation = 0;

            games.Add(gameToAdd);
            threads.Add(new Thread(gameToAdd.Play));
        }
        public static void CreateManyGames(int countOfGames)
        {
            gamesMaxCount = countOfGames;
            games = new List<Game>();
            threads = new List<Thread>();

            for (int i = 0; i < gamesMaxCount; i++)
            {
                games.Add(new Game(i));
                games[i].SetGameOptions();
                threads.Add(new Thread(games[i].Play));
                threads[i].Start();
                currentLoadedGamesCount++;
                currentDrawingGame = 0;
            }

        }
    }
}
