using System.Windows;
using System.Windows.Controls;

namespace Wpf2p2p
{
	public partial class HomeUC : UserControl
	{
		public HomeUC()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			DashboardW dw = Tag as DashboardW;
			dw.LoadNewView("Game");
		}
	}
}