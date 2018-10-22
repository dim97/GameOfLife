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

        public static void Draw()
        {
            int lastDrawedGeneration = 0;
            while (true)
            {
                Game gameToDraw = games[currentDrawingGame];
                if (!Game.Pause)
                {
                    if (gameToDraw.Generation != lastDrawedGeneration)
                    {
                        lastDrawedGeneration = gameToDraw.Generation;
                        Console.Clear();
                        
                        Console.WriteLine(gameToDraw.GetGameData());
                        gameToDraw.Field.DrawFieldToConsole();
                    }
                }
            }
        }

        public static void Start()
        {
            currentLoadedGamesCount = 0;
            RequestGameOptions();

            Thread globalKeyHandler = new Thread(GlobalKeyHandler.HandleKeys);
            globalKeyHandler.Start();

            games = new List<Game>();
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < gamesMaxCount; i++)
            {
                games.Add(new Game(i));
                games[i].SetGameOptions();
                threads.Add(new Thread(games[i].Play));
                threads[i].Start();
                currentLoadedGamesCount++;
                currentDrawingGame = 0;
            }

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
    }
}
