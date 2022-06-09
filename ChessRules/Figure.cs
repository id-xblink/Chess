using System;

namespace ChessRules
{
	/// <summary>
	/// Перечисление фигур
	/// </summary>
	public enum Figure
	{
		none = '.',

		whiteKing = 'K',
		whiteQueen = 'Q',
		whiteRook = 'R',
		whiteBishop = 'B',
		whiteKnight = 'N',
		whitePawn = 'P',

		blackKing = 'k',
		blackQueen = 'q',
		blackRook = 'r',
		blackBishop = 'b',
		blackKnight = 'n',
		blackPawn = 'p',
	}

	/// <summary>
	/// Создание статичного класса для создания методов перечисления
	/// </summary>
	static class FigureMethods
	{
		/// <summary>
		/// Получение цвета фигуры
		/// </summary>
		/// <param name="figure">Фигура</param>
		/// <returns></returns>
		public static Color GetColor(this Figure figure)
		{
			if (figure == Figure.none)
				return Color.none;
			return (figure == Figure.whiteKing || figure == Figure.whiteQueen || figure == Figure.whiteRook ||
					figure == Figure.whiteBishop || figure == Figure.whiteKnight || figure == Figure.whitePawn)
					? Color.white : Color.black;
		}

		/// <summary>
		/// Получение ценности фигуры
		/// </summary>
		/// <param name="figure">Фигура</param>
		/// <returns></returns>
		public static int GetEvaluation(this Figure figure, bool IsShortFormat)
		{
			int points = 0;
			switch (figure)
			{
				case Figure.whiteKing:
					points = 20000;
					break;
				case Figure.blackKing:
					points = -20000;
					break;

				case Figure.whiteQueen:
					points = 900;
					break;
				case Figure.blackQueen:
					points = -900;
					break;

				case Figure.whiteRook:
					points = 500;
					break;
				case Figure.blackRook:
					points = -500;
					break;

				case Figure.whiteBishop:
					if (IsShortFormat)
						points = 300;
					else
						points = 330;
					break;
				case Figure.blackBishop:
					if (IsShortFormat)
						points = -300;
					else
						points = -330;
					break;

				case Figure.whiteKnight:
					if (IsShortFormat)
						points = 300;
					else
						points = 320;
					break;
				case Figure.blackKnight:
					if (IsShortFormat)
						points = -300;
					else
						points = -320;
					break;

				case Figure.whitePawn:
					points = 100;
					break;
				case Figure.blackPawn:
					points = -100;
					break;

				case Figure.none:
					points = 0;
					break;

				default:
					points = 0;
					break;
			}
			if (IsShortFormat && points != 0)
				points /= 100;
			if (IsShortFormat && Math.Abs(points) == 20000)
				return 0;
			else
				return points;
		}
	}
}