using System.Windows.Media.Imaging;

namespace Wpf2p2p
{
	class Leader
	{
		public int ID { get; set; }
		public int Position { get; set; }
		public BitmapImage Avatar { get; set; }
		public string Login { get; set; }
		public int Raiting { get; set; }
		public string Region { get; set; }
		public int Game { get; set; }

		public Leader(int id = 0, int position = 0, BitmapImage avatar = null, string login = "", int raiting = 0, string region = "", int game = 0)
		{
			ID = id;
			Position = position;
			Avatar = avatar;
			Login = login;
			Raiting = raiting;
			Region = region;
			Game = game;
		}
	}
}