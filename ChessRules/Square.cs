using System.Collections.Generic;

namespace ChessRules
{
	/// <summary>
	/// Клетка шахматной доски
	/// </summary>
	struct Square
	{
		//Координата отсутствует
		public static Square none = new Square(-1, -1);

		//Координаты клетки
		public int x { get; private set; }
		public int y { get; private set; }

		/// <summary>
		/// Создать координату по двум числам
		/// </summary>
		/// <param name="x">Координата</param>
		/// <param name="y">Координата</param>
		public Square(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		/// <summary>
		/// Создать координату по названию клетки
		/// </summary>
		/// <param name="e2">Текстовое название клетки</param>
		public Square(string e2)
		{
			if (e2.Length == 2 &&
				e2[0] >= 'a' && e2[0] <= 'h' &&
				e2[1] >= '1' && e2[1] <= '8')
			{
				x = e2[0] - 'a';
				y = e2[1] - '1';
			}
			else
				this = none;
		}

		/// <summary>
		/// Проверка находится ли клетка на доске
		/// </summary>
		/// <returns></returns>
		public bool OnBoard()
		{
			return x >= 0 && x < 8 &&
				y >= 0 && y < 8;
		}

		/// <summary>
		/// Имя координаты
		/// </summary>
		public string Name { get { return ((char)('a' + x)).ToString() + (y + 1).ToString(); } }

		/// <summary>
		/// Оператор равенства
		/// </summary>
		/// <param name="a">Клетка</param>
		/// <param name="b">Клетка</param>
		/// <returns></returns>
		public static bool operator ==(Square a, Square b)
		{
			return a.x == b.x && a.y == b.y;
		}

		/// <summary>
		/// Перебор всех клеток
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<Square> YieldSquares()
		{
			for (int y = 0; y < 8; y++)
				for (int x = 0; x < 8; x++)
					yield return new Square(x, y);
		}

		/// <summary>
		/// Оператор неравенства
		/// </summary>
		/// <param name="a">Клетка</param>
		/// <param name="b">Клетка</param>
		/// <returns></returns>
		public static bool operator !=(Square a, Square b)
		{
			return !(a == b);
		}
	}
}