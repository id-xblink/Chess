using System;
using System.Collections.Generic;
using System.Text;

namespace ChessRules
{
	/// <summary>
	/// Работа с доской
	/// </summary>
	class Board
	{
		// Универсальная нотация, задающая любую позицию шахмат
		public string fen { get; private set; }
		// Массив всех фигур
		private Figure[,] figures = new Figure[8, 8];
		// Чей ход по цвету
		public Color moveColor { get; private set; }
		// После каждого хода чёрных увеличивается кол-во ходов
		public int moveNumber { get; private set; }
		// Лист чёрных превращений
		public readonly List<Figure> blackPromotions = new List<Figure>
			{
				Figure.blackQueen,
				Figure.blackRook,
				Figure.blackBishop,
				Figure.blackKnight,
			};
		// Лист белых превращений
		public readonly List<Figure> whitePromotions = new List<Figure>
			{
				Figure.whiteQueen,
				Figure.whiteRook,
				Figure.whiteBishop,
				Figure.whiteKnight,
			};

		/// <summary>
		/// Инициализация по фену
		/// </summary>
		/// <param name="fen">Фен</param>
		public Board(string fen)
		{
			this.fen = fen;
			// "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"
			// 0										   1 2	  3 4 5
			// 0 - расположение фигур; 1 - кто ходит; 2 - флаги рокировки; 3 - битое поле; 4 - кол-во ходов для правила 50 ходов; 5 - номер хода сейчас
			string[] parts = fen.Split();
			if (parts.Length != 6)
				return;
			InitFigures(parts[0]);
			moveColor = (parts[1] == "b") ? Color.black : Color.white;
			moveNumber = int.Parse(parts[5]);
		}

		/// <summary>
		/// Инициализация фигур по нулевой (первой части)
		/// </summary>
		/// <param name="data">Расположение фигур</param>
		private void InitFigures(string data)
		{
			for (int j = 8; j >= 2; j--)
				data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
			data = data.Replace("1", ".");
			string[] lines = data.Split('/');
			for (int y = 7; y >= 0; y--)
				for (int x = 0; x < 8; x++)
					figures[x, y] = lines[7 - y][x] == '.' ? Figure.none : (Figure)lines[7 - y][x];
		}

		/// <summary>
		/// Перебор всех фигур текущего цвета
		/// </summary>
		/// <returns></returns>
		public IEnumerable<FigureOnSquare> YieldFigures()
		{
			foreach (Square square in Square.YieldSquares())
				if (GetFigureAt(square).GetColor() == moveColor)
					yield return new FigureOnSquare(GetFigureAt(square), square);
		}

		/// <summary>
		/// Перебор всех фигур на доске
		/// </summary>
		/// <returns></returns>
		public IEnumerable<FigureOnSquare> GetFiguresOnSquare()
		{
			foreach (Square square in Square.YieldSquares())
				if (GetFigureAt(square).GetColor() != Color.none)
					yield return new FigureOnSquare(GetFigureAt(square), square);
		}

		/// <summary>
		/// Генерация fen по текущим ситуациям на доске 
		/// </summary>
		private void GenerateFen(string flags, string intercept, string fiftyMove)
		{
			fen = $"{FenFigures()} {(moveColor == Color.white ? "w" : "b")} {flags} {intercept} {fiftyMove} {moveNumber}";
		}

		/// <summary>
		/// Генерация участка fen для фигур по текущим расположениям фигур
		/// </summary>
		/// <returns></returns>
		private string FenFigures()
		{
			StringBuilder sb = new StringBuilder();
			for (int y = 7; y >= 0; y--)
			{
				for (int x = 0; x < 8; x++)
					sb.Append(figures[x, y] == Figure.none ? '1' : (char)figures[x, y]);
				if (y > 0)
					sb.Append('/');
			}
			string eight = "11111111";
			for (int j = 8; j >= 2; j--)
				sb.Replace(eight.Substring(0, j), j.ToString());
			return sb.ToString();
		}

		/// <summary>
		/// Получение любой фигуры по заданной клетке
		/// </summary>
		/// <param name="square">Клетка</param>
		/// <returns></returns>
		public Figure GetFigureAt(Square square)
		{
			if (square.OnBoard())
				return figures[square.x, square.y];
			return Figure.none;
		}

		/// <summary>
		/// Установка фигур
		/// </summary>
		/// <param name="square">Клетка</param>
		/// <param name="figure">Фигура</param>
		private void SetFigureAt(Square square, Figure figure)
		{
			if (square.OnBoard())
				figures[square.x, square.y] = figure;
		}

		/// <summary>
		/// Реализация хода
		/// </summary>
		/// <param name="fm">Перемещение фигуры</param>
		/// <returns></returns>
		public Board Move(FigureMoving fm)
		{
			Board next = new Board(fen);
			// Игровые параметры
			string[] parts = fen.Split();
			string flags = parts[2];
			string intercept = parts[3];
			int fiftyMove = Convert.ToInt32(parts[4]) + 1;
			// Обнуление 50 при невозвратимом ходе
			if (fm.figure == Figure.blackPawn || fm.figure == Figure.whitePawn || next.GetFigureAt(fm.to) != Figure.none)
				fiftyMove = 0;
			next.SetFigureAt(fm.from, Figure.none);
			next.SetFigureAt(fm.to, fm.promotion == Figure.none ? fm.figure : fm.promotion);
			// Битое поле
			if (EnPassant(fm, ref intercept))
				next.SetFigureAt(new Square(fm.to.x, fm.to.y + (fm.figure.GetColor() == Color.white ? -1 : 1)), Figure.none);
			// Рокировка
			Castling(fm, ref flags, next);
			// Изменение счётчика хода
			if (moveColor == Color.black)
				next.moveNumber++;
			next.moveColor = moveColor.FlipColor();
			next.GenerateFen(flags, intercept, fiftyMove.ToString());
			return next;
		}

		/// <summary>
		/// Проверка битого поля
		/// </summary>
		/// <param name="fm">Движение фигуры</param>
		/// <param name="intercept">Битое поле</param>
		/// <returns></returns>
		private bool EnPassant(FigureMoving fm, ref string intercept)
		{
			//Если есть битое поле
			if (intercept != "-")
			{
				Square square = new Square(intercept);
				intercept = "-";
				//Если используется битое поле
				if (fm.to == square)
					if (fm.figure == Figure.blackPawn || fm.figure == Figure.whitePawn) //Ходит пешка
						return true;
			}
			//Проверка хода пешки
			if ((fm.figure == Figure.blackPawn || fm.figure == Figure.whitePawn) && fm.AbsDeltaY == 2)
			{
				Figure pawn = moveColor == Color.black ? Figure.whitePawn : Figure.blackPawn;
				//Есть ли по бокам пешки?
				if (GetFigureAt(new Square(fm.to.x - 1, fm.to.y)) == pawn || GetFigureAt(new Square(fm.to.x + 1, fm.to.y)) == pawn)
					intercept = new Square(fm.to.x, fm.to.y + (fm.figure.GetColor() == Color.white ? -1 : 1)).Name;
			}
			return false;
		}

		/// <summary>
		/// Проверки на рокировку
		/// </summary>
		/// <param name="alphaY">Строка, определяемая цветом ходящих фигур</param>
		/// <param name="fm">Движение фигуры</param>
		/// <param name="flags">Флаги рокировок</param>
		/// <param name="next">Доска</param>
		private void Castling(FigureMoving fm, ref string flags, Board next)
		{
			if (flags == "-")
				return;
			int alphaY = moveColor == Color.black ? 7 : 0;
			// Проверка хода короля
			if (fm.figure == (moveColor == Color.black ? Figure.blackKing : Figure.whiteKing))
			{
				// Проверка рокировки
				if (fm.AbsDeltaX == 2)
				{
					if (fm.SignX > 0) // k рокировка
						next.SetFigureAt(new Square(7, alphaY), Figure.none);
					else // q рокировка
						next.SetFigureAt(new Square(0, alphaY), Figure.none);
					next.SetFigureAt(new Square(fm.to.x + (fm.SignX * -1), alphaY), moveColor == Color.black ? Figure.blackRook : Figure.whiteRook);
				}
				// Снятие флагов
				flags = moveColor == Color.black ? flags.Replace("q", "") : flags.Replace("Q", "");
				flags = moveColor == Color.black ? flags.Replace("k", "") : flags.Replace("K", "");
			}
			// Проверка хода ладьи
			if (fm.figure == (moveColor == Color.black ? Figure.blackRook : Figure.whiteRook))
			{
				// Проверка позиций ладьи
				if (fm.from.y == alphaY && fm.from.x == 0) // Снятие q флага
					flags = moveColor == Color.black ? flags.Replace("q", "") : flags.Replace("Q", "");
				if (fm.from.y == alphaY && fm.from.x == 7) // Снятие k флага
					flags = moveColor == Color.black ? flags.Replace("k", "") : flags.Replace("K", "");
			}

			// Флаги рокировок
			bool CanLongCastle = moveColor == Color.black ? flags.Contains("Q") : flags.Contains("q");
			bool CanShortCastle = moveColor == Color.black ? flags.Contains("K") : flags.Contains("k");

			// Если была атакована вражеская ладья с наличием соответствующего флага
			alphaY = moveColor == Color.black ? 0 : 7;
			if (CanLongCastle)
				if (fm.to.y == alphaY && fm.to.x == 0)
					flags = moveColor == Color.black ? flags.Replace("Q", "") : flags.Replace("q", "");
			if (CanShortCastle)
				if (fm.to.y == alphaY && fm.to.x == 7)
					flags = moveColor == Color.black ? flags.Replace("K", "") : flags.Replace("k", "");

			if (flags == "")
				flags = "-";
		}

		/// <summary>
		/// Можно ли съесть короля
		/// </summary>
		/// <returns></returns>
		private bool CanEatKing()
		{
			Square badKing = FindBadKing();
			Moves moves = new Moves(this);
			foreach (FigureOnSquare fs in YieldFigures())
			{
				FigureMoving fm = new FigureMoving(fs, badKing);
				if (fs.figure == (moveColor == Color.black ? Figure.blackPawn : Figure.whitePawn))
				{
					foreach (Figure promotion in (moveColor == Color.black ? blackPromotions : whitePromotions))
					{
						fm = new FigureMoving(fs, badKing, promotion);
						if (moves.CanMove(fm))
							return true;
					}
				}
				else
				{
					fm = new FigureMoving(fs, badKing);
					if (moves.CanMove(fm))
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Нахождение координаты плохого (чужого) короля
		/// </summary>
		/// <returns></returns>
		private Square FindBadKing()
		{
			Figure badKing = moveColor == Color.black ? Figure.whiteKing : Figure.blackKing;
			foreach (Square square in Square.YieldSquares())
				if (GetFigureAt(square) == badKing)
					return square;
			return Square.none;
		}

		/// <summary>
		/// Проверка шаха
		/// </summary>
		/// <returns></returns>
		public bool IsCheck()
		{
			Board after = new Board(fen);
			after.moveColor = moveColor.FlipColor();
			return after.CanEatKing();
		}

		/// <summary>
		/// Есть ли шах после хода
		/// </summary>
		/// <param name="fm">Передвижение фигуры</param>
		/// <returns></returns>
		public bool IsCheckAfterMove(FigureMoving fm)
		{
			Board after = Move(fm);
			return after.CanEatKing();
		}
	}
}