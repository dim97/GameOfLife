using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Drawing;

namespace GameOfLifeConsole
{
    public class Game
    {
        public bool IsDrawing { get; set; }
        public int GameNumber { get; set; }
        static int _enteredWidth; 
        static int _enteredHeigth;
        public List<Cell> Cells { get; set; }
        public Field Field { get; set; }
        public int Generation { get; set; }
        public static bool Pause { get; set; }
        int countOfLivingCells = 0;


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
                    //Drawing current generation
                    if (IsDrawing)
                    {
                        Console.Clear();
                        CountLivingCells();
                        DrawField();
                    }

                    Generation++;

                    //Processing next generation
                    foreach (Cell cell in Cells)
                    {
                        int neighborsCount = CountLivingNeighbors(cell);

                        if ((neighborsCount < 2) || (neighborsCount > 3)) cell.IsAlive = false;
                        if ((neighborsCount == 3) && (cell.IsAlive == false)) cell.IsAlive = true;
                        if ((neighborsCount == 2) && (cell.IsAlive == false)) cell.IsAlive = false;

                    }

                    Thread.Sleep(1000);
                }
        }

        public int CountLivingNeighbors(Cell cell)
        {
            int count = 0;
            List<Point> neighborsPositions = cell.GetNeighbors(_enteredWidth, _enteredHeigth);
            foreach (Point neighbor in neighborsPositions)
            {
                if (Field.CellsToDraw[neighbor.Y, neighbor.X] == true)
                    count++;
            }

            return count;
        }

        public static void RequestGameOptions()
        {
            Console.Write("\nPlease enter width of field -> ");
            string inputWidth = Console.ReadLine();
            Int32.TryParse(inputWidth, out _enteredWidth);

            Console.Write("\nPlease enter heigth of field -> ");
            string inputHeigth = Console.ReadLine();
            Int32.TryParse(inputHeigth, out _enteredHeigth);
            Console.Clear();
        }

        public void SetGameOptions()
        {
            Field = new Field(_enteredWidth, _enteredHeigth);

            Cells = new List<Cell>();

            for (int i = 0; i < _enteredHeigth; i++)
            {
                for (int j = 0; j < _enteredWidth; j++)
                {
                    Cells.Add(new Cell(j, i));
                }
            }

            //List<Point> o = cells[34].GetNeighbors(enteredWidth, enteredHeigth);
            //o.ToString();
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
                else Field.CellsToDraw[cell.Location.Y, cell.Location.X] = false;
            }
        }

        public void DrawField()
        {
            //Drawing
            Console.WriteLine("Game number: " + GameNumber + "\n");
            Console.WriteLine("Current generation: " + Generation);
            Console.WriteLine("Count of living cells: " + countOfLivingCells + "\n");           

            for (int i = 0; i < Field.Heigth; i++)
            {
                for (int j = 0; j < Field.Width; j++)
                {
                    if (Field.CellsToDraw[i, j])
                        Console.Write('#');
                    else
                        Console.Write('.');
                    Console.Write(' ');
                }
                Console.Write("\n");
            }
            //Console.Read();
        }

        public void SetRandomLivingCells()
        {
            Random random = new Random();
            foreach (Cell cell in Cells)
            {
                if (random.Next(2) == 0)
                    cell.IsAlive = false;
                else cell.IsAlive = true;
            }
        }

        public void ButtonHandler()
        {
            if (IsDrawing)
            {
                ConsoleKeyInfo keyinfo;
                while (true)
                {
                    //Pause Key: "Spacebar"
                    keyinfo = Console.ReadKey();
                    if ((keyinfo.Key == ConsoleKey.Spacebar))
                        Pause = !Pause;

                    //Exit Key: "Escape"
                    if (keyinfo.Key == ConsoleKey.Escape)
                        Environment.Exit(0);

                    //Save Key: "S"
                    if (keyinfo.Key == ConsoleKey.S)
                    {
                        string fileName = "Save.json";
                        string output = JsonConvert.SerializeObject(Cells);

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

                        Pause = true;
                        Cells.Clear();
                        Cells = JsonConvert.DeserializeObject<List<Cell>>(input);
                        _enteredWidth = Cells[Cells.Count - 1].Location.X + 1;
                        _enteredHeigth = Cells[Cells.Count - 1].Location.Y + 1;
                        Field = new Field(_enteredWidth, _enteredHeigth);

                        Generation = 0;

                        Pause = false;
                    }
                }
            }
        }
    }
}
