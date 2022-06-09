using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wpf2p2p
{
	public partial class ControlUC : UserControl
	{
		public Window CurrentWindow { get; set; }

		public ControlUC()
		{
			InitializeComponent();
		}

		public void Splash()
		{
			string[] Splashes =
			{
				"It's a game!",
				"Oh man!",
				"Wow!",
				"Technologic!",
				"Great job?",
				"Haha, LEL",
				"Chess!",
			};
			Random random = new Random();
			string splash = Splashes[random.Next(Splashes.Length)];
			TBTitle.Text += " - " + splash;
		}

		private void GControl_MouseUp(object sender, MouseButtonEventArgs e)
		{
			// На весь экран
			if (CurrentWindow.WindowState == WindowState.Maximized)
				PCMaximize.Kind = PackIconKind.WindowRestore;
			else // Обычное окно
				PCMaximize.Kind = PackIconKind.WindowMaximize;
		}

		private void GControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (CurrentWindow.WindowState == WindowState.Maximized)
			{
				CurrentWindow.WindowState = WindowState.Normal;
				Point windowPosition = Mouse.GetPosition(this);
				CurrentWindow.Top = -3;
				CurrentWindow.Left = windowPosition.X - (CurrentWindow.Width / 2);
			}
			if (e.ChangedButton == MouseButton.Left)
				CurrentWindow.DragMove();
		}

		private void BMaximize_Click(object sender, RoutedEventArgs e)
		{
			// Окно доступно для всего экрана
			if (PCMaximize.Kind == PackIconKind.WindowMaximize)
			{
				CurrentWindow.WindowState = WindowState.Maximized;
				PCMaximize.Kind = PackIconKind.WindowRestore;
			}
			else
			{
				CurrentWindow.WindowState = WindowState.Normal;
				PCMaximize.Kind = PackIconKind.WindowMaximize;
			}
		}

		private void BMinimaze_Click(object sender, RoutedEventArgs e)
		{
			CurrentWindow.WindowState = WindowState.Minimized;
		}

		public void Maximize()
		{
			BMaximize_Click(null, null);
		}

		private void BClose_Click(object sender, RoutedEventArgs e)
		{
			foreach (Window window in Application.Current.Windows)
			{
				if (window.GetType().Name == "DashboardW")
				{
					DashboardW dw = window as DashboardW;
					dw.SaveWindowData();
					break;
				}
			}
			Environment.Exit(0);
		}
	}
}