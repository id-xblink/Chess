using MaterialDesignThemes.Wpf;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf2p2p
{
	public partial class LeaderUC : UserControl
	{
		public LeaderUC()
		{
			InitializeComponent();
			LoadLeaderBoard(LoadProfile());
			RBLeaderWorld.Checked += RBLeaderWorld_Checked;
			RBLeaderRegion.Checked += RBLeaderWorld_Checked;
		}

		private string LoadProfile()
		{
			User user = DBConnection.GetProfile(DBConnection.UserID);
			IAvatar.ImageSource = user.Avatar;
			TBLogin.Text = user.Login;
			TBRegion.Text = "Регион: " + user.Region;
			TBRating.Text = "Рейтинг: " + user.Raiting;
			TBGames.Text = "Игр: " + user.GamesCount;
			CalculateRank(user.Raiting);
			return user.Region;
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

		private void LoadLeaderBoard(string region)
		{
			// Загрузка регионов
			List<Region> Regions = DBConnection.GetRegions();
			CBRegion.ItemsSource = Regions;
			CBRegion.SelectedIndex = Regions.First(r => r.Name == region).ID - 1;
			// Загрузка таблицы лидеров
			GetLeaders();
		}

		private void GetLeaders()
		{
			// Формирование таблицы лидеров
			string currentQuery = "";
			DGLeaders.ItemsSource = null;
			if (RBLeaderRegion.IsChecked == true && CBRegion.SelectedIndex != -1)
			{
				// По региону
				currentQuery = $"SET @n := 0; SELECT @n:= @n + 1 AS `position`, `l`.* FROM " +
				$"(SELECT `u`.id, `u`.avatar, `u`.login, `u`.raiting, `r`.name AS 'region', " +
				$"(SELECT COUNT(*) FROM `user_game` AS `u_g` WHERE `u_g`.user_id = `u`.id) AS 'game' FROM `user` AS `u` " +
				$"INNER JOIN `region` AS `r` ON `u`.region_id = `r`.id " +
				$"WHERE `u`.region_id = {CBRegion.SelectedIndex + 1} ORDER BY `u`.`raiting` DESC LIMIT 100) AS `l`";
			}
			else
			{
				// По миру
				currentQuery = $"SET @n := 0; SELECT @n:= @n + 1 AS `position`, `l`.* FROM " +
				$"(SELECT `u`.id, `u`.avatar, `u`.login, `u`.raiting, `r`.name AS 'region', " +
				$"(SELECT COUNT(*) FROM `user_game` AS `u_g` WHERE `u_g`.user_id = `u`.id) AS 'game' FROM `user` AS `u` " +
				$"INNER JOIN `region` AS `r` ON `u`.region_id = `r`.id " +
				$"ORDER BY `u`.`raiting` DESC LIMIT 100) AS `l`";
			}
			DGLeaders.ItemsSource = DBConnection.GetLeaders(currentQuery);
		}

		private void BLocalPosition_Click(object sender, RoutedEventArgs e)
		{
			ShowPlace(DBConnection.UserID, (bool)RBLocalWorld.IsChecked);
		}

		private void ShowPlace(int id, bool IsWorld)
		{
			string currentQuery = "";
			// Узнать позицию
			if (IsWorld)
			{
				// По миру
				currentQuery = $"SET @n := 0; SELECT `u`.position FROM " +
				$"(SELECT id, @n:= @n + 1 AS 'position', login FROM `user` ORDER BY `raiting` DESC) AS `u` " +
				$"WHERE `u`.id = {id}";
			}
			else
			{
				// По региону
				currentQuery = $"SET @n := 0; SELECT `u`.position FROM " +
				$"(SELECT id, @n:= @n + 1 AS 'position' FROM `user` WHERE `region_id` = " +
				$"(SELECT `region_id` FROM `user` WHERE id = {id}) ORDER BY `raiting` DESC) AS `u` " +
				$"WHERE `u`.id = {id}";
			}
			object place = DBConnection.GetLeaderPlace(currentQuery);
			if (place != null)
			{
				if (IsWorld)
					InfoMessage($"Место по миру: {place}");
				else
					InfoMessage($"Место по региону: {place}");
			}
			else
				InfoMessage("Игрок не найден");
		}

		private void BClearFind_Click(object sender, RoutedEventArgs e)
		{
			TBFind.Text = "";
		}

		private void Field_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]");
			if (regex.IsMatch(e.Text))
				e.Handled = true;
		}

		private void TBFind_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && TBFind.Text != "")
			{
				ShowPlace(Convert.ToInt32(TBFind.Text), (bool)RBLeaderWorld.IsChecked);
				TBFind.Text = "";
			}
		}

		private void CBRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			GetLeaders();
		}

		private void RBLeaderWorld_Checked(object sender, RoutedEventArgs e)
		{
			GetLeaders();
		}

		private void InfoMessage(string content, string button = "X")
		{
			InfoBar.MessageQueue.Enqueue(content, button, delegate () { });
		}
	}
}