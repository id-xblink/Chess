   M S B A M L   `   `   `  ����   Wpf2p2p    Wpf2p2p.CheckerboardUC   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  :d2http://schemas.microsoft.com/expression/blend/2008  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006        MaterialDesignBody#���     MaterialDesignPaper#���  $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�.���   PrimaryHueMidBrush#���    � RowDefinitions  �� $
��auto ��� .+��� $��0��$6��0Настройка шахматной доски��$��15q�$��30(�$��Center=�   PrimaryHueMidForegroundBrush#���  � $��1��#���   � ColumnDefinitions �� �� ����  �� ElementName$ GChess�� �� Path$ ActualHeight�.+�� $��0�� �� $
��auto ��� .+�9� $��0�� 9� ExpandDirection$ Right=�����  J� RenderTransformOrigin$ .5,.5&�$D��>Выбор шахматных клеток и маркеров��$��16(� � LayoutTransform ��  �� Angle$ 90Q�.���   �� �� �� �� �� .+�� $��0��$��10q�  �� $
��auto ��� .+��� $��0��$��2��$��Белая клетка��$��5q�$��20(�$��Center=�$��	Underline{��� -   
 � Tag$ White��$��1��$��0q�  MaterialDesignFlatButton#���  $
��auto��{��� $ BSelectedIcon��$	 Tag�.��'  !MaterialDesignThemes.Wpf.PackIcon  	   Kind$	 SquareOutline=�$
��auto��$
��auto��� $��1��$��10q�  �� $
��auto ��� .+��� $��0��$��2��$��Чёрная клетка��$��5q�$��20(�$��Center=�$��	Underline{��� -   $ Black��$��1��$��0q�#���  $
��auto��{��� $ BSelectedIcon��$	 Tag�.��  $	 Square=�$
��auto��$
��auto��� $��2��$��10q�  �� $
��auto ��� .+��� $��0��$��2��$��Движение��$��5q�$��20(�$��Center=�$��	Underline{��� -   $
 Move��$��1��$��0q�#���  $
��auto��{��� $ BSelectedIcon��$	 Tag�.��  $	 ArrowAll=�$
��auto��$
��auto��� $��3��$��10q�  �� $
��auto ��� .+��� $��0��$��2��$��
Атака��$��5q�$��20(�$��Center=�$��	Underline{��� -   $ Attack��$��1��$��0q�#���  $
��auto��{��� $ BSelectedIcon��$	 Tag�.��  $	 Sword=�$
��auto��$
��auto��� $��4��$��10q�  �� $
��auto ��� .+��� $��0��$��2��$��Шах��$��5q�$��20(�$��Center=�$��	Underline{��� -   $ Check��$��1��$��0q�#���  $
��auto��{��� $ BSelectedIcon��$	 Tag�.��  $	 ShieldOutline=�$
��auto��$
��auto��� $��1��  �� $
��auto ��� �� $
��auto ��� $
��auto �.+��� $��0��$��2��$4��.Настройка цвета элемента��$��5q�$��24(�$��Center=�$��	Underline{�*  $MaterialDesignThemes.Wpf.ColorPicker  -   
  Name$
 	CPElement��$��1��$��0��$
��20 0q�   Color$ #808080��� $��2��$��5q�  �� $
��auto ��� $
��auto ��� .+��� $��0��$��2��$��Выбранный��$��5 5 5 0q�$��20(�$��Center=�$
��Wrap=��� $��1��$��2��$��элемент��$��5 0 5 5q�$��20(�$��Center=�$
��Wrap=��� -    ��Name$ BSelectedIcon��$��2��$��0q�#���  $	��100��$	��100��{�.  $
 True������  �� Color �� $ 	CPElement��$ Color�.��  -     Name$ 
PISelected��$	 Close=�$
��auto��$
��auto��� $��3�� �� �� .+��� -	   $ BCancel��$��0��$��10q�$��Отмена��{��� $ BSelectedIcon��$ 	IsEnabled��� -
   $ BSave��$��1��$��10q�$��Сохранить��{��� $ BSelectedIcon��$ 	IsEnabled�� -    �Name$ GChess��$��1��.+�� -   $ GBoard��$��1��$��25q�'  !MaterialDesignThemes.Wpf.Snackbar  -     Name$ InfoBar��$��1��$��2��   MessageQueue 4  .MaterialDesignThemes.Wpf.MessageQueueExtension  