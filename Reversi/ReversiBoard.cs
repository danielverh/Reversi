using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi
{
    class ReversiBoard : Panel
    {

        public Field this[int x, int y]
        {
            get => fields[x, y];
            set
            {
                fields[x, y] = value;
                this.Invalidate();
            }
        }

        public bool Help { get; set; } = true;

        // De opties voor als de hulp functie wordt gebruikt.
        public bool[,] Possible { get; set; }

        public Field[,] Fields => fields;

        public event EventHandler<FieldClickEventArgs> FieldClick;


        // Constante definitie voor de grootte van het speelveld: grootte * grootte.
        public const int BoardSize = 6;

        // Het veld in een 2D array.
        private Field[,] fields;


        private float fieldSize = 0;

        public ReversiBoard()
        {
            // Voorkomt het knipperen na het opnieuw tekenen van het veld
            DoubleBuffered = true;

            Paint += OnPaint;
            MouseClick += OnMouseClick;
            fields = new Field[BoardSize, BoardSize];
            Possible = new bool[BoardSize, BoardSize];
            fieldSize = Math.Min(Width = 400, Height = 400) / (float) BoardSize;

            int center0 = (int) Math.Floor(BoardSize / 2f);
            int center1 = center0 - 1;

            fields[center0, center1] = Field.Red;
            fields[center1, center0] = Field.Red;
            fields[center1, center1] = Field.Blue;
            fields[center0, center0] = Field.Blue;

        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            int x, y;
            x = (int) Math.Floor(e.X / fieldSize);
            y = (int) Math.Floor(e.Y / fieldSize);
            if (x >= 0 && x < fields.GetLength(0) && y >= 0 && y < fields.GetLength(1))
                FieldClick?.Invoke(this, new FieldClickEventArgs(x, y, fields[x, y]));
        }

        private void OnPaint(object sender, PaintEventArgs e)
        {
            float xPos, yPos;
            var g = e.Graphics;
            for (int x = 0; x < fields.GetLength(0); x++)
            for (int y = 0; y < fields.GetLength(1); y++)
            {
                Brush color = null;
                xPos = fieldSize * x;
                yPos = fieldSize * y;
                if (fields[x, y] == Field.Red)
                    color = Brushes.Red;
                else if (fields[x, y] == Field.Blue)
                    color = Brushes.Blue;
                if (color != null)
                    g.FillEllipse(color, xPos, yPos, fieldSize, fieldSize);

                if (Help && Possible[x, y])
                {
                    float radius = fieldSize / 2;
                    g.DrawEllipse(Pens.Black, xPos + radius / 2, yPos + radius / 2, radius, radius);
                }

                g.DrawRectangle(Pens.Black, xPos, yPos, fieldSize, fieldSize);
            }
        }
    }

    class FieldClickEventArgs : EventArgs
    {
        public FieldClickEventArgs(int x, int y, Field field)
        {
            X = x;
            Y = y;
            Field = field;
        }

        public int X { get; }
        public int Y { get; }
        public Field Field { get; }
    }

    enum Field
    {
        Empty,
        Red,
        Blue
    }
}