   M S B A M L   `   `   `  ����B  >Wpf2p2p, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:Wpf2p2pWpf2p2p  5   	       Wpf2p2p.EnterW   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=null:md1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   :d2http://schemas.microsoft.com/expression/blend/2008  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Wpf2p2p     5   	   -   5          �Name$  Window��5	   	    � Title$
 2p2p��6   $	��600��6&   $	��800��63    � WindowStartupLocation$ CenterScreen=�6?    � WindowStyle$
 None=�6d      MaterialDesignBody#���   5
   	     MaterialDesignPaper#���  5   	   $X��Rpack://application:,,,/MaterialDesignThemes.Wpf;component/Resources/Roboto/#Roboto)�5   	   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
      Wpf2p2p.ControlUC  -   5          Name$ ControlPanel��5   '   $��0��6   6'   �� 5   
   $��1��6     
Background#���  6   $��UniformToFill=�6C   �� 5   
   $��1��6    �� CornerRadius$ 15��6   $��2q�60   $	��350��6D   $	��400��6P   #���  5      #���   6D    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 50Q�6#   	   ShadowDepth$	 0Q�63   
   Color$
 
DarkOrchid��6C   ._�5      � $��20q�6    5      �� 5      $
��auto �6$   �� 5      $��* �6$   �� 5       $
��auto �6$   �� 5!      $
��auto �6$   .+�5$      �� $��4��6   $	��-75q�6*   $ 5��67   $��80��6H   $��80��6S   $	��Top=�6_     PrimaryHueLightBrush#���  5%      #���   6M   $��2q�6�   ._�5&      ��   dn#���  6   5'      �� 5)      -   5         ��Name$ TBTitle��5)   )   $��0��6   $��Авторизация��6:   $
��Bold"�6M   $��22(�6_   $��Center=�5*      $��Center=�64   $��0 15 0 0q�6Q   �� 5,      $��1��6   $	��5 0q�6*   $��Center=�67   .+�5-      �� .+�5.      �� -   5         ��Name$ TBLogin��5.   "   $��14(�63   $	��0 5q�6A   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 
Логин��5/   !    % !MaterialDesignFloatingHintTextBox#��[ 6<    �� 	MaxLength$ 20��6w   G� 50      -   5         G�Name$ PBPass��50   &   $��14(�66   $	��0 5q�6D   $ Пароль��51   !    ) %MaterialDesignFloatingHintPasswordBox#��[ 6=    G� 	MaxLength$ 20��6|   G� 52      -   5        $ 	PBRepPass��52   &   $y�	Collapsed=�69   $��14(�6P   $	��0 5q�6^   $ Подтвердить��53   !   #��[ 6B   $ 20��6�   54      �� 55      -   5         ��Name$ CBRegion��55   ?   $y�	Collapsed=�6Q   $��14(�6h   $	��0 5q�6v   $ Регион��56       & "MaterialDesignFloatingHintComboBox#��[ 69   ��55      �� *	Regions+��57      �� 58      .R�59   "   �� ��6,   ��  �� Path$
 Name�5:      5<      �� 5>      -   5         ��Name$ 
CBRemember��5>      $��Запомнить��63   $	��Top=�6G   $��12(�6f   $��0 15 0 5q�6t   5?      �� 5A      -	   5         ��Name$	 BGo��5A   &   $��2��6   $��Продолжить��63   6H   �� 5C      -
   5        $ TBSwap��5C      
 � Tag$
 True��6,   $��3��67   $��12(�6D   $��Center=�6R   $��Center=�6m   $��
0 10 0 -10q�6�   �� 5D      -   5        $ TBChange��5D       $��Нет аккаунта?��62    �� 5E      -   5          SecondaryAccentBrush#���  5E   8   .�6l   .�5F      �� -   5        $ HLChange��5F   $   $)��#Зарегистрируйтесь!��66   5G      5H      5I      5J      '  !MaterialDesignThemes.Wpf.Snackbar  5L   
   -   5          Name$ InfoBar��5L   #   $��1��6      MessageQueue 64   4  .MaterialDesignThemes.Wpf.MessageQueueExtension  5M      5N      