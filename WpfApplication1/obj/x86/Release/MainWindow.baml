   M S B A M L   `   `   `  ����   WpfApplication1     WpfApplication1.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006       � Title$  
HebusSniff�� � Icon$. (/WpfApplication1;component/Images/45.jpg�� � AllowsTransparency .   � WindowStartupLocation$ CenterOwner=� � Topmost .   � WindowStyle$ ThreeDBorderWindow=� � 
ResizeMode$ CanResizeWithGrip=�#��["��$��1q� � WindowState$ Normal=�$	��646��$	��988��.��� -   $��Stretch=�$��Stretch=� J� OpacityMask Z� 	 � RowDefinitions	 �� $
��250* ��� $	��86* ��� $
��275* ��� $
��Auto �
 � ColumnDefinitions
 �� $
��199* ��� $
��765* �.+��� -   $��textBlockLimit��$��12,66,82,157q�$��Télécharger��$	��105��$��15(�$
��Left=�$	��Top=��� -   $��buttonGo��$
��GO !��$��67��$
��Left=�$��
12,185,0,0q�$	��Top=�$	��181��$��15(���.   J� IsManipulationEnabled .  }�. $��2������  �� UpdateSourceTrigger$ Explicit=� �� IsAsync . �� -   $��textBlockState��$��
12,219,0,0q�$��Etat : Inactif��$	��186��$��22(�$��Italic$�$	��Top=�$
��Left=� �� TextEffects z�  z� 
Foreground �	#FFDE1414 z� PositionCount$ 4���� -   $��textBlockCopied��$��12,2,0,0q�$��Images copiées: ��$��15(�$	��105��$��2��$
��Left=�$	��Top=��� -   $��
textBlock3��$��23��$
��Left=�$��	12,37,0,0q�$��Commencer à��$	��Top=�$	��105��$��15(��� -   $��textBoxDebut��$��23��$
��Left=�$��
122,37,0,0q�$	��Top=�$��75��$��15(�$�� ���� -   $��textBlockVarNB��$�� ��$��69��$��18(�$��WordEllipsis=�$��WrapWithOverflow=�$��	130,3,0,0q�$��25��$��2��$
��Left=�$	��Top=��� -	   $��textBlockInfos��$�� ��$��	12,11,0,0q�$
��Bold"�$��Italic$�$��10(�$��25��$
��Left=�$	��Top=�$��2��$	��188��$��1���� -
   $��textBlockDuree��$��15(�$��23��$��	12,42,0,0q�$�� ��$	��186��$	��Top=�$
��Left=�$��1���� -   $��button1��$��Voir la liste��$��30��$
��Left=�$��
12,149,0,0q�$	��Top=�$	��181��{�.  $��2��(  "System.Windows.Controls.WebBrowser  -   $��webBrowser1��$��LeftToRight=�$��1��$��4��$��0,3,0,-3q� �� �� -   $��btPrec��$��Précédent��$��23��$
��Left=�$��	12,48,0,0q�$	��Top=�$��74��{�.  $��2���� -   $��btSuiv��$��Suivant��$��23��$
��Left=�$��
122,48,0,0q�$��75��{�.  $��2��$	��Top=��� -   $��tbPage��$��23��$
��Left=�$��
122,83,0,0q�$	��Top=�$��32��$��1�� �� 
AcceptsTab .  �� AcceptsReturn .  {�.   �� TextAlignment$ Center=�$��2���� -   $��btVoir��$��Accéder à la page :��$��23��$��	12,82,0,0q�$	��105��{�.  $��2��$
��Left=�$	��Top=��� -   $��	tbImgPage��$��23��$��
12,116,0,0q�$��32��$��9��$��2��$
��Left=�$	��Top=��� -   $��lNbImagesPage��$��images par page :��$��28��$��
49,116,0,0q�$	��105��$��2��$
��Left=�$	��Top=��� -   $��	btImgPage��$��OK��$��23��$��160,118,0,0q�$��33��{�.  $��2��$
��Left=�$	��Top=��� -   $��cbRewriteFile��$��Ajouter��$��20��$��
11,160,0,0q�$y�Visible=� d� 	IsChecked$
 TrueY�$
��Left=�$	��Top=�$��74���� -   $��	comboBox1��$��23��$
��Left=�$��	12,12,0,0q�$	��Top=�$	��186��.@��� $��Hebus�� �� 
IsSelected . �� $��Steffi��{�.  $y�Hidden=�� -   $��progressBar1��$��31��$��
10,108,0,0q�$	��186��$	��Top=�$
��Left=��� -   $��comboBoxLimit��$��29��$��
122,63,0,0q�$��75��$��Center=�$��Center=�$
��Left=�$	��Top=�.@��� $��1���� $��10�� . �� $��50���� $	��100���� $	��250���� $	��500���� $
��1000���� $
��2500���� $
��5000���� $��10000���� $��25000���� $��50000���� $��100000���� $��250000���� $��500000���� $��999999���� -   $��lPage��$��2��$��24��$��
160,82,0,0q�$��37��$��Center=�$��Center=�$
��Left=�$	��Top=��� -   $��cbNom��$#��Récupérer les noms (Beta !)��$��20��$
��Left=�$ FalseY�$��
11,186,0,0q�$	��Top=�$y�Visible=�$	��186������  �� ImageSource$9 3/WpfApplication1;component/Images/009%20%282%29.jpg��