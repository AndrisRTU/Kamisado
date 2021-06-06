using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kamisado
{
    class GameController : IGameController
	{
		private readonly Board GameBoard;
		private readonly FigureController WhitePlayerFigureController;
        private readonly FigureController BlackPlayerFigureController;
		private readonly GameSituation GameSituation = new();

		public event Action<List<Coordinate>> MoveCalculated;
		public event GameStateHandler ReachedTheEnd;
		public event GameStateHandler NoAvailableMoves;
		public event GameHandler NewGameStarted;
		public event GameHandler FigureMoved;

		public GameController(Board gameBoard, FigureController first, FigureController second)
		{
			GameBoard = gameBoard;
			WhitePlayerFigureController = first;
			BlackPlayerFigureController = second;
		}

		private bool IsGameEnded()
		{
			Player player = GameSituation.Turn;
			FigureColor color = GameSituation.Color;
			if (player == Player.White)
			{
				Coordinate coordinate = WhitePlayerFigureController.GetFigureCoordinate(color);
				if (coordinate.Y == 0) return true;
			}
			else
			{
				Coordinate coordinate = BlackPlayerFigureController.GetFigureCoordinate(color);
				if (coordinate.Y == 7) return true;
			}
			return false;
		}

		private bool IsAvailableMoves()
		{
			byte leftBiase = 0;
			byte rightBiase = 0;
			sbyte yBiase = 0;
			FigureController Controller;
			if (GameSituation.Turn == Player.White)
			{
				Controller = WhitePlayerFigureController;
				yBiase = 1;
			}
			else
			{
				Controller = BlackPlayerFigureController;
				yBiase = -1;
			}
			Coordinate coordinate = Controller.GetFigureCoordinate(GameSituation.Color);
			if (coordinate.X == 0) leftBiase = 1;
			if (coordinate.X == 7) rightBiase = 1;
			for (sbyte i = (sbyte)(-1 + leftBiase); i <= 1 - rightBiase; i++)
			{
				if (!BlackPlayerFigureController.IsCellOccupied((byte)(coordinate.X + i), (byte)(coordinate.Y - yBiase)) &&
					!WhitePlayerFigureController.IsCellOccupied((byte)(coordinate.X + i), (byte)(coordinate.Y - yBiase))) return true;
			}
			return false;
		}

		enum Direction
		{
			Left,
			Right,
			Center,
			Down,
			Up,
		}

		private void CalculateMove(FigureColor color)
		{
			List<Coordinate> AvailableCoordinates = new();
			Coordinate figureCoordinate;
			Direction yDirection;
			if (GameSituation.Turn == Player.White)
			{
				figureCoordinate = WhitePlayerFigureController.GetFigureCoordinate(color);
				yDirection = Direction.Up;
			}
			else
			{
				figureCoordinate = BlackPlayerFigureController.GetFigureCoordinate(color);
				yDirection = Direction.Down;
			}
			CheckForMoves(Direction.Left);
			CheckForMoves(Direction.Center);
			CheckForMoves(Direction.Right);
			GameSituation.AvailableCoordinates = AvailableCoordinates;

			void CheckForMoves(Direction xDirection)
			{
				sbyte xBiase = 0;
				sbyte x = (sbyte)figureCoordinate.X;
				if (xDirection == Direction.Left) xBiase = -1;
				if (xDirection == Direction.Right) xBiase = 1;
				sbyte yBiase = (sbyte)(yDirection == Direction.Down ? 1 : -1);
				for (sbyte y = (sbyte)(figureCoordinate.Y + yBiase); y <= GameBoard.Size - 1 && y >= 0; y += yBiase)
				{
					x += xBiase;
					if (x > GameBoard.Size - 1 || x < 0) return;
					Coordinate move = new((byte)x, (byte)y);
					for (int i = 0; i < GameBoard.Size; i++)
					{
						if (WhitePlayerFigureController.Figures[i].Coordinate == move)
						{
							return;
						}
						if (BlackPlayerFigureController.Figures[i].Coordinate == move)
                        {
							return;
                        }
					}
					AvailableCoordinates.Add(move);
				}
			}
		}

        public void Restart()
        {
			WhitePlayerFigureController.Restart();
			BlackPlayerFigureController.Restart();
			GameSituation.Restart();
			NewGameStarted?.Invoke();
		}

        private bool IsProperCell(Coordinate coordinate)
		{
			foreach (Coordinate availableCoordinate in GameSituation.AvailableCoordinates)
            {
				if (coordinate == availableCoordinate) return true;
            }
			return false;
		}

		public void CellSelected(Coordinate coordinate)
		{
			FigureController controller;
			if (GameSituation.Turn == Player.White) controller = WhitePlayerFigureController;
			else controller = BlackPlayerFigureController;
			if (GameSituation.Color == FigureColor.Count)
			{
				foreach (Figure figure in controller.Figures)
				{
					if (figure.Coordinate == coordinate)
					{
						CalculateMove(figure.Color);
						GameSituation.Color = figure.Color;
						MoveCalculated?.Invoke(GameSituation.AvailableCoordinates);
						return;
					}
				}
				return;
			}
			else
			{
				if (IsProperCell(coordinate))
				{
					controller.ChangeFigureCoordinate(GameSituation.Color, coordinate.X, coordinate.Y);
					FigureMoved?.Invoke();
				}
				else return;
			}
			if (IsGameEnded())
			{
				ReachedTheEnd?.Invoke(GameSituation.Turn);
				Restart();
				return;
			}
			GameSituation.NextTurn(GameBoard.GetColor(coordinate.X, coordinate.Y));
			if (!IsAvailableMoves())
			{
				NoAvailableMoves?.Invoke(GameSituation.Turn);
				Restart();
				return;
			}
			CalculateMove(GameSituation.Color);
			MoveCalculated?.Invoke(GameSituation.AvailableCoordinates);
		}
	}
}
