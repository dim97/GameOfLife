using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;

namespace GameOfLifeConsole
{
    public class Game
    {
        public static bool Pause;
        public static int Width;
        public static int Heigth;
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

            Life();
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
                        if ((neighborsCount == 3) && !cell.IsAlive)
                        {
                            cell.IsAlive = true;
                        }
                        if ((neighborsCount == 2) && !cell.IsAlive)
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
            List<Point> neighborsPositions = cell.GetNeighbors(Width, Heigth);
            foreach (Point neighbor in neighborsPositions)
            {
                if (Field.CellsToDraw[neighbor.Y, neighbor.X])
                {
                    count++;
                }
            }

            return count;
        }

        public void SetGameOptions()
        {
            Field = new Field(Width, Heigth);

            Cells = new List<Cell>();

            for (int i = 0; i < Heigth; i++)
            {
                for (int j = 0; j < Width; j++)
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

        public string GetGameData()
        {
            //For Drawing 
            string output = "";
            output += "Game number: " + GameNumber + "\n"
            + "Current generation: " + Generation + "\n"
            + "Count of living cells: " + countOfLivingCells + "\n";

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
                    Width = Cells[Cells.Count - 1].Location.X + 1;
                    Heigth = Cells[Cells.Count - 1].Location.Y + 1;
                    Field = new Field(Width, Heigth);

                    Generation = 0;

                    Pause = false;
                }
            }

        }
    }
}
