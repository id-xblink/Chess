using System.Windows.Media.Imaging;

namespace Wpf2p2p
{
	class ChangeFigures
    {
		public string Name { get; set; }
		public BitmapImage Image { get; set; }

		public ChangeFigures(string name, BitmapImage image)
		{
			Name = name;
			Image = image;
		}
    }
}