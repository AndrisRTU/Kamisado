using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kamisado
{
    partial class Form1 : Form
    {
        private readonly Board GameBoard;
        private readonly IFigureController WhitePlayerFigureController;
        private readonly IFigureController BlackPlayerFigureController;
        private int TileWidth { get; set; }
        private int TileHeight { get; set; }
        private List<Coordinate> Coordinates;

        public delegate void CoordinateHandler(Coordinate coordinate);
        public event CoordinateHandler CellIsClicked;

        public Form1(Board gameBoard, IFigureController first, IFigureController second)
        {
            GameBoard = gameBoard;
            WhitePlayerFigureController = first;
            BlackPlayerFigureController = second;
            InitializeComponent();
            TileWidth = panel1.Width / (int)FigureColor.Count;
            TileHeight = panel1.Height / (int)FigureColor.Count;
        }

        public void DrawGame()
        {
            using (Graphics graphics = panel1.CreateGraphics())
            {
                CreateBoard(graphics);
                DrawFigures(graphics);
                DrawMoves(graphics);
            }
        }

        public void SaveMoves(List<Coordinate> coordinates)
        {
            Coordinates = coordinates;
            DrawGame();
        }

        private void CreateBoard(Graphics graphics)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    FigureColor color = GameBoard.GetColor((byte)x, (byte)y);
                    SolidBrush b = new(Color.FromName(color.ToString()));
                    graphics.FillRectangle(b, new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight));
                }
            }
        }

        private void DrawFigures(Graphics graphics)
        {
            DrawFigures(BlackPlayerFigureController, Player.Black, graphics);
            DrawFigures(WhitePlayerFigureController, Player.White, graphics);

            void DrawFigures(IFigureController controller, Player player, Graphics graphics)
            {
                foreach (var figures in controller.Figures)
                {
                    SolidBrush outlineBrush = new(Color.Black);
                    SolidBrush figureColor = new(Color.FromName(figures.Color.ToString()));
                    SolidBrush playerColor = new(player == Player.White ? Color.White : Color.Black);
                    graphics.FillEllipse(outlineBrush, figures.Coordinate.X * TileWidth + TileWidth / 6, figures.Coordinate.Y * TileHeight + TileHeight / 6, TileWidth / 3 * 2, TileHeight / 3 * 2);
                    graphics.FillEllipse(playerColor, figures.Coordinate.X * TileWidth + TileWidth / 18 * 5, figures.Coordinate.Y * TileHeight + TileHeight / 18 * 5, TileWidth / 9 * 5, TileHeight / 9 * 5);
                    graphics.FillEllipse(figureColor, figures.Coordinate.X * TileWidth + TileWidth / 32 * 16, figures.Coordinate.Y * TileHeight + TileHeight / 32 * 16, TileWidth / 4, TileHeight / 4);
                }
            }
        }

        public void DrawMoves(Graphics graphics)
        {
            Coordinates?.ForEach(coordinate =>
            {
                Pen p = new(Color.White);
                graphics.DrawEllipse(p, coordinate.X * TileWidth + TileWidth / 6, coordinate.Y * TileHeight + TileHeight / 6, TileWidth / 3 * 2, TileHeight / 3 * 2);
            });
        }

        public void GameEnded(Player player)
        {
            Coordinates = null;
            string whoWon = player == Player.White ? "White" : "Black";
            MessageBox.Show(whoWon + " has won the game!");
        }

        public void NewGame() => DrawGame();

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / TileWidth;
            int y = e.Y / TileHeight;
            CellIsClicked?.Invoke(new Coordinate((byte)x, (byte)y));
        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            TileWidth = panel1.Width / (int)FigureColor.Count;
            TileHeight = panel1.Height / (int)FigureColor.Count;
            DrawGame();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            DrawGame();
        }
    }
}
