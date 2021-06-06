using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kamisado
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Board gameBoard = new();
            FigureController whitePlayerFigureController = new(Player.White);
            FigureController blackPlayerFigureController = new(Player.Black);
            IGameController game = new GameController(gameBoard, whitePlayerFigureController, blackPlayerFigureController);
            Form1 form = new(gameBoard, whitePlayerFigureController, blackPlayerFigureController);
            form.CellIsClicked += game.CellSelected;
            game.MoveCalculated += form.SaveMoves;
            game.FigureMoved += form.DrawGame;
            game.ReachedTheEnd += form.GameEnded;
            game.NoAvailableMoves += form.GameEnded;
            game.NewGameStarted += form.NewGame;
            game.Restart();

            Application.Run(form);
        }
    }
}
