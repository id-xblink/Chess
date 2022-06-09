using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Wpf2p2p
{
	public partial class EnterW : Window
	{
		public EnterW()
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
			// Настройка словаря ресурсов
			ResourceDictionary rd = Application.Current.Resources.MergedDictionaries[3];
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			if (ChessKey.GetValue("style") == null)
			{
				// Первоначальная настройка
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
				ChessKey.SetValue("old", false);
			}
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
			// Проверка автологина
			object login = ChessKey.GetValue("login");
			object password = ChessKey.GetValue("password");
			ChessKey.Close();
			if (login == null || password == null)
			{
				CBRegion.ItemsSource = DBConnection.GetRegions();
				TBLogin.Focus();
				if (Tag == null)
					InfoMessage("Добро пожаловать!", "Привет!");
				else
					InfoMessage("Добро пожаловать!", "Виделись уже");
			}
			else
				Authorization(login.ToString(), password.ToString(), true);
		}

		private void Hyperlink_Click(object sender, RoutedEventArgs e)
		{
			// Смена режима работы окна
			if (Convert.ToBoolean(TBSwap.Tag))
			{
				TBTitle.Text = "Регистрация";
				PBRepPass.Visibility = Visibility.Visible;
				CBRegion.Visibility = Visibility.Visible;
				CBRemember.Visibility = Visibility.Collapsed;
				TBChange.Text = "Есть аккаунт?";
				HLChange.Text = "Авторизуйтесь!";
			}
			else
			{
				TBTitle.Text = "Авторизация";
				PBRepPass.Visibility = Visibility.Collapsed;
				CBRegion.Visibility = Visibility.Collapsed;
				CBRemember.Visibility = Visibility.Visible;
				TBChange.Text = "Нет аккаунта?";
				HLChange.Text = "Зарегистрируйтесь!";
			}
			TBSwap.Tag = !Convert.ToBoolean(TBSwap.Tag);
			TBLogin.Focus();
		}

		private void BGo_Click(object sender, RoutedEventArgs e)
		{
			if (!Convert.ToBoolean(TBSwap.Tag)) // Регистрация
			{
				string result = new RegistrationCheck().LadderValidation(TBLogin.Text, PBPass.Password, PBRepPass.Password, CBRegion.SelectedIndex);
				if (result.Equals("Вы зарегистрировались"))
				{
					DBConnection.RegisterNewAccount(TBLogin.Text, PBPass.Password, CBRegion.SelectedIndex + 1);
					InfoMessage(result, "Ура!");
					TBLogin.Text = "";
					PBPass.Password = "";
					PBRepPass.Password = "";
					CBRegion.SelectedIndex = -1;
					Hyperlink_Click(sender, e);
				}
				else
					InfoMessage(result);
			}
			else // Авторизация
				Authorization(TBLogin.Text, PBPass.Password, false);
		}

		private void Authorization(string login, string password, bool auto)
		{
			int id = DBConnection.Authorization(login, password);
			if (id != 0)
			{
				// Подключение автологина
				if (Convert.ToBoolean(CBRemember.IsChecked))
				{
					RegistryKey CurrentUserKey = Registry.CurrentUser;
					RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
					ChessKey.SetValue("login", login);
					ChessKey.SetValue("password", password);
					ChessKey.Close();
				}
				DBConnection.UserID = id;
				DashboardW dw = new DashboardW();
				dw.Show();
				Close();
			}
			else
			{
				if (auto)
				{
					RegistryKey CurrentUserKey = Registry.CurrentUser;
					RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
					if (ChessKey.GetValue("login") != null)
						ChessKey.DeleteValue("login");
					if (ChessKey.GetValue("password") != null)
						ChessKey.DeleteValue("password");
					ChessKey.Close();
					InfoMessage("Авторизуйтесь повторно");
				}
				else
					InfoMessage("Неверные данные");
			}
		}

		private void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex(@"[^\w]");
			if (regex.IsMatch(e.Text) && e.Text != " ")
				e.Handled = true;
		}

		private void Text_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Space)
				e.Handled = true;
			else
				if (e.Key == Key.Enter)
					BGo_Click(sender, e);
		}

		private void InfoMessage(string content, string button = "X")
		{
			InfoBar.MessageQueue.Enqueue(content, button, delegate () { });
		}
	}
}