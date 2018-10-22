using System;

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

        public string GetFieldInString()
        {
            //For Drawing 
            string output = "";

            for (int i = 0; i < Heigth; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (CellsToDraw[i, j])
                    {
                        output += '■';
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

        public void DrawFieldToConsole()
        {
            Console.Write(GetFieldInString());
        }
    }
}
