   M S B A M L   `   `   `  ����   Wpf2p2p    Wpf2p2p.StatisticsUC   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  :d2http://schemas.microsoft.com/expression/blend/2008  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006        MaterialDesignBody#���     MaterialDesignPaper#���  $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�  � 	Resources   �� 
TargetType �� Property �� Value �� 
TargetType  PrimaryHueDarkBrush ��Name  PrimaryHueMidBrush �� Triggers P� Property P� Value	 �� 
TargetName��%^  '��       '�� ?     �� " ��.���  � �� �� " ��.R��� #���  $��1q��� " ��.���  � �� �� " ��.R��� ._�� .+��� $ Border��#���  �� $��Header��$��Center=�$��Center=� P�  �@J�IsMouseOver . . ��� $	 Border��# �   � ��P�  �@��
IsSelected . . ��� $	 Border��# �   � ��.��� #���  
 � RowDefinitions
 �� $
��auto ��� .+��� $��0��$��Статистика��$��15q�$��30(�$��Center=�   PrimaryHueMidForegroundBrush#���  � $��1��#���   � ColumnDefinitions �� $	��275 ��� �� $	��275 �.+�� $��0��$��15q�
 �� �� �� .+��� $��0��$��15q�$��18(�.  (MaterialDesignThemes.Wpf.ColorZoneAssist   Mode$ Accent=�{���  �� ElementName$ DGGames�� �� Path$	 Tag����� $!��Результат игры�� " SecondaryAccentForegroundBrush#���  $��CharacterEllipsis=�$��Center=�.���� $��1��.+��� -    ��Name$ CBResultWin��
 � Tag$	 Win��$��Победа��$��16(� d� 	IsChecked$
 TrueY��� -   $ CBResultLose��$
 Lose��$��Поражение��$��16(�$
 TrueY��� -   $ CBResultDraw��$
 Draw��$��
Ничья��$��16(�$
 TrueY��� -   $ CBResultStalemate��$ 	Stalemate��$��Пат��$��16(�$
 TrueY��� $��1��$��15q�$��18(�$ Accent=�{��� $ DGGames��$	 Tag����� $%��Временной период��#���  $��CharacterEllipsis=�$��Center=�.���� $��1��.+�(  "System.Windows.Controls.DatePicker  -     Name$ DPStart��$��0��$��14(�$��Bottom=�)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ Начало�� ( $MaterialDesignFloatingHintDatePicker#��[ $
��10 0q�   DisplayDateEnd �� $ DPEnd��$ SelectedDate�  -   $ DPEnd��$��1��$��14(�$��Bottom=�$ 
Конец��#��[ $
��10 0q�   DisplayDateStart �� $ DPStart��$ SelectedDate��� $��2��$��15q�$��18(�$ Accent=�{��� $ DGGames��$	 Tag����� $%��Контроль времени��#���  $��CharacterEllipsis=�$��Center=�.���� $��1��.+�	� -    	�Name$ 
RBTimerAny��$	 Any�� 	� 	GroupName$ TimerControl��$��
Любой��$��16(�$
 TrueY�	� -   $ 
RBTimerYes��$	 Yes��$ TimerControl��$
��Да��$��16(�	� -	   $ 	RBTimerNo��$ No��$ TimerControl��$��Нет��$��16(�� $��1��
 �� $
��auto ��� �� $
��auto �.+��� $��0��$��Таблица игр��$��10q�$��24(�$��Center=�$��	Underline{�&   System.Windows.Controls.DataGrid  -
     Name$ DGGames��   CanUserAddRows .     CanUserDeleteRows .     
IsReadOnly . $��1��$��
25 0 25 25q�$y�Visible=�  PrimaryHueLightBrush#���     AutoGenerateColumns .  {�.  $
 True��   Columns 0  *System.Windows.Controls.DataGridTextColumn   " PrimaryHueLightForegroundBrush   
Foreground# �  1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *id+,  &System.Windows.Controls.DataGridColumn    Header  �� $��ID��$��14(�$��CharacterEllipsis=�#���    # �   �� *datetime_start+  �� $��Дата начала��$��14(�$��CharacterEllipsis=�#���    # �   �� *
duration+  �� $4��.Длительность (в секундах)��$��14(�$��CharacterEllipsis=�#���    # �   �� *type+  �� $��Тип��$��14(�$��CharacterEllipsis=�#���    # �   �� *result+  �� $��
Исход��$��14(�$��CharacterEllipsis=�#���    # �   �� *color+  �� $��Цвет��$��14(�$��CharacterEllipsis=�#���    # �   �� *timer+  �� $%��Контроль времени��$��14(�$��CharacterEllipsis=�#���    # �   �� *	raiting+  �� $��Рейтинг��$��14(�$��CharacterEllipsis=�#���  � -   ! �Name$! GStatistics��$��1��$��25q�$y�	Collapsed=�
 �� �� $
��auto �.+�� $��0��.+�A� " A� Columns$" 4��$	��5,0q�.+�� -   $��c1���� #���  .@��� #���  ���� $ c1��$ ActualWidth����  �� $
��auto ��� .+�'  !MaterialDesignThemes.Wpf.PackIcon  $��0��#   Kind$# 
SwordCross=�$
��auto��$
��auto��#���  �� $��1��$�� Атака��$��Center=�$��18(�#���  $��CharacterEllipsis=�.��� -   $! GAttack��
 �� $
��auto ���  �� �� .+��� $��0��$��0��$0��*Белые (за игры / за игру)��$��18(�$��Center=�$��5q�$��CharacterEllipsis=�#���  �� $��0��$��1��$2��,Чёрные (за игры / за игру)��$��18(�$��Center=�$��5q�$��CharacterEllipsis=�#���  �� $��1��$��0��$��10q�.@��� -   $ ��Name$$ TBWEatK��$*��$Съедено королём: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBWEatQ��$.��(Съедено королевой: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBWEatR��$(��"Съедено ладьей: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBWEatB��$(��"Съедено слоном: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBWEatN��$&�� Съедено конём: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBWEatP��$(��"Съедено пешкой: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ 	TBWEatAll��$&�� Всего съедено: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� -   $$ TBWLostQ��$,��&Потеряно королев: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBWLostR��$(��"Потеряно ладей: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBWLostB��$*��$Потеряно слонов: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBWLostN��$(��"Потеряно коней: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBWLostP��$(��"Потеряно пешек: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ 
TBWLostAll��$(��"Всего потеряно: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� $��1��$��1��$��10q�.@��� -   $$ TBBEatK��$*��$Съедено королём: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBBEatQ��$.��(Съедено королевой: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBBEatR��$(��"Съедено ладьей: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBBEatB��$(��"Съедено слоном: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -   $$ TBBEatN��$&�� Съедено конём: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -    $$ TBBEatP��$(��"Съедено пешкой: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -!   $$ 	TBBEatAll��$&�� Всего съедено: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� -"   $$ TBBLostQ��$,��&Потеряно королев: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -#   $$ TBBLostR��$(��"Потеряно ладей: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -$   $$ TBBLostB��$*��$Потеряно слонов: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -%   $$ TBBLostN��$(��"Потеряно коней: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -&   $$ TBBLostP��$(��"Потеряно пешек: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -'   $$ 
TBBLostAll��$(��"Всего потеряно: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� #���  ���� $ c1��$ ActualWidth����  �� $
��auto ��� .+�  $��0��$# Checkerboard=�$
��auto��$
��auto��#���  �� $��1��$�� Действия��$��Center=�$��18(�#���  $��CharacterEllipsis=�.��� -(   $! GAction��
 �� $
��auto ���  �� �� .+��� $��0��$��0��$0��*Белые (за игры / за игру)��$��18(�$��Center=�$��5q�$��CharacterEllipsis=�#���  �� $��0��$��1��$2��,Чёрные (за игры / за игру)��$��18(�$��Center=�$��5q�$��CharacterEllipsis=�#���  �� $��1��$��0��$��10q�.@��� -)   $$ 
TBWCastleL��$.��(Длинных рокировок: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -*   $$ 
TBWCastleS��$0��*Коротких рокировок: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -+   $$ TBWCastleAll��$*��$Всего рокировок: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� -,   $$ TBWCheckAll��$"��Всего шахов: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� --   $$ TBWEnpassantAll��$$��Всего взятий: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� -.   $$ TBWPromoteQ��$7��1Превращений в королеву: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -/   $$ TBWPromoteR��$1��+Превращений в ладью: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -0   $$ TBWPromoteB��$1��+Превращений в слона: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -1   $$ TBWPromoteN��$/��)Превращений в коня: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -2   $$ TBWPromoteAll��$.��(Всего превращений: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� $��1��$��1��$��10q�.@��� -3   $$ 
TBBCastleL��$.��(Длинных рокировок: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -4   $$ 
TBBCastleS��$0��*Коротких рокировок: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -5   $$ TBBCastleAll��$*��$Всего рокировок: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� -6   $$ TBBCheckAll��$"��Всего шахов: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� -7   $$ TBBEnpassantAll��$$��Всего взятий: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� -8   $$ TBBPromoteQ��$7��1Превращений в королеву: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -9   $$ TBBPromoteR��$1��+Превращений в ладью: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -:   $$ TBBPromoteB��$1��+Превращений в слона: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -;   $$ TBBPromoteN��$/��)Превращений в коня: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� -<   $$ TBBPromoteAll��$.��(Всего превращений: 0 / 0��$��14(�$��CharacterEllipsis=�#���  �� �� #���  ���� $ c1��$ ActualWidth����  �� $
��auto ��� .+�  $��0��$# Crown=�$
��auto��$
��auto��#���  �� $��1��$�� Общие��$��Center=�$��18(�#���  $��CharacterEllipsis=�.��� -=   $! GCommon��
 �� $
��auto ���  �� �� .+��� $��0��$��0��$��
Белые��$��18(�$��Center=�$��5q�$��CharacterEllipsis=�#���  �� $��0��$��1��$��Чёрные��$��18(�$��Center=�$��5q�$��CharacterEllipsis=�#���  �� $��1��$��0��$��10q�.@��� ->   $$ TBWTypeD��$��Двойных игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -?   $$ 	TBWTypeS1��$0��*Одиночных (Бот №1) игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -@   $$ 	TBWTypeS2��$0��*Одиночных (Бот №2) игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -A   $$ 	TBWTypeS3��$0��*Одиночных (Бот №3) игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -B   $$ 	TBWTypeS4��$0��*Одиночных (Бот №4) игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -C   $$ 	TBWTypeS5��$0��*Одиночных (Бот №5) игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -D   $$ TBWTypeN��$��Сетевых игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -E   $$ 
TBWTypeAll��$��Всего игр: 0��$��14(�$��CharacterEllipsis=�#���  �� �� -F   $$ TBWColorAll��$��Выбор: 0%��$��14(�$��CharacterEllipsis=�#���  �� �� -G   $$ 
TBWResultW��$��Побед: 0��$��14(�$��CharacterEllipsis=�#���  �� -H   $$ 
TBWResultD��$��Ничьих: 0��$��14(�$��CharacterEllipsis=�#���  �� -I   $$ 
TBWResultS��$��Патов: 0��$��14(�$��CharacterEllipsis=�#���  �� -J   $$ 
TBWResultL��$��Поражений: 0��$��14(�$��CharacterEllipsis=�#���  �� -K   $$ TBWResultWR��$#��Процент побед: 0%��$��14(�$��CharacterEllipsis=�#���  �� �� $��1��$��1��$��10q�.@��� -L   $$ TBBTypeD��$��Двойных игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -M   $$ 	TBBTypeS1��$0��*Одиночных (Бот №1) игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -N   $$ 	TBBTypeS2��$0��*Одиночных (Бот №2) игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -O   $$ 	TBBTypeS3��$0��*Одиночных (Бот №3) игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -P   $$ 	TBBTypeS4��$0��*Одиночных (Бот №4) игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -Q   $$ 	TBBTypeS5��$0��*Одиночных (Бот №5) игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -R   $$ TBBTypeN��$��Сетевых игр: 0��$��14(�$��CharacterEllipsis=�#���  �� -S   $$ 
TBBTypeAll��$��Всего игр: 0��$��14(�$��CharacterEllipsis=�#���  �� �� -T   $$ TBBColorAll��$��Выбор: 0%��$��14(�$��CharacterEllipsis=�#���  �� �� -U   $$ 
TBBResultW��$��Побед: 0��$��14(�$��CharacterEllipsis=�#���  �� -V   $$ 
TBBResultD��$��Ничьих: 0��$��14(�$��CharacterEllipsis=�#���  �� -W   $$ 
TBBResultS��$��Патов: 0��$��14(�$��CharacterEllipsis=�#���  �� -X   $$ 
TBBResultL��$��Поражений: 0��$��14(�$��CharacterEllipsis=�#���  �� -Y   $$ TBBResultWR��$#��Процент побед: 0%��$��14(�$��CharacterEllipsis=�#���  �� �� #���  ���� $ c1��$ ActualWidth����  �� $
��auto ��� .+�  $��0��$# Numbers=�$
��auto��$
��auto��#���  �� $��1��$�� Уникальные��$��Center=�$��18(�#���  $��CharacterEllipsis=�.��� -Z   $! GUnique��
 �� $
��auto ���  �� �� .+��� $��0��$��0��$0��*Белые (за игры / за игру)��$��18(�$��Center=�$��5q�$��CharacterEllipsis=�#���  �� $��0��$��1��$2��,Чёрные (за игры / за игру)��$��18(�$��Center=�$��5q�$��CharacterEllipsis=�#���  �� $��1��$��0��$��10q�.@��� -[   $$ TBWEvaluationAll��$+��%Ценность досок: 0 / 0.00��$��14(�$��CharacterEllipsis=�#���  �� �� -\   $$ TBWDurationAll��$(��"Минут в играх: 0 / 0.00��$��14(�$��CharacterEllipsis=�#���  �� �� -]   $$ TBWTimerAll��$+��%Игр с таймером: 0 / 0.00%��$��14(�$��CharacterEllipsis=�#���  �� �� -^   $$ TBWRaitingAll��$1��+Получено рейтинга: 0 / 0.00��$��14(�$��CharacterEllipsis=�#���  �� �� $��1��$��1��$��10q�.@��� -_   $$ TBBEvaluationAll��$+��%Ценность досок: 0 / 0.00��$��14(�$��CharacterEllipsis=�#���  �� �� -`   $$ TBBDurationAll��$(��"Минут в играх: 0 / 0.00��$��14(�$��CharacterEllipsis=�#���  �� �� -a   $$ TBBTimerAll��$+��%Игр с таймером: 0 / 0.00%��$��14(�$��CharacterEllipsis=�#���  �� �� -b   $$ TBBRaitingAll��$1��+Получено рейтинга: 0 / 0.00��$��14(�$��CharacterEllipsis=�#���  �� �� -c   % ��Name$% BShowAll��$��1��$/��)Вернуться к списку игр��$��10q� 	 SecondaryAccentBrush#��� 	 #���  � $��2�� �� �� �� .+��� -d   $ Single��$��0��$��10q�$+��%Узнать об одной игре��#���  #���  {��� $ DGGames��$ 	IsEnabled��� -e   $ Multiple��$��1��$��10q�$5��/Узнать о нескольких играх��#���  #���  {��� $ DGGames��$ 	IsEnabled��� -f   $	 All��$��2��$��10q�$)��#Узнать о всех играх��#���   !
 PrimaryHueDarkForegroundBrush#��� 
 {��� $ DGGames��$ 	IsEnabled�� $��2��$��15q�
 �� �� �� .+��� $��0��$��15q�$��18(�$ Accent=�{��� $ DGGames��$	 Tag����� $��Цвет фигур��#���  $��CharacterEllipsis=�$��Center=�.���� $��1��.+�	� -g   $ 
RBColorAny��$	 Any��$ 
PieceColor��$��
Любой��$��16(�$
 TrueY�	� -h   $ RBColorWhite��$ White��$ 
PieceColor��$��
Белый��$��16(�	� -i   $ RBColorBlack��$ Black��$ 
PieceColor��$��Чёрный��$��16(��� $��1��$��15q�$��18(�$ Accent=�{��� $ DGGames��$	 Tag����� $��Тип игры��#���  $��CharacterEllipsis=�$��Center=�.���� $��1��.+��  �� �� .+��� -j   $ 	CBTypeDuo��$��0��$��Двойной��$��16(�$
 TrueY�$��Center=��� -k   $ 	CBTypeNet��$��1��$��По сети��$��16(�$
 TrueY�$��Center=��� $��1q�#��� 	 ._�� 
 �� �� ��  �� �� .+�� $��0��$��0��.+��� -l   $ 
CBTypeSolo��$��Одиночный��$��13(�$
 TrueY�$��Center=��� $��1��#��� 	 $��Bottom=��� $��1��#��� 	 $��Right=��� -m   $ CBTypeSolo1��$��0��$��1��$��Уровень: 1��$��13(�$
 TrueY�$��Center=�{��� $ 
CBTypeSolo��$ 	IsChecked��� -n   $ CBTypeSolo2��$��1��$��0��$��Уровень: 2��$��13(�$
 TrueY�$��Center=�{��� $ 
CBTypeSolo��$ 	IsChecked��� -o   $ CBTypeSolo3��$��1��$��1��$��Уровень: 3��$��13(�$
 TrueY�$��Center=�{��� $ 
CBTypeSolo��$ 	IsChecked��� -p   $ CBTypeSolo4��$��2��$��0��$��Уровень: 4��$��13(�$
 TrueY�$��Center=�{��� $ 
CBTypeSolo��$ 	IsChecked��� -q   $ CBTypeSolo5��$��2��$��1��$��Уровень: 5��$��13(�$
 TrueY�$��Center=�{��� $ 
CBTypeSolo��$ 	IsChecked��� $��2��$��15q�$��18(�$ Accent=�{��� $ DGGames��$	 Tag����� $��Рейтинг��#���  $��CharacterEllipsis=�$��Center=�.���� $��0 10 0 0q�.+��� $	��7 0q����� $ SRaitingFrom��$ Value�& �� StringFormat$ & От рейтинга: {0}���� -r   ' ��Name$' SRaitingFrom��( � Minimum$( 0Q�) �� TickFrequency$) 1Q�* � Value$* 0Q�+ �� IsSnapToTickEnabled+ . $	��7 0q�, � Maximum, �� $ 
SRaitingTo��$ Value��� $	��7 0q����� $ 
SRaitingTo��$ Value�$& По рейтинг: {0}���� -s   $' 
SRaitingTo��$, 10Q�$) 1Q�$* 0Q�+ . $	��7 0q�( �� $ SRaitingFrom��$ Value�'	  !MaterialDesignThemes.Wpf.Snackbar	  -t   - 	 Name$- InfoBar��$��1��. 	  MessageQueue. 4
  .MaterialDesignThemes.Wpf.MessageQueueExtension
  