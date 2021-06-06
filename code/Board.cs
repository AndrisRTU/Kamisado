using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    class Board
    {
		private readonly FigureColor[,] Cells = new FigureColor[(int) FigureColor.Count, (int) FigureColor.Count];

		public byte Size { get; } = (byte)FigureColor.Count;

		public Board() => InitBoard();

		private void InitBoard()
        {
			for (int i = 0; i < Size; i++) {
				Cells[(0 + 9 * i) % (Size), i] = FigureColor.Orange;
				Cells[(1 + 3 * i) % (Size), i] = FigureColor.Blue;
				Cells[(2 + 5 * i) % (Size), i] = FigureColor.Purple;
				Cells[(3 + 7 * i) % (Size), i] = FigureColor.Pink;
				Cells[(4 + 9 * i) % (Size), i] = FigureColor.Yellow;
				Cells[(5 + 3 * i) % (Size), i] = FigureColor.Red;
				Cells[(6 + 5 * i) % (Size), i] = FigureColor.Green;
				Cells[(7 + 7 * i) % (Size), i] = FigureColor.Brown;
			}
		}

		public FigureColor GetColor(byte x, byte y) => Cells[x, y];
    }
}
