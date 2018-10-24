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
                //Exit Key: "Escape"
                if (keyinfo.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
                //Next Key: "RightArrow"
                if ((keyinfo.Key == ConsoleKey.RightArrow) && (Menu.SelectedOption == Menu.ThousandGamesSelected))
                {
                    if (GamesHandler.currentDrawingGame < GamesHandler.games.Count - 1)
                    {
                        GamesHandler.currentDrawingGame++;
                    }
                }
                //Next Previous: "LeftArrow"
                if ((keyinfo.Key == ConsoleKey.LeftArrow) && (Menu.SelectedOption == Menu.ThousandGamesSelected))
                {
                    if (GamesHandler.currentDrawingGame > 0)
                    {
                        GamesHandler.currentDrawingGame--;
                    }
                }
                //Save current game "S"
                if ((keyinfo.Key == ConsoleKey.S) && ((Menu.SelectedOption == Menu.ThousandGamesSelected)||(Menu.SelectedOption == Menu.OneGameSelected) || (Menu.SelectedOption == Menu.LoadGameSelected)))
                {
                    GamesHandler.SaveCurrentGameToFile(GamesHandler.currentDrawingGame);
                }
            }
        }
    }
}
