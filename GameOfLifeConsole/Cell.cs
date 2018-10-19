using System.Collections.Generic;
using System.Drawing;

namespace GameOfLifeConsole
{
    public class Cell
    {

        public bool IsAlive;
        public Point Location;

        public Cell()
        {
        }
        public Cell(int x, int y)
        {
            Location = new Point(x, y);
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
                {
                    processedNeighbors.Add(neighbor);
                }
            return processedNeighbors;
        }
    }
}
