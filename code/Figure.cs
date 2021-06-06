using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    class Figure
    {
        public readonly FigureColor Color;
        public Coordinate Coordinate { get; private set; } = new();

        public Figure(FigureColor color, byte x = 0, byte y = 0)
        {
            Color = color;
            Coordinate.X = x;
            Coordinate.Y = y;
        }
    }
}
