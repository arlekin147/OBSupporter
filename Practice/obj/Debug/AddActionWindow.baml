   M S B A M L   `   `   `  ����C  ?Practice, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:PracticePractice  5   	       Practice.AddActionWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Practice     5   	     � Title$  	AddAction��5   	    � SizeToContent$ WidthAndHeight=�6   $	��200��6:    � 
ResizeMode$ NoResize=�6G   ���	#FF555555.��5	      � $��
10,10,10,0q�6   $	��140��6   $	��Top=�6,    � RowDefinitions 5
   
   �� 5      $��20 �6   �� 5      $��20 �6   �� 5      $
��auto �6   .+�5   
   �� $��0,0,0,5q�6   �� 6&   $�Input a ��6*   �� 6<   $�	shortcut ��6@   �� 6S   $�of the action��6W   6o   �� 5   
   -   5         ��Name$ ShortcutBox��5      $��1��6'   $	��250��64   $��Digit��6C   � 5   
   $��2��6   $��Right=�6!   $��
0,72,0,-57q�6=   .+�5      �� -   5         �� 	IsDefault . $��60��5   ;   $��0,0,10,0q�6I   .��6[   OK6_   �� 5      -   5         �� IsCancel . $��60��5   :   $��Cancel��6H   5      �� 5   
   $ ��Input a text of the action��6   $��0,1,0,14q�66   $��2��6H   �� 5   
   -   5        $ 	ActionBox��5      $��2��6%   $	��250��62   $
��Text��6A   $��
0,27,0,-12q�6M   5      5      