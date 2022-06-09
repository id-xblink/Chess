using System;

namespace ChessRules
{
	/// <summary>
	/// Ценность позиций фигур
	/// </summary>
	class EvaluationOnSquare
	{
		public int[,] kingLate { get; private set; }
		public int[,] kingMid { get; private set; }
		public int[,] queen { get; private set; }
		public int[,] rook { get; private set; }
		public int[,] bishop { get; private set; }
		public int[,] knight { get; private set; }
		public int[,] pawn { get; private set; }

		public EvaluationOnSquare()
		{
			kingLate = new int[,]
			{
				{-50,-40,-30,-20,-20,-30,-40,-50},
				{-30,-20,-10,  0,  0,-10,-20,-30},
				{-30,-10, 20, 30, 30, 20,-10,-30},
				{-30,-10, 30, 40, 40, 30,-10,-30},
				{-30,-10, 30, 40, 40, 30,-10,-30},
				{-30,-10, 20, 30, 30, 20,-10,-30},
				{-30,-30,  0,  0,  0,  0,-30,-30},
				{-50,-30,-30,-30,-30,-30,-30,-50}
			};
			kingMid = new int[,]
			{
				{-30,-40,-40,-50,-50,-40,-40,-30},
				{-30,-40,-40,-50,-50,-40,-40,-30},
				{-30,-40,-40,-50,-50,-40,-40,-30},
				{-30,-40,-40,-50,-50,-40,-40,-30},
				{-20,-30,-30,-40,-40,-30,-30,-20},
				{-10,-20,-20,-20,-20,-20,-20,-10},
				{ 20, 20,  0,  0,  0,  0, 20, 20},
				{ 20, 30, 10,  0,  0, 10, 30, 20}
			};
			queen = new int[,]
			{
				{-20,-10,-10, -5, -5,-10,-10,-20},
				{-10,  0,  0,  0,  0,  0,  0,-10},
				{-10,  0,  5,  5,  5,  5,  0,-10},
				{ -5,  0,  5,  5,  5,  5,  0, -5},
				{  0,  0,  5,  5,  5,  5,  0, -5},
				{-10,  5,  5,  5,  5,  5,  0,-10},
				{-10,  0,  5,  0,  0,  0,  0,-10},
				{-20,-10,-10, -5, -5,-10,-10,-20}
			};
			rook = new int[,]
			{
				{0,  0,  0,  0,  0,  0,  0,  0},
				{5, 10, 10, 10, 10, 10, 10,  5},
				{-5,  0,  0,  0,  0,  0,  0, -5},
				{-5,  0,  0,  0,  0,  0,  0, -5},
				{-5,  0,  0,  0,  0,  0,  0, -5},
				{-5,  0,  0,  0,  0,  0,  0, -5},
				{-5,  0,  0,  0,  0,  0,  0, -5},
				{0,  0,  0,  5,  5,  0,  0,  0}
			};
			bishop = new int[,]
			{
				{-20,-10,-10,-10,-10,-10,-10,-20},
				{-10,  0,  0,  0,  0,  0,  0,-10},
				{-10,  0,  5, 10, 10,  5,  0,-10},
				{-10,  5,  5, 10, 10,  5,  5,-10},
				{-10,  0, 10, 10, 10, 10,  0,-10},
				{-10, 10, 10, 10, 10, 10, 10,-10},
				{-10,  5,  0,  0,  0,  0,  5,-10},
				{-20,-10,-10,-10,-10,-10,-10,-20},
			};
			knight = new int[,]
			{
				{-50,-40,-30,-30,-30,-30,-40,-50},
				{-40,-20,  0,  0,  0,  0,-20,-40},
				{-30,  0, 10, 15, 15, 10,  0,-30},
				{-30,  5, 15, 20, 20, 15,  5,-30},
				{-30,  0, 15, 20, 20, 15,  0,-30},
				{-30,  5, 10, 15, 15, 10,  5,-30},
				{-40,-20,  0,  5,  5,  0,-20,-40},
				{-50,-40,-30,-30,-30,-30,-40,-50},
			};
			pawn = new int[,]
			{
				{0,  0,  0,  0,  0,  0,  0,  0 },
				{50, 50, 50, 50, 50, 50, 50, 50},
				{10, 10, 20, 30, 30, 20, 10, 10},
				{5,  5, 10, 25, 25, 10,  5,  5},
				{0,  0,  0, 20, 20,  0,  0,  0},
				{5, -5,-10,  0,  0,-10, -5,  5},
				{5, 10, 10,-20,-20, 10, 10,  5},
				{0,  0,  0,  0,  0,  0,  0,  0}
			};
		}

		public int GetEvalution(FigureOnSquare figureOnSquare, bool IsLateGame)
		{
			Figure figure = figureOnSquare.figure;
			Square square = figureOnSquare.square;

			int[,] arrayUse = null;

			switch (figure)
			{
				case Figure.whiteKing:
				case Figure.blackKing:
					if (!IsLateGame)
						arrayUse = kingMid;
					else
						arrayUse = kingLate;
					break;

				case Figure.whiteQueen:
				case Figure.blackQueen:
					arrayUse = queen;
					break;

				case Figure.whiteRook:
				case Figure.blackRook:
					arrayUse = rook;
					break;

				case Figure.whiteBishop:
				case Figure.blackBishop:
					arrayUse = bishop;
					break;

				case Figure.whiteKnight:
				case Figure.blackKnight:
					arrayUse = knight;
					break;

				case Figure.whitePawn:
				case Figure.blackPawn:
					arrayUse = pawn;
					break;

				case Figure.none:
					return 0;

				default:
					return 0;
			}
			int alphaY = figure.GetColor() == Color.black ? 0 : 7;
			return arrayUse[Math.Abs(alphaY - square.y), square.x] * (figure.GetColor() == Color.black ? -1 : 1);
		}
	}
}