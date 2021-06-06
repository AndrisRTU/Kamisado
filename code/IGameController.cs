using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kamisado
{
    delegate void GameStateHandler(Player player);
    delegate void GameHandler();
    interface IGameController
    {
        event Action<List<Coordinate>> MoveCalculated;
        event GameStateHandler ReachedTheEnd;
        event GameStateHandler NoAvailableMoves;
        event GameHandler NewGameStarted;
        event GameHandler FigureMoved;
        void CellSelected(Coordinate coordinate);
        void Restart();
    }
}
