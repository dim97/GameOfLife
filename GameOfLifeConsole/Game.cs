using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace GameOfLifeConsole
{
    public class Game
    {
        public static bool Pause;
        private static int enteredWidth;
        private static int enteredHeigth;
        public static string FileName = "Save.json";

        public int GameNumber;
        public List<Cell> Cells;
        public Field Field;
        public int Generation;
        private int countOfLivingCells = 0;


        public Game(int numberOfTheGame)
        {
            GameNumber = numberOfTheGame;
        }

        public void Play()
        {
            Pause = false;

            SetRandomLivingCells();
            Thread game = new Thread(Life);
            Thread buttonHandler = new Thread(ButtonHandler);

            game.Start();
            buttonHandler.Start();

        }

        public void Life()
        {
            Generation = 0;
            while (true)
                if (Pause == false)
                {

                    CountLivingCells();

                    Generation++;

                    //Processing next generation
                    foreach (Cell cell in Cells)
                    {
                        int neighborsCount = CountLivingNeighbors(cell);

                        if ((neighborsCount < 2) || (neighborsCount > 3))
                        {
                            cell.IsAlive = false;
                        }
                        if ((neighborsCount == 3) && (cell.IsAlive == false))
                        {
                            cell.IsAlive = true;
                        }
                        if ((neighborsCount == 2) && (cell.IsAlive == false))
                        {
                            cell.IsAlive = false;
                        }

                    }

                    Thread.Sleep(1000);
                }
        }

        public int CountLivingNeighbors(Cell cell)
        {
            int count = 0;
            List<Point> neighborsPositions = cell.GetNeighbors(enteredWidth, enteredHeigth);
            foreach (Point neighbor in neighborsPositions)
            {
                if (Field.CellsToDraw[neighbor.Y, neighbor.X] == true)
                {
                    count++;
                }
            }

            return count;
        }

        public static void RequestGameOptions()
        {
            string inputWidth = "";
            string inputHeigth = "";

            while (!Regex.IsMatch(inputWidth, @"^\d+$"))
            {
                Console.Write("Please enter width of field  -> ");
                inputWidth = Console.ReadLine();
            }
            Int32.TryParse(inputWidth, out enteredWidth);

            while (!Regex.IsMatch(inputHeigth, @"^\d+$"))
            {
                Console.Write("Please enter heigth of field  -> ");
                inputHeigth = Console.ReadLine();
            }
            Int32.TryParse(inputHeigth, out enteredHeigth);
            if ((enteredWidth < 3) || (enteredWidth >= 1000))
            {
                enteredWidth = 3;
            }
            if ((enteredHeigth < 3) || (enteredHeigth >= 1000))
            {
                enteredHeigth = 3;
            }
            Console.Clear();
        }

        public void SetGameOptions()
        {
            Field = new Field(enteredWidth, enteredHeigth);

            Cells = new List<Cell>();

            for (int i = 0; i < enteredHeigth; i++)
            {
                for (int j = 0; j < enteredWidth; j++)
                {
                    Cells.Add(new Cell(j, i));
                }
            }
        }

        public void CountLivingCells()
        {
            countOfLivingCells = 0;

            foreach (Cell cell in Cells)
            {
                if (cell.IsAlive)
                {
                    Field.CellsToDraw[cell.Location.Y, cell.Location.X] = true;
                    countOfLivingCells++;
                }
                else
                {
                    Field.CellsToDraw[cell.Location.Y, cell.Location.X] = false;
                }
            }
        }

        public string GetFieldString()
        {
            //For Drawing 
            string output = "";
            output += "Game number: " + GameNumber + "\n"
            + "Current generation: " + Generation + "\n"
            + "Count of living cells: " + countOfLivingCells + "\n";

            for (int i = 0; i < Field.Heigth; i++)
            {
                for (int j = 0; j < Field.Width; j++)
                {
                    if (Field.CellsToDraw[i, j])
                    {
                        output += '#';
                    }
                    else
                    {
                        output += '.';
                    }
                    output += ' ';
                }
                output += "\n";
            }
            return output;
        }

        public void SetRandomLivingCells()
        {
            Random random = new Random(GameNumber);
            foreach (Cell cell in Cells)
            {
                if (random.Next(2) == 0)
                {
                    cell.IsAlive = false;
                }
                else
                {
                    cell.IsAlive = true;
                }
                Thread.Sleep(3);
            }
        }

        public void ButtonHandler()
        {

            ConsoleKeyInfo keyinfo;
            while (true)
            {
                //Pause Key: "Spacebar"
                keyinfo = Console.ReadKey();
                if (keyinfo.Key == ConsoleKey.Spacebar)
                {
                    Pause = !Pause;
                }

                //Exit Key: "Escape"
                if (keyinfo.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }

                //Save Key: "S"
                if (keyinfo.Key == ConsoleKey.S)
                {
                    string output = JsonConvert.SerializeObject(Cells);

                    using (StreamWriter sw = new StreamWriter(FileName))
                    {
                        sw.Write(output);
                    }
                }
                //Load Key: "L"
                if (keyinfo.Key == ConsoleKey.L)
                {
                    string input;

                    using (StreamReader sr = new StreamReader(FileName))
                    {
                        input = sr.ReadLine();
                    }

                    Pause = true;
                    Cells.Clear();
                    Cells = JsonConvert.DeserializeObject<List<Cell>>(input);
                    enteredWidth = Cells[Cells.Count - 1].Location.X + 1;
                    enteredHeigth = Cells[Cells.Count - 1].Location.Y + 1;
                    Field = new Field(enteredWidth, enteredHeigth);

                    Generation = 0;

                    Pause = false;
                }
            }

        }
    }
}
