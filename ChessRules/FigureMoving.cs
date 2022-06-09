using System;

namespace ChessRules
{
	/// <summary>
	/// Класс для передвижения фигур
	/// </summary>
	class FigureMoving
	{
		// Фигура
		public Figure figure { get; private set; }
		// Откуда идёт (где находится фигура)
		public Square from { get; private set; }
		// Куда идёт (где будет находиться)
		public Square to { get; private set; }
		// Превращение пешки в другую фигуру
		public Figure promotion { get; private set; }

		/// <summary>
		/// Инициализация класса
		/// </summary>
		/// <param name="fs">Контейнер фигура в квадрате</param>
		/// <param name="to">Куда движется</param>
		/// <param name="promotion">Превращение</param>
		public FigureMoving(FigureOnSquare fs, Square to, Figure promotion = Figure.none)
		{
			figure = fs.figure;
			from = fs.square;
			this.to = to;
			this.promotion = promotion;
		}

		/// <summary>
		/// Принятие хода в текстовом варианте
		/// </summary>
		/// <param name="move">Текстовое название хода</param>
		public FigureMoving(string move) //Pe2e4	Pe7e8Q
		{                                //01234	012345
			figure = (Figure)move[0];
			from = new Square(move.Substring(1, 2));
			to = new Square(move.Substring(3, 2));
			//Если пешка превращается : никакая фигура
			promotion = (move.Length == 6) ? (Figure)move[5] : Figure.none;
		}

		// Разница координат
		public int DeltaX { get { return to.x - from.x; } }
		public int DeltaY { get { return to.y - from.y; } }

		// Модуль разницы
		public int AbsDeltaX { get { return Math.Abs(DeltaX); } }
		public int AbsDeltaY { get { return Math.Abs(DeltaY); } }

		// Знак числа разницы (направление)
		public int SignX { get { return Math.Sign(DeltaX); } }
		public int SignY { get { return Math.Sign(DeltaY); } }

		/// <summary>
		/// Формирация FigureMoving в виде строчки
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string text = (char)figure + from.Name + to.Name;
			if (promotion != Figure.none)
				text += (char)promotion;
			return text;
		}
	}
}