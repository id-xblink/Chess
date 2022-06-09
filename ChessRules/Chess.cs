using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessRules
{
	/// <summary>
	/// Основной класс шахмат
	/// </summary>
	public class Chess
	{
		// Универсальная нотация, задающая любую позицию шахмат
		public string fen { get; private set; }
		// Доска
		Board board;
		// Ходы фигур
		Moves moves;
		// Все доступные ходы
		List<FigureMoving> availableMoves;
		// Продолжительность игры
		bool IsLateGame = false;

		/// <summary>
		/// Инициализация по фену
		/// </summary>
		/// <param name="fen">Фен</param>
		public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
		{
			this.fen = fen;
			board = new Board(fen);
			moves = new Moves(board);
		}

		/// <summary>
		/// Инициализация по доске
		/// </summary>
		/// <param name="board"></param>
		private Chess(Board board)
		{
			this.board = board;
			fen = board.fen;
			moves = new Moves(board);
		}

		/// <summary>
		/// Количество фигур на доске
		/// </summary>
		/// <returns></returns>
		public int GetFiguresCount()
		{
			return board.GetFiguresOnSquare().Count();
		}

		/// <summary>
		/// Количество полуходов
		/// </summary>
		/// <returns></returns>
		public int GetFiftyMoveCount()
		{
			string[] parts = fen.Split();
			return Convert.ToInt32(parts[4]);
		}

		/// <summary>
		/// Цвет ходящих фигур
		/// </summary>
		/// <returns></returns>
		public string GetCurrentColor()
		{
			return board.moveColor.ToString();
		}

		/// <summary>
		/// Получение цвета фигуры
		/// </summary>
		/// <param name="figure">Фигура</param>
		/// <returns></returns>
		public string GetFigureColor(Figure figure)
		{
			return figure.GetColor().ToString();
		}

		/// <summary>
		/// Подсчёт ценности доски
		/// </summary>
		/// <param name="IsImproved">Использовать сетку позиций</param>
		/// <param name="IsShortData">Сокращать числовые значения</param>
		/// <returns></returns>
		public int GetBoardEvaluation(bool IsImproved, bool IsShortData)
		{
			EvaluationOnSquare evaluationOnSquare = new EvaluationOnSquare();
			int totalEvaluation = 0;
			List<FigureOnSquare> figuresOnSquares = board.GetFiguresOnSquare().ToList();
			
			if (!IsLateGame)
			{
				int queenCount = 0;
				int majorPieceCount = 0;
				int minorBlackPieceCount = 0;
				int minorWhitePieceCount = 0;
				foreach (FigureOnSquare figure in figuresOnSquares)
				{
					if (figure.figure == Figure.blackQueen || figure.figure == Figure.whiteQueen)
						queenCount++;

					if (figure.figure == Figure.blackRook || figure.figure == Figure.whiteRook)
						majorPieceCount++;

					if (figure.figure == Figure.blackBishop || figure.figure == Figure.blackKnight)
						minorBlackPieceCount++;

					if (figure.figure == Figure.whiteBishop || figure.figure == Figure.whiteKnight)
						minorWhitePieceCount++;
				}

				if (queenCount == 0 || (majorPieceCount == 0 && minorBlackPieceCount <= 1 && minorWhitePieceCount <= 1 ))
					IsLateGame = true;
			}

			foreach (FigureOnSquare figure in figuresOnSquares)
			{
				totalEvaluation += figure.figure.GetEvaluation(IsShortData);

				if (IsImproved)
					totalEvaluation += evaluationOnSquare.GetEvalution(figure, IsLateGame);
			}
			return totalEvaluation;
		}

		/// <summary>
		/// Функция реализации хода
		/// </summary>
		/// <param name="move">Текстовый вариант хода</param>
		/// <returns></returns>
		public Chess Move(string move)
		{
			FigureMoving fm = new FigureMoving(move);
			if (!moves.CanMove(fm))
				return this;
			if (board.IsCheckAfterMove(fm))
				return this;
			Board nextBoard = board.Move(fm);
			Chess nextChess = new Chess(nextBoard);
			return nextChess;
		}

		/// <summary>
		/// Получение фигуры по координатам
		/// </summary>
		/// <param name="x">Координата x</param>
		/// <param name="y">Координата y</param>
		/// <returns></returns>
		public char GetFigureAt(int x, int y)
		{
			Square square = new Square(x, y);
			Figure f = board.GetFigureAt(square);
			return f == Figure.none ? '.' : (char)f;
		}

		/// <summary>
		/// Нахождение всех доступных ходов
		/// </summary>
		void FindAllMoves()
		{
			availableMoves = new List<FigureMoving>();
			foreach (FigureOnSquare fs in board.YieldFigures())
				foreach (Square to in Square.YieldSquares())
				{
					FigureMoving fm;
					// Если ходит пешка
					if (fs.figure == (board.moveColor == Color.black ? Figure.blackPawn : Figure.whitePawn) &&
						to.y == (board.moveColor == Color.black ? 0 : 7))
					{
						foreach (Figure promotion in board.moveColor == Color.black ? board.blackPromotions : board.whitePromotions)
						{
							fm = new FigureMoving(fs, to, promotion);
							if (moves.CanMove(fm))
								if (!board.IsCheckAfterMove(fm))
									availableMoves.Add(fm);
						}
					}
					else
					{
						fm = new FigureMoving(fs, to);
						if (moves.CanMove(fm))
							if (!board.IsCheckAfterMove(fm))
								availableMoves.Add(fm);
					}
				}
		}

		/// <summary>
		/// Возвращает все ходы в строчном формате
		/// </summary>
		/// <returns></returns>
		public List<string> GetAllMoves()
		{
			FindAllMoves();
			List<string> list = new List<string>();
			foreach (FigureMoving fm in availableMoves)
				list.Add(fm.ToString());
			return list;
		}

		/// <summary>
		/// Есть ли шах
		/// </summary>
		/// <returns></returns>
		public bool IsCheck()
		{
			return board.IsCheck();
		}

		/// <summary>
		/// Получить текущий номер хода
		/// </summary>
		/// <returns></returns>
		public int GetMoveNumber()
		{
			return board.moveNumber;
		}

		/// <summary>
		/// Ход бота
		/// </summary>
		/// <param name="color">Цвет фигур бота</param>
		/// <param name="level">Уровень бота</param>
		/// <returns></returns>
		public string GetBotMove(string color, int level)
		{
			Random random = new Random();
			List<string> allMoves = GetAllMoves();
			string move = "";

			if (level != 1)
			{
				List<ValuableMove> valuableMoves = new List<ValuableMove>();
				foreach (string availableMove in allMoves)
					valuableMoves.Add(new ValuableMove(availableMove,
						MinimaxRoot(availableMove,
						level - 1 != 4 ? level - 1 : 3,
						color == "black" ? false : true,
						this,
						level != 5 ? false : true)));

				if (color == "black")
					valuableMoves = valuableMoves.Where(m => m.Evaluation == valuableMoves.Min(q => q.Evaluation)).ToList();
				else
					valuableMoves = valuableMoves.Where(m => m.Evaluation == valuableMoves.Max(q => q.Evaluation)).ToList();

				move = valuableMoves[random.Next(valuableMoves.Count)].Move;
			}
			else
				move = allMoves[random.Next(allMoves.Count)];
			
			return move;
		}

		private int MinimaxRoot(string move, int depth, bool maximizingPlayer, Chess chess, bool IsImproved)
		{
			return Minimax(move, --depth, int.MinValue, int.MaxValue, !maximizingPlayer, chess, IsImproved);
		}

		private int Minimax(string move, int depth, int alpha, int beta, bool maximizingPlayer, Chess chess, bool IsImproved)
		{
			List<string> newMoves;
			chess = chess.Move(move);

			if (depth == 0)
				return chess.GetBoardEvaluation(IsImproved, false);
			else
				newMoves = chess.GetAllMoves();

			if (maximizingPlayer)
			{
				int maxEval = int.MinValue;
				foreach (string turn in newMoves)
				{
					int eval = Minimax(turn, depth - 1, alpha, beta, false, chess, IsImproved);
					maxEval = Max(maxEval, eval);
					alpha = Max(alpha, eval);
					if (beta <= alpha)
						break;
				}
				if (!chess.IsCheck() && newMoves.Count == 0)
					maxEval++;
				return maxEval;
			}
			else
			{
				int minEval = int.MaxValue;
				foreach (string turn in newMoves)
				{
					int eval = Minimax(turn, depth - 1, alpha, beta, true, chess, IsImproved);
					minEval = Min(minEval, eval);
					beta = Min(beta, eval);
					if (beta <= alpha)
						break;
				}
				if (!chess.IsCheck() && newMoves.Count == 0)
					minEval--;
				return minEval;
			}
		}

		private int Max(int a, int b)
		{
			if (a > b)
				return a;
			return b;
		}

		private int Min(int a, int b)
		{
			if (a < b)
				return a;
			return b;
		}
	}
}