   M S B A M L   `   `   `  ����  Auto_File_Transfer_Program+    %Auto_File_Transfer_Program.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   -   5          � Title$  AFTProg��5   	   $	��650��6   $	��805��6&    � WindowStartupLocation$ Manual=�6K    � Left$ 0��6j   
 � Top$ 0��6s   .��5      �  � RowDefinitions 5	   
   �� 5
      $��55 �6   �� 5       � ColumnDefinitions 5   
   �� 5      $��85 �6   �� 5      $	��100 �6   �� 5      $	��100 �6   �� 5      $	��100 �6   �� 5      $	��100 �6   �� 5      $	��100 �6   �� 5      $	��100 �6   �� 5      $	��100 �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0��6.   $��8��6;   $
��Left=�6O   $��Center=�6j   $��10,0,0,0q�6�   .+�5      �� -   5         ��Name$ presentYear��5      $
��2000��6-   $��30(�69   $��Right=�6G   $��Center=�6c   $
��Auto��6~   $
��Auto��6�   �� 5      $	��년��6   $��30(�6!   $��0,0,10,0q�6/   $��Right=�6A   $
��Auto��6^   $
��Auto��6k   �� 5      -   5        $ presentMonth��5      $��1��6.   $��30(�67   $��Right=�6E   $��Center=�6a   $
��Auto��6}   $
��Auto��6�   �� 5      $	��월��6   $��30(�6!   $
��Left=�6/   $
��Auto��6J   $
��Auto��6W   5      �� 5   
   -   5         ��Name$ 
nowSetMode��5      $��80��6%   $��40��60   $��설정��6<   $��0,0,0,0q�6I   $��0��6s   $��3��6�   $��Right=�5       
�������6-   �� 5!   
   -   5        $ addSchedule��5!      $
��Left=�6&   $��80��6A   $��40��6L   $��추가��6X   $��0,0,0,0q�5"      $��0��6D   $��4��6Q   
�������6a   (  "System.Windows.Controls.DatePicker  5$   
   -   5        	  Name$	 ResDatePicker��5$      
������ $��0q�6F   $��0��5%      $��8��6"   $��Right=�66   $��0,0,10,0q�6R   $��Center=�5&      
 � 	Resources
 5'       �� 
TargetType �� Property �� Value��%R   '��       �� 5(      " ��6   .�5)      ��  � ��$ 20��6.   5*      5+      5,      �� 5.   
   $
��Auto=�6   $
��Auto=�6:   $��1��6_   $��8��6l   .��5/      � -   5         �Name$ myGrid��5/      $��Center=�6$   $��Center=�6A   
 50      ��%$  '��       '�� m     �� 51      " ��6   .�52      ��  � ��$ 1q�6<   �� 53       � ��
 �   �68   54      �� 55      " ��6   .�56      ��  � ��$	 100��62   �� 57       � ��$ 20��63   �� 58       �@��TextAlignment$ Center=�6:   59      5:       5;      �� 5<      �� 5=      �� 5>      �� 5?      �� 5@      �� 5A      �� 5B      �� 5C      �� 5D      �� 5E      �� 5F      �� 5G      �� 5H      �� 5I      �� 5J      �� 5K      �� 5L      �� 5M      �� 5N      �� 5O      �� 5P      �� 5Q      �� 5R      �� 5S      �� 5T       5V      �� 5W      $��85 �6'   �� 5X      $	��100 �6'   �� 5Y      $	��100 �6'   �� 5Z      $	��100 �6'   �� 5[      $	��100 �6'   �� 5\      $	��100 �6'   �� 5]      $	��100 �6'   �� 5^      $	��100 �6'   .+�5`      �� $��0��6   $��0��6&   ._�5a      �� $��시간��6    $��85��6*    �� TextAlignment$ Center=�65   5b      �� 5c      $��0��6   $��1��6&   ._�5d      �� -   5        $ colName0��5d       $��
1일 (일)��62   5e      �� 5f      $��0��6   $��2��6&   ._�5g      �� -	   5        $ colName1��5g       $��
2일 (월)��62   5h      �� 5i      $��0��6   $��3��6&   ._�5j      �� -
   5        $ colName2��5j       $��
3일 (화)��62   5k      �� 5l      $��0��6   $��4��6&   ._�5m      �� -   5        $ colName3��5m       $��
4일 (수)��62   5n      �� 5o      $��0��6   $��5��6&   ._�5p      �� -   5        $ colName4��5p       $��
5일 (목)��62   5q      �� 5r      $��0��6   $��6��6&   ._�5s      �� -   5        $ colName5��5s       $��
6일 (금)��62   5t      �� 5u      $��0��6   $��7��6&   ._�5v      �� -   5        $ colName6��5v       $��
7일 (토)��62   5w      �� 5y      $��1��6   $��0��6&   ._�5z      �� $
��0시��6    $
��Auto��6*   5{      �� 5|      $��2��6   $��0��6&   ._�5}      �� $
��1시��6    $
��Auto��6*   5~      �� 5      $��3��6   $��0��6&   ._�5�      �� $
��2시��6    $
��Auto��6*   5�      �� 5�      $��4��6   $��0��6&   ._�5�      �� $
��3시��6    $
��Auto��6*   5�      �� 5�      $��5��6   $��0��6&   ._�5�      �� $
��4시��6    $
��Auto��6*   5�      �� 5�      $��6��6   $��0��6&   ._�5�      �� $
��5시��6    $
��Auto��6*   5�      �� 5�      $��7��6   $��0��6&   ._�5�      �� $
��6시��6    $
��Auto��6*   5�      �� 5�      $��8��6   $��0��6&   ._�5�      �� $
��7시��6    $
��Auto��6*   5�      �� 5�      $��9��6   $��0��6&   ._�5�      �� $
��8시��6    $
��Auto��6*   5�      �� 5�      $��10��6   $��0��6'   ._�5�      �� $
��9시��6    $
��Auto��6*   5�      �� 5�      $��11��6   $��0��6'   ._�5�      �� $��10시��6    $
��Auto��6+   5�      �� 5�      $��12��6   $��0��6'   ._�5�      �� $��11시��6    $
��Auto��6+   5�      �� 5�      $��13��6   $��0��6'   ._�5�      �� $��12시��6    $
��Auto��6+   5�      �� 5�      $��14��6   $��0��6'   ._�5�      �� $��13시��6    $
��Auto��6+   5�      �� 5�      $��15��6   $��0��6'   ._�5�      �� $��14시��6    $
��Auto��6+   5�      �� 5�      $��16��6   $��0��6'   ._�5�      �� $��15시��6    $
��Auto��6+   5�      �� 5�      $��17��6   $��0��6'   ._�5�      �� $��16시��6    $
��Auto��6+   5�      �� 5�      $��18��6   $��0��6'   ._�5�      �� $��17시��6    $
��Auto��6+   5�      �� 5�      $��19��6   $��0��6'   ._�5�      �� $��18시��6    $
��Auto��6+   5�      �� 5�      $��20��6   $��0��6'   ._�5�      �� $��19시��6    $
��Auto��6+   5�      �� 5�      $��21��6   $��0��6'   ._�5�      �� $��20시��6    $
��Auto��6+   5�      �� 5�      $��22��6   $��0��6'   ._�5�      �� $��21시��6    $
��Auto��6+   5�      �� 5�      $��23��6   $��0��6'   ._�5�      �� $��22시��6    $
��Auto��6+   5�      �� 5�      $��24��6   $��0��6'   ._�5�      �� $��23시��6    $
��Auto��6+   5�      5�      5�      5�      5�      