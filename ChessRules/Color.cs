namespace ChessRules
{
	/// <summary>
	/// Перечисление цветов
	/// </summary>
	enum Color
	{
		none,
		white,
		black
	}

	/// <summary>
	/// Создание статичного класса для создания методов перечисления
	/// </summary>
	static class ColorMethods
	{
		/// <summary>
		/// Смена цвета (для выполнения хода)
		/// </summary>
		/// <param name="color">Цвет</param>
		/// <returns></returns>
		public static Color FlipColor(this Color color)
		{
			if (color == Color.black)
				return Color.white;
			if (color == Color.white)
				return Color.black;
			return Color.none;
		}
	}
}