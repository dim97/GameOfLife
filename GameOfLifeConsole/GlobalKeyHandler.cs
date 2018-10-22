using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeConsole
{
    public static class GlobalKeyHandler
    {

        public static void HandleKeys()
        {
            ConsoleKeyInfo keyinfo;
            while (true)
            {   
                //Pause Key: "Spacebar"
                keyinfo = Console.ReadKey();
                if (keyinfo.Key == ConsoleKey.Spacebar)
                {
                    Game.Pause = !Game.Pause;
                }
                //Next Key: "RightArrow"
                keyinfo = Console.ReadKey();
                if (keyinfo.Key == ConsoleKey.RightArrow)
                {
                    if (GamesHandler.currentDrawingGame < GamesHandler.games.Count - 1)
                    {
                        GamesHandler.currentDrawingGame++;
                    }
                }
                //Next Previous: "LeftArrow"
                if (keyinfo.Key == ConsoleKey.LeftArrow)
                {
                    if (GamesHandler.currentDrawingGame > 0)
                    {
                        GamesHandler.currentDrawingGame--;
                    }
                }
            }
        }
    }
}
