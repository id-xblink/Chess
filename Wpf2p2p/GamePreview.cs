using System.Windows.Media.Imaging;

namespace Wpf2p2p
{
	class GamePreview
    {
		public int UG_ID { get; set; }
		public int ID { get; set; }
		public string MyColor { get; set; }
		public string Result { get; set; }
		public string GameType { get; set; }
		public BitmapImage Avatar { get; set; }

		public GamePreview(int ug_id, int id, string myColor, string result, string gameType, BitmapImage avatar)
		{
			UG_ID = ug_id;
			ID = id;
			MyColor = myColor;
			Result = result;
			GameType = gameType;
			Avatar = avatar;
		}
	}
}