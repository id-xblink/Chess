using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wpf2p2p
{
	public partial class ProfileUC : UserControl
    {
		public ProfileUC()
        {
            InitializeComponent();
			InitColors();
			TakeFromDB();
		}

		private void TakeFromDB()
		{
			CBRegion.ItemsSource = DBConnection.GetRegions();
			LoadProfile();
		}

		private void LoadProfile()
		{
			User user = DBConnection.GetProfile(DBConnection.UserID);
			IAvatar.ImageSource = user.Avatar;
			TBLoginProfile.Text = user.Login;
			TBRegionProfile.Text = "Регион: " + user.Region;
			TBRatingProfile.Text = "Рейтинг: " + user.Raiting;
			TBGamesProfile.Text = "Игр: " + user.GamesCount;
			CalculateRank(user.Raiting);
		}

		private void CalculateRank(int raiting)
		{
			if (raiting > 0)
			{
				if (raiting > 3600)
				{
					IRank.SetResourceReference(Image.SourceProperty, "dk");
					Star1.Kind = PackIconKind.Star;
					Star2.Kind = PackIconKind.Star;
					Star3.Kind = PackIconKind.Star;
					TBRank.Text = "-+Король+-";
				}
				else
				{
					double level = Math.Ceiling(Convert.ToDouble(raiting) / 600);
					string rank = "";

					switch (level)
					{
						case 1:
							{
								IRank.SetResourceReference(Image.SourceProperty, "dp");
								rank = "Пешка";
								break;
							}
						case 2:
							{
								IRank.SetResourceReference(Image.SourceProperty, "dn");
								rank = "Конь";
								break;
							}
						case 3:
							{
								IRank.SetResourceReference(Image.SourceProperty, "db");
								rank = "Слон";
								break;
							}
						case 4:
							{
								IRank.SetResourceReference(Image.SourceProperty, "dr");
								rank = "Ладья";
								break;
							}
						case 5:
							{
								IRank.SetResourceReference(Image.SourceProperty, "dq");
								rank = "Королева";
								break;
							}
						case 6:
							{
								IRank.SetResourceReference(Image.SourceProperty, "dk");
								rank = "Король";
								break;
							}
					}
					SetStart(raiting, level, rank);
				}
			}
		}

		private void SetStart(int raiting, double level, string rank)
		{
			int stars = 0;
			raiting -= 600 * (Convert.ToInt32(level) - 1);
			if (raiting > 0)
			{
				stars++;
				Star1.Kind = PackIconKind.Star;
			}
			if (raiting > 200)
			{
				stars++;
				Star2.Kind = PackIconKind.Star;
			}
			if (raiting > 400)
			{
				stars++;
				Star3.Kind = PackIconKind.Star;
			}
			TBRank.Text = $"{rank} {stars}";
		}

		private void InitColors()
		{
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			object isDark = ChessKey.GetValue("isDarkTheme");
			ChessKey.Close();
			TBSwitchTheme.IsChecked = Convert.ToBoolean(isDark);
			TBSwitchTheme.Click += TBSwitchTheme_Click;
			foreach (Swatch swatch in new SwatchesProvider().Swatches)
			{
				switch (swatch.Name)
				{
					case "yellow":
						{
							ByellowP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							ByellowA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "amber":
						{
							BamberP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BamberA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "deeporange":
						{
							BdeeporangeP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BdeeporangeA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "lightblue":
						{
							BlightblueP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BlightblueA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "teal":
						{
							BtealP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BtealA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "cyan":
						{
							BcyanP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BcyanA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "pink":
						{
							BpinkP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BpinkA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "green":
						{
							BgreenP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BgreenA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "deeppurple":
						{
							BdeeppurpleP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BdeeppurpleA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "indigo":
						{
							BindigoP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BindigoA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "lightgreen":
						{
							BlightgreenP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BlightgreenA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "blue":
						{
							BblueP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BblueA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "lime":
						{
							BlimeP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BlimeA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "red":
						{
							BredP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BredA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "orange":
						{
							BorangeP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BorangeA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "purple":
						{
							BpurpleP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							BpurpleA.Background = new SolidColorBrush { Color = swatch.AccentExemplarHue.Color };
							break;
						}
					case "bluegrey":
						{
							BbluegrayP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							break;
						}
					case "grey":
						{
							BgreyP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							break;
						}
					case "brown":
						{
							BbrownP.Background = new SolidColorBrush { Color = swatch.ExemplarHue.Color };
							break;
						}
				}
			}
		}

		private void BChangePrimaryColor_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			ITheme theme = Application.Current.Resources.GetTheme();
			Color color = ((SolidColorBrush)b.Background).Color;
			theme.SetPrimaryColor(color);
			Application.Current.Resources.SetTheme(theme);
			SaveRegeditParametr("primary", color.ToString());
			InfoMessage("Основной цвет изменён");
		}

		private void BChangeAccentColor_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			ITheme theme = Application.Current.Resources.GetTheme();
			Color color = ((SolidColorBrush)b.Background).Color;
			theme.SetSecondaryColor(color);
			Application.Current.Resources.SetTheme(theme);
			SaveRegeditParametr("accent", color.ToString());
			InfoMessage("Дополнительный цвет изменён");
		}

		private void TBSwitchTheme_Click(object sender, RoutedEventArgs e)
		{
			ITheme theme = Application.Current.Resources.GetTheme();
			theme.SetBaseTheme((bool)TBSwitchTheme.IsChecked ? Theme.Dark : Theme.Light);
			Application.Current.Resources.SetTheme(theme);
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			object style = ChessKey.GetValue("style");
			ChessKey.Close();
			ResourceDictionary rd = Application.Current.Resources.MergedDictionaries[3];
			foreach (FigureName figureName in (FigureName[])Enum.GetValues(typeof(FigureName)))
			{
				if (figureName.ToString()[0] == 'd')
					rd[figureName.ToString()] = DBConnection.GetImage($"assets/piece/{style}/{((bool)TBSwitchTheme.IsChecked ? "w" : "b")}{figureName.ToString()[1]}.png");
			}
			SaveRegeditParametr("isDarkTheme", ((bool)TBSwitchTheme.IsChecked).ToString());
			InfoMessage("Тема изменена");
		}

		public void RollBackTogglebutton()
		{
			TBSwitchTheme.Click -= TBSwitchTheme_Click;
			TBSwitchTheme.IsChecked = false;
			TBSwitchTheme.Click += TBSwitchTheme_Click;
		}

		private void SaveRegeditParametr(string name, string value)
		{
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			ChessKey.SetValue(name, value);
			ChessKey.Close();
		}
		
		private void BLoadImage_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				Filter = "Png files (*.png)|*.png|All files (*.*)|*.*",
				FilterIndex = 1
			};
			bool? result = dialog.ShowDialog();
			if (result == true)
			{
				try
				{
					string filename = dialog.FileName;
					using (FileStream fs = new FileStream(filename, FileMode.Open))
					{
						byte[] image = new byte[fs.Length];
						fs.Read(image, 0, image.Length);
						BitmapImage bi = DBConnection.GetImage(image);
						bool loaded = false;
						for (int i = 1; i <= 6; i++)
						{
							if (bi.PixelWidth == 16 * i && bi.PixelHeight == 16 * i)
							{
								loaded = true;
								IAvatarPreview.ImageSource = bi;
								break;
							}
						}
						if (!loaded)
							InfoMessage("Неверное разрешение изображения");
					}
				}
				catch (Exception)
				{
					InfoMessage("Неверный файл");
				}
			}
		}

		private void BClear_Click(object sender, RoutedEventArgs e)
		{
			IAvatarPreview.ImageSource = null;
			TBLogin.Text = "";
			PBCurrPass.Password = "";
			PBPass.Password = "";
			PBRepPass.Password = "";
			CBRegion.SelectedIndex = -1;
		}

		private void BSave_Click(object sender, RoutedEventArgs e)
		{
			if (!DBConnection.CheckChangeProfile(DBConnection.UserID, PBCurrPass.Password))
			{
				InfoMessage("Текущий пароль неверен");
				return;
			}
			RegistrationCheck check = new RegistrationCheck();
			// Изменение аватара
			if (IAvatarPreview.ImageSource != null)
			{
				byte[] bytes = null;
				BitmapSource bitmapSource = IAvatarPreview.ImageSource as BitmapSource;
				JpegBitmapEncoder encoder = new JpegBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
				using (MemoryStream stream = new MemoryStream())
				{
					encoder.Save(stream);
					bytes = stream.ToArray();
				}
				DBConnection.UpdateProfileAvatar(DBConnection.UserID, bytes);
				IAvatar.ImageSource = DBConnection.GetImage(bytes);
				InfoMessage("Аватар изменён");
			}
			// Изменение логина
			if (TBLogin.Text != "")
			{
				if (check.IsFreeLogin(TBLogin.Text, DBConnection.UserID) && TBLogin.Text.Length > 2)
				{
					DBConnection.UpdateProfileLogin(DBConnection.UserID, TBLogin.Text);
					TBLoginProfile.Text = TBLogin.Text;
					InfoMessage("Логин изменён");
				}
				else
					InfoMessage("Неверные данные логина");
			}
			// Изменение региона
			if (CBRegion.SelectedIndex != -1)
			{
				DBConnection.UpdateProfileRegion(DBConnection.UserID, CBRegion.SelectedIndex + 1);
				TBRegionProfile.Text = "Регион: " + ((Region)CBRegion.SelectedItem).Name;
				InfoMessage("Регион изменён");
			}
			// Изменение пароля
			if (PBPass.Password != "")
			{
				if (check.IsPasswordEqual(PBPass.Password, PBRepPass.Password) && PBPass.Password.Length > 2)
				{
					DBConnection.UpdateProfilePassword(DBConnection.UserID, PBPass.Password);
					InfoMessage("Пароль изменён");
				}
				else
					InfoMessage("Неверные данные паролей");
			}
			BClear_Click(sender, e);
		}

		private void BRemoveAvatar_Click(object sender, RoutedEventArgs e)
		{
			if (!DBConnection.CheckChangeProfile(DBConnection.UserID, PBCurrPass.Password))
			{
				InfoMessage("Текущий пароль неверен");
				return;
			}
			// Удаление аватара
			IAvatar.ImageSource = DBConnection.RemoveProfileAvatar(DBConnection.UserID);
			BClear_Click(sender, e);
			InfoMessage("Аватар удалён");
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
					BSave_Click(sender, e);
		}

		private void InfoMessage(string content, string button = "X")
		{
			InfoBar.MessageQueue.Enqueue(content, button, delegate () { });
		}
	}
}