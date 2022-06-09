namespace ChessRules
{
	/// <summary>
	/// Ходы бота по ценности
	/// </summary>
	class ValuableMove
	{
		public string Move { get; set; }
		public int Evaluation { get; set; }

		public ValuableMove(string move, int evaluation)
		{
			Move = move;
			Evaluation = evaluation;
		}
	}
}