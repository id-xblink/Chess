namespace ChessRules
{
	/// <summary>
	/// Проверка правильности ходов
	/// </summary>
	class Moves
	{
		// Передвижение фигуры
		FigureMoving fm;
		// Доступ к доске
		Board board;

		/// <summary>
		/// Инициализация
		/// </summary>
		/// <param name="board">Доска</param>
		public Moves(Board board)
		{
			this.board = board;
		}

		/// <summary>
		/// Можно или нельзя ходить
		/// </summary>
		/// <param name="fm">Передвижение фигуры (ход)</param>
		/// <returns></returns>
		public bool CanMove(FigureMoving fm)
		{
			this.fm = fm;
			return
				CanMoveFrom() && CanMoveTo() && CanFigureMove();
		}

		/// <summary>
		/// Можно ли пойти с той клетки, с которой мы идём
		/// </summary>
		/// <returns></returns>
		bool CanMoveFrom()
		{
			return fm.from.OnBoard() &&
				fm.figure.GetColor() == board.moveColor;
		}

		/// <summary>
		/// Можно ли пойти на ту клетку, куда мы собрались идти
		/// </summary>
		/// <returns></returns>
		bool CanMoveTo()
		{
			return fm.to.OnBoard() &&
				fm.from != fm.to &&
				board.GetFigureAt(fm.to).GetColor() != board.moveColor;
		}

		/// <summary>
		/// Может ли фигура сделать этот ход
		/// </summary>
		/// <returns></returns>
		bool CanFigureMove()
		{
			switch (fm.figure)
			{
				case Figure.whiteKing:
				case Figure.blackKing:
					return CanKingMove();

				case Figure.whiteQueen:
				case Figure.blackQueen:
					return CanStraightMove();

				case Figure.whiteRook:
				case Figure.blackRook:
					return (fm.SignX == 0 || fm.SignY == 0) &&
						CanStraightMove();

				case Figure.whiteBishop:
				case Figure.blackBishop:
					return (fm.SignX != 0 && fm.SignY != 0) &&
						CanStraightMove();

				case Figure.whiteKnight:
				case Figure.blackKnight:
					return CanKnightMove();

				case Figure.whitePawn:
				case Figure.blackPawn:
					return CanPawnMove();

				default: return false;
			}
		}

		/// <summary>
		/// Ход пешки
		/// </summary>
		/// <returns></returns>
		private bool CanPawnMove()
		{
			if (fm.from.y < 1 || fm.from.y > 6)
				return false;
			int stepY = fm.figure.GetColor() == Color.white ? 1 : -1;
			if (fm.promotion == Figure.none && (stepY == 1 && fm.from.y == 6 || stepY == -1 && fm.from.y == 1))
				return false;
			return
				CanPawnGo(stepY) || CanPawnJump(stepY) || CanPawnEat(stepY) || CanPawnIntercept(stepY);
		}

		/// <summary>
		/// Может ли пешка взять на проходе
		/// </summary>
		/// <param name="stepY"></param>
		/// <returns></returns>
		private bool CanPawnIntercept(int stepY)
		{
			string[] parts = board.fen.Split();
			//Если есть клетка перехвата
			if (parts[3] != "-")
			{
				if (fm.to == new Square(parts[3]))
					if (fm.AbsDeltaX == 1)
						if (fm.DeltaY == stepY)
							return true;
			}
			return false;
		}

		/// <summary>
		/// Может ли пешка пойти
		/// </summary>
		/// <param name="stepY">Направление</param>
		/// <returns></returns>
		private bool CanPawnGo(int stepY)
		{
			if (board.GetFigureAt(fm.to) == Figure.none)
				if (fm.DeltaX == 0)
					if (fm.DeltaY == stepY)
						return true;
			return false;
		}

		/// <summary>
		/// Может ли пешка прыгнуть
		/// </summary>
		/// <param name="stepY">Направление</param>
		/// <returns></returns>
		private bool CanPawnJump(int stepY)
		{
			if (board.GetFigureAt(fm.to) == Figure.none)
				if (fm.DeltaX == 0)
					if (fm.DeltaY == 2 * stepY)
						if (fm.from.y == 1 || fm.from.y == 6)
							if (board.GetFigureAt(new Square(fm.from.x, fm.from.y + stepY)) == Figure.none)
								return true;
			return false;
		}

		/// <summary>
		/// Может ли пешка съесть
		/// </summary>
		/// <param name="stepY">Направление</param>
		/// <returns></returns>
		private bool CanPawnEat(int stepY)
		{
			if (board.GetFigureAt(fm.to) != Figure.none)
				if (fm.AbsDeltaX == 1)
					if (fm.DeltaY == stepY)
						return true;
			return false;
		}

		/// <summary>
		/// Может ли королева/ладья двигаться в направлении
		/// </summary>
		/// <returns></returns>
		private bool CanStraightMove()
		{
			Square at = fm.from;
			do
			{
				at = new Square(at.x + fm.SignX, at.y + fm.SignY);
				if (at == fm.to)
					return true;
			} while (at.OnBoard() && board.GetFigureAt(at) == Figure.none);
			return false;
		}

		/// <summary>
		/// Может ли король двигаться
		/// </summary>
		/// <returns></returns>
		private bool CanKingMove()
		{
			//Мой
			return CanKingGo() || CanKingCastling();
		}

		/// <summary>
		/// Может ли король идти
		/// </summary>
		/// <returns></returns>
		private bool CanKingGo()
		{
			if (fm.AbsDeltaX <= 1 && fm.AbsDeltaY <= 1)
				return true;
			return false;
		}

		/// <summary>
		/// Может ли король сделать рокировку
		/// </summary>
		/// <returns></returns>
		private bool CanKingCastling()
		{
			string[] parts = board.fen.Split();
			string flags = parts[2];
			if (flags != "-")
			{
				bool CanLongCastle = fm.figure.GetColor() == Color.black ? flags.Contains("q") : flags.Contains("Q");
				bool CanShortCastle = fm.figure.GetColor() == Color.black ? flags.Contains("k") : flags.Contains("K");

				if (fm.DeltaY == 0)
				{
					if (CanLongCastle && fm.DeltaX == -2 || CanShortCastle && fm.DeltaX == 2)
					{
						Square x1 = new Square(fm.from.x + fm.SignX, fm.from.y);
						Square x2 = new Square(fm.from.x + fm.DeltaX, fm.from.y);
						//Движение вправо
						if (board.GetFigureAt(x1) == Figure.none &&
							board.GetFigureAt(x2) == Figure.none)
						{
							FigureMoving x1Check = new FigureMoving(new FigureOnSquare(fm.figure, fm.from), x1);
							FigureMoving x2Check = new FigureMoving(new FigureOnSquare(fm.figure, fm.from), x2);
							if (!board.IsCheckAfterMove(x1Check) && !board.IsCheckAfterMove(x2Check) && !board.IsCheck())
							{
								if (fm.DeltaX == -2)
								{
									Square x3 = new Square(fm.from.x - 3, fm.from.y);
									if (board.GetFigureAt(x3) == Figure.none)
										return true;
								}
								else
									return true;
							}
						}
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Может ли конь скакать
		/// </summary>
		/// <returns></returns>
		private bool CanKnightMove()
		{
			if (fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2 ||
				fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1)
				return true;
			return false;
		}
	}
}