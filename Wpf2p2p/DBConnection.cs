using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Wpf2p2p
{
	class DBConnection
	{
		public static MySqlConnection Main = GetDBConnection(); // Подключение к БД

		public static int UserID = 0;

		private static MySqlConnection GetDBConnection()
		{
			string host = "188.120.245.106";
			int port = 3306;
			string database = "Note9_Chess";
			string username = "Note9";
			string password = "NZC9RMrYpdDqPgXI";
			string connString = $"Server = {host}; Database = {database}; port = {port}; User Id = {username}; password = {password}; AllowUserVariables = True";
			return new MySqlConnection(connString);
		}

		public static void ShowInternetError()
		{
			MessageBox.Show("Не удалось получить данные. Возможно отсутствует подключение к интернету.", "2p2p", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		public static List<Region> GetRegions()
		{
			try
			{
				Main.Open();
				List<Region> regions = new List<Region>();
				MySqlDataReader reader = new MySqlCommand("SELECT * FROM `region`", Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
						regions.Add(new Region(reader["id"].ToString(), reader["name"].ToString()));
				}
				reader.Close();
				Main.Close();
				return regions;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static bool FreeLogin(string login, int id)
		{
			try
			{
				Main.Open();
				bool result;
				if (id == 0)
					result = Convert.ToBoolean(new MySqlCommand($"SELECT IF(COUNT(`id`) = 1, 0, 1) FROM `user` WHERE `login` = '{login}'", Main).ExecuteScalar());
				else
					result = Convert.ToBoolean(new MySqlCommand($"SELECT IF(COUNT(`id`) = 1, 0, 1) FROM `user` WHERE `login` = '{login}' AND `id` != {id}", Main).ExecuteScalar());
				Main.Close();
				return result;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static void RegisterNewAccount(string login, string password, int region_id)
		{
			try
			{
				Main.Open();
				new MySqlCommand($"INSERT INTO `user` (`login`, `password`, `region_id`, `avatar`, `raiting`, `registration_datetime`) " +
				$"VALUES ('{login}', '{Hasher(password)}', '{region_id}', NULL, '0','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')", Main).ExecuteNonQuery();
				Main.Close();
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static int Authorization(string login, string password)
		{
			try
			{
				Main.Open();
				int id = Convert.ToInt32(new MySqlCommand($"SELECT `id` FROM `user` WHERE `login` = '{login}' and `password` = '{Hasher(password)}'", Main).ExecuteScalar());
				Main.Close();
				return id;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		private static string Hasher(string text)
		{
			MD5 md5 = MD5.Create();
			byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
			return Convert.ToBase64String(hash);
		}

		public static User GetProfile(int id)
		{
			try
			{
				Main.Open();
				string query = $"SELECT `u`.id AS 'id', `u`.login AS 'login', `u`.raiting AS 'raiting', `r`.name AS 'region', `u`.avatar AS 'avatar', (SELECT COUNT(*) FROM `user_game` AS `u_g` WHERE `u_g`.user_id = '{UserID}') AS 'game' " +
				$"FROM `user` AS `u` INNER JOIN `region` AS `r` ON `r`.id = `u`.region_id WHERE `u`.id = '{id}'";
				User user = null;
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						BitmapImage bi = null;
						if (reader["avatar"] == DBNull.Value)
							bi = GetImage($"assets/other/unnamed.png");
						else
							bi = GetImage((byte[])reader["avatar"]);
						user = new User(Convert.ToInt32(reader["id"]), bi, reader["login"].ToString(), reader["region"].ToString(), Convert.ToInt32(reader["raiting"]), Convert.ToInt32(reader["game"]));
					}
				}
				reader.Close();
				Main.Close();
				return user;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static BitmapImage GetImage(byte[] image)
		{
			BitmapImage bi = new BitmapImage();
			bi.BeginInit();
			bi.StreamSource = new MemoryStream(image);
			bi.EndInit();
			return bi;
		}

		public static BitmapImage GetImage(string longRelativeUri)
		{
			BitmapImage bi = new BitmapImage();
			bi.BeginInit();
			bi.UriSource = new Uri(longRelativeUri, UriKind.Relative);
			bi.EndInit();
			return bi;
		}

		public static BitmapImage RemoveProfileAvatar(int id)
		{
			try
			{
				Main.Open();
				new MySqlCommand($"UPDATE `user` SET `avatar` = NULL WHERE `id` = {id}", Main).ExecuteNonQuery();
				Main.Close();
				return GetImage($"assets/other/unnamed.png");
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static void UpdateProfileAvatar(int id, byte[] image)
		{
			try
			{
				Main.Open();
				MySqlCommand command = new MySqlCommand($"UPDATE `user` SET `avatar` = @image WHERE `id` = {id}", Main);
				command.Parameters.Add(new MySqlParameter("@image", image));
				command.ExecuteNonQuery();
				Main.Close();
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static bool CheckChangeProfile(int id, string password)
		{
			try
			{
				Main.Open();
				bool result = Convert.ToBoolean(new MySqlCommand($"SELECT COUNT(`id`) FROM `user` WHERE `id` = {id} AND `password` = '{Hasher(password)}'", Main).ExecuteScalar());
				Main.Close();
				return result;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static void UpdateProfileLogin(int id, string login)
		{
			try
			{
				Main.Open();
				new MySqlCommand($"UPDATE `user` SET `login` = '{login}' WHERE `id` = {id}", Main).ExecuteNonQuery();
				Main.Close();
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static void UpdateProfileRegion(int id, int region_id)
		{
			try
			{
				Main.Open();
				new MySqlCommand($"UPDATE `user` SET `region_id` = '{region_id}' WHERE `id` = {id}", Main).ExecuteNonQuery();
				Main.Close();
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static void UpdateProfilePassword(int id, string password)
		{
			try
			{
				Main.Open();
				new MySqlCommand($"UPDATE `user` SET `password` = '{Hasher(password)}' WHERE `id` = {id}", Main).ExecuteNonQuery();
				Main.Close();
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static DataSet GetUserGames(string query)
		{
			try
			{
				Main.Open();
				DataSet ds = new DataSet();
				new MySqlDataAdapter(query, Main).Fill(ds);
				Main.Close();
				return ds;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static List<int> GetColorGames(string query)
		{
			try
			{
				Main.Open();
				List<int> ids = new List<int>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						ids.Add(Convert.ToInt32(reader["game_id"]));
					}
				}
				Main.Close();
				return ids;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static Dictionary<string, string> GetEatFromStat(string query, int color)
		{
			try
			{
				Main.Open();
				Dictionary<string, string> Eat = new Dictionary<string, string>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						if (color == 1)
						{
							Eat.Add("wk", "Съедено королём: " + reader["white_king"].ToString());
							Eat.Add("wq", "Съедено королевой: " + reader["white_queen"].ToString());
							Eat.Add("wr", "Съедено ладьей: " + reader["white_rook"].ToString());
							Eat.Add("wb", "Съедено слоном: " + reader["white_bishop"].ToString());
							Eat.Add("wn", "Съедено конём: " + reader["white_knight"].ToString());
							Eat.Add("wp", "Съедено пешкой: " + reader["white_pawn"].ToString());
							Eat.Add("wa", "Всего съедено: " + reader["white_total"].ToString());
						}
						else
						{
							Eat.Add("bk", "Съедено королём: " + reader["black_king"].ToString());
							Eat.Add("bq", "Съедено королевой: " + reader["black_queen"].ToString());
							Eat.Add("br", "Съедено ладьей: " + reader["black_rook"].ToString());
							Eat.Add("bb", "Съедено слоном: " + reader["black_bishop"].ToString());
							Eat.Add("bn", "Съедено конём: " + reader["black_knight"].ToString());
							Eat.Add("bp", "Съедено пешкой: " + reader["black_pawn"].ToString());
							Eat.Add("ba", "Всего съедено: " + reader["black_total"].ToString());
						}
					}
				}
				reader.Close();
				Main.Close();
				return Eat;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static Dictionary<string, string> GetLostFromStat(string query, int color)
		{
			try
			{
				Main.Open();
				Dictionary<string, string> Lost = new Dictionary<string, string>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						if (color == 1)
						{
							Lost.Add("wq", "Потеряно королев: " + reader["white_queen"].ToString());
							Lost.Add("wr", "Потеряно ладей: " + reader["white_rook"].ToString());
							Lost.Add("wb", "Потеряно слонов: " + reader["white_bishop"].ToString());
							Lost.Add("wn", "Потеряно коней: " + reader["white_knight"].ToString());
							Lost.Add("wp", "Потеряно пешек: " + reader["white_pawn"].ToString());
							Lost.Add("wa", "Всего потеряно: " + reader["white_total"].ToString());
						}
						else
						{
							Lost.Add("bq", "Потеряно королев: " + reader["black_queen"].ToString());
							Lost.Add("br", "Потеряно ладей: " + reader["black_rook"].ToString());
							Lost.Add("bb", "Потеряно слонов: " + reader["black_bishop"].ToString());
							Lost.Add("bn", "Потеряно коней: " + reader["black_knight"].ToString());
							Lost.Add("bp", "Потеряно пешек: " + reader["black_pawn"].ToString());
							Lost.Add("ba", "Всего потеряно: " + reader["black_total"].ToString());
						}
					}
				}
				reader.Close();
				Main.Close();
				return Lost;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static Dictionary<string, string> GetCastleFromStat(string query, int color)
		{
			try
			{
				Main.Open();
				Dictionary<string, string> Castle = new Dictionary<string, string>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						if (color == 1)
						{
							Castle.Add("wl", "Длинных рокировок: " + reader["white_long"].ToString());
							Castle.Add("ws", "Коротких рокировок: " + reader["white_short"].ToString());
							Castle.Add("wa", "Всего рокировок: " + reader["white_total"].ToString());
						}
						else
						{
							Castle.Add("bl", "Длинных рокировок: " + reader["black_long"].ToString());
							Castle.Add("bs", "Коротких рокировок: " + reader["black_short"].ToString());
							Castle.Add("ba", "Всего рокировок: " + reader["black_total"].ToString());
						}
					}
				}
				reader.Close();
				Main.Close();
				return Castle;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static Dictionary<string, string> GetCheckFromStat(string query, int color)
		{
			try
			{
				Main.Open();
				Dictionary<string, string> Check = new Dictionary<string, string>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						if (color == 1)
						{
							Check.Add("wa", "Всего шахов: " + reader["white_total"].ToString());
						}
						else
						{
							Check.Add("ba", "Всего шахов: " + reader["black_total"].ToString());
						}
					}
				}
				reader.Close();
				Main.Close();
				return Check;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static Dictionary<string, string> GetEnpassantFromStat(string query, int color)
		{
			try
			{
				Main.Open();
				Dictionary<string, string> Enpassant = new Dictionary<string, string>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						if (color == 1)
						{
							Enpassant.Add("wa", "Всего взятий: " + reader["white_total"].ToString());
						}
						else
						{
							Enpassant.Add("ba", "Всего взятий: " + reader["black_total"].ToString());
						}
					}
				}
				reader.Close();
				Main.Close();
				return Enpassant;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static Dictionary<string, string> GetPromoteFromStat(string query, int color)
		{
			try
			{
				Main.Open();
				Dictionary<string, string> Promote = new Dictionary<string, string>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						if (color == 1)
						{
							Promote.Add("wq", "Превращений в королеву: " + reader["white_queen"].ToString());
							Promote.Add("wr", "Превращений в ладью: " + reader["white_rook"].ToString());
							Promote.Add("wb", "Превращений в слона: " + reader["white_bishop"].ToString());
							Promote.Add("wn", "Превращений в коня: " + reader["white_knight"].ToString());
							Promote.Add("wa", "Всего превращений: " + reader["white_total"].ToString());
						}
						else
						{
							Promote.Add("bq", "Превращений в королеву: " + reader["black_queen"].ToString());
							Promote.Add("br", "Превращений в ладью: " + reader["black_rook"].ToString());
							Promote.Add("bb", "Превращений в слона: " + reader["black_bishop"].ToString());
							Promote.Add("bn", "Превращений в коня: " + reader["black_knight"].ToString());
							Promote.Add("ba", "Всего превращений: " + reader["black_total"].ToString());
						}
					}
				}
				reader.Close();
				Main.Close();
				return Promote;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static string GetCommonFromStat(string query)
		{
			try
			{
				Main.Open();
				string Count = new MySqlCommand(query, Main).ExecuteScalar().ToString();	
				Main.Close();
				return Count;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static string GetUniqueFromStat(string query)
		{
			try
			{
				Main.Open();
				string Result = new MySqlCommand(query, Main).ExecuteScalar().ToString();
				Main.Close();
				return Result;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static List<Leader> GetLeaders(string query)
		{
			try
			{
				Main.Open();
				List<Leader> Leaders = new List<Leader>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						BitmapImage bi = null;
						if (reader["avatar"] == DBNull.Value)
							bi = GetImage($"assets/other/unnamed.png");
						else
							bi = GetImage((byte[])reader["avatar"]);
						Leaders.Add(new Leader(Convert.ToInt32(reader["id"]), Convert.ToInt32(reader["position"]), bi, reader["login"].ToString(), Convert.ToInt32(reader["raiting"]), reader["region"].ToString(), Convert.ToInt32(reader["game"])));
					}
				}
				reader.Close();
				Main.Close();
				return Leaders;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static object GetLeaderPlace(string query)
		{
			try
			{
				Main.Open();
				object Place = new MySqlCommand(query, Main).ExecuteScalar();
				Main.Close();
				return Place;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static BitmapImage GetAvatar(string query)
		{
			try
			{
				Main.Open();
				object bi = new MySqlCommand(query, Main).ExecuteScalar();
				if (bi == DBNull.Value)
					bi = GetImage($"assets/other/unnamed.png");
				else
					bi = GetImage((byte[])bi);
				Main.Close();
				return (BitmapImage)bi;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static List<GamePreview> GetGamePreviews(string query)
		{
			try
			{
				BitmapImage SenderAvatar = GetAvatar($"SELECT `avatar` FROM `user` WHERE `id` = {UserID}");
				Main.Open();
				List<GamePreview> Previews = new List<GamePreview>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						GamePreview Preview = new GamePreview
						(
							Convert.ToInt32(reader["ug_id"]),
							Convert.ToInt32(reader["id"]),
							reader["color"].ToString(),
							reader["result"].ToString(),
							reader["type"].ToString(),
							reader["enemy_avatar"] == DBNull.Value ? GetImage($"assets/other/unnamed.png") : GetImage((byte[])reader["enemy_avatar"])
						);
						Previews.Add(Preview);
					}
					foreach (GamePreview Preview in Previews)
						SetAvatar(Preview, SenderAvatar);
				}
				reader.Close();
				Main.Close();
				return Previews;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		private static void SetAvatar(GamePreview Preview, BitmapImage SenderAvatar)
		{
			if (Preview.GameType != "По сети")
			{
				if (Preview.GameType != "Двойной")
				{
					// Игры против ботов
					char type = Preview.GameType[Preview.GameType.Length - 2];
					switch (type)
					{
						case '1':
							Preview.Avatar = GetImage("assets/bot/passive.png");
							break;
						case '2':
							Preview.Avatar = GetImage("assets/bot/easy.png");
							break;
						case '3':
							Preview.Avatar = GetImage("assets/bot/medium.png");
							break;
						case '4':
							Preview.Avatar = GetImage("assets/bot/hard.png");
							break;
						case '5':
							Preview.Avatar = GetImage("assets/bot/unfair.png");
							break;
					}
				}
				else
					Preview.Avatar = SenderAvatar;
			}
		}

		public static Dictionary<string, object> GetLostFromPreview(string query)
		{
			try
			{
				Main.Open();
				Dictionary<string, object> Lost = new Dictionary<string, object>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						Application app = Application.Current;
						// Белые потери
						List<LostFigures> WhiteLost = new List<LostFigures>
						{
							new LostFigures("Королева", app.FindResource("wq"), Convert.ToInt32(reader["white_queen"])),
							new LostFigures("Ладья", app.FindResource("wr"), Convert.ToInt32(reader["white_rook"])),
							new LostFigures("Слон", app.FindResource("wb"), Convert.ToInt32(reader["white_bishop"])),
							new LostFigures("Конь", app.FindResource("wn"), Convert.ToInt32(reader["white_knight"])),
							new LostFigures("Пешка", app.FindResource("wp"), Convert.ToInt32(reader["white_pawn"]))
						};
						// Чёрные потери
						List<LostFigures> BlackLost = new List<LostFigures>
						{
							new LostFigures("Королева", app.FindResource("bq"), Convert.ToInt32(reader["black_queen"])),
							new LostFigures("Ладья", app.FindResource("br"), Convert.ToInt32(reader["black_rook"])),
							new LostFigures("Слон", app.FindResource("bb"), Convert.ToInt32(reader["black_bishop"])),
							new LostFigures("Конь", app.FindResource("bn"), Convert.ToInt32(reader["black_knight"])),
							new LostFigures("Пешка", app.FindResource("bp"), Convert.ToInt32(reader["black_pawn"]))
						};
						Lost.Add("wl", WhiteLost);
						Lost.Add("bl", BlackLost);
						Lost.Add("wa", Convert.ToInt32(reader["white_all"]));
						Lost.Add("ba", Convert.ToInt32(reader["black_all"]));
					}
				}
				reader.Close();
				Main.Close();
				return Lost;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static Dictionary<string, string> GetGameDataFromPreview(string query)
		{
			try
			{
				Main.Open();
				Dictionary<string, string> GameData = new Dictionary<string, string>();
				MySqlDataReader reader = new MySqlCommand(query, Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						DateTime dt = Convert.ToDateTime(reader["datetime"]);
						GameData.Add("type", reader["type"].ToString());
						GameData.Add("color", $"Цвет фигур: {reader["color"].ToString()}");
						GameData.Add("data", $"Дата и время: {dt.ToShortDateString()} {dt.ToShortTimeString()}");
						GameData.Add("id", reader["id"].ToString());
						GameData.Add("result", $"Исход: {reader["result"].ToString()}");
						GameData.Add("raiting", $"Рейтинг: {reader["raiting"].ToString()}");
						GameData.Add("timer", reader["timer"].ToString());
					}
				}
				reader.Close();
				Main.Close();
				return GameData;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static string GetFenOverview(string query)
		{
			try
			{
				Main.Open();
				string[] parts = new MySqlCommand(query, Main).ExecuteScalar().ToString().Split();
				Main.Close();
				return parts[0];
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static bool IsGameExistOverview(int id)
		{
			try
			{
				Main.Open();
				bool result = Convert.ToBoolean(new MySqlCommand($"SELECT COUNT(`id`) FROM `game` WHERE `id` = {id}", Main).ExecuteScalar());
				Main.Close();
				return result;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static List<ChessTurn> GetChessTurnsOverview(int id)
		{
			try
			{
				Main.Open();
				List<ChessTurn> Turns = new List<ChessTurn>();
				MySqlDataReader reader = new MySqlCommand($"SELECT * FROM `turn` WHERE `game_id` = {id}", Main).ExecuteReader();
				if (reader.HasRows)
				{
					while (reader.Read())
					{
						Turns.Add(new ChessTurn
						{
							TurnNumber = Convert.ToInt32(reader["number"]),
							White = reader["white_turn"].ToString(),
							EvaluationAfterWhite = Convert.ToInt32(reader["evaluation_after_white"]),
							FenAfterWhite = reader["fen_after_white"].ToString(),
							Black = reader["black_turn"].ToString(),
							EvaluationAfterBlack = Convert.ToInt32(reader["evaluation_after_black"]),
							FenAfterBlack = reader["fen_after_black"].ToString(),
						});
					}
				}
				reader.Close();
				Main.Close();
				return Turns;
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}

		public static void SaveGame(GameStats gs, List<ChessTurn> Turns)
		{
			try
			{
				Main.Open();
				if (gs.Type == 1)
				{
					RegistryKey CurrentUserKey = Registry.CurrentUser;
					RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
					gs.MyColor = Convert.ToInt32(ChessKey.GetValue("double_color"));
					ChessKey.Close();
				}
				// Сбор данных
				var idLost = new MySqlCommand("INSERT INTO `lost` (`black_queen`, `black_rook`, `black_bishop`, `black_knight`, `black_pawn`, `white_queen`, `white_rook`, `white_bishop`, `white_knight`, `white_pawn`) " +
				$"VALUES ('{gs.LostBlackQueen}', '{gs.LostBlackRook}', '{gs.LostBlackBishop}', '{gs.LostBlackKnight}', '{gs.LostBlackPawn}', " +
				$"'{gs.LostWhiteQueen}', '{gs.LostWhiteRook}', '{gs.LostWhiteBishop}', '{gs.LostWhiteKnight}', '{gs.LostWhitePawn}'); SELECT LAST_INSERT_ID();", Main).ExecuteScalar();
				var idEat = new MySqlCommand("INSERT INTO `eat` (`black_king`, `black_queen`, `black_rook`, `black_bishop`, `black_knight`, `black_pawn`, `white_king`, `white_queen`, `white_rook`, `white_bishop`, `white_knight`, `white_pawn`) " +
				$"VALUES ('{gs.EatByBlackKing}', '{gs.EatByBlackQueen}', '{gs.EatByBlackRook}', '{gs.EatByBlackBishop}', '{gs.EatByBlackKnight}', '{gs.EatByBlackPawn}', " +
				$"'{gs.EatByWhiteKing}', '{gs.EatByWhiteQueen}', '{gs.EatByWhiteRook}', '{gs.EatByWhiteBishop}', '{gs.EatByWhiteKnight}', '{gs.EatByWhitePawn}'); SELECT LAST_INSERT_ID();", Main).ExecuteScalar();
				var idPromotion = new MySqlCommand("INSERT INTO `promotion` (`black_queen`, `black_rook`, `black_bishop`, `black_knight`, `white_queen`, `white_rook`, `white_bishop`, `white_knight`) " +
				$"VALUES ('{gs.BlackPromoteQueen}', '{gs.BlackPromoteRook}', '{gs.BlackPromoteBishop}', '{gs.BlackPromoteKnight}', " +
				$"'{gs.WhitePromoteQueen}', '{gs.WhitePromoteRook}', '{gs.WhitePromoteBishop}', '{gs.WhitePromoteKnight}'); SELECT LAST_INSERT_ID();", Main).ExecuteScalar();
				var idEnPassant = new MySqlCommand("INSERT INTO `enpassant` (`black_pawn`, `white_pawn`) " +
				$"VALUES ('{gs.BlackIntercepted}', '{gs.WhiteIntercepted}'); SELECT LAST_INSERT_ID();", Main).ExecuteScalar();
				var idChecked = new MySqlCommand("INSERT INTO `checked` (`black_king`, `white_king`) " +
				$"VALUES ('{gs.BlackChecked}', '{gs.WhiteChecked}'); SELECT LAST_INSERT_ID();", Main).ExecuteScalar();
				var idCastling = new MySqlCommand("INSERT INTO `castling` (`black_long`, `black_short`, `white_long`, `white_short`) " +
				$"VALUES ('{(gs.IsBlackLongCastle ? 1 : 0)}', '{(gs.IsBlackShortCastle ? 1 : 0)}', " +
				$"'{(gs.IsWhiteLongCastle ? 1 : 0)}', '{(gs.IsWhiteShortCastle ? 1 : 0)}'); SELECT LAST_INSERT_ID();", Main).ExecuteScalar();
				// Закрепление данных
				var idStat = new MySqlCommand("INSERT INTO `stat` (`lost_id`, `eat_id`, `promotion_id`, `enpassant_id`, `checked_id`, `castling_id`) " +
					$"VALUES ('{idLost}', '{idEat}', '{idPromotion}', '{idEnPassant}', '{idChecked}', '{idCastling}'); SELECT LAST_INSERT_ID();", Main).ExecuteScalar();
				// Таймер
				object idTimer = "NULL";
				if (gs.Time != 0)
					idTimer = new MySqlCommand("INSERT INTO `timer` (`time_for_game`, `time_after_turn`, `black_time`, `white_time`) " +
					$"VALUES ('{gs.Time}', '{gs.AddTime}', '{gs.BlackTime}', '{gs.WhiteTime}'); SELECT LAST_INSERT_ID();", Main).ExecuteScalar();
				int raiting = 0;
				if (gs.Type == 7)
				{
					if (gs.Result < 3)
					{
						Random random = new Random();
						raiting = random.Next(3, 11);
					}
				}
				// Игра
				var idGame = new MySqlCommand("INSERT INTO `game` (`datetime_start`, `datetime_end`, `type_id`, `result_id`, `evaluation`, `fen`, `timer_id`, `stat_id`, `raiting`) " +
					$"VALUES ('{gs.GameBegin}', '{gs.GameEnd}', '{gs.Type}', '{gs.Result}', '{gs.Evaluation}', '{gs.Fen}', {idTimer}, '{idStat}', '{raiting}'); SELECT LAST_INSERT_ID();", Main).ExecuteScalar();
				foreach (ChessTurn Turn in Turns)
				{
					new MySqlCommand("INSERT INTO `turn` (`number`, `white_turn`, `evaluation_after_white`, `fen_after_white`, `black_turn`, `evaluation_after_black`, `fen_after_black`, `game_id`) " +
						$"VALUES ('{Turn.TurnNumber}', '{Turn.White}', '{Turn.EvaluationAfterWhite}', '{Turn.FenAfterWhite}', '{Turn.Black}', '{Turn.EvaluationAfterBlack}', '{Turn.FenAfterBlack}', '{idGame}')", Main).ExecuteNonQuery();
				}
				// Игры игроков
				new MySqlCommand("INSERT INTO `user_game` (`user_id`, `game_id`, `color_id`) " +
					$"VALUES ('{UserID}', '{idGame}', '{gs.MyColor}')", Main).ExecuteScalar();
				// Запись данных противника
				if (gs.Type == 7)
				{
					new MySqlCommand("INSERT INTO `user_game` (`user_id`, `game_id`, `color_id`) " +
					$"VALUES ('{gs.EnemyNetID}', '{idGame}', '{(gs.MyColor == 1 ? 2 : 1)}')", Main).ExecuteScalar();
					// Результат локальной победы
					string myMark = "";
					string enemyMark = "";
					if (gs.Result == gs.MyColor)
					{
						myMark = "+";
						enemyMark = "-";
					}
					else
					{
						myMark = "-";
						enemyMark = "+";
					}
					new MySqlCommand($"UPDATE `user` SET `raiting` = `raiting` {myMark} {raiting} WHERE `user`.`id` = {UserID}", Main).ExecuteScalar();
					new MySqlCommand($"UPDATE `user` SET `raiting` = `raiting` {enemyMark} {raiting} WHERE `user`.`id` = {gs.EnemyNetID}", Main).ExecuteScalar();
				}
				Main.Close();
			}
			catch (Exception)
			{
				ShowInternetError();
				Environment.Exit(0);
				throw;
			}
		}
	}
}