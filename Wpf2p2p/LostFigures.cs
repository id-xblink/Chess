using System.Windows.Media.Imaging;

namespace Wpf2p2p
{
	class LostFigures
	{
		public string Name { get; set; }
		public BitmapImage ResourceImage { get; set; }
		public int Count { get; set; }

		public LostFigures(string name, object resourceImage, int count)
		{
			Name = name;
			ResourceImage = (BitmapImage)resourceImage;
			Count = count;
		}
	}
}