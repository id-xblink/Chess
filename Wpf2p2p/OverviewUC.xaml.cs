using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Wpf2p2p
{
	public partial class OverviewUC : UserControl
	{
		List<Button> Buttons = new List<Button>();
		List<ChessTurn> Turns = new List<ChessTurn>();
		int ScrollIndex = 0;
		bool IsBlack = true;

		public OverviewUC()
		{
			InitializeComponent();
			LoadListView();
		}

		private void LoadListView()
		{
			string queryGames = $"SELECT `ug`.id AS 'ug_id', `g`.id AS 'id', `c`.name AS 'color', IF(`g`.`result_id` < 3, IF(`g`.`result_id` = `ug`.`color_id`, 'Победа', 'Поражение'), `r`.name) AS 'result', " +
			$"`t`.name AS 'type', (SELECT `in_u`.avatar FROM `user` AS `in_u` " +
			$"LEFT JOIN `user_game` AS `enemy_ug` ON `enemy_ug`.user_id = `in_u`.id WHERE `enemy_ug`.game_id = `ug`.game_id AND `in_u`.id != {DBConnection.UserID}) AS 'enemy_avatar' " +
			$"FROM `user_game` AS `ug` INNER JOIN `user` AS `u` INNER JOIN `game` AS `g` INNER JOIN `color` AS `c` INNER JOIN `result` AS `r` INNER JOIN `type` AS `t` " +
			$"ON `ug`.user_id = `u`.id AND `ug`.game_id = `g`.id AND `ug`.color_id = `c`.id AND `g`.result_id = `r`.id AND `g`.type_id = `t`.id " +
			$"WHERE `ug`.user_id = {DBConnection.UserID} ORDER BY `g`.id ASC";
			List<GamePreview> Previews = DBConnection.GetGamePreviews(queryGames);
			if (Previews.Count == 0)
				InfoMessage("Игры отсутствуют");
			else
				LBGames.ItemsSource = Previews;
		}

		private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ListViewItem lvi = sender as ListViewItem;
			int id = Convert.ToInt32(lvi.Tag);
			LoadLost(id);
			LoadData(id);
			InitBoard(GPreviewBoard, id, true);
		}

		private void LoadLost(int id)
		{
			string lostQuery = $"SELECT `l`.*, ((`l`.`black_queen` * 9) + (`l`.`black_rook` * 5) + ((`l`.`black_bishop` + `l`.`black_knight`) * 3) + (`l`.`black_pawn` * 1)) AS 'black_all', " +
			$"((`l`.`white_queen` *9) + (`l`.`white_rook` *5) +((`l`.`white_bishop` + `l`.`white_knight`) *3) +(`l`.`white_pawn` *1)) AS 'white_all' " +
			$"FROM `user_game` AS `ug` INNER JOIN `lost` AS `l` ON `l`.id = `ug`.game_id WHERE `ug`.id = {id}";
			Dictionary<string, object> Lost = DBConnection.GetLostFromPreview(lostQuery);
			LVWLost.ItemsSource = (List<LostFigures>)Lost["wl"];
			LVBLost.ItemsSource = (List<LostFigures>)Lost["bl"];
			TBCountWLost.Text = Lost["wa"].ToString();
			TBCountBLost.Text = Lost["ba"].ToString();
		}

		private void LoadData(int id)
		{
			string queryGameData = $"SELECT `g`.id AS 'id', `g`.datetime_start AS 'datetime', `t`.name AS 'type', `c`.name AS 'color', " +
			$"IF(`g`.`result_id` < 3, IF(`g`.`result_id` = `ug`.`color_id`, 'Победа', 'Поражение'), `r`.name) AS 'result', " +
			$"IF(`g`.`result_id` = `ug`.`color_id`, `g`.`raiting`, `g`.`raiting` *-1) AS 'raiting', " +
			$"IFNULL((SELECT CONCAT(CONVERT((`tr`.time_for_game / 60), INT), ' + ', `tr`.time_after_turn) " +
			$"FROM `timer` AS `tr` WHERE `tr`.id = `g`.timer_id), 'Нет') AS 'timer' FROM `user_game` AS `ug` " +
			$"INNER JOIN `game` AS `g` INNER JOIN `color` AS `c` INNER JOIN `type` AS `t` INNER JOIN `result` AS `r` " +
			$"ON `ug`.game_id = `g`.id AND `ug`.color_id = `c`.id AND `g`.type_id = `t`.id AND `g`.result_id = `r`.id " +
			$"WHERE `ug`.id = {id}";
			Dictionary<string, string> GameData = DBConnection.GetGameDataFromPreview(queryGameData);
			TBGameType.Text = GameData["type"];
			TBGameColor.Text = GameData["color"];
			TBGameDT.Text = GameData["data"];
			TBGameID.Text = $"ID игры: {GameData["id"]}";
			BOverviewGame.Tag = GameData["id"];
			TBGameResult.Text = GameData["result"];
			TBGameRaiting.Text = GameData["raiting"];
			TBGameTimeControl.Text = GameData["timer"];
		}
		
		private void InitBoard(Grid ChessBoardGrid, int id, bool IsPreview)
		{
			Buttons.Clear();
			ChessBoardGrid.Children.Clear();
			ChessBoardGrid.RowDefinitions.Clear();
			ChessBoardGrid.ColumnDefinitions.Clear();
			Style FlatStyle = FindResource("MaterialDesignFlatButton") as Style;
			for (int i = 0; i < 8; i++)
			{
				ChessBoardGrid.RowDefinitions.Add(new RowDefinition());
				ChessBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
			}
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					Rectangle r = new Rectangle();
					Panel.SetZIndex(r, 0);
					if (i % 2 == 0)
					{
						if (j % 2 == 0)
							r.SetResourceReference(Shape.FillProperty, "ws");
						else
							r.SetResourceReference(Shape.FillProperty, "bs");
					}
					else
					{
						if (j % 2 == 0)
							r.SetResourceReference(Shape.FillProperty, "bs");
						else
							r.SetResourceReference(Shape.FillProperty, "ws");
					}
					if (j == 7)
					{
						TextBlock tb = new TextBlock
						{
							FontSize = 12,
							HorizontalAlignment = HorizontalAlignment.Right,
							VerticalAlignment = VerticalAlignment.Top,
							Foreground = Brushes.Red,
							Text = (8 - i).ToString(),
							Margin = new Thickness(0, 2, 2, 0),
						};
						if (i % 2 == 0)
						{
							if (j % 2 == 0)
								tb.SetResourceReference(ForegroundProperty, "bs");
							else
								tb.SetResourceReference(ForegroundProperty, "ws");
						}
						else
						{
							if (j % 2 == 0)
								tb.SetResourceReference(ForegroundProperty, "ws");
							else
								tb.SetResourceReference(ForegroundProperty, "bs");
						}
						Panel.SetZIndex(tb, 1);
						Grid.SetRow(tb, i);
						Grid.SetColumn(tb, j);
						ChessBoardGrid.Children.Add(tb);
					}
					if (i == 7)
					{
						TextBlock tb = new TextBlock
						{
							FontSize = 12,
							HorizontalAlignment = HorizontalAlignment.Left,
							VerticalAlignment = VerticalAlignment.Bottom,
							Foreground = Brushes.Red,
							Text = ((char)('a' + j)).ToString(),
							Margin = new Thickness(2, 0, 0, 2),
						};
						if (i % 2 == 0)
						{
							if (j % 2 == 0)
								tb.SetResourceReference(ForegroundProperty, "bs");
							else
								tb.SetResourceReference(ForegroundProperty, "ws");
						}
						else
						{
							if (j % 2 == 0)
								tb.SetResourceReference(ForegroundProperty, "ws");
							else
								tb.SetResourceReference(ForegroundProperty, "bs");
						}
						Panel.SetZIndex(tb, 1);
						Grid.SetRow(tb, i);
						Grid.SetColumn(tb, j);
						ChessBoardGrid.Children.Add(tb);
					}
					Button b = new Button
					{
						Padding = new Thickness(0),
						Width = double.NaN,
						Height = double.NaN,
						Style = FlatStyle,
					};
					Panel.SetZIndex(b, 2);
					Grid.SetRow(r, i);
					Grid.SetColumn(r, j);
					Grid.SetRow(b, i);
					Grid.SetColumn(b, j);
					ChessBoardGrid.Children.Add(r);
					ChessBoardGrid.Children.Add(b);
					Buttons.Add(b);
				}
			}
			if (IsPreview)
			{
				string queryPreviewFen = $"SELECT `game`.fen FROM `user_game` INNER JOIN `game` ON `user_game`.game_id = `game`.id WHERE `user_game`.id = {id}";
				InitFigures(DBConnection.GetFenOverview(queryPreviewFen), false);
			}
			else
				InitFigures("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", false);
		}

		private void InitFigures(string data, bool IsOverview)
		{
			if (IsOverview)
				Buttons.ForEach(b => b.Content = null);
			for (int j = 8; j >= 2; j--)
				data = data.Replace(j.ToString(), (j - 1).ToString() + "1");
			data = data.Replace("1", ".");
			string[] lines = data.Split('/');
			for (int y = 7; y >= 0; y--)
				for (int x = 0; x < 8; x++)
					if (lines[7 - y][x] != '.')
						SetFigures(7 - y, x, lines[7 - y][x]);
		}

		private void SetFigures(int y, int x, char f)
		{
			Button b = Buttons.FirstOrDefault(z => Grid.GetRow(z) == y && Grid.GetColumn(z) == x);
			string name = "";
			switch ((ChessRules.Figure)f)
			{
				case ChessRules.Figure.whiteKing:
					{
						name = "wk";
						break;
					}
				case ChessRules.Figure.whiteQueen:
					{
						name = "wq";
						break;
					}
				case ChessRules.Figure.whiteRook:
					{
						name = "wr";
						break;
					}
				case ChessRules.Figure.whiteBishop:
					{
						name = "wb";
						break;
					}
				case ChessRules.Figure.whiteKnight:
					{
						name = "wn";
						break;
					}
				case ChessRules.Figure.whitePawn:
					{
						name = "wp";
						break;
					}
				case ChessRules.Figure.blackKing:
					{
						name = "bk";
						break;
					}
				case ChessRules.Figure.blackQueen:
					{
						name = "bq";
						break;
					}
				case ChessRules.Figure.blackRook:
					{
						name = "br";
						break;
					}
				case ChessRules.Figure.blackBishop:
					{
						name = "bb";
						break;
					}
				case ChessRules.Figure.blackKnight:
					{
						name = "bn";
						break;
					}
				case ChessRules.Figure.blackPawn:
					{
						name = "bp";
						break;
					}
			}
			Image image = new Image();
			image.SetResourceReference(Image.SourceProperty, name);
			b.Content = image;
		}

		private void GetTurns(int id)
		{
			BPrevious.IsEnabled = false;
			BNext.IsEnabled = true;
			Turns = DBConnection.GetChessTurnsOverview(id);
			DGTurns.ItemsSource = Turns;
			TBPage.Text = $"0 из {Turns.Count}";
		}

		private void BTurnScroll_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			switch (b.Name)
			{
				case "BStart":
					{
						ScrollIndex = 0;
						IsBlack = true;
						break;
					}
				case "BPrevious":
					{
						if (ScrollIndex != 0)
						{
							if (!IsBlack)
								ScrollIndex--;
							IsBlack = !IsBlack;
						}
						break;
					}
				case "BNext":
					{
						if (ScrollIndex != Turns.Count || (Turns[ScrollIndex - 1].Black != "" ? (ScrollIndex == Turns.Count && !IsBlack) : false))
						{
							if (IsBlack)
								ScrollIndex++;
							IsBlack = !IsBlack;
						}
						break;
					}
				case "BEnd":
					{
						ScrollIndex = Turns.Count;
						if (Turns[ScrollIndex - 1].Black != "")
							IsBlack = true;
						else
							IsBlack = false;
						break;
					}
			}
			ReleaseTurn();
		}

		private void ReleaseTurn()
		{
			// Изменение доски
			if (ScrollIndex == 0 && IsBlack)
				InitFigures("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", true);
			else
			{
				string fen = "";

				if (!IsBlack)
					fen = Turns[ScrollIndex - 1].FenAfterWhite;
				else
					fen = Turns[ScrollIndex - 1].FenAfterBlack;

				string[] parts = fen.Split();
				InitFigures(parts[0], true);
			}
			// Изменение текста навигатора
			if (ScrollIndex != 0)
			{
				if (!IsBlack)
					TBColor.Text = "Белые";
				else
					TBColor.Text = "Чёрные";
			}
			else
				TBColor.Text = "Начало игры";
			TBPage.Text = $"{ScrollIndex} из {Turns.Count}";
			// Блокировка кнопок
			if (ScrollIndex != Turns.Count || (Turns[ScrollIndex - 1].Black != "" ? (ScrollIndex == Turns.Count && !IsBlack) : false))
				BNext.IsEnabled = true;
			else
				BNext.IsEnabled = false;
			if (ScrollIndex != 0)
				BPrevious.IsEnabled = true;
			else
				BPrevious.IsEnabled = false;
		}
		
		private void ShowOverview(int id)
		{
			// Проверка на существование игры
			if (!DBConnection.IsGameExistOverview(id))
			{
				InfoMessage("Игра не найдена");
				return;
			}
			GetTurns(id);
			InitBoard(GOverviewBoard, 0, false);
			GOverview.Visibility = Visibility.Visible;
			GPreview.Visibility = Visibility.Collapsed;
		}

		private void BPreviewGame_Click(object sender, RoutedEventArgs e)
		{
			ScrollIndex = 0;
			TBColor.Text = "Начало игры";
			TBPage.Text = "0 из 0";
			GPreview.Visibility = Visibility.Visible;
			GOverview.Visibility = Visibility.Collapsed;
		}

		private void BOverviewGame_Click(object sender, RoutedEventArgs e)
		{
			// Если ничего не выбрано
			if (BOverviewGame.Tag == null)
			{
				InfoMessage("Выберите игру");
				return;
			}
			ShowOverview(Convert.ToInt32(BOverviewGame.Tag));
		}

		private void TBFind_PreviewKeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter && TBFind.Text != "")
			{
				ShowOverview(Convert.ToInt32(TBFind.Text));
				TBFind.Text = "";
			}
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
		
		private void BOpenWhiteTurn_Click(object sender, RoutedEventArgs e)
		{
			if (DGTurns.SelectedIndex == -1)
			{
				InfoMessage("Выберите ход");
				return;
			}
			ScrollIndex = DGTurns.SelectedIndex + 1;
			IsBlack = false;
			ReleaseTurn();
		}

		private void BOpenBlackTurn_Click(object sender, RoutedEventArgs e)
		{
			if (DGTurns.SelectedIndex == -1)
			{
				InfoMessage("Выберите ход");
				return;
			}
			int Index = DGTurns.SelectedIndex + 1;
			// Проверка последнего хода
			if (Index == Turns.Count && Turns[Index - 1].Black == "")
			{
				InfoMessage("Чёрного хода нет");
				return;
			}
			ScrollIndex = Index;
			IsBlack = true;
			ReleaseTurn();
		}

		private void InfoMessage(string content, string button = "X")
		{
			InfoBar.MessageQueue.Enqueue(content, button, delegate () { });
		}
	}
}