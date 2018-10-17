using System.Collections.Generic;

namespace GameOfLifeConsole
{
    public class Cell
    {
        bool _isAlive;
        Point _location;

        public bool IsAlive { get => _isAlive; set => _isAlive = value; }
        public Point Location { get => _location; set => _location = value; }

        public Cell()
        {
        }
        public Cell(int x, int y)
        {
            Location = new Point(x, y);
            Location.X = x;
            Location.Y = y;
        }

        public List<Point> GetNeighbors(int width, int heigth)
        {
            List<Point> neighbors = new List<Point>();
            neighbors.Add(new Point(Location.X - 1, Location.Y - 1));
            neighbors.Add(new Point(Location.X, Location.Y - 1));
            neighbors.Add(new Point(Location.X + 1, Location.Y - 1));
            neighbors.Add(new Point(Location.X - 1, Location.Y));
            neighbors.Add(new Point(Location.X + 1, Location.Y));
            neighbors.Add(new Point(Location.X - 1, Location.Y + 1));
            neighbors.Add(new Point(Location.X, Location.Y + 1));
            neighbors.Add(new Point(Location.X + 1, Location.Y + 1));

            List<Point> processedNeighbors = new List<Point>();
            foreach (Point neighbor in neighbors)
                if (!((neighbor.X < 0) || (neighbor.X >= width) || (neighbor.Y < 0) || (neighbor.Y >= heigth)))
                    processedNeighbors.Add(neighbor);
            return processedNeighbors;
        }
    }

    public class Point
    {
        int _x;
        int _y;

        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
