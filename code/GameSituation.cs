using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    class GameSituation
    {
        public Player Turn { get; private set; }
        public FigureColor Color { get; set; }
        public List<Coordinate> AvailableCoordinates { get; set; } = new();

        public GameSituation() => Restart();

        public void Restart()
        {
            Random r = new();
            Turn = (Player)r.Next(0, (int)Player.Count);
            Color = FigureColor.Count;
            AvailableCoordinates.Clear();
        }

        public void NextTurn(FigureColor color)
        {
            if (Turn == Player.White) Turn = Player.Black;
            else Turn = Player.White;
            Color = color;
        }
    }
}
