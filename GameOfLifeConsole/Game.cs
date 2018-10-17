using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace GameOfLifeConsole
{
    public class Game
    {
        public static int enteredWidth;
        public static int enteredHeigth;
        public static List<Cell> cells;
        public static Field field;
        public static int generation;
        public static bool pause;

        public static void Play()
        {
            pause = false;

            Thread game = new Thread(Life);
            Thread buttonHandler = new Thread(ButtonHandler);

            SetGameOptions();
            DrawField();
            DevOptions();

            game.Start();
            buttonHandler.Start();

        }

        public static void Life()
        {
            generation = 0;
            while (true)
                if (pause == false)
                {
                    //Drawing current generation
                    Console.Clear();
                    DrawField();
                    generation++;

                    //Processing next generation
                    foreach (Cell cell in cells)
                    {
                        int neighborsCount = CountLivingNeighbors(cell);

                        if ((neighborsCount < 2) || (neighborsCount > 3)) cell.IsAlive = false;
                        if ((neighborsCount == 3) && (cell.IsAlive == false)) cell.IsAlive = true;
                        if ((neighborsCount == 2) && (cell.IsAlive == false)) cell.IsAlive = false;

                    }

                    Thread.Sleep(1000);
                }
        }

        public static int CountLivingNeighbors(Cell cell)
        {
            int count = 0;
            List<Point> neighborsPositions = cell.GetNeighbors(enteredWidth, enteredHeigth);
            foreach (Point neighbor in neighborsPositions)
            {
                if (field.CellsToDraw[neighbor.Y, neighbor.X] == true)
                    count++;
            }

            return count;
        }

        public static void SetGameOptions()
        {
            Console.Write("\nPlease enter width of field -> ");
            string inputWidth = Console.ReadLine();
            Int32.TryParse(inputWidth, out enteredWidth);

            Console.Write("\nPlease enter heigth of field -> ");
            string inputHeigth = Console.ReadLine();
            Int32.TryParse(inputHeigth, out enteredHeigth);
            Console.Clear();

            field = new Field(enteredWidth, enteredHeigth);

            cells = new List<Cell>();
            for (int i = 0; i < enteredHeigth; i++)
            {
                for (int j = 0; j < enteredWidth; j++)
                {
                    cells.Add(new Cell(j, i));
                }
            }

            //List<Point> o = cells[34].GetNeighbors(enteredWidth, enteredHeigth);
            //o.ToString();
        }

        public static void DrawField()
        {
            int countOfLivingCells = 0;

            foreach (Cell cell in cells)
            {
                if (cell.IsAlive)
                {
                    field.CellsToDraw[cell.Location.Y, cell.Location.X] = true;
                    countOfLivingCells++;
                }
                else field.CellsToDraw[cell.Location.Y, cell.Location.X] = false;
            }

            //Drawing
            Console.WriteLine("Current generation: " + generation);
            Console.WriteLine("Count of living cells: " + countOfLivingCells + "\n");

            for (int i = 0; i < field.Heigth; i++)
            {
                for (int j = 0; j < field.Width; j++)
                {
                    if (field.CellsToDraw[i, j])
                        Console.Write('#');
                    else
                        Console.Write('.');
                    Console.Write(' ');
                }
                Console.Write("\n");
            }
            //Console.Read();
        }

        public static void DevOptions()
        {
            //temporary options to tests

            //Field 40 X 40 - Glider
            //cells[41].IsAlive = true;

            //cells[82].IsAlive = true;

            //cells[120].IsAlive = true;
            //cells[121].IsAlive = true;
            //cells[122].IsAlive = true;
            //

            // Random
            Random random = new Random();
            foreach (Cell cell in cells)
            {
                if (random.Next(2) == 0)
                    cell.IsAlive = false;
                else cell.IsAlive = true;
            }
            //
        }

        public static void ButtonHandler()
        {
            ConsoleKeyInfo keyinfo;
            while (true)
            {
                //Pause Key: "Spacebar"
                keyinfo = Console.ReadKey();
                if ((keyinfo.Key == ConsoleKey.Spacebar)&&(pause==false))
                    pause = true;
                else if ((keyinfo.Key == ConsoleKey.Spacebar) && (pause == true))
                    pause = false;

                //Exit Key: "Escape"
                if (keyinfo.Key == ConsoleKey.Escape)
                    Environment.Exit(0);

                //Save Key: "S"
                if (keyinfo.Key == ConsoleKey.S)
                {
                    string fileName = "Save.json";
                    string output = JsonConvert.SerializeObject(cells);

                    using (StreamWriter sw = new StreamWriter(fileName))
                    {
                        sw.Write(output);
                    }
                }
                //Load Key: "L"
                if (keyinfo.Key == ConsoleKey.L)
                {
                    string fileName = "Save.json";
                    string input;

                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        input = sr.ReadLine();
                    }

                    pause = true;
                    cells.Clear();
                    cells = JsonConvert.DeserializeObject<List<Cell>>(input);
                    enteredWidth = cells[cells.Count - 1].Location.X+1;
                    enteredHeigth = cells[cells.Count - 1].Location.Y+1;
                    field = new Field(enteredWidth, enteredHeigth);

                    generation = 0;

                    pause = false;
                }
            }
        }
    }
}
