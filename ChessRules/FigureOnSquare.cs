namespace ChessRules
{
	/// <summary>
	/// Контейнер для хранения фигур и квадратов
	/// </summary>
	class FigureOnSquare
	{
		// Фигура
		public Figure figure { get; private set; }
		// Клетка
		public Square square { get; private set; }

		/// <summary>
		/// Инициализация
		/// </summary>
		/// <param name="figure">Фигура</param>
		/// <param name="square">Клетка</param>
		public FigureOnSquare(Figure figure, Square square)
		{
			this.figure = figure;
			this.square = square;
		}
	}
}