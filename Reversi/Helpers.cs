using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    static class Helpers
    {
        public static Field[] GetColumn(this Field[,] fields, int x)
        {
            var column = new Field[fields.GetLength(0)];
            for (int i = 0; i < column.Length; i++)
            {
                column[i] = fields[x, i];
            }

            return column;
        }

        public static Field[] GetRow(this Field[,] fields, int y)
        {
            var row = new Field[fields.GetLength(0)];
            for (int i = 0; i < row.Length; i++)
            {
                row[i] = fields[i, y];
            }

            return row;
        }

        public static (Item[] One, Item[] Two) GetDiagonals(this Field[,] fields, int _x, int _y)
        {
            var d = GetAllDiagonals(fields);
            var output = new List<Item[]>();
            foreach (var item in d)
            {
                if (item.Select(x => x.X == _x && x.Y == _y).Any())
                {
                    output.Add(item.ToArray());
                }
            }

            return (output[0], output[1]);
        }

        private static List<Item>[] GetAllDiagonals(Field[,] fields)
        {
            var list = new List<List<Item>>();
            for (int i = 0; i < ReversiBoard.BoardSize; i++)
            {
                var items = new List<Item>();
                int x = i, y = 0;
                var max = ReversiBoard.BoardSize - i;
                for (int j = 0; j < max; j++)
                {
                    items.Add(new Item(x, y, fields[x, y]));
                    x++;
                    y++;
                }

                list.Add(items);
                items = new List<Item>();
                x = 0;
                y = i;
                max = i + 1;
                for (int j = 0; j < max; j++)
                {
                    items.Add(new Item(x, y, fields[x, y]));
                    x++;
                    y--;
                }

                list.Add(items);
            }

            return list.ToArray();
        }

        public class Item
        {
            public Item(int x, int y, Field f)
            {
                X = x;
                Y = y;
                Field = f;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public Field Field { get; set; }
        }
    }
}