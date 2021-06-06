using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kamisado
{
    class FigureController : IFigureController
    {
        public byte StartingLocation { get; }
        public byte Size { get; private set; } = (byte)FigureColor.Count;
        public Player Player { get; private set; }
        public Figure[] Figures { get; private set; } = new Figure[(byte)FigureColor.Count];

        public FigureController(Player player)
        {
            Player = player;
            if (player == Player.White) StartingLocation = 7;
            else StartingLocation = 0;
            InitFigures(player);
        }

        private void InitFigures(Player player)
        {
            if (player == Player.White)
            {
                for (byte i = 0; i < Size; i++)
                {
                    Figures[i] = new Figure((FigureColor)i, (byte)(FigureColor.Count - 1 - i), StartingLocation);
                }
            }
            else
            {
                for (byte i = 0; i < Size; i++)
                {
                    Figures[i] = new Figure((FigureColor)i, i, StartingLocation);
                }
            }
        }

        public void Restart()
        {
            if (Player == Player.White)
            {
                for (byte i = 0; i < Size; i++)
                {
                    Figures[i].Coordinate.X = (byte)(FigureColor.Count - 1 - i);
                    Figures[i].Coordinate.Y = StartingLocation;
                }
            }
            else
            {
                for (byte i = 0; i < Size; i++)
                {
                    Figures[i].Coordinate.X = i;
                    Figures[i].Coordinate.Y = StartingLocation;
                }
            }
        }

        public void ChangeFigureCoordinate(FigureColor color, byte x, byte y)
        {
            Figures[(byte)color].Coordinate.X = x;
            Figures[(byte)color].Coordinate.Y = y;
        }

        public Coordinate GetFigureCoordinate(FigureColor color) => Figures[(byte)color].Coordinate;

        public bool IsCellOccupied(byte x, byte y)
        {
            for (byte i = 0; i < Size; i++)
            {
                Coordinate coordinate = Figures[i].Coordinate;
                if (coordinate.X == x && coordinate.Y == y)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
