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
       // public static string FileName = "Save.json";

        public int GameNumber;
        public List<Cell> Cells;
        public Field Field;
        public int Generation;
        public int CountOfLivingCells = 0;


        public Game(int numberOfTheGame)
        {
            GameNumber = numberOfTheGame;
        }

        public void Play()
        {
            Pause = false;
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
                    List<Cell> oldCells = new List<Cell>(Cells);
                    
                    for (int i = 0; i < Cells.Count; i++)
                    {
                        int neighborsCount = CountLivingNeighbors(oldCells[i]);

                        if ((neighborsCount < 2) || (neighborsCount > 3))
                        {
                            Cells[i].IsAlive = false;
                        }
                        if ((neighborsCount == 3) && !oldCells[i].IsAlive)
                        {
                            Cells[i].IsAlive = true;
                        }
                        if ((neighborsCount == 2) && !oldCells[i].IsAlive)
                        {
                            Cells[i].IsAlive = false;
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
            SetRandomLivingCells();
        }

        public void CountLivingCells()
        {
            CountOfLivingCells = 0;

            foreach (Cell cell in Cells)
            {
                if (cell.IsAlive)
                {
                    Field.CellsToDraw[cell.Location.Y, cell.Location.X] = true;
                    CountOfLivingCells++;
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
            output += "Game number: " + GameNumber + Environment.NewLine + "Current generation: " + Generation + Environment.NewLine + "Count of living cells: " + CountOfLivingCells + Environment.NewLine;

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
                //Thread.Sleep(3);
            }
        }

    }
}
