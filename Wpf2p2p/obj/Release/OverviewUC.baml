   M S B A M L   `   `   `  ����   Wpf2p2p    Wpf2p2p.OverviewUC   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  :d2http://schemas.microsoft.com/expression/blend/2008  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006        MaterialDesignBody#���     MaterialDesignPaper#���  $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�.���   PrimaryHueMidBrush#���    � RowDefinitions  �� $
��auto ��� .+��� $��0��$��Просмотр��$��15q�$��30(�$��Center=�   PrimaryHueMidForegroundBrush#���  � $��1��#���  .+�� -    �Name$ GPreview��$y�Visible=� � ColumnDefinitions �� $	��350 ��� .+�� -   $ 	GGameList��$��0��$y�Visible=�  PrimaryHueLightBrush#���  .+��� -    ��Name$ LBGames������ *GamePreview+���� .R��� -   $	��120��
 � Tag �� *UG_ID+.����  " PrimaryHueLightForegroundBrush#���  $��0 0 0 1q�$	��325��$	��100��._��� .+��� $��15(�$��	Underline{�#���  $��CharacterEllipsis=�$��5 0 0 0q����� *ID+ �� StringFormat$ ID игры: {0}����  �� Orientation$ 
Horizontal=�$��Center=�.+��� $
��Left=�$��64��$��64��$��5q� �� CornerRadius$ 10��$��2q�#���  ����  j� Stretch$ UniformToFill=�	 �� ImageSource	 �� *Avatar+�� $ Vertical=�$��Center=�.+��� $��14(�#���  $��CharacterEllipsis=����� *Result+$ Исход: {0}���� $��14(�$	��0 5q�#���  $��CharacterEllipsis=����� *
GameType+$ Тип: {0}���� $��14(�#���  $��CharacterEllipsis=����� *	MyColor+$ Цвет: {0}��� $��1��  �� $
��auto ��� $��* ��� $
��auto �.+�� $��0�� �� $
��auto ��� $��* ��� $
��auto �.+��� $��0��$��10q�.+��� $��Режим игры��$��18(�$��CharacterEllipsis=��� -   
 ��Name$
 
TBGameType��$	��???��$��22(�$��CharacterEllipsis=��� -   $
 TBGameResult��$��Результат: ...��$��24(�$��Center=�$��CharacterEllipsis=��� $��1��$��Center=�$��Bottom=�.+��� $%��Контроль времени��$��24(�$��CharacterEllipsis=��� $ 
Horizontal=�$��Center=�.+�'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Clock=�$��45��$��45��#���  �� -   $
 TBGameTimeControl��$��0 + 0��$��20(�$��5q�$��Center=�$��CharacterEllipsis=��� $��2��$��2q�$��38��#���  $��Center=�$��10q�._�� $��2q� �� $
��auto ��� �� $
��auto �.+�  $ Magnify=�$
��auto��$
��auto��#���  �� -    ��Name$ TBFind��$��1��)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Открыть по ID��$��14(�$
��10 0q� �� 	MaxLength$ 11���� -	    ��Name$ 
BClearFind��$��2��$
��auto��$��0q�$
��auto��  MaterialDesignFlatButton#��[ .��  $ Clear=�$
��auto��$
��auto��� -
   $ GMiniPreview��$��1�� �� $	��150 ��� $��* ��� $	��150 �.+�� $��0��$��10q�  �� $
��auto ��� .+��� $��0��.+��� $��Сумма потерь��$��18(�$
��Left=��� $��белых фигур��$��18(�$
��Left=��� -   $
 TBCountWLost��$	��...��$��18(�$
��Left=�$��	Underline{��� $��1��#���  .���� -    ��Name$ LVWLost������ *LostFigures+���� .R�� $��10q�$��90��$	��100��  �� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$��0 0 0 -5q�$��19(�$��CharacterEllipsis=�$��Center=�#���  ���� *Name+�� $��1��$��70��$��70��$ 5�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     ShadowDepth$ 1Q����� $ UniformToFill=�	 �� *ResourceImage+�� $��2�� �� Padding$ 0q�#���  $ 30��$��30��$��30��$��-5 -20 0 -5q�  SecondaryAccentBrush#���  $	��Top=�$��Right=�$��1q�._��� $��20(�$��Center=�$��Center=� " SecondaryAccentForegroundBrush#���  ���� *Count+���� .R�� � -   $ GPreviewBoard��$��1������  �� RelativeSource �� *Self+ �� Path$ ActualHeight�����  �� ElementName$ GMiniPreview��$ ActualHeight�� $��2��$��10q�  �� $
��auto ��� .+��� $��0��.+��� $��Сумма потерь��$��18(�$��Right=��� $��чёрных фигур��$��18(�$��Right=��� -   $
 TBCountBLost��$	��...��$��18(�$��Right=�$��	Underline{��� $��1��#���  .���� -   $ LVBLost������ *LostFigures+���� .R�� $��10q�$��90��$	��100��  �� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$��0 0 0 -5q�$��19(�$��CharacterEllipsis=�$��Center=�#���  ���� *Name+�� $��1��$��70��$��70��$ 5��   $ 1Q����� $ UniformToFill=�	 �� *ResourceImage+�� $��2��$ 0q�#���  $ 30��$��30��$��30��$��-5 -20 0 -5q�#���  $	��Top=�$��Right=�$��1q�._��� $��20(�$��Center=�$��Center=�#���  ���� *Count+���� .R�� � $��2�� �� $
��auto ��� �� �� $
��auto �.+��� $��0��$��10q�.+��� -   $
 TBGameDT��$!��Дата и время: ...��$��18(�$��0 10 0 0q�$��CharacterEllipsis=��� -   $
 TBGameID��$��ID игры: ...��$��20(�$��CharacterEllipsis=��� -   $
 TBGameColor��$��1��$��Цвет фигур: ...��$��19(�$��Center=�$��Center=�$��CharacterEllipsis=��� -   $
 TBGameRaiting��$��2��$��Рейтинг: ...��$��19(�$��Center=�$��Center=�$��CharacterEllipsis=��� -   $ BOverviewGame��$��3��$��15q�#���  .���� $ 
Horizontal=�.+��� $��В игре��$��Center=�$��CharacterEllipsis=�#���    $ ArrowTopRightThick=�$
��auto��$
��auto��#���  � -   $ 	GOverview��$y�	Collapsed=� �� $	��350 ��� $��* ��� $
��auto �.+�� $��0��#���    �� �� $
��auto �.+�&   System.Windows.Controls.DataGrid  -     Name$ DGTurns��$��0��   CanUserReorderColumns .     CanUserSortColumns .     CanUserResizeColumns .     CanUserResizeRows .     
IsReadOnly .    AutoGenerateColumns .  #���     SelectionMode$ Single=����� *	ChessTurn+   Columns 0  *System.Windows.Controls.DataGridTextColumn      
Foreground#  �  1  +System.Windows.Controls.DataGridBoundColumn!   Binding! �� *
TurnNumber+,  &System.Windows.Controls.DataGridColumn"   Header" �� $��Номер хода��$��14(�$��CharacterEllipsis=�#���    #  �  ! �� *White+" �� $��
Белые��$��14(�$��CharacterEllipsis=�#���    #  �  ! �� *Black+" �� $��Чёрные��$��14(�$��CharacterEllipsis=�#���  � $��1�� �� �� .+��� -   $ BOpenWhiteTurn��$��0��$��Белый ход��$��10q��� -   $ BOpenBlackTurn��$��1��$��Чёрный ход��$��10q�� -   $ GMiniOverview��$��1��$��25q�.+�� -   $ GOverviewBoard��$��2��$��1������  �� *Self+$ ActualHeight����� $ GMiniOverview��$ ActualHeight�� $��2��  �� $��* ��� $
��auto �.+�� $��0��.+��� $ Vertical=�$��Center=�.+��� -   $ BStart��$
��0 10q�$��Center=�{��� $ 	BPrevious��$ 	IsEnabled�.��  $ SkipPrevious=�$
��auto��$
��auto���� -   $ 	BPrevious��$
��0 10q�$��Center=�.��  $ 	PageFirst=�$
��auto��$
��auto���� .+��� -   $
 TBColor��$��Начало игры��$��14(�$��Center=��� -   $
 TBPage��$��0 из 0��$��14(�$��Center=��� -   $ BNext��$
��0 10q�$��Center=�.��  $ PageLast=�$
��auto��$
��auto���� -    $
 BEnd��$
��0 10q�$��Center=�{��� $ BNext��$ 	IsEnabled�.��  $ SkipNext=�$
��auto��$
��auto���� -!   $ BPreviewGame��$��1��$��Right=�$��15 15 15 20q�#���  .���� $ 
Horizontal=�.+�  $ ArrowBottomLeftThick=�$
��auto��$
��auto��#���  �� $��
Назад��$��Center=�#���  '  !MaterialDesignThemes.Wpf.Snackbar  -"   #  Name$# InfoBar��$��1��$   MessageQueue$ 4	  .MaterialDesignThemes.Wpf.MessageQueueExtension	  