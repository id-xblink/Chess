   M S B A M L   `   `   `  ���� B  >Wpf2p2p, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:Wpf2p2pWpf2p2p      Wpf2p2p.EnterW   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  :d2http://schemas.microsoft.com/expression/blend/2008  R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:Wpf2p2p     -     �Name$  Window�� � Title$
 2p2p��$	��600��$	��800�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=�   MaterialDesignBody#���     MaterialDesignPaper#���  $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�.���  � RowDefinitions �� $
��auto ��� $��* �.+�   Wpf2p2p.ControlUC  -     Name$ ControlPanel��$��0���� $��1��  
Background#���  $��UniformToFill=��� $��1�� �� CornerRadius$ 15��$��2q�$	��350��$	��400��#���  #���    J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     
BlurRadius$ 50Q�	   ShadowDepth$	 0Q�
   Color$
 
DarkOrchid��._�� $��20q� �� $
��auto ��� $��* ��� $
��auto ��� $
��auto �.+��� $��4��$	��-75q�$ 5��$��80��$��80��$	��Top=�  PrimaryHueLightBrush#���  #���   $��2q�._���   dn#���  �� -    ��Name$ TBTitle��$��0��$��Авторизация��$
��Bold"�$��22(�$��Center=�$��Center=�$��0 15 0 0q��� $��1��$	��5 0q�$��Center=�.+��� .+��� -    ��Name$ TBLogin��$��14(�$	��0 5q�)  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 
Логин�� % !MaterialDesignFloatingHintTextBox#��[  �� 	MaxLength$ 20��G� -    G�Name$ PBPass��$��14(�$	��0 5q�$ Пароль�� ) %MaterialDesignFloatingHintPasswordBox#��[  G� 	MaxLength$ 20��G� -   $ 	PBRepPass��$y�	Collapsed=�$��14(�$	��0 5q�$ Подтвердить��#��[ $ 20���� -    ��Name$ CBRegion��$y�	Collapsed=�$��14(�$	��0 5q�$ Регион�� & "MaterialDesignFloatingHintComboBox#��[ ���� *	Regions+���� .R��� ����  �� Path$
 Name��� -    ��Name$ 
CBRemember��$��Запомнить��$	��Top=�$��12(�$��0 15 0 5q��� -	    ��Name$	 BGo��$��2��$��Продолжить���� -
   $ TBSwap��
 � Tag$
 True��$��3��$��12(�$��Center=�$��Center=�$��
0 10 0 -10q��� -   $ TBChange��$��Нет аккаунта?�� �� -     SecondaryAccentBrush#���  .�.��� -   $ HLChange��$)��#Зарегистрируйтесь!��'  !MaterialDesignThemes.Wpf.Snackbar  -     Name$ InfoBar��$��1��   MessageQueue 4  .MaterialDesignThemes.Wpf.MessageQueueExtension  