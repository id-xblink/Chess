using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Wpf2p2p
{
	public partial class CheckerboardUC : UserControl
	{
		List<Button> Buttons = new List<Button>();
		List<Rectangle> Marks = new List<Rectangle>();

		public CheckerboardUC()
		{
			InitializeComponent();
			InitBoard();
			SetMarks();
		}

		private void SetMarks()
		{
			InitMark("move", 0, 5);
			InitMark("move", 1, 5);
			InitMark("attack", 0, 6);
			InitMark("check", 1, 6);
		}

		private void InitMark(string type, int x, int y)
		{
			Rectangle mark = new Rectangle
			{
				StrokeDashArray = new DoubleCollection() { 5 },
				StrokeDashCap = PenLineCap.Round,
				StrokeThickness = 3,
				Margin = new Thickness(2),
			};
			switch (type)
			{
				case "move":
					mark.SetResourceReference(Shape.StrokeProperty, "ms");
					break;
				case "attack":
					mark.SetResourceReference(Shape.StrokeProperty, "as");
					break;
				case "check":
					mark.SetResourceReference(Shape.StrokeProperty, "cs");
					break;
			}
			Panel.SetZIndex(mark, 1);
			Grid.SetColumn(mark, x);
			Grid.SetRow(mark, y);
			Marks.Add(mark);
			GBoard.Children.Add(mark);
		}

		private void InitBoard()
		{
			Style style = FindResource("MaterialDesignFlatButton") as Style;

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
						Style = style,
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
			InitFigures("2n1r1k1/5p1p/Q5p1/4q3/P1P5/8/nK6/1RB3R1");
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
		
		private void BChangeColor_Click(object sender, RoutedEventArgs e)
		{
			if (PISelected.Kind != PackIconKind.Close)
				return;
			Button b = sender as Button;
			switch (b.Tag.ToString())
			{
				case "White":
					PISelected.Kind = PackIconKind.SquareOutline;
					CPElement.Color = ((SolidColorBrush)FindResource("ws")).Color;
					break;
				case "Black":
					PISelected.Kind = PackIconKind.Square;
					CPElement.Color = ((SolidColorBrush)FindResource("bs")).Color;
					break;
				case "Move":
					PISelected.Kind = PackIconKind.ArrowAll;
					CPElement.Color = ((SolidColorBrush)FindResource("ms")).Color;
					break;
				case "Attack":
					PISelected.Kind = PackIconKind.Sword;
					CPElement.Color = ((SolidColorBrush)FindResource("as")).Color;
					break;
				case "Check":
					PISelected.Kind = PackIconKind.ShieldOutline;
					CPElement.Color = ((SolidColorBrush)FindResource("cs")).Color;
					break;
			}
			BSelectedIcon.IsEnabled = true;
			BSelectedIcon.Tag = false;
		}

		private void BCancel_Click(object sender, RoutedEventArgs e)
		{
			PISelected.Kind = PackIconKind.Close;
			BSelectedIcon.IsEnabled = false;
			BSelectedIcon.Tag = true;
		}

		private void BSave_Click(object sender, RoutedEventArgs e)
		{
			ResourceDictionary rd = Application.Current.Resources.MergedDictionaries[3];
			RegistryKey CurrentUserKey = Registry.CurrentUser;
			RegistryKey ChessKey = CurrentUserKey.CreateSubKey("2p2p");
			SolidColorBrush brush = new SolidColorBrush { Color = CPElement.Color };
			switch (PISelected.Kind)
			{
				case PackIconKind.SquareOutline:
					ChessKey.SetValue("ws", brush.Color);
					rd["ws"] = brush;
					break;
				case PackIconKind.Square:
					ChessKey.SetValue("bs", brush.Color);
					rd["bs"] = brush;
					break;
				case PackIconKind.ArrowAll:
					ChessKey.SetValue("ms", brush.Color);
					rd["ms"] = brush;
					break;
				case PackIconKind.Sword:
					ChessKey.SetValue("as", brush.Color);
					rd["as"] = brush;
					break;
				case PackIconKind.ShieldOutline:
					ChessKey.SetValue("cs", brush.Color);
					rd["cs"] = brush;
					break;
			}
			ChessKey.Close();
			InfoMessage("Изменение сохранено");
			BCancel_Click(sender, e);
		}

		private void InfoMessage(string content, string button = "X")
		{
			InfoBar.MessageQueue.Enqueue(content, button, delegate () { });
		}
	}
}