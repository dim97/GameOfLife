using System.Collections.Generic;

namespace GameOfLifeConsole
{    

    public class Field
    {
        int _width;
        int _heigth;
        private bool[,] _cells;

        public int Width { get => _width; set => _width = value; }
        public int Heigth { get => _heigth; set => _heigth = value; }
        public bool[,] CellsToDraw { get => _cells; set => _cells = value; }

        public Field(int w, int h)
        {
            CellsToDraw = new bool[h, w];
            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    CellsToDraw[i, j] = false;
                }
            }

            Width = w;
            Heigth = h;
        }
    }
}
