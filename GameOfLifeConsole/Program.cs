using System;
using System.Collections.Generic;
using System.Threading;


namespace GameOfLifeConsole
{
    class Program
    {
        public static int currentDrawingGame = 0;
        public static List<Game> games = new List<Game>();

        static void Main(string[] args)
        {
            Thread keyHandling = new Thread(GlobalKeyHandler);
            Thread drawing = new Thread(Draw);

            Start();
            keyHandling.Start();
            drawing.Start();
        }

        static void Draw()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(games[currentDrawingGame].GetFieldString());

                Thread.Sleep(1000);
            }
        }

        static void Start()
        {
            //Game game = new Game();
            //game.Play();           

            Game.RequestGameOptions();

            games = new List<Game>();
            List<Thread> threads = new List<Thread>();

            for (int i = 0; i < 100; i++)
            {

                games.Add(new Game(i));
                games[i].SetGameOptions();
                threads.Add(new Thread(games[i].Play));
                threads[i].Start();

                //if(i-1>=0) games[i-1].IsDrawing = false;
                //games[i].IsDrawing = true;
                //Thread.Sleep(100);

                if (i == 0)
                {
                    //games[i].IsDrawing = true;
                    currentDrawingGame = i;
                }
            }
        }

        static void GlobalKeyHandler()
        {
            ConsoleKeyInfo keyinfo;
            while (true)
            {
                //Next Key: "RightArrow"
                keyinfo = Console.ReadKey();
                if (keyinfo.Key == ConsoleKey.RightArrow)
                {
                    if (currentDrawingGame < games.Count-1)
                    {
                        //games[currentDrawingGame].IsDrawing = false;
                        currentDrawingGame++;
                        //games[currentDrawingGame].IsDrawing = true;
                    }
                }
                //Next Previous: "LeftArrow"
                if (keyinfo.Key == ConsoleKey.LeftArrow)
                {
                    if (currentDrawingGame > 0)
                    {
                        //games[currentDrawingGame].IsDrawing = false;
                        currentDrawingGame--;
                        //games[currentDrawingGame].IsDrawing = true;
                    }
                }
            }
        }
    }
}
