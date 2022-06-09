namespace Wpf2p2p
{
	class GameStats
	{
		// Потеряно чёрных фигур
		public int LostBlackQueen = 0;
		public int LostBlackRook = 0;
		public int LostBlackBishop = 0;
		public int LostBlackKnight = 0;
		public int LostBlackPawn = 0;
		// Потеряно белых фигур
		public int LostWhiteQueen = 0;
		public int LostWhiteRook = 0;
		public int LostWhiteBishop = 0;
		public int LostWhiteKnight = 0;
		public int LostWhitePawn = 0;
		// Съедено чёрной фигурой
		public int EatByBlackKing = 0;
		public int EatByBlackQueen = 0;
		public int EatByBlackRook = 0;
		public int EatByBlackBishop = 0;
		public int EatByBlackKnight = 0;
		public int EatByBlackPawn = 0;
		// Съедено белой фигурой
		public int EatByWhiteKing = 0;
		public int EatByWhiteQueen = 0;
		public int EatByWhiteRook = 0;
		public int EatByWhiteBishop = 0;
		public int EatByWhiteKnight = 0;
		public int EatByWhitePawn = 0;
		// Превращений чёрных за игру
		public int BlackPromoteQueen = 0;
		public int BlackPromoteRook = 0;
		public int BlackPromoteBishop = 0;
		public int BlackPromoteKnight = 0;
		// Превращений белых за игру
		public int WhitePromoteQueen = 0;
		public int WhitePromoteRook = 0;
		public int WhitePromoteBishop = 0;
		public int WhitePromoteKnight = 0;
		// Взятий на проходе
		public int BlackIntercepted = 0;
		public int WhiteIntercepted = 0;
		// Шахи
		public int BlackChecked = 0;
		public int WhiteChecked = 0;
		// Рокировки
		public bool IsBlackLongCastle = false;
		public bool IsBlackShortCastle = false;
		public bool IsWhiteLongCastle = false;
		public bool IsWhiteShortCastle = false;
		// Общие данные
		public int Type = 0;
		public int Result = 0;
		public int Evaluation = 0;
		public string Fen = "";
		// Время
		public int Time = 0;
		public int AddTime = 0;
		public int BlackTime = 0;
		public int WhiteTime = 0;
		// Начало конец партии
		public string GameBegin = "";
		public string GameEnd = "";
		// Дополнительно
		public int MyColor = 0;
		public int EnemyNetID = 0;
	}
}