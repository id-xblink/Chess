   M S B A M L   `   `   `  ����   Wpf2p2p    Wpf2p2p.ProfileUC   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  :d2http://schemas.microsoft.com/expression/blend/2008  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006        MaterialDesignBody#���     MaterialDesignPaper#���  $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�.���   PrimaryHueMidBrush#���    � RowDefinitions  �� $
��auto ��� .+��� $��0��$'��!Настройка профиля��$��15q�$��30(�$��Center=�   PrimaryHueMidForegroundBrush#���  � $��1��#���   � ColumnDefinitions �� �� �� .+�� $��0��$��5q�  �� $
��auto ��� �� .+��� $��0��$��Демонстрация��$��15q�$��24(�$��Center=�$��	Underline{�� $��1��$��Center=�$
��50 0q�  �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto �.+��� $��0��$��Ваш профиль��$��21(�$��Center=�$��0 0 0 10q���  �� Orientation$ 
Horizontal=�$��1��$��15q�.+��� $��2q�#���    �� CornerRadius$ 10��$��64��$��64������ -   IAvatar�� j� Stretch$ UniformToFill=��� -    ��Name$ TBLoginProfile��$��noname��$��Center=�$��21(�$��15q�$��CharacterEllipsis=��� -   $ TBRegionProfile��$��2��$��Регион: ???��$��16(�$��10q�$��CharacterEllipsis=��� -   $ TBRatingProfile��$��3��$��Рейтинг: ???��$��16(�$��10q�$��CharacterEllipsis=��� -   $ TBGamesProfile��$��4��$��Игр: ???��$��16(�$��10q�$��CharacterEllipsis=�� $��2��$
��30 0q�$��Center=�  �� $
��auto ��� $
��auto �.+��� $��0��$��Ваш ранг��$��21(�$��Center=�$��0 0 0 10q��� $��1��$��2q�$ 15��#���  ._��� $��5q�.+��� -   $ TBRank��$��Пешка 0��$��21(�$��Center=��� -    ��Name$ IRank��  dp#���  $	��100��$	��100���� $ 
Horizontal=�$��Center=�.+�'  !MaterialDesignThemes.Wpf.PackIcon  -     Name$ Star1��   Kind$ StarOutline=�$��32��$��32��  -	   $ Star2��$ StarOutline=�$��32��$��32��$��0 10 0 0q�  -
   $ Star3��$ StarOutline=�$��32��$��32��� $��1��$��5q�  �� $
��auto ��� .+��� $��0��$-��'Настройка интерфейса��$��15q�$��24(�$��Center=�$��	Underline{�� $��1��  �� $
��auto ��� $
��auto ��� $
��auto ��� �� $
��auto ��� .+��� $��0��$ 
Horizontal=�$��15q�.+��� $��Center=�$��Светлая��$��14(�d� -   	 d�Name$	 TBSwitchTheme��$	��5 0q��� $��Center=�$��Тёмная��$��14(�� $��1��$��5q�  �� �� �� .+��� -   
 ��Name$
 RPrimary��$��0��$��30��#���  �� $��0��$'��!Основной - Обычный��$��14(�#���  � $��1�� �� �� �� .+��� -   $
 	RPrimaryL��$��0��$��30��  PrimaryHueLightBrush#���  �� $��0��$��Cветлый��$��14(� " PrimaryHueLightForegroundBrush#���  �� -   $
 	RPrimaryM��$��1��$��30��#���  �� $��1��$��Обычный��$��14(�#���  �� -   $
 	RPrimaryD��$��2��$��30��  PrimaryHueDarkBrush#���  �� $��2��$��Тёмный��$��14(� ! PrimaryHueDarkForegroundBrush#���  �� -   $
 RAccent��$��2��$��30�� 	 SecondaryAccentBrush#��� 	 �� $��2��$"��Дополнительный��$��14(� "
 SecondaryAccentForegroundBrush#��� 
 �� $��2��$��Основной цвет��$��5q�$��21(�$��Center=�� $��3��$��10q�  �� �� �� ��  �� �� �� �� �� .+��� -    ��Name$ ByellowP��$��0��$��0��
 � Tag$ yellow��  MaterialDesignRaisedButton#���  $
��auto��$��2q�����  �� RelativeSource �� *Self+ �� Path$ ActualHeight��� -   $ BamberP��$��0��$��1��$ amber��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BdeeporangeP��$��0��$��2��$ 
deeporange��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BlightblueP��$��0��$��3��$ 	lightblue��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BtealP��$��0��$��4��$
 teal��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BcyanP��$��1��$��0��$
 cyan��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BpinkP��$��1��$��1��$
 pink��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BgreenP��$��1��$��2��$ green��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BdeeppurpleP��$��1��$��3��$ 
deeppurple��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BindigoP��$��1��$��4��$ indigo��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BlightgreenP��$��2��$��0��$ 
lightgreen��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BblueP��$��2��$��1��$
 blue��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BlimeP��$��2��$��2��$
 lime��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BredP��$��2��$��3��$	 red��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -   $ BorangeP��$��2��$��4��$ orange��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -    $ BpurpleP��$��3��$��0��$ purple��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -!   $ 
BbluegrayP��$��3��$��1��$ bluegray��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -"   $ BgreyP��$��3��$��2��$
 grey��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -#   $ BbrownP��$��3��$��3��$ brown��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� $��4��$+��%Дополнительный цвет��$��5q�$��21(�$��Center=�� $��5��$��10q�  �� �� �� ��  �� �� �� �� �� .+��� -$   $ ByellowA��$��0��$��0��$ yellow��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -%   $ BamberA��$��0��$��1��$ amber��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -&   $ BdeeporangeA��$��0��$��2��$ 
deeporange��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -'   $ BlightblueA��$��0��$��3��$ 	lightblue��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -(   $ BtealA��$��0��$��4��$
 teal��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -)   $ BcyanA��$��1��$��0��$
 cyan��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -*   $ BpinkA��$��1��$��1��$
 pink��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -+   $ BgreenA��$��1��$��2��$ green��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -,   $ BdeeppurpleA��$��1��$��3��$ 
deeppurple��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� --   $ BindigoA��$��1��$��4��$ indigo��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -.   $ BlightgreenA��$��2��$��0��$ 
lightgreen��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -/   $ BblueA��$��2��$��1��$
 blue��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -0   $ BlimeA��$��2��$��2��$
 lime��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -1   $ BredA��$��2��$��3��$	 red��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -2   $ BorangeA��$��2��$��4��$ orange��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight��� -3   $ BpurpleA��$��3��$��0��$ purple��#���  $
��auto��$��2q�����  �� *Self+$ ActualHeight�� $��2��$��5q�  �� $
��auto ��� �� $
��auto �.+��� $��0��$2��,Настройка личных данных��$��15q�$��24(�$��Center=�$��	Underline{�� $��1��  �� $
��auto ��� $
��auto ��� $
��auto ��� .+��� $��0��$��Аватар��$��0 10 0 5q�$��21(�$��Center=�� $��1�� �� $
��auto ��� .+��� $
��25 5q�$��0��$��2q�#���   $ 10��$��64��$��64������ -4   IAvatarPreview��$ UniformToFill=�� $��1��  �� �� .+��� -5   $ 
BLoadImage��$��0��$
��20 5q�$)��#Загрузить картинку���� -6   $ BRemoveAvatar��$��1��$
��20 5q�$!��Удалить аватар��#��� 	 #��� 
 �� $��2��$)��#Основные параметры��$��0 10 0 5q�$��21(�$��Center=�� $��3��$
��35 0q�  �� �� �� �� �� .+��� -7    ��Name$ TBLogin��$��0��)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Новый логин��$��Center=�$��14(�$	��0 5q� �� 	MaxLength$ 20�� % !MaterialDesignFloatingHintTextBox#��[ G� -8    G�Name$ 
PBCurrPass��$��1��$! Текущий пароль��$��Center=�$��14(�$	��0 5q� G� 	MaxLength$ 20�� ) %MaterialDesignFloatingHintPasswordBox#��[ G� -9   $ PBPass��$��2��$ Новый пароль��$��Center=�$��14(�$	��0 5q�$ 20��#��[ G� -:   $ 	PBRepPass��$��3��$ Подтвердить��$��Center=�$��14(�$	��0 5q�$ 20��#��[ �� -;    ��Name$ CBRegion��$��4��$��Center=�$ Новый регион�� & "MaterialDesignFloatingHintComboBox#��[ $��14(�$	��0 5q����� *	Regions+���� .R��� ���� $
 Name�� $��2�� �� �� .+��� -<   $ BClear��$��0��$��5q�$��Очистить��#��� 	 #��� 
 �� -=   $ BSave��$��1��$��5q�$��Сохранить��'  !MaterialDesignThemes.Wpf.Snackbar  ->     Name$ InfoBar��$��1��   MessageQueue 4  .MaterialDesignThemes.Wpf.MessageQueueExtension  