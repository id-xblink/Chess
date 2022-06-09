using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Wpf2p2p
{
	public partial class StatisticsUC : UserControl
	{
		string whereResult = "";
		string whereDateTime = "";
		string whereTimer = "";
		string whereColor = "";
		string whereType = "";
		string whereRaiting = "";

		public StatisticsUC()
		{
			InitializeComponent();
			BindEvents();
			TakeFromBD();
		}

		private void BindEvents()
		{
			CBResultWin.Click += CBResult_Click;
			CBResultLose.Click += CBResult_Click;
			CBResultDraw.Click += CBResult_Click;
			CBResultStalemate.Click += CBResult_Click;
			RBTimerAny.Click += RBTimer_Click;
			RBTimerYes.Click += RBTimer_Click;
			RBTimerNo.Click += RBTimer_Click;
			RBColorAny.Click += RBColor_Click;
			RBColorWhite.Click += RBColor_Click;
			RBColorBlack.Click += RBColor_Click;
			CBTypeDuo.Click += CBType_Click;
			CBTypeNet.Click += CBType_Click;
			CBTypeSolo.Click += CBType_Click;
			CBTypeSolo1.Click += CBType_Click;
			CBTypeSolo2.Click += CBType_Click;
			CBTypeSolo3.Click += CBType_Click;
			CBTypeSolo4.Click += CBType_Click;
			CBTypeSolo5.Click += CBType_Click;
			SRaitingFrom.ValueChanged += SRaiting_ValueChanged;
			SRaitingTo.ValueChanged += SRaiting_ValueChanged;
		}

		private void TakeFromBD()
		{
			string gameQuery = $"SELECT `game`.id, `game`.datetime_start, TIME_TO_SEC(TIMEDIFF(`datetime_end`, `datetime_start`)) AS 'duration', " +
			$"`type`.name AS 'type', IF(`result_id` < 3, IF(`result_id` = `color_id`, 'Победа', 'Поражение'), `result`.name) AS 'result', " +
			$"`color`.name AS 'color', IF(`timer_id` IS NOT NULL, 'Есть', 'Нет') AS 'timer', `game`.raiting " +
			$"FROM `game` " +
			$"INNER JOIN `user_game` INNER JOIN `color` " +
			$"INNER JOIN `result` INNER JOIN `type` " +
			$"ON `game`.id = `user_game`.game_id AND `game`.result_id = `result`.id AND " +
			$"`user_game`.color_id = `color`.id AND `game`.type_id = `type`.id " +
			$"WHERE `user_game`.user_id = {DBConnection.UserID}" +
			whereResult + whereDateTime + whereTimer + whereColor + whereType + whereRaiting;
			DGGames.ItemsSource = DBConnection.GetUserGames(gameQuery).Tables[0].DefaultView;
			if (DGGames.Items.Count == 0)
			{
				DGGames.IsEnabled = false;
				InfoMessage("Игры отсутствуют");
			}
			else
				DGGames.IsEnabled = true;
		}

		private void BShowMore_Click(object sender, RoutedEventArgs e)
		{
			if (DGGames.Visibility != Visibility.Visible)
				return;
			Button b = sender as Button;
			switch (b.Tag.ToString())
			{
				case "Single":
					if (DGGames.SelectedItems.Count != 1)
					{
						InfoMessage("Выберите одну игру");
						return;
					}
					break;
				case "Multiple":
					if (DGGames.SelectedItems.Count < 1)
					{
						InfoMessage("Выберите хотя бы одну игру");
						return;
					}
					break;
				case "All":
					if (DGGames.Items.Count < 1)
					{
						InfoMessage("Игры отсутствуют");
						return;
					}
					break;
			}
			List<int> ids = new List<int>();
			if (b.Tag.ToString() == "All")
			{
				foreach (DataRowView row in DGGames.Items)
					ids.Add(Convert.ToInt32(row["id"]));
			}
			else
			{
				foreach (DataRowView row in DGGames.SelectedItems)
					ids.Add(Convert.ToInt32(row["id"]));
			}

			string idQuery = " (";
			foreach (int id in ids)
			{
				idQuery += $"`game_id` = {id} OR ";
			}
			int idx = idQuery.LastIndexOf(" OR ");
			idQuery = idQuery.Remove(idx, 4) + ")";
			List<int> WhiteGames = DBConnection.GetColorGames($"SELECT `game_id` FROM `user_game` WHERE `color_id` = 1 AND `user_id` = {DBConnection.UserID} AND {idQuery}");
			List<int> BlackGames = DBConnection.GetColorGames($"SELECT `game_id` FROM `user_game` WHERE `color_id` = 2 AND `user_id` = {DBConnection.UserID} AND {idQuery}");
			string bracketQuery = " AND (";
			foreach (int id in ids)
			{
				bracketQuery += $"`game`.`id` = {id} OR ";
			}
			idx = bracketQuery.LastIndexOf(" OR ");
			bracketQuery = bracketQuery.Remove(idx, 4) + ")";
			for (int i = 1; i <= 2; i++)
			{
				string orQuery = " WHERE (";
				foreach (int id in i == 1 ? WhiteGames : BlackGames)
				{
					orQuery += $"`id` = {id} OR ";
				}
				idx = orQuery.LastIndexOf(" OR ");
				if (idx != -1)
				{
					orQuery = orQuery.Remove(idx, 4) + ")";
					DataAttack(orQuery, i);
					DataAction(orQuery, i);
				}
			}
			DataCommon(bracketQuery);
			DataUnique(bracketQuery);
			DGGames.IsEnabled = false;
			DGGames.Tag = false;
			GStatistics.Visibility = Visibility.Visible;
			DGGames.Visibility = Visibility.Collapsed;
		}

		private void BShowAll_Click(object sender, RoutedEventArgs e)
		{
			DGGames.Visibility = Visibility.Visible;
			GStatistics.Visibility = Visibility.Collapsed;
			DGGames.Tag = true;
			DGGames.IsEnabled = true;
		}

		private void DataAttack(string orQuery, int color)
		{
			if (color == 1)
			{
				string eatQuery = "SELECT " +
				"CAST(CONCAT(SUM(`white_king`), ' / ', ROUND(AVG(`white_king`), 2)) AS CHAR) AS 'white_king', " +
				"CAST(CONCAT(SUM(`white_queen`), ' / ', ROUND(AVG(`white_queen`), 2)) AS CHAR) AS 'white_queen', " +
				"CAST(CONCAT(SUM(`white_rook`), ' / ', ROUND(AVG(`white_rook`), 2)) AS CHAR) AS 'white_rook', " +
				"CAST(CONCAT(SUM(`white_bishop`), ' / ', ROUND(AVG(`white_bishop`), 2)) AS CHAR) AS 'white_bishop', " +
				"CAST(CONCAT(SUM(`white_knight`), ' / ', ROUND(AVG(`white_knight`), 2)) AS CHAR) AS 'white_knight', " +
				"CAST(CONCAT(SUM(`white_pawn`), ' / ', ROUND(AVG(`white_pawn`), 2)) AS CHAR) AS 'white_pawn', " +
				"CAST(CONCAT(SUM(`white_king`) + SUM(`white_queen`) + SUM(`white_rook`) + SUM(`white_bishop`) + SUM(`white_knight`) + SUM(`white_pawn`), ' / ', " +
				"ROUND((AVG(`white_king`) + AVG(`white_queen`) + AVG(`white_rook`) + AVG(`white_bishop`) + AVG(`white_knight`) + AVG(`white_pawn`)), 2)) AS CHAR) AS 'white_total' " +
				"FROM `eat`" + orQuery;
				string lostQuery = "SELECT " +
				"CAST(CONCAT(SUM(`white_queen`), ' / ', ROUND(AVG(`white_queen`), 2)) AS CHAR) AS 'white_queen', " +
				"CAST(CONCAT(SUM(`white_rook`), ' / ', ROUND(AVG(`white_rook`), 2)) AS CHAR) AS 'white_rook', " +
				"CAST(CONCAT(SUM(`white_bishop`), ' / ', ROUND(AVG(`white_bishop`), 2)) AS CHAR) AS 'white_bishop', " +
				"CAST(CONCAT(SUM(`white_knight`), ' / ', ROUND(AVG(`white_knight`), 2)) AS CHAR) AS 'white_knight', " +
				"CAST(CONCAT(SUM(`white_pawn`), ' / ', ROUND(AVG(`white_pawn`), 2)) AS CHAR) AS 'white_pawn', " +
				"CAST(CONCAT(SUM(`white_queen`) + SUM(`white_rook`) + SUM(`white_bishop`) + SUM(`white_knight`) + SUM(`white_pawn`), ' / ', " +
				"ROUND((AVG(`white_queen`) + AVG(`white_rook`) + AVG(`white_bishop`) + AVG(`white_knight`) + AVG(`white_pawn`)), 2)) AS CHAR) AS 'white_total' " +
				"FROM `lost`" + orQuery;
				// Съедено
				Dictionary<string, string> Eat = DBConnection.GetEatFromStat(eatQuery, color);
				TBWEatK.Text = Eat["wk"];
				TBWEatQ.Text = Eat["wq"];
				TBWEatR.Text = Eat["wr"];
				TBWEatB.Text = Eat["wb"];
				TBWEatN.Text = Eat["wn"];
				TBWEatP.Text = Eat["wp"];
				TBWEatAll.Text = Eat["wa"];
				// Потеряно
				Dictionary<string, string> Lost = DBConnection.GetLostFromStat(lostQuery, color);
				TBWLostQ.Text = Lost["wq"];
				TBWLostR.Text = Lost["wr"];
				TBWLostB.Text = Lost["wb"];
				TBWLostN.Text = Lost["wn"];
				TBWLostP.Text = Lost["wp"];
				TBWLostAll.Text = Lost["wa"];
			}
			else
			{
				string eatQuery = "SELECT " +
				"CAST(CONCAT(SUM(`black_king`), ' / ', ROUND(AVG(`black_king`), 2)) AS CHAR) AS 'black_king', " +
				"CAST(CONCAT(SUM(`black_queen`), ' / ', ROUND(AVG(`black_queen`), 2)) AS CHAR) AS 'black_queen', " +
				"CAST(CONCAT(SUM(`black_rook`), ' / ', ROUND(AVG(`black_rook`), 2)) AS CHAR) AS 'black_rook', " +
				"CAST(CONCAT(SUM(`black_bishop`), ' / ', ROUND(AVG(`black_bishop`), 2)) AS CHAR) AS 'black_bishop', " +
				"CAST(CONCAT(SUM(`black_knight`), ' / ', ROUND(AVG(`black_knight`), 2)) AS CHAR) AS 'black_knight', " +
				"CAST(CONCAT(SUM(`black_pawn`), ' / ', ROUND(AVG(`black_pawn`), 2)) AS CHAR) AS 'black_pawn', " +
				"CAST(CONCAT(SUM(`black_king`) + SUM(`black_queen`) + SUM(`black_rook`) + SUM(`black_bishop`) + SUM(`black_knight`) + SUM(`black_pawn`), ' / ', " +
				"ROUND((AVG(`black_king`) + AVG(`black_queen`) + AVG(`black_rook`) + AVG(`black_bishop`) + AVG(`black_knight`) + AVG(`black_pawn`)), 2)) AS CHAR) AS 'black_total' " +
				"FROM `eat`" + orQuery;
				string lostQuery = "SELECT " +
				"CAST(CONCAT(SUM(`black_queen`), ' / ', ROUND(AVG(`black_queen`), 2)) AS CHAR) AS 'black_queen', " +
				"CAST(CONCAT(SUM(`black_rook`), ' / ', ROUND(AVG(`black_rook`), 2)) AS CHAR) AS 'black_rook', " +
				"CAST(CONCAT(SUM(`black_bishop`), ' / ', ROUND(AVG(`black_bishop`), 2)) AS CHAR) AS 'black_bishop', " +
				"CAST(CONCAT(SUM(`black_knight`), ' / ', ROUND(AVG(`black_knight`), 2)) AS CHAR) AS 'black_knight', " +
				"CAST(CONCAT(SUM(`black_pawn`), ' / ', ROUND(AVG(`black_pawn`), 2)) AS CHAR) AS 'black_pawn', " +
				"CAST(CONCAT(SUM(`black_queen`) + SUM(`black_rook`) + SUM(`black_bishop`) + SUM(`black_knight`) + SUM(`black_pawn`), ' / ', " +
				"ROUND((AVG(`black_queen`) + AVG(`black_rook`) + AVG(`black_bishop`) + AVG(`black_knight`) + AVG(`black_pawn`)), 2)) AS CHAR) AS 'black_total' " +
				"FROM `lost`" + orQuery;
				// Съедено
				Dictionary<string, string> Eat = DBConnection.GetEatFromStat(eatQuery, color);
				TBBEatK.Text = Eat["bk"];
				TBBEatQ.Text = Eat["bq"];
				TBBEatR.Text = Eat["br"];
				TBBEatB.Text = Eat["bb"];
				TBBEatN.Text = Eat["bn"];
				TBBEatP.Text = Eat["bp"];
				TBBEatAll.Text = Eat["ba"];
				// Потеряно
				Dictionary<string, string> Lost = DBConnection.GetLostFromStat(lostQuery, color);
				TBBLostQ.Text = Lost["bq"];
				TBBLostR.Text = Lost["br"];
				TBBLostB.Text = Lost["bb"];
				TBBLostN.Text = Lost["bn"];
				TBBLostP.Text = Lost["bp"];
				TBBLostAll.Text = Lost["ba"];
			}
		}

		private void DataAction(string orQuery, int color)
		{
			if (color == 1)
			{
				string castlingQuery = "SELECT " +
				"CAST(CONCAT(SUM(`white_long`), ' / ', ROUND(AVG(`white_long`), 2)) AS CHAR) AS 'white_long', " +
				"CAST(CONCAT(SUM(`white_short`), ' / ', ROUND(AVG(`white_short`), 2)) AS CHAR) AS 'white_short', " +
				"CAST(CONCAT(SUM(`white_long`) + SUM(`white_short`), ' / ', " +
				"ROUND((AVG(`white_long`) + AVG(`white_short`)), 2)) AS CHAR) AS 'white_total' " +
				"FROM `castling`" + orQuery;
				string checkQuery = "SELECT " +
				"CAST(CONCAT(SUM(`white_king`), ' / ', ROUND(AVG(`white_king`), 2)) AS CHAR) AS 'white_total' " +
				"FROM `checked`" + orQuery;
				string enpassantQuery = "SELECT " +
				"CAST(CONCAT(SUM(`white_pawn`), ' / ', ROUND(AVG(`white_pawn`), 2)) AS CHAR) AS 'white_total' " +
				"FROM `enpassant`" + orQuery;
				string promotionQuery = "SELECT " +
				"CAST(CONCAT(SUM(`white_queen`), ' / ', ROUND(AVG(`white_queen`), 2)) AS CHAR) AS 'white_queen', " +
				"CAST(CONCAT(SUM(`white_rook`), ' / ', ROUND(AVG(`white_rook`), 2)) AS CHAR) AS 'white_rook', " +
				"CAST(CONCAT(SUM(`white_bishop`), ' / ', ROUND(AVG(`white_bishop`), 2)) AS CHAR) AS 'white_bishop', " +
				"CAST(CONCAT(SUM(`white_knight`), ' / ', ROUND(AVG(`white_knight`), 2)) AS CHAR) AS 'white_knight', " +
				"CAST(CONCAT(SUM(`white_queen`) + SUM(`white_rook`) + SUM(`white_bishop`) + SUM(`white_knight`), ' / ', " +
				"ROUND((AVG(`white_queen`) + AVG(`white_rook`) + AVG(`white_bishop`) + AVG(`white_knight`)), 2)) AS CHAR) AS 'white_total' " +
				"FROM `promotion`" + orQuery;
				// Рокировки
				Dictionary<string, string> Castling = DBConnection.GetCastleFromStat(castlingQuery, color);
				TBWCastleL.Text = Castling["wl"];
				TBWCastleS.Text = Castling["ws"];
				TBWCastleAll.Text = Castling["wa"];
				// Шахи
				Dictionary<string, string> Check = DBConnection.GetCheckFromStat(checkQuery, color);
				TBWCheckAll.Text = Check["wa"];
				// Взятий на проходе
				Dictionary<string, string> Enpassant = DBConnection.GetEnpassantFromStat(enpassantQuery, color);
				TBWEnpassantAll.Text = Enpassant["wa"];
				// Превращений
				Dictionary<string, string> Promotion = DBConnection.GetPromoteFromStat(promotionQuery, color);
				TBWPromoteQ.Text = Promotion["wq"];
				TBWPromoteR.Text = Promotion["wr"];
				TBWPromoteB.Text = Promotion["wb"];
				TBWPromoteN.Text = Promotion["wn"];
				TBWPromoteAll.Text = Promotion["wa"];
			}
			else
			{
				string castlingQuery = "SELECT " +
				"CAST(CONCAT(SUM(`black_long`), ' / ', ROUND(AVG(`black_long`), 2)) AS CHAR) AS 'black_long', " +
				"CAST(CONCAT(SUM(`black_short`), ' / ', ROUND(AVG(`black_short`), 2)) AS CHAR) AS 'black_short', " +
				"CAST(CONCAT(SUM(`black_long`) + SUM(`black_short`), ' / ', " +
				"ROUND((AVG(`black_long`) + AVG(`black_short`)), 2)) AS CHAR) AS 'black_total' " +
				"FROM `castling`" + orQuery;
				string checkQuery = "SELECT " +
				"CAST(CONCAT(SUM(`black_king`), ' / ', ROUND(AVG(`black_king`), 2)) AS CHAR) AS 'black_total' " +
				"FROM `checked`" + orQuery;
				string enpassantQuery = "SELECT " +
				"CAST(CONCAT(SUM(`black_pawn`), ' / ', ROUND(AVG(`black_pawn`), 2)) AS CHAR) AS 'black_total' " +
				"FROM `enpassant`" + orQuery;
				string promotionQuery = "SELECT " +
				"CAST(CONCAT(SUM(`black_queen`), ' / ', ROUND(AVG(`black_queen`), 2)) AS CHAR) AS 'black_queen', " +
				"CAST(CONCAT(SUM(`black_rook`), ' / ', ROUND(AVG(`black_rook`), 2)) AS CHAR) AS 'black_rook', " +
				"CAST(CONCAT(SUM(`black_bishop`), ' / ', ROUND(AVG(`black_bishop`), 2)) AS CHAR) AS 'black_bishop', " +
				"CAST(CONCAT(SUM(`black_knight`), ' / ', ROUND(AVG(`black_knight`), 2)) AS CHAR) AS 'black_knight', " +
				"CAST(CONCAT(SUM(`black_queen`) + SUM(`black_rook`) + SUM(`black_bishop`) + SUM(`black_knight`), ' / ', " +
				"ROUND((AVG(`black_queen`) + AVG(`black_rook`) + AVG(`black_bishop`) + AVG(`black_knight`)), 2)) AS CHAR) AS 'black_total' " +
				"FROM `promotion`" + orQuery;
				// Рокировки
				Dictionary<string, string> Castling = DBConnection.GetCastleFromStat(castlingQuery, color);
				TBBCastleL.Text = Castling["bl"];
				TBBCastleS.Text = Castling["bs"];
				TBBCastleAll.Text = Castling["ba"];
				// Шахи
				Dictionary<string, string> Check = DBConnection.GetCheckFromStat(checkQuery, color);
				TBBCheckAll.Text = Check["ba"];
				// Взятий на проходе
				Dictionary<string, string> Enpassant = DBConnection.GetEnpassantFromStat(enpassantQuery, color);
				TBBEnpassantAll.Text = Enpassant["ba"];
				// Превращений
				Dictionary<string, string> Promotion = DBConnection.GetPromoteFromStat(promotionQuery, color);
				TBBPromoteQ.Text = Promotion["bq"];
				TBBPromoteR.Text = Promotion["br"];
				TBBPromoteB.Text = Promotion["bb"];
				TBBPromoteN.Text = Promotion["bn"];
				TBBPromoteAll.Text = Promotion["ba"];
			}
		}

		private void DataCommon(string bracketQuery)
		{
			int whiteGames = 0;
			int blackGames = 0;
			// Тип игры
			for (int i = 1; i <= 2; i++) // 1 / 2 | Белый / Чёрный цвет
			{
				int total = 0;
				// Типы игры
				for (int j = 1; j <= 7; j++)
				{
					string typeQuery = $"SELECT COUNT(`type_id`) AS 'count' FROM `game` " +
					$"INNER JOIN `user_game` ON `game`.id = `user_game`.game_id " +
					$"WHERE `user_game`.user_id = {DBConnection.UserID} AND `type_id` = {j} AND `user_game`.color_id = {i}" + bracketQuery;
					string result = DBConnection.GetCommonFromStat(typeQuery);
					total += Convert.ToInt32(result);
					switch (j)
					{
						case 1:
							if (i == 1)
								TBWTypeD.Text = "Двойных игр: " + result;
							else
								TBBTypeD.Text = "Двойных игр: " + result;
							break;
						case 2:
							if (i == 1)
								TBWTypeS1.Text = "Одиночных (Бот №1) игр: " + result;
							else
								TBBTypeS1.Text = "Одиночных (Бот №1) игр: " + result;
							break;
						case 3:
							if (i == 1)
								TBWTypeS2.Text = "Одиночных (Бот №2) игр: " + result;
							else
								TBBTypeS2.Text = "Одиночных (Бот №2) игр: " + result;
							break;
						case 4:
							if (i == 1)
								TBWTypeS3.Text = "Одиночных (Бот №3) игр: " + result;
							else
								TBBTypeS3.Text = "Одиночных (Бот №3) игр: " + result;
							break;
						case 5:
							if (i == 1)
								TBWTypeS4.Text = "Одиночных (Бот №4) игр: " + result;
							else
								TBBTypeS4.Text = "Одиночных (Бот №4) игр: " + result;
							break;
						case 6:
							if (i == 1)
								TBWTypeS5.Text = "Одиночных (Бот №5) игр: " + result;
							else
								TBBTypeS5.Text = "Одиночных (Бот №5) игр: " + result;
							break;
						case 7:
							if (i == 1)
								TBWTypeN.Text = "Сетевых игр: " + result;
							else
								TBBTypeN.Text = "Сетевых игр: " + result;
							break;
					}
				}
				if (i == 1)
				{
					TBWTypeAll.Text = "Всего игр: " + total;
					whiteGames = total;
				}
				else
				{
					TBBTypeAll.Text = "Всего игр: " + total;
					blackGames = total;
				}
			}
			// Соотношение цветов
			double totalGames = whiteGames + blackGames;
			double GamesForOnePercent = totalGames / 100;
			double whitePercent = Math.Round(whiteGames / GamesForOnePercent, 2);
			double blackPercent = Math.Round(blackGames / GamesForOnePercent, 2);
			TBWColorAll.Text = $"Выбор: {whitePercent}%";
			TBBColorAll.Text = $"Выбор: {blackPercent}%";
			// Результат
			for (int i = 1; i <= 2; i++) // 1 / 2 | Белый / Чёрный цвет
			{
				totalGames = 0;
				double winGames = 0;
				// Результаты игры
				for (int j = 1; j <= 4; j++)
				{
					string resultQuery = $"SELECT COUNT(`result_id`) AS 'count' FROM `game` " +
					$"INNER JOIN `user_game` ON `game`.id = `user_game`.game_id " +
					$"WHERE `user_game`.user_id = {DBConnection.UserID} AND `result_id` = {j} AND `user_game`.color_id = {i}" + bracketQuery;
					string result = DBConnection.GetCommonFromStat(resultQuery);
					totalGames += Convert.ToInt32(result);
					switch (j)
					{
						case 1:
							if (i == 1)
							{
								TBWResultW.Text = "Побед: " + result;
								winGames = Convert.ToInt32(result);
							}
							else
								TBBResultL.Text = "Поражений: " + result;
							break;
						case 2:
							if (i == 1)
								TBWResultL.Text = "Поражений: " + result;
							else
							{
								TBBResultW.Text = "Побед: " + result;
								winGames = Convert.ToInt32(result);
							}
							break;
						case 3:
							if (i == 1)
								TBWResultS.Text = "Патов: " + result;
							else
								TBBResultS.Text = "Патов: " + result;
							break;
						case 4:
							if (i == 1)
								TBWResultD.Text = "Ничьих: " + result;
							else
								TBBResultD.Text = "Ничьих: " + result;
							break;
					}
				}
				GamesForOnePercent = totalGames / 100;
				double winrate = Math.Round(winGames / GamesForOnePercent, 2);
				if (double.IsNaN(winrate))
					winrate = 0;
				if (i == 1)
					TBWResultWR.Text = $"Процент побед: {winrate}%";
				else
					TBBResultWR.Text = $"Процент побед: {winrate}%";
			}
		}

		private void DataUnique(string bracketQuery)
		{
			for (int i = 1; i <= 2; i++)
			{
				// Ценность
				string evaluationQuery = $"SELECT " +
				$"IFNULL(CAST(CONCAT(SUM(`evaluation` {(i == 1 ? "* 1" : "* -1")}), ' / ', " +
				$"ROUND(AVG(`evaluation` {(i == 1 ? "* 1" : "* -1")}), 2)) AS CHAR), '0 / 0.00') AS 'evaluation' " +
				$"FROM `game` " +
				$"INNER JOIN `user_game` ON `game`.id = `user_game`.game_id " +
				$"WHERE `user_game`.user_id = {DBConnection.UserID} AND `user_game`.color_id = {i}" + bracketQuery;
				if (i == 1)
					TBWEvaluationAll.Text = "Ценность досок: " + DBConnection.GetUniqueFromStat(evaluationQuery);
				else
					TBBEvaluationAll.Text = "Ценность досок: " + DBConnection.GetUniqueFromStat(evaluationQuery);
				// Продолжительность игр
				string durationQuery = $"SELECT " +
				$"IFNULL(CAST(CONCAT(ROUND(SUM(TIME_TO_SEC(TIMEDIFF(`datetime_end`, `datetime_start`))) / 60, 2), ' / ', " +
				$"ROUND(AVG(TIME_TO_SEC(TIMEDIFF(`datetime_end`, `datetime_start`))) / 60, 2)) AS CHAR), '0 / 0.00') AS 'duration' " +
				$"FROM `game` " +
				$"INNER JOIN `user_game` ON `game`.id = `user_game`.game_id " +
				$"WHERE `user_game`.user_id = {DBConnection.UserID} AND `user_game`.color_id = {i}" + bracketQuery;
				if (i == 1)
					TBWDurationAll.Text = "Минут в играх: " + DBConnection.GetUniqueFromStat(durationQuery);
				else
					TBBDurationAll.Text = "Минут в играх: " + DBConnection.GetUniqueFromStat(durationQuery);
				// Таймер
				string timerQuery = $"SELECT " +
				$"IFNULL(CAST(CONCAT(COUNT(`timer_id`), ' / ', " +
				$"ROUND(COUNT(`timer_id`) / (COUNT(`game`.id) / 100), 2), '%') AS CHAR), '0 / 0.00%') AS 'timer' " +
				$"FROM `game` " +
				$"INNER JOIN `user_game` ON `game`.id = `user_game`.game_id " +
				$"WHERE `user_game`.user_id = {DBConnection.UserID} AND `user_game`.color_id = {i}" + bracketQuery;
				if (i == 1)
					TBWTimerAll.Text = "Игр с таймером: " + DBConnection.GetUniqueFromStat(timerQuery);
				else
					TBBTimerAll.Text = "Игр с таймером: " + DBConnection.GetUniqueFromStat(timerQuery);
				// Рейтинг
				string raitingQuery = $"SELECT " +
				$"IFNULL(CAST(CONCAT(SUM(IF(`game`.`result_id` = `user_game`.color_id, `raiting`, `raiting` * -1)), ' / ', " +
				$"ROUND(AVG(IF(`game`.`result_id` = `user_game`.color_id, `raiting`, `raiting` * -1)), 2)) AS CHAR), '0 / 0.00') AS 'raiting' " +
				$"FROM `game` " +
				$"INNER JOIN `user_game` ON `game`.id = `user_game`.game_id " +
				$"WHERE `user_game`.user_id = {DBConnection.UserID} AND `user_game`.color_id = {i}" + bracketQuery;
				if (i == 1)
					TBWRaitingAll.Text = "Получено рейтинга: " + DBConnection.GetUniqueFromStat(raitingQuery);
				else
					TBBRaitingAll.Text = "Получено рейтинга: " + DBConnection.GetUniqueFromStat(raitingQuery);
			}
		}

		private void CBResult_Click(object sender, RoutedEventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			whereResult = "";
			if (!(bool)CBResultWin.IsChecked)
				whereResult += " AND IF(`result_id` < 3, IF(`result_id` = `color_id`, 'Победа', 'Поражение'), `result`.name) != 'Победа'";
			if (!(bool)CBResultLose.IsChecked)
				whereResult += " AND IF(`result_id` < 3, IF(`result_id` = `color_id`, 'Победа', 'Поражение'), `result`.name) != 'Поражение'";
			if (!(bool)CBResultDraw.IsChecked)
				whereResult += " AND IF(`result_id` < 3, IF(`result_id` = `color_id`, 'Победа', 'Поражение'), `result`.name) != 'Ничья'";
			if (!(bool)CBResultStalemate.IsChecked)
				whereResult += " AND IF(`result_id` < 3, IF(`result_id` = `color_id`, 'Победа', 'Поражение'), `result`.name) != 'Пат'";
			TakeFromBD();
		}

		private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			whereDateTime = "";
			if (DPStart.SelectedDate != null)
				whereDateTime += $" AND `game`.datetime_start >= '{Convert.ToDateTime(DPStart.SelectedDate).ToString("yyyy-MM-dd 00:00:00")}'";
			if (DPEnd.SelectedDate != null)
				whereDateTime += $" AND `game`.datetime_start <= '{Convert.ToDateTime(DPEnd.SelectedDate).ToString("yyyy-MM-dd 23:59:59")}'";
			TakeFromBD();
		}

		private void RBTimer_Click(object sender, RoutedEventArgs e)
		{
			RadioButton rb = sender as RadioButton;
			switch (rb.Tag.ToString())
			{
				case "Any":
					whereTimer = "";
					break;
				case "Yes":
					whereTimer = " AND `game`.timer_id IS NOT NULL";
					break;
				case "No":
					whereTimer = " AND `game`.timer_id IS NULL";
					break;
			}
			TakeFromBD();
		}

		private void RBColor_Click(object sender, RoutedEventArgs e)
		{
			RadioButton rb = sender as RadioButton;
			switch (rb.Tag.ToString())
			{
				case "Any":
					whereColor = "";
					break;
				case "White":
					whereColor = " AND `user_game`.color_id = 1";
					break;
				case "Black":
					whereColor = " AND `user_game`.color_id = 2";
					break;
			}
			TakeFromBD();
		}

		private void CBType_Click(object sender, RoutedEventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			whereType = "";
			if (!(bool)CBTypeDuo.IsChecked)
				whereType += " AND `game`.type_id != 1";
			if (!(bool)CBTypeNet.IsChecked)
				whereType += " AND `game`.type_id != 7";
			if ((bool)CBTypeSolo.IsChecked)
			{
				if (!(bool)CBTypeSolo1.IsChecked)
					whereType += " AND `game`.type_id != 2";
				if (!(bool)CBTypeSolo2.IsChecked)
					whereType += " AND `game`.type_id != 3";
				if (!(bool)CBTypeSolo3.IsChecked)
					whereType += " AND `game`.type_id != 4";
				if (!(bool)CBTypeSolo4.IsChecked)
					whereType += " AND `game`.type_id != 5";
				if (!(bool)CBTypeSolo5.IsChecked)
					whereType += " AND `game`.type_id != 6";
			}
			else
				whereType += " AND `game`.type_id NOT BETWEEN 2 AND 6";
			TakeFromBD();
		}

		private void SRaiting_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (SRaitingFrom.Value != 0 || SRaitingTo.Value != 0)
			{
				if (SRaitingTo.Value == 0)
					whereRaiting = $" AND `game`.raiting >= {SRaitingFrom.Value}";
				else
					whereRaiting = $" AND `game`.raiting BETWEEN {SRaitingFrom.Value} AND {SRaitingTo.Value}";
			}
			else
				whereRaiting = "";
			TakeFromBD();
		}

		private void InfoMessage(string content, string button = "X")
		{
			InfoBar.MessageQueue.Enqueue(content, button, delegate () { });
		}
	}
}