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
        private static string fileName = "Save.json";
        public static int currentDrawingGame = 0;

        public static List<Game> games;
        public static List<Thread> threads;

        public static void DrawOneGame()
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
        public static void DrawEightGames()
        {
            CreateGames(1000);

            int lastDrawedGenerationForFirstGame = 0;
            Random random = new Random();
            List<Game> randomEightGames = new List<Game>();
            for (int i = 0; i < 8; i++)
            {
                randomEightGames.Add(games[random.Next(gamesMaxCount)]);
            }

            while (true)
            {
                if ((!Game.Pause) && (randomEightGames[0].Generation != lastDrawedGenerationForFirstGame))
                {
                    Console.Clear();
                    lastDrawedGenerationForFirstGame = randomEightGames[0].Generation;

                    foreach (Game gameToDraw in randomEightGames)
                    {
                        Console.WriteLine(gameToDraw.GetGameData());
                        gameToDraw.Field.DrawFieldToConsole();
                    }

                }

            }
        }

        public static void StartThousandGames()
        {
            Thread globalKeyHandler = new Thread(GlobalKeyHandler.HandleKeys);
            globalKeyHandler.Start();
            CreateGames(1000);
            DrawOneGame();
        }

        public static void SaveAllGamesToFile()
        {
            string output = JsonConvert.SerializeObject(games);

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(output);
            }
        }
        public static void SaveCurrentGameToFile(int gameNumber)
        {
            string output = JsonConvert.SerializeObject(games[gameNumber]);

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(output);
            }
        }

        public static void LoadGameFromFile()
        {
            games = new List<Game>();
            threads = new List<Thread>();

            currentDrawingGame = 0;
            string input;

            StreamReader sr = new StreamReader(fileName);
            input = sr.ReadLine();
            sr.Close();

            Game gameToAdd = JsonConvert.DeserializeObject<Game>(input);
            Game.Width = gameToAdd.Cells[gameToAdd.Cells.Count - 1].Location.X + 1;
            Game.Heigth = gameToAdd.Cells[gameToAdd.Cells.Count - 1].Location.Y + 1;
            
            gameToAdd.GameNumber = 0;
            games.Add(gameToAdd);
            threads.Add(new Thread(gameToAdd.Play));
            threads[0].Start();
            
            Thread globalKeyHandler = new Thread(GlobalKeyHandler.HandleKeys);
            globalKeyHandler.Start();
            DrawOneGame();

        }
        public static void CreateGames(int countOfGames)
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
                currentDrawingGame = 0;
            }

        }
    }
}
