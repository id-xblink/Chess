   M S B A M L   `   `   `  ���� B  >Wpf2p2p, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:Wpf2p2pWpf2p2p      Wpf2p2p.DashboardW   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  :d2http://schemas.microsoft.com/expression/blend/2008  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:Wpf2p2p     -     � Title$
  2p2p��$	��720��$
��1080�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=�   MaterialDesignBody#���     MaterialDesignPaper#���  $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�.��� #���   � RowDefinitions �� $
��auto ��� $
��auto ��� $��* �.+�   Wpf2p2p.ControlUC  -     Name$ ControlPanel��$��0��� $��1��  PrimaryHueLightBrush#���   � ColumnDefinitions �� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $
��auto ��� $��* ��� $
��auto ��� $
��auto ��� $
��auto �.+�'  !MaterialDesignThemes.Wpf.PopupBox  -   $��0q�$��40��$��40��$��0��$��5q�   	StaysOpen .    ToggleContent '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ GearOutline=�  PrimaryHueDarkBrush#���  $
��auto��$
��auto��	   PopupContent.	 � $	��300�� �� $
��auto ��� �� $
��auto �.+��� $��0��$��Настройки��$��0 15 0 0q�$��20(�$��Center=�� $��1��$��10q� �� $
��auto ��� .+��� $��0��.+��� -   
 ��Name$
 
BPlaySound��$��0q�$��30��$��30��$	��Top=�  SecondaryAccentBrush#���  .��  $ 	MusicNote=�$
��auto��$
��auto���� -    ��Name$ SVolume�� �� TickFrequency$ 10Q� �� TickPlacement$ BottomRight=� �� Orientation$ Vertical=� � Value$	 100Q� � Minimum$ 0Q� � Maximum$	 100Q�$	��100�� �� IsSnapToTickEnabled . � $��1�� �� $
��auto ��� $��* ��� $
��auto �.+��� -    ��Name$ 
CBAutoSave��$��0��$0��*Сохранять позицию окна��$��14(�$��Center=�� $��1�� �� �� �� $
��auto �.+��� $��0��$'��!Приоритетный цвет��$��14(�$��CharacterEllipsis=�$
��Wrap=�$��Bottom=�$��Center=��� $��1�� �� Orientation$ 
Horizontal=�$��Center=�.+�	� -    	�Name$ DCWhite�� 	� 	GroupName$ DuoColor��$��
Белый��$��12(� d� 	IsChecked$
 trueY�$��0 0 15 0q�	� -   $ DCBlack��$ DuoColor��$��Чёрный��$��12(�$��15 0 0 0q��� $��2��$��1��  PrimaryHueMidBrush#���  $
��15 0q��� -	   $
 BPopupFactory��$��2��$0��*Настройки по умолчанию��$��16(�$��Center=�$
��10 0q�
���  ��#���  � $��2�� �� �� .+��� -
   $
 BPopupCancel��$��0��$��Закрыть��$��16(�$��Center=�   ClosePopupCommand#��Z #���   �� -   $
 
BPopupSave��$��1��$��Сохранить��$��16(�$��Center=�#��Z #���  �� -   $
 BHome��$��22(�$��70��$��70��$��0q�+  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth0=�
 � Tag$
 Home��$��1��$��0q�.����   dn#���  $
��auto��$
��auto���� -   $
 Game��$��2��$��Игра��#���    MaterialDesignFlatButton#���  $��22(�$
��auto��$��1 1 1 2q��� -   $ Profile��$��3��$��Профиль��#���  #���  $��22(�$
��auto��$��1 1 1 2q��� -   $ 
Statistics��$��4��$��Статистика��#���  #���  $��22(�$
��auto��$��1 1 1 2q��� -   $ Overview��$��5��$��Просмотр��#���  #���  $��22(�$
��auto��$��1 1 1 2q��� -   $ Leaders��$��6��$��Лидеры��#���  #���  $��22(�$
��auto��$��1 1 1 2q��� -   $ Checkerboard��$��8��$��0q�#���  #���  $��40��$��40��$��5q�.��  $ Checkerboard=�$
��auto��$
��auto���� -   $ Pieces��$��9��$��0q�#���  #���  $��40��$��40��$��5q�.��  $ 	ChessKing=�$
��auto��$
��auto���� -   $ Power��$��10��$��0q�#���  #���  $��40��$��40��$��5q�.��  $ Power=�$
��auto��$
��auto��� -    �Name$ GArea��$��2��'  !MaterialDesignThemes.Wpf.Snackbar  -     Name$ InfoBar��$��2��   MessageQueue 4  .MaterialDesignThemes.Wpf.MessageQueueExtension  