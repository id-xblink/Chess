using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wpf2p2p
{
	public partial class DashboardW : Window
	{
		UserControl uc = null;
		MediaPlayer Sound = new MediaPlayer();

		public DashboardW()
		{
			InitializeComponent();
			AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new RoutedEventHandler(PlaySound));
			ControlPanel.CurrentWindow = this;
			ControlPanel.Splash();
			MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
			MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;
		}

		private void PlaySound(object sender, RoutedEventArgs e)
		{
			new Thread(delegate ()
			{
				RegistryKey CurrentUserKey = Registry.CurrentUser;
				RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
				int volume = Convert.ToInt32(ChessKey.GetValue("volume"));
				ChessKey.Close();
				MediaPlayer Sound = new MediaPlayer
				{
					Volume = volume / 100
				};
				Sound.Open(new Uri("assets/sound/click.wav", UriKind.Relative));
				Sound.Play();
			}).Start();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			double width = Convert.ToDouble(ChessKey.GetValue("width"));
			double height = Convert.ToDouble(ChessKey.GetValue("height"));
			string top = ChessKey.GetValue("top").ToString();
			string left = ChessKey.GetValue("left").ToString();
			bool old = Convert.ToBoolean(ChessKey.GetValue("old"));
			if (!old)
			{
				ChessKey.SetValue("old", true);
				LoadNewView("Home");
			}
			else
				LoadNewView("Game");
			ChessKey.Close();
			// Размер окна окна
			if (width + 2 != MaxWidth || height != MaxHeight)
			{
				Width = width;
				Height = height;
			}
			else
				ControlPanel.Maximize();
			if (top != "-" && left != "-")
			{
				Top = Convert.ToDouble(top);
				Left = Convert.ToDouble(left);
			}
		}
		
		private void PopupBox_Opened(object sender, RoutedEventArgs e)
		{
			// Инициализация значений
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			int volume = Convert.ToInt32(ChessKey.GetValue("volume"));
			bool autoSave = Convert.ToBoolean(ChessKey.GetValue("auto_save"));
			int doubleColor = Convert.ToInt32(ChessKey.GetValue("double_color"));
			ChessKey.Close();
			// Звук
			SVolume.Value = volume;
			// Автосохранение
			if (autoSave)
				CBAutoSave.IsChecked = true;
			else
				CBAutoSave.IsChecked = false;
			// Цвет двойного режима
			if (doubleColor == 1)
				DCWhite.IsChecked = true;
			else
				DCBlack.IsChecked = true;
		}

		private void BPopupFactory_Click(object sender, RoutedEventArgs e)
		{
			// Первоначальная настройка
			ResourceDictionary rd = Application.Current.Resources.MergedDictionaries[3];
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			ChessKey.SetValue("style", "cburnett");
			ChessKey.SetValue("ws", "#FFF0D9B5");
			ChessKey.SetValue("bs", "#FF946f51");
			ChessKey.SetValue("ms", "#FF3BB000");
			ChessKey.SetValue("as", "#FFFFA600");
			ChessKey.SetValue("cs", "#FFC90000");
			ChessKey.SetValue("isDarkTheme", false);
			ChessKey.SetValue("primary", "#FF795548");
			ChessKey.SetValue("accent", "#FF3D5AFE");
			ChessKey.SetValue("volume", "100");
			ChessKey.SetValue("width", "1080");
			ChessKey.SetValue("height", "720");
			ChessKey.SetValue("top", "-");
			ChessKey.SetValue("left", "-");
			ChessKey.SetValue("auto_save", false);
			ChessKey.SetValue("double_color", "1");
			// Загрузка данных
			object style = ChessKey.GetValue("style");
			object wS = ChessKey.GetValue("ws");
			object bS = ChessKey.GetValue("bs");
			object mS = ChessKey.GetValue("ms");
			object aS = ChessKey.GetValue("as");
			object cS = ChessKey.GetValue("cs");
			object isDark = ChessKey.GetValue("isDarkTheme");
			object primary = ChessKey.GetValue("primary");
			object accent = ChessKey.GetValue("accent");
			ChessKey.Close();
			// Настройка цветового словаря
			rd["ws"] = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(wS.ToString()) };
			rd["bs"] = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(bS.ToString()) };
			rd["ms"] = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(mS.ToString()) };
			rd["as"] = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(aS.ToString()) };
			rd["cs"] = new SolidColorBrush { Color = (Color)ColorConverter.ConvertFromString(cS.ToString()) };
			foreach (FigureName figureName in (FigureName[])Enum.GetValues(typeof(FigureName)))
			{
				if (figureName.ToString()[0] != 'd')
					rd[figureName.ToString()] = DBConnection.GetImage($"assets/piece/{style}/{figureName}.png");
				else
					rd[figureName.ToString()] = DBConnection.GetImage($"assets/piece/{style}/{(Convert.ToBoolean(isDark) ? "w" : "b")}{figureName.ToString()[1]}.png");
			}
			// Настройка интерфейса приложения
			ITheme theme = Application.Current.Resources.GetTheme();
			theme.SetPrimaryColor((Color)ColorConverter.ConvertFromString(primary.ToString()));
			theme.SetSecondaryColor((Color)ColorConverter.ConvertFromString(accent.ToString()));
			theme.SetBaseTheme(Convert.ToBoolean(isDark) ? Theme.Dark : Theme.Light);
			Application.Current.Resources.SetTheme(theme);
			// Откат кнопки-переключателя в профиле
			if (uc != null && uc.GetType().Name == "ProfileUC")
				((ProfileUC)uc).RollBackTogglebutton();
			SVolume.Value = 100;
			// Автосохранение
			CBAutoSave.IsChecked = false;
			// Цвет двойного режима
			DCWhite.IsChecked = true;
			InfoMessage("Заводские настройки установлены");
		}

		private void BPopupSave_Click(object sender, RoutedEventArgs e)
		{
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			ChessKey.SetValue("volume", SVolume.Value);
			ChessKey.SetValue("auto_save", (bool)CBAutoSave.IsChecked);
			ChessKey.SetValue("double_color", (bool)DCWhite.IsChecked ? "1" : "2");
			ChessKey.Close();
			InfoMessage("Изменения сохранены");
		}

		private void BPlaySound_Click(object sender, RoutedEventArgs e)
		{
			Sound.Volume = SVolume.Value / 100;
			Sound.Open(new Uri("assets/sound/move.wav", UriKind.Relative));
			Sound.Play();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			SaveWindowData();
		}

		public void SaveWindowData()
		{
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			bool autoSave = Convert.ToBoolean(ChessKey.GetValue("auto_save"));
			if (autoSave)
			{
				ChessKey.SetValue("width", Width);
				ChessKey.SetValue("height", Height);
				ChessKey.SetValue("top", Top);
				ChessKey.SetValue("left", Left);
			}
			ChessKey.Close();
		}

		private void BPower_Click()
		{
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			if (ChessKey.GetValue("login") != null)
				ChessKey.DeleteValue("login");
			if (ChessKey.GetValue("password") != null)
				ChessKey.DeleteValue("password");
			ChessKey.Close();
			EnterW ew = new EnterW { Tag = "zxc" };
			ew.Show();
			Close();
		}

		private void BFunctional_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			if (uc != null && uc.GetType().Name == "GameUC")
				((GameUC)uc).SafeLeave(b);
			else
				LoadNewView(b.Tag.ToString());
		}

		public void LoadNewView(string ViewName)
		{
			ResourceDictionary rd = Application.Current.Resources.MergedDictionaries[3];
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			object style = ChessKey.GetValue("style");
			object isDark = ChessKey.GetValue("isDarkTheme");
			ChessKey.Close();
			foreach (FigureName figureName in (FigureName[])Enum.GetValues(typeof(FigureName)))
			{
				if (figureName.ToString()[0] != 'd')
					rd[figureName.ToString()] = DBConnection.GetImage($"assets/piece/{style}/{figureName}.png");
				else
					rd[figureName.ToString()] = DBConnection.GetImage($"assets/piece/{style}/{(Convert.ToBoolean(isDark) ? "w" : "b")}{figureName.ToString()[1]}.png");
			}
			GArea.Children.Clear();
			switch (ViewName)
			{
				case "Home":
					{
						uc = new HomeUC { Tag = this };
						break;
					}
				case "Game":
					{
						uc = new GameUC { Tag = this };
						break;
					}
				case "Profile":
					{
						uc = new ProfileUC();
						break;
					}
				case "Statistics":
					{
						uc = new StatisticsUC();
						break;
					}
				case "Overview":
					{
						uc = new OverviewUC();
						break;
					}
				case "Leaders":
					{
						uc = new LeaderUC();
						break;
					}
				case "Checkerboard":
					{
						uc = new CheckerboardUC();
						break;
					}
				case "Pieces":
					{
						uc = new PieceUC();
						break;
					}
				case "Power":
					{
						BPower_Click();
						break;
					}
			}
			if (ViewName != "Power")
				GArea.Children.Add(uc);
		}

		private void InfoMessage(string content, string button = "X")
		{
			InfoBar.MessageQueue.Enqueue(content, button, delegate () { });
		}
	}
}