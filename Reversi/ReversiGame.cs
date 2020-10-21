using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Reversi
{
    class ReversiGame
    {
        private bool turnBlue;
        public Field Turn => turnBlue ? Field.Blue : Field.Red;
        public EventHandler TurnChanged;
        public ReversiBoard Board { get; }

        public ReversiGame(ReversiBoard board)
        {
            Board = board;
            Board.FieldClick += BordOnFieldClick;
        }

        private void BordOnFieldClick(object sender, FieldClickEventArgs e)
        {
            if (e.Field != Field.Empty) // Alleen het click event gebruiken als het geklikte veld leeg is.
                return;
            var field = turnBlue ? Field.Blue : Field.Red;
            if (!ValidMove(e.X, e.Y, Turn))
                return;

            Board[e.X, e.Y] = field;
            // Keer de waarde om:
            turnBlue ^= true;

            if (Board.Help)
            {
                var helper = new bool[ReversiBoard.BoardSize, ReversiBoard.BoardSize];
                for (var x = 0; x < ReversiBoard.BoardSize; x++)
                for (var y = 0; y < ReversiBoard.BoardSize; y++)
                {
                    helper[x, y] = ValidMove(x, y, Turn);
                }

                Board.Possible = helper;
            }
        }


        private bool ValidMove(int _x, int _y, Field player)
        {
            Field op = player == Field.Blue ? Field.Red : Field.Blue;
            Field[] yAxis = Board.Fields.GetColumn(_x);
            Field[] xAxis = Board.Fields.GetRow(_y);

            var diagAxis = Board.Fields.GetDiagonals(_x, _y);
            bool dResult = CheckDiagonalAxis(diagAxis.One, player, _x, _y) ||
                           CheckDiagonalAxis(diagAxis.Two, player, _x, _y);

            bool yResult = CheckAxis(yAxis, player, _y);
            bool xResult = CheckAxis(xAxis, player, _x);
            return xResult || yResult;
        }

        private bool CheckDiagonalAxis(Helpers.Item[] i, Field player, int x, int y)
        {
            int index = i.ToList().FindIndex(z => z.X == x && z.Y == y);
            return CheckAxis(i.Select(z => z.Field).ToArray(), player, index);
        }

        private bool CheckAxis(Field[] axis, Field player, int iInAxis)
        {
            var op = player == Field.Blue ? Field.Red : Field.Blue;
            bool hasOp = false, hasPlayer = false;

            for (int i = 0; i < axis.Length; i++)
            {
                if (axis[i] == player)
                    hasPlayer = true;
                else if (i == iInAxis)
                {
                }
                else continue;

                int opCount = 0;
                i++;
                while (i < axis.Length && axis[i] == op)
                {
                    opCount++;
                    i++;
                }

                if (i < axis.Length && (opCount > 0 && ((axis[i] == player) || (hasPlayer && i == iInAxis))))
                    return true;
            }

            return false;
        }
    }
}