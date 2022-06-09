using System.Windows.Media.Imaging;

namespace Wpf2p2p
{
	class User
	{
		public int ID { get; set; }
		public BitmapImage Avatar { get; set; }
		public string Login { get; set; }
		public string Region { get; set; }
		public int Raiting { get; set; }
		public int GamesCount { get; set; }

		public User(int id, BitmapImage avatar, string login, string region, int raiting, int gamesCount)
		{
			ID = id;
			Avatar = avatar;
			Login = login;
			Region = region;
			Raiting = raiting;
			GamesCount = gamesCount;
		}
	}
}