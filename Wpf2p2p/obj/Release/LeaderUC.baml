   M S B A M L   `   `   `  ����   Wpf2p2p    Wpf2p2p.LeaderUC   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  :d2http://schemas.microsoft.com/expression/blend/2008  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006        MaterialDesignBody#���     MaterialDesignPaper#���  $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�.���   PrimaryHueMidBrush#���    � RowDefinitions  �� $
��auto ��� .+��� $��0��$#��Таблица лидеров��$��15q�$��30(�$��Center=�   PrimaryHueMidForegroundBrush#���  � $��1��#���   � ColumnDefinitions �� �� $
��auto �.+�� $��0��  �� $
��auto ��� $
��auto ��� .+��� $��0��$��Настройки��$��10q�$��24(�$��Center=�$��	Underline{�� $��1�� �� �� �� .+�� $��0��$
��10 0q�  �� ��  �� �� .+��� $��0��$��2��$!��Масштаб отбора��$��22(�$��Center=�$��Center=�$��0 0 0 10q�	� -    	�Name$ RBLeaderWorld�� 	� 	GroupName$ Global��$��1��$��0��$��По миру��$��Center=�$��Center=�$��18(� d� 	IsChecked$
 TrueY�	� -   $ RBLeaderRegion��$ Global��$��1��$��1��$��По региону��$��Center=�$��Center=�$��18(��� -    ��Name$ CBRegion��$��1��)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Регион�� & "MaterialDesignFloatingHintComboBox#��[ $��14(�$
��10 0q����� *	Regions+{���  �� ElementName$ RBLeaderRegion�� �� Path$ 	IsChecked����� .R��� ���� $
 Name��� $��2��$��2q�$��38��$
��25 0q�#���  ._�� $��2q� �� $
��auto ��� �� $
��auto �.+�'  !MaterialDesignThemes.Wpf.PackIcon  	   Kind$	 Magnify=�$
��auto��$
��auto��#���  �� -   
 ��Name$
 TBFind��$��1��$% Узнать место по ID��$��14(�$
��10 0q� �� 	MaxLength$ 11���� -    ��Name$ 
BClearFind��$��2��$
��auto��$��0q�$
��auto��  MaterialDesignFlatButton#��[ .��  $	 Clear=�$
��auto��$
��auto��&   System.Windows.Controls.DataGrid  -     Name$ 	DGLeaders��$��2��   CanUserReorderColumns .     CanUserSortColumns .     CanUserResizeRows .     
IsReadOnly .    AutoGenerateColumns .  $��25q�  PrimaryHueLightBrush#���  ���� *Leader+   Columns 0  *System.Windows.Controls.DataGridTextColumn   " PrimaryHueLightForegroundBrush   
Foreground# �  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *ID+,  &System.Windows.Controls.DataGridColumn   Header �� $��ID��$��14(�$��CharacterEllipsis=�#���    # �   �� *
Position+ �� $��
Место��$��14(�$��CharacterEllipsis=�#���  4  .System.Windows.Controls.DataGridTemplateColumn   �� $��Аватар��$��14(�$��CharacterEllipsis=�#���     CellTemplate �� .R��� $��34��$��34�� �� CornerRadius$ 5��#���  $��2q�����  j� Stretch$ UniformToFill=� �� ImageSource �� *Avatar+  # �   �� *Login+ �� $��
Логин��$��14(�$��CharacterEllipsis=�#���    # �   �� *	Raiting+ �� $��Рейтинг��$��14(�$��CharacterEllipsis=�#���    # �   �� *Region+ �� $��Регион��$��14(�$��CharacterEllipsis=�#���    # �   �� *Game+ �� $��Игр��$��14(�$��CharacterEllipsis=�#���  � $��1��  �� $
��auto ��� $��* ��� $
��auto �.+�� $��0��$��Center=�  �� �� $
��auto ��� $
��auto ��� $
��auto �.+���  �� Orientation$ 
Horizontal=�$��0��$��
15 15 15 5q�.+��� $��2q�#���   $ 10��$��64��$��64������ -   IAvatar��$ UniformToFill=��� -    ��Name$ TBLogin��$��noname��$��Center=�$��21(�$��15q�$��CharacterEllipsis=��� -	   $ TBRegion��$��1��$��Регион: ???��$��16(�$��5q�$��CharacterEllipsis=��� -
   $ TBRating��$��2��$��Рейтинг: ???��$��16(�$��5q�$��CharacterEllipsis=��� -   $ TBGames��$��3��$��Игр: ???��$��16(�$��5q�$��CharacterEllipsis=�� $��1��$��Center=�.+��� $
��15 0q�.+��� $��Ваш ранг��$��25(�$��Center=�$��0 0 0 5q��� $��2q�$ 15��#���  ._��� $��5q�.+��� -   $ TBRank��$��Пешка 0��$��21(�$��Center=��� -    ��Name$ IRank��  dp#���  $��80��$��80���� $ 
Horizontal=�$��Center=�.+�  -     Name$ Star1��$	 StarOutline=�$��32��$��32��  -   $ Star2��$	 StarOutline=�$��32��$��32��$��0 10 0 0q�  -   $ Star3��$	 StarOutline=�$��32��$��32��� $��2��$��Center=�$��20q�.+��� $��18(�.  (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=����� $*��$Узнать личное место�� "	 SecondaryAccentForegroundBrush#��� 	 $��CharacterEllipsis=�$��Center=�.���� $��Center=�$ 
Horizontal=�.+��� .+�	� -   $ RBLocalWorld��$ Local��$��1��$��0��$��По миру��$��18(�$
 TrueY�	� -   $ RBLocalRegion��$ Local��$��1��$��1��$��По региону��$��18(�$	��0 5q��� -   $ BLocalPosition��$��0q�$��50��$��50��$��10 0 0 0q�.��  $	 TrophyOutline=�$
��auto��$
��auto��'	  !MaterialDesignThemes.Wpf.Snackbar	  -     	 Name$  InfoBar��$��1��! 	  MessageQueue! 4
  .MaterialDesignThemes.Wpf.MessageQueueExtension
  