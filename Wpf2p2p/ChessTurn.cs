namespace Wpf2p2p
{
	class ChessTurn
	{
		public int TurnNumber { get; set; }
		public string White { get; set; }
		public int EvaluationAfterWhite { get; set; }
		public string FenAfterWhite { get; set; }
		public string Black { get; set; }
		public int EvaluationAfterBlack { get; set; }
		public string FenAfterBlack { get; set; }
	}
}