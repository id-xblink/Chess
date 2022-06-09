using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Wpf2p2p
{
	public partial class PieceUC : UserControl
    {
		public enum Figure
		{
			alpha,
			california,
			cardinal,
			cburnett,
			chess7,
			chessnut,
			companion,
			dubrovny,
			fantasy,
			fresca,
			gioco,
			icpieces,
			kosal,
			leipzig,
			letter,
			libra,
			maestro,
			merida,
			mono,
			pirouetti,
			pixel,
			reillycraig,
			riohacha,
			shapes,
			spatial,
			staunty,
			tatiana
		}

		string name = "";

		List<Button> Buttons = new List<Button>();

		public PieceUC()
        {
            InitializeComponent();
			InitBoard();
			LoadFigures();
		}

		private void LoadFigures()
		{
			List<ChangeFigures> changes = new List<ChangeFigures>();
			foreach (Figure figure in (Figure[])Enum.GetValues(typeof(Figure)))
				changes.Add(new ChangeFigures(figure.ToString(), DBConnection.GetImage($"assets/piece/{figure.ToString()}/wn.png")));
			LVWLost.ItemsSource = changes;
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
			InitFigures("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR");
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

		private void BSave_Click(object sender, RoutedEventArgs e)
		{
			if (name != "")
			{
				RegistryKey CurrentUserKey = Registry.CurrentUser;
				RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
				ChessKey.SetValue("style", name);
				ChessKey.Close();
				InfoMessage("Стиль фигур сохранён");
			}
			else
				InfoMessage("Выберите стиль фигур");
		}

		private void BChangeFigure_Click(object sender, RoutedEventArgs e)
		{
			Button b = sender as Button;
			name = b.Tag.ToString();
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			object isDark = ChessKey.GetValue("isDarkTheme");
			ChessKey.Close();
			ResourceDictionary rd = Application.Current.Resources.MergedDictionaries[3];
			foreach (FigureName figureName in (FigureName[])Enum.GetValues(typeof(FigureName)))
			{
				if (figureName.ToString()[0] != 'd')
					rd[figureName.ToString()] = DBConnection.GetImage($"assets/piece/{name}/{figureName}.png");
				else
					rd[figureName.ToString()] = DBConnection.GetImage($"assets/piece/{name}/{(Convert.ToBoolean(isDark) ? "w" : "b")}{figureName.ToString()[1]}.png");
			}
		}

		private void InfoMessage(string content, string button = "X")
		{
			InfoBar.MessageQueue.Enqueue(content, button, delegate () { });
		}
	}
}