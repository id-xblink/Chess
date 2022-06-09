using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Wpf2p2p
{
	public partial class GameUC : UserControl
	{
		Button RedirectButton = null;
		GameStats gs = new GameStats();
		List<Button> Buttons = new List<Button>();
		Button SelectedButton = null;
		Button PromotionButton = null;
		List<string> allMoves;
		List<string> currentMoves;
		List<Rectangle> marks = new List<Rectangle>();
		List<ChessTurn> turns = new List<ChessTurn>();
		string GameType = "";
		string MyColor = "";
		int BotLevel = 0;
		int BlackTime = 0;
		int WhiteTime = 0;
		int AddedTime = 0;
		int EnemyNetID = 0;
		bool Timer = false;
		bool IsReadyNet = false;
		ChessRules.Chess chess = new ChessRules.Chess("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
		bool gg = false;
		TcpClient client;
		StreamReader sr;
		StreamWriter sw;
		DispatcherTimer GeneralTimer = new DispatcherTimer();
		bool IsNetAdmin = false;
		Rectangle CheckHighlight = null;
		bool IsClosing = false;

		public GameUC()
		{
			InitializeComponent();
		}

		private void BMenu_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			GameType = b.Name.ToString().Substring(1);
			switch (GameType)
			{
				case "Duo":
					{
						GTime.IsEnabled = true;
						break;
					}
				case "Solo":
					{
						GColor.IsEnabled = true;
						GBot.IsEnabled = true;
						TBColor.Foreground = Brushes.Crimson;
						TBDifficulty.Foreground = Brushes.Crimson;
						break;
					}
				case "Net":
					{
						GNet.IsEnabled = true;
						break;
					}
			}
			GSetting.Visibility = Visibility.Visible;
			GMenu.Visibility = Visibility.Collapsed;
		}

		private void BPickColor_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			MyColor = b.Tag.ToString();
			TBColor.Foreground = Brushes.Green;
		}

		private void BPickBot_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			BotLevel = Convert.ToInt32(b.Tag);
			TBDifficulty.Foreground = Brushes.Green;
		}
		
		private async void BClientConnect_Click(object sender, RoutedEventArgs e)
		{
			string ip = TBIP.Text;
			if (client == null || client?.Connected == false)
			{
				await Task.Factory.StartNew(() =>
				{
					try
					{
						client = new TcpClient();
						client.Connect(ip, 5050);
						sr = new StreamReader(client.GetStream());
						sw = new StreamWriter(client.GetStream())
						{
							AutoFlush = true
						};
						Dispatcher.BeginInvoke(new ThreadStart(delegate
						{
							PIConnection.Foreground = Brushes.Green;
							PBConnection.IsIndeterminate = true;
							TBIPStatus.Text = "Связь: есть";
							InfoMessage("Вы подключены к серверу");
						}));
						sw.WriteLine($"_login|{DBConnection.UserID}");
						if (client.Connected)
						{
							Dispatcher.BeginInvoke(new ThreadStart(delegate
							{
								Task.Factory.StartNew(() =>
								{
									while (true)
									{
										try
										{
											if (client?.Connected == true)
											{
												var line = sr.ReadLine();

												if (line != null)
												{
													Dispatcher.BeginInvoke(new ThreadStart(delegate
													{
														NetAction(line);
													}));
												}
												else
												{
													Dispatcher.BeginInvoke(new ThreadStart(delegate
													{
														DiffuseConnection();
													}));
													break;
												}
											}
											Task.Delay(10).Wait();
										}
										catch (Exception)
										{
											Dispatcher.BeginInvoke(new ThreadStart(delegate
											{
												InfoMessage("Принудительный разрыв соединения");
												allMoves = new List<string>();
												IsClosing = true;
												Task.Delay(5000).ContinueWith(q =>
												{
													Dispatcher.BeginInvoke(new ThreadStart(delegate
													{
														DashboardW dashboardW = Tag as DashboardW;
														dashboardW.LoadNewView("Game");
													}));
												});
											}));
											break;
										}
									}
								});
							}));
						}
					}
					catch (Exception) { }
				});
			}
			else
				InfoMessage("Вы уже подключены к серверу");
		}

		private void BClientDisconnect_Click(object sender, RoutedEventArgs e)
		{
			if (client != null && client?.Connected == true)
			{
				if (sender is string)
					SendMessage($"!leave|{DBConnection.UserID}");
				else
					SendMessage("!ready|false");
			}
		}

		private void DiffuseConnection()
		{
			client.Close();
			client = null;
			sr = null;
			sw = null;
			PIConnection.Foreground = Brushes.DarkRed;
			PBConnection.IsIndeterminate = false;
			TBIPStatus.Text = "Связь: отсутствует";
			if (IsReadyNet)
			{
				IsReadyNet = false;
				PIReady.Foreground = Brushes.DarkRed;
				TBIPReady.Text = "К старту: не готов";
			}
			InfoMessage("Вы отключены от сервера");
		}

		private void BGo_Click(object sender, RoutedEventArgs e)
		{
			switch (GameType)
			{
				case "Duo":
					{
						if ((bool)TBTime.IsChecked)
						{
							BlackTime = Convert.ToInt32(SMinute.Value) * 60;
							WhiteTime = BlackTime;
							AddedTime = Convert.ToInt32(SSecond.Value);
							Timer = true;
							CalculateTime(BlackTime, "black");
							CalculateTime(WhiteTime, "white");
						}
						break;
					}
				case "Solo":
					{
						if (BotLevel == 0 || MyColor == "")
						{
							InfoMessage("Выбраны не все настройки");
							return;
						}
						else
						{
							if (MyColor == "random")
							{
								string[] color = { "white", "black" };
								Random random = new Random();
								MyColor = color[random.Next(color.Length)];
							}
						}
						break;
					}
				case "Net":
					{
						if (client != null && client?.Connected == true)
						{
							if (!IsReadyNet)
							{
								IsReadyNet = true;
								SendMessage("!ready|true");
								PIReady.Foreground = Brushes.Green;
								TBIPReady.Text = "К старту: готов";
							}
							else
								InfoMessage("Вы уже готовы");
						}
						else
							InfoMessage("Необходимо подключиться к серверу");
						break;
					}
			}
			if (GameType != "Net")
				GameInitialize();
		}

		private void GameInitialize()
		{
			GNet.IsEnabled = true;
			LoadOpponents();
			InitBoard();
			CheckBoard();
			GGame.Visibility = Visibility.Visible;
			GSetting.Visibility = Visibility.Collapsed;
			switch (GameType)
			{
				case "Duo":
					{
						if (Timer)
						{
							gs.Time = BlackTime;
							gs.AddTime = AddedTime;
						}
						gs.Type = 1;
						break;
					}
				case "Solo":
					{
						if (MyColor == "black")
						{
							BotMove();
						}
						gs.Type = 1 + BotLevel;
						break;
					}
				case "Net":
					{
						gs.Type = 7;
						break;
					}
			}
			DGMoves.ItemsSource = turns;
			gs.MyColor = MyColor == "black" ? 2 : 1;
			gs.GameBegin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			InfoMessage("Игра началась");
		}

		private void LoadOpponents()
		{
			User Me = DBConnection.GetProfile(DBConnection.UserID);
			switch (GameType)
			{
				case "Duo":
					{
						//Загрузить себя дважды
						InsertBlackPlayerData(Me);
						InsertWhitePlayerData(Me);
						break;
					}
				case "Solo":
					{
						//Загрузить бота
						if (MyColor == "black")
						{
							InsertBlackPlayerData(Me);
							InsertWhitePlayerData(GetBotData());
						}
						else
						{
							InsertBlackPlayerData(GetBotData());
							InsertWhitePlayerData(Me);
						}
						break;
					}
				case "Net":
					{
						// Получить чужие данные из бд
						if (MyColor == "black")
						{
							InsertBlackPlayerData(Me);
							InsertWhitePlayerData(DBConnection.GetProfile(EnemyNetID));
						}
						else
						{
							InsertBlackPlayerData(DBConnection.GetProfile(EnemyNetID));
							InsertWhitePlayerData(Me);
						}
						break;
					}
			}
		}

		private void InsertBlackPlayerData(User user)
		{
			IBlackAvatar.ImageSource = user.Avatar;
			TBBlackLogin.Text = user.Login;
			TBBlackRegion.Text = "Регион: " + user.Region;
			TBBlackRating.Text = "Рейтинг: " + user.Raiting.ToString();
			TBBlackGames.Text = "Игр: " + user.GamesCount.ToString();
		}

		private void InsertWhitePlayerData(User user)
		{
			IWhiteAvatar.ImageSource = user.Avatar;
			TBWhiteLogin.Text = user.Login;
			TBWhiteRegion.Text = "Регион: " + user.Region;
			TBWhiteRating.Text = "Рейтинг: " + user.Raiting.ToString();
			TBWhiteGames.Text = "Игр: " + user.GamesCount.ToString();
		}

		private User GetBotData()
		{
			// Загрузка регионов
			List<Region> Regions = DBConnection.GetRegions();
			Random random = new Random();
			User user = null;
			BitmapImage bi = null;
			string login = "";
			switch (BotLevel)
			{
				case 1:
					{
						bi = DBConnection.GetImage("assets/bot/passive.png");
						login = "Passive";
						break;
					}
				case 2:
					{
						bi = DBConnection.GetImage("assets/bot/easy.png");
						login = "Easy";
						break;
					}
				case 3:
					{
						bi = DBConnection.GetImage("assets/bot/medium.png");
						login = "Simple";
						break;
					}
				case 4:
					{
						bi = DBConnection.GetImage("assets/bot/hard.png");
						login = "Increased";
						break;
					}
				case 5:
					{
						bi = DBConnection.GetImage("assets/bot/unfair.png");
						login = "Optimal";
						break;
					}
			}
			string region = Regions[random.Next(Regions.Count)].Name;
			int raiting = random.Next(0, 4001);
			int games = random.Next(0, 10001);
			user = new User(BotLevel, bi, login, region, raiting, games);
			return user;
		}

		private void InitBoard()
		{
			Style FlatStyle = FindResource("MaterialDesignFlatButton") as Style;
			for (int i = 0; i < 8; i++)
			{
				GBoard.RowDefinitions.Add(new RowDefinition());
				GBoard.ColumnDefinitions.Add(new ColumnDefinition());
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
						Panel.SetZIndex(tb, 2);
						Grid.SetRow(tb, i);
						Grid.SetColumn(tb, j);
						GBoard.Children.Add(tb);
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
						Panel.SetZIndex(tb, 2);
						Grid.SetRow(tb, i);
						Grid.SetColumn(tb, j);
						GBoard.Children.Add(tb);
					}
					Button b = new Button
					{
						Padding = new Thickness(0),
						Width = double.NaN,
						Height = double.NaN,
						Style = FlatStyle,
					};
					b.Click += Selected;
					Panel.SetZIndex(b, 3);
					Grid.SetRow(r, i);
					Grid.SetColumn(r, j);
					Grid.SetRow(b, i);
					Grid.SetColumn(b, j);
					GBoard.Children.Add(r);
					GBoard.Children.Add(b);
					Buttons.Add(b);
				}
			}
			string[] parts = chess.fen.Split();
			InitFigures(parts[0]);
		}

		private void InitFigures(string data)
		{
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
			b.Tag = f;
		}

		private void Selected(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			// Если чужой ход
			if (!GameType.Equals("Duo"))
				if (MyColor != chess.GetCurrentColor())
					return;
			bool IsPromotion = false;
			if (SelectedButton != null)
				if (SelectedButton != b)
					if (b.Tag != null)
					{
						if (chess.GetFigureColor((ChessRules.Figure)(char)b.Tag).Equals(chess.GetCurrentColor()))
							SelectedButton = null;
						else
							IsPromotion = GenerateMove(b, (ChessRules.Figure)(char)SelectedButton.Tag);
					}
					else
						IsPromotion = GenerateMove(b, (ChessRules.Figure)(char)SelectedButton.Tag);
			if (!IsPromotion)
				RefreshMarks(b);
		}

		private void RefreshMarks(Button b)
		{
			foreach (Rectangle mark in marks)
				GBoard.Children.Remove(mark);
			marks.Clear();
			if (SelectedButton != b)
				Highlight(b);
			else
				SelectedButton = null;
		}

		private void Highlight(Button b)
		{
			int x = Grid.GetColumn(b);
			int y = 8 - Grid.GetRow(b);
			string from = b.Tag + ((char)('a' + x)).ToString() + y.ToString();
			currentMoves = allMoves.Where(z => z.Substring(0, 3).Contains(from)).ToList();
			if (currentMoves.Count > 0)
			{
				foreach (string coord in currentMoves)
				{
					Rectangle highlightMark = new Rectangle
					{
						StrokeDashArray = new DoubleCollection() { 5 },
						StrokeDashCap = PenLineCap.Round,
						StrokeThickness = 3,
						Margin = new Thickness(2),
					};
					Panel.SetZIndex(highlightMark, 1);
					string to = coord.Length == 5 ? coord.Substring(3, 2) : coord.Substring(3, 2);
					Button buttonMark = Buttons.FirstOrDefault(z => (char)('a' + Grid.GetColumn(z)) == to[0] && (8 - Grid.GetRow(z)).ToString() == to[1].ToString());
					if (buttonMark.Tag == null)
					{
						// Проверка взятия на проходе
						if (Grid.GetColumn(b) != Grid.GetColumn(buttonMark) && Grid.GetRow(b) != Grid.GetRow(buttonMark) && (b.Tag.ToString() == "p" || b.Tag.ToString() == "P"))
							highlightMark.SetResourceReference(Shape.StrokeProperty, "as");
						else
							highlightMark.SetResourceReference(Shape.StrokeProperty, "ms");
					}
					else
						highlightMark.SetResourceReference(Shape.StrokeProperty, "as");
					Grid.SetRow(highlightMark, Grid.GetRow(buttonMark));
					Grid.SetColumn(highlightMark, Grid.GetColumn(buttonMark));
					marks.Add(highlightMark);
					GBoard.Children.Add(highlightMark);
				}
				SelectedButton = b;
			}
			else
				SelectedButton = null;
		}

		private bool GenerateMove(Button b, ChessRules.Figure figure)
		{
			int x = Grid.GetColumn(b);
			int y = Grid.GetRow(b);
			Rectangle currentMark = marks.FirstOrDefault(z => Grid.GetColumn(z) == x && Grid.GetRow(z) == y);
			if (currentMark == null)
			{
				SelectedButton = b;
				return false;
			}
			int fromX = Grid.GetColumn(SelectedButton);
			int fromY = Grid.GetRow(SelectedButton);
			int toX = Grid.GetColumn(b);
			int toY = Grid.GetRow(b);
			string from = ((char)('a' + fromX)).ToString() + (8 - fromY).ToString();
			string to = ((char)('a' + toX)).ToString() + (8 - toY).ToString();
			string turn = (char)figure + from + to;
			if (GeneratePromotion(toY, figure))
			{
				PromotionButton = b;
				GPromoteOptions.Tag = turn;
				BQ.IsEnabled = true;
				GBoard.IsEnabled = false;
				return true;
			}
			else
			{
				if (GameType != "Net")
					GlobalMove(turn);
				else
					SendMessage(turn);
			}
			return false;
		}

		private void GlobalMove(string turn)
		{
			// Звук хода
			PlaySound();
			// Разбор строки хода
			ChessRules.Figure figure = (ChessRules.Figure)turn[0];
			ChessRules.Figure promotion = turn.Length == 6 ? (ChessRules.Figure)turn[5] : ChessRules.Figure.none;
			string from = turn.Substring(1, 2);
			string to = turn.Substring(3, 2);
			int fromX = from[0] - 'a';
			int fromY = 7 - (from[1] - '1');
			int toX = to[0] - 'a';
			int toY = 7 - (to[1] - '1');
			// Нахождение использованных клеток для хода
			Button fromB = Buttons.First(b => Grid.GetColumn(b) == fromX && Grid.GetRow(b) == fromY);
			Button toB = Buttons.First(b => Grid.GetColumn(b) == toX && Grid.GetRow(b) == toY);
			// Проверка наличия особых игровых действий
			EnPassant(to, figure, toX, toY);
			Castling(toX, fromX, figure);
			bool NeedRefresh = Promotion(fromB, promotion);
			// Смена фигур на доске
			toB.Content = fromB.Content;
			toB.Tag = fromB.Tag;
			fromB.Content = null;
			fromB.Tag = null;
			// Запись хода
			if (chess.GetCurrentColor() == "black")
				turns[turns.Count - 1].Black = turn;
			else
				turns.Add(new ChessTurn { TurnNumber = chess.GetMoveNumber(), White = turn });
			// Разбор хода для статистики
			CountPoints(turn);
			// Добавление времени на ход
			if (Timer && GeneralTimer.IsEnabled)
			{
				if (chess.GetCurrentColor() == "black")
				{
					BlackTime += AddedTime;
					CalculateTime(BlackTime, "black");
				}
				else
				{
					WhiteTime += AddedTime;
					CalculateTime(WhiteTime, "white");
				}
			}
			// Реализация хода
			chess = chess.Move(turn);
			CheckBoard();
			// Подсчёт очков после хода
			if (chess.GetCurrentColor() == "black") // Считаем для white
			{
				turns[turns.Count - 1].EvaluationAfterWhite = chess.GetBoardEvaluation(false, true);
				turns[turns.Count - 1].FenAfterWhite = chess.fen;
			}
			else // Считаем для black
			{
				turns[turns.Count - 1].EvaluationAfterBlack = chess.GetBoardEvaluation(false, true);
				turns[turns.Count - 1].FenAfterBlack = chess.fen;
			}
			// Обновление таблицы
			DGMoves.ItemsSource = null;
			DGMoves.ItemsSource = turns;
			if (VisualTreeHelper.GetChild(DGMoves, 0) is Decorator border)
				if (border.Child is ScrollViewer scroll)
					scroll.ScrollToEnd();
			// Если было превращение
			if (NeedRefresh)
				RefreshMarks(toB);
			// Ход бота
			if (chess.GetCurrentColor() != MyColor && !gg && GameType.Equals("Solo"))
				BotMove();
		}

		private void SwitchPromotionImage(Button b, string color)
		{
			Image image = new Image();
			image.SetResourceReference(Image.SourceProperty, color + b.Name.Substring(1, 1).ToLower());
			b.Content = image;
		}

		private void EnPassant(string to, ChessRules.Figure figure, int toX, int toY)
		{
			string[] parts = chess.fen.Split();
			if (parts[3].Equals(to) && ((char)figure).ToString().ToLower().Equals("p"))
			{
				//Взятие на проходе
				Button EnPassant = Buttons.First(z => Grid.GetColumn(z) == toX && Grid.GetRow(z) == toY + ((char)figure == 'p' ? -1 : 1));
				EnPassant.Content = null;
				EnPassant.Tag = null;
				if (figure == ChessRules.Figure.blackPawn)
				{
					// Чёрный взял на проходе белого
					Eat(ChessRules.Figure.blackPawn, ChessRules.Figure.whitePawn);
					gs.BlackIntercepted++;
				}
				else
				{
					// Белый взял на проходе чёрного
					Eat(ChessRules.Figure.whitePawn, ChessRules.Figure.blackPawn);
					gs.WhiteIntercepted++;
				}
			}
		}

		private void Castling(int toX, int fromX, ChessRules.Figure figure)
		{
			int DeltaX = toX - fromX;
			if (Math.Abs(DeltaX) == 2 && ((char)figure).ToString().ToLower().Equals("k"))
			{
				int alphaY = chess.GetCurrentColor() == "black" ? 0 : 7;
				Button ButtonRook = Buttons.First(z => Grid.GetColumn(z) == (DeltaX > 0 ? 7 : 0) && Grid.GetRow(z) == alphaY);
				Button ButtonReplace = Buttons.First(z => Grid.GetColumn(z) == toX + (Math.Sign(DeltaX) * -1) && Grid.GetRow(z) == alphaY);
				ButtonReplace.Content = ButtonRook.Content;
				ButtonReplace.Tag = ButtonRook.Tag;
				ButtonRook.Content = null;
				ButtonRook.Tag = null;
				if (DeltaX == 2) // Короткая
				{
					if (alphaY == 0) // Чёрный
						gs.IsBlackShortCastle = true;
					else // Белый
						gs.IsWhiteShortCastle = true;
				}
				else // Длинная
				{
					//Длинная
					if (alphaY == 0) // Чёрный
						gs.IsBlackLongCastle = true;
					else // Белый
						gs.IsWhiteLongCastle = true;
				}
			}
		}

		private bool Promotion(Button b, ChessRules.Figure figure)
		{
			if (figure != ChessRules.Figure.none)
			{
				Image image = new Image();
				image.SetResourceReference(Image.SourceProperty, chess.GetCurrentColor().Substring(0, 1) + ((char)figure).ToString().ToLower());
				b.Content = image;
				b.Tag = Convert.ToChar(figure);
				return true;
			}
			return false;
		}

		private bool GeneratePromotion(int toY, ChessRules.Figure figure)
		{
			// Если превращается пешка
			if (((char)figure).ToString().ToLower().Equals("p"))
			{
				string color = chess.GetCurrentColor().Substring(0, 1);
				if (toY == (color == "b" ? 7 : 0))
				{
					// Настройка изображений
					SwitchPromotionImage(BQ, color);
					SwitchPromotionImage(BR, color);
					SwitchPromotionImage(BB, color);
					SwitchPromotionImage(BN, color);
					return true;
				}
			}
			return false;
		}

		private void BGeneratePromotionMove_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			string figure = b.Name.Substring(1, 1);
			string color = chess.GetCurrentColor().Substring(0, 1);
			if (!figure.Equals("C"))
			{
				if (color.Equals("b"))
					figure = figure.ToLower();
				if (GameType != "Net")
					GlobalMove(GPromoteOptions.Tag.ToString() + figure);
				else
					SendMessage(GPromoteOptions.Tag.ToString() + figure);
			}
			GPromoteOptions.Tag = null;
			BQ.IsEnabled = false;
			GBoard.IsEnabled = true;
		}

		private void CheckBoard()
		{
			if (Timer && !GeneralTimer.IsEnabled && chess.GetMoveNumber() == 2)
				StartGeneralTimer();
			string result = "";
			allMoves = chess.GetAllMoves();
			if (!gg)
			{
				// Снятие подсветки шаха
				if (CheckHighlight != null)
				{
					GBoard.Children.Remove(CheckHighlight);
					CheckHighlight = null;
				}
				if (chess.IsCheck())
				{
					if (allMoves.Count == 0)
					{
						result = "Мат";
						gg = true;
					}
					else
					{
						Rectangle highlightMark = new Rectangle
						{
							StrokeDashArray = new DoubleCollection() { 5 },
							StrokeDashCap = PenLineCap.Round,
							StrokeThickness = 3,
							Margin = new Thickness(2),
						};
						highlightMark.SetResourceReference(Shape.StrokeProperty, "cs");
						Panel.SetZIndex(highlightMark, 1);
						Button buttonMark = Buttons.FirstOrDefault(z => z.Tag != null && z.Tag.ToString() == (chess.GetCurrentColor() == "black" ? "k" : "K"));
						Grid.SetRow(highlightMark, Grid.GetRow(buttonMark));
						Grid.SetColumn(highlightMark, Grid.GetColumn(buttonMark));
						CheckHighlight = highlightMark;
						GBoard.Children.Add(highlightMark);
						if (chess.GetCurrentColor() == "black")
							gs.WhiteChecked++;
						else
							gs.BlackChecked++;
						if (chess.GetFiftyMoveCount() == 50)
						{
							result = "Ничья";
							gg = true;
						}
						else
							InfoMessage("Шах");
					}
				}
				else
				{
					if (allMoves.Count == 0)
					{
						result = "Пат";
						gg = true;
					}
					if (chess.GetFiguresCount() == 2 || chess.GetFiftyMoveCount() == 50)
					{
						result = "Ничья";
						gg = true;
					}
				}
			}
			else
			{
				if (Timer)
				{
					result = "Время истекло";
					allMoves = new List<string>();
				}
			}
			if (gg)
			{
				if (Timer)
				{
					StopGeneralTimer();
					gs.BlackTime = BlackTime;
					gs.WhiteTime = WhiteTime;
				}
				// Подсчёт очков после хода
				if (chess.GetCurrentColor() == "black") // Считаем для white
				{
					turns[turns.Count - 1].EvaluationAfterWhite = chess.GetBoardEvaluation(false, true);
					turns[turns.Count - 1].FenAfterWhite = chess.fen;
				}
				else // Считаем для black
				{
					turns[turns.Count - 1].EvaluationAfterBlack = chess.GetBoardEvaluation(false, true);
					turns[turns.Count - 1].FenAfterBlack = chess.fen;
				}
				gs.GameEnd = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				gs.Evaluation = chess.GetBoardEvaluation(false, true);
				gs.Fen = chess.fen;
				switch (result)
				{
					case "Мат":
						{
							if (chess.GetCurrentColor() == "black")
							{
								gs.Result = 1;
								InfoMessage("Победа белых");
							}
							else
							{
								gs.Result = 2;
								InfoMessage("Победа чёрных");
							}
							break;
						}
					case "Ничья":
						{
							gs.Result = 3;
							InfoMessage("Ничья");
							break;
						}
					case "Пат":
						{
							gs.Result = 4;
							InfoMessage("Пат");
							break;
						}
					case "Время истекло":
						{
							if (chess.GetCurrentColor() == "black")
							{
								gs.Result = 1;
								InfoMessage("Победа белых");
							}
							else
							{
								gs.Result = 2;
								InfoMessage("Победа чёрных");
							}
							break;
						}
				}
				if (GameType == "Net" && !IsNetAdmin)
					return;
				DBConnection.SaveGame(gs, turns);
				IsClosing = true;
				if (GameType == "Net")
					SendMessage($"!leave|{DBConnection.UserID}");
				Task.Delay(5000).ContinueWith(q =>
				{
					Dispatcher.BeginInvoke(new ThreadStart(delegate
					{
						DashboardW dashboardW = Tag as DashboardW;
						dashboardW.LoadNewView("Game");
					}));
				});
			}
		}
		
		private void NetAction(string action)
		{
			string[] parts = action.Split('|');
			if (action.Contains("_go"))
			{
				MyColor = parts[1];
				IsNetAdmin = Convert.ToBoolean(parts[2]);
				EnemyNetID = Convert.ToInt32(parts[3]);
				gs.EnemyNetID = EnemyNetID;
				GSetting.Visibility = Visibility.Collapsed;
				GameInitialize();
			}
			if (action.Contains("_turn"))
			{
				string turn = parts[1];
				GlobalMove(turn);
			}
		}

		private void SendMessage(string message)
		{
			if (client?.Connected == true && !string.IsNullOrWhiteSpace(message))
				Task.Factory.StartNew(() =>
				{
					try
					{
						sw.WriteLine(message);
					}
					catch (Exception) { }
				});
			else
			{
				InfoMessage("Не удалось отправить данные");
				allMoves = new List<string>();
				IsClosing = true;
				Task.Delay(5000).ContinueWith(q =>
				{
					Dispatcher.BeginInvoke(new ThreadStart(delegate
					{
						DashboardW dashboardW = Tag as DashboardW;
						dashboardW.LoadNewView("Game");
					}));
				});
			}
		}

		private void BotMove()
		{
			if (MyColor == "black")
				PBWhite.Visibility = Visibility.Visible;
			else
				PBBlack.Visibility = Visibility.Visible;
			new Thread(delegate ()
			{
				Task.Factory.StartNew(() =>
				{
					string move = chess.GetBotMove(MyColor == "black" ? "white" : "black", BotLevel);
					Dispatcher.BeginInvoke(new ThreadStart(delegate
					{
						if (MyColor == "black")
							PBWhite.Visibility = Visibility.Hidden;
						else
							PBBlack.Visibility = Visibility.Hidden;
						GlobalMove(move);
					}));
				});
			}).Start();
		}

		private void StartGeneralTimer()
		{
			GeneralTimer.Tick += new EventHandler(GeneralTimeFlow);
			GeneralTimer.Interval = new TimeSpan(0, 0, 0, 1);
			GeneralTimer.Start();
		}

		private void StopGeneralTimer()
		{
			GeneralTimer.Stop();
			GeneralTimer.Tick -= new EventHandler(GeneralTimeFlow);
		}

		private void GeneralTimeFlow(object sender, EventArgs e)
		{
			if (chess.GetCurrentColor() == "black")
			{
				BlackTime--;
				new Thread(delegate ()
				{
					Task.Factory.StartNew(() =>
					{
						Dispatcher.BeginInvoke(new ThreadStart(delegate
						{
							CalculateTime(BlackTime, "black");
						}));
					});
				}).Start();
				if (BlackTime == 0)
				{
					StopGeneralTimer();
					gg = true;
					CheckBoard();
				}
			}
			else
			{
				WhiteTime--;
				new Thread(delegate ()
				{
					Task.Factory.StartNew(() =>
					{
						Dispatcher.BeginInvoke(new ThreadStart(delegate
						{
							CalculateTime(WhiteTime, "white");
						}));
					});
				}).Start();
				if (WhiteTime == 0)
				{
					StopGeneralTimer();
					gg = true;
					CheckBoard();
				}
			}
		}

		private void CalculateTime(int time, string color)
		{
			int minutes = time / 60;
			int seconds = time - minutes * 60;
			TextBlock TBM;
			TextBlock TBS;
			if (color == "black")
			{
				// Ход чёрных
				TBM = TBBM;
				TBS = TBBS;
			}
			else
			{
				// Ход белых
				TBM = TBWM;
				TBS = TBWS;
			}
			if (minutes > 9)
				TBM.Text = minutes.ToString();
			else
				TBM.Text = "0" + minutes.ToString();
			if (seconds > 9)
				TBS.Text = seconds.ToString();
			else
				TBS.Text = "0" + seconds.ToString();
		}

		private void CountPoints(string turn)
		{
			int x = turn.Substring(3, 2)[0] - 'a';
			int y = turn.Substring(3, 2)[1] - '1';
			ChessRules.Figure mainFigure = (ChessRules.Figure)turn[0];
			ChessRules.Figure toFigure = (ChessRules.Figure)chess.GetFigureAt(x, y);
			ChessRules.Figure promotion = turn.Length == 6 ? (ChessRules.Figure)turn[5] : ChessRules.Figure.none;
			if (toFigure != ChessRules.Figure.none)
				Eat(mainFigure, toFigure);
			if (promotion != ChessRules.Figure.none)
				StatPromote(promotion);
		}

		private void Eat(ChessRules.Figure hunter, ChessRules.Figure victim)
		{
			switch (hunter)
			{
				case ChessRules.Figure.whiteKing:
					gs.EatByWhiteKing++;
					break;
				case ChessRules.Figure.whiteQueen:
					gs.EatByWhiteQueen++;
					break;
				case ChessRules.Figure.whiteRook:
					gs.EatByWhiteRook++;
					break;
				case ChessRules.Figure.whiteBishop:
					gs.EatByWhiteBishop++;
					break;
				case ChessRules.Figure.whiteKnight:
					gs.EatByWhiteKnight++;
					break;
				case ChessRules.Figure.whitePawn:
					gs.EatByWhitePawn++;
					break;
				case ChessRules.Figure.blackKing:
					gs.EatByBlackKing++;
					break;
				case ChessRules.Figure.blackQueen:
					gs.EatByBlackQueen++;
					break;
				case ChessRules.Figure.blackRook:
					gs.EatByBlackRook++;
					break;
				case ChessRules.Figure.blackBishop:
					gs.EatByBlackBishop++;
					break;
				case ChessRules.Figure.blackKnight:
					gs.EatByBlackKnight++;
					break;
				case ChessRules.Figure.blackPawn:
					gs.EatByBlackPawn++;
					break;
			}
			switch (victim)
			{
				case ChessRules.Figure.whiteQueen:
					gs.LostWhiteQueen++;
					LWQ.Text = (Convert.ToInt32(LWQ.Text) + 1).ToString();
					break;
				case ChessRules.Figure.whiteRook:
					gs.LostWhiteRook++;
					LWR.Text = (Convert.ToInt32(LWR.Text) + 1).ToString();
					break;
				case ChessRules.Figure.whiteBishop:
					gs.LostWhiteBishop++;
					LWB.Text = (Convert.ToInt32(LWB.Text) + 1).ToString();
					break;
				case ChessRules.Figure.whiteKnight:
					gs.LostWhiteKnight++;
					LWN.Text = (Convert.ToInt32(LWN.Text) + 1).ToString();
					break;
				case ChessRules.Figure.whitePawn:
					gs.LostWhitePawn++;
					LWP.Text = (Convert.ToInt32(LWP.Text) + 1).ToString();
					break;
				case ChessRules.Figure.blackQueen:
					gs.LostBlackQueen++;
					LBQ.Text = (Convert.ToInt32(LBQ.Text) + 1).ToString();
					break;
				case ChessRules.Figure.blackRook:
					gs.LostBlackRook++;
					LBR.Text = (Convert.ToInt32(LBR.Text) + 1).ToString();
					break;
				case ChessRules.Figure.blackBishop:
					gs.LostBlackBishop++;
					LBB.Text = (Convert.ToInt32(LBB.Text) + 1).ToString();
					break;
				case ChessRules.Figure.blackKnight:
					gs.LostBlackKnight++;
					LBN.Text = (Convert.ToInt32(LBN.Text) + 1).ToString();
					break;
				case ChessRules.Figure.blackPawn:
					gs.LostBlackPawn++;
					LBP.Text = (Convert.ToInt32(LBP.Text) + 1).ToString();
					break;
			}
		}

		private void StatPromote(ChessRules.Figure rise)
		{
			switch (rise)
			{
				case ChessRules.Figure.whiteQueen:
					gs.WhitePromoteQueen++;
					break;
				case ChessRules.Figure.whiteRook:
					gs.WhitePromoteRook++;
					break;
				case ChessRules.Figure.whiteBishop:
					gs.WhitePromoteBishop++;
					break;
				case ChessRules.Figure.whiteKnight:
					gs.WhitePromoteKnight++;
					break;
				case ChessRules.Figure.blackQueen:
					gs.BlackPromoteQueen++;
					break;
				case ChessRules.Figure.blackRook:
					gs.BlackPromoteRook++;
					break;
				case ChessRules.Figure.blackBishop:
					gs.BlackPromoteBishop++;
					break;
				case ChessRules.Figure.blackKnight:
					gs.BlackPromoteKnight++;
					break;
				default:
					break;
			}
		}

		private void PlaySound()
		{
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			int volume = Convert.ToInt32(ChessKey.GetValue("volume"));
			ChessKey.Close();
			MediaPlayer Sound = new MediaPlayer
			{
				Volume = volume / 100
			};
			Sound.Open(new Uri("assets/sound/move.wav", UriKind.Relative));
			Sound.Play();
		}

		public void SafeLeave(Button b)
		{
			if (IsClosing)
			{
				InfoMessage("Переход будет автоматическим");
				return;
			}
			DashboardW dashboardW = Tag as DashboardW;
			if (gs.GameBegin != "")
			{
				if (GameType == "Net")
				{
					RedirectButton = b;
					BClientDisconnect_Click("leave", null);
				}
				else
				{
					if (GameType == "Duo")
					{
						if (GeneralTimer.IsEnabled)
						{
							StopGeneralTimer();
							GeneralTimer = null;
						}
					}
					dashboardW.LoadNewView(b.Tag.ToString());
				}
			}
			else
				dashboardW.LoadNewView(b.Tag.ToString());
		}

		private void InfoMessage(string content, string button = "X")
		{
			InfoBar.MessageQueue.Enqueue(content, button, delegate () { });
		}
	}
}