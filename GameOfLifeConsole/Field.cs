using System.Collections.Generic;

namespace GameOfLifeConsole
{    

    public class Field
    {

        public int Width;
        public int Heigth;
        public bool[,] CellsToDraw;

        public Field(int w, int h)
        {
            CellsToDraw = new bool[h, w];

            Width = w;
            Heigth = h;
        }
    }
}
