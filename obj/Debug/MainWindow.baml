   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	   c _GMap.NET.WindowsPresentation, Version=1.7.5.0, Culture=neutral, PublicKeyToken=b85b9027b614afefqPclr-namespace:GMap.NET.WindowsPresentation;assembly=GMap.NET.WindowsPresentationGMap.NET.WindowsPresentation 5   	       WpfApp1.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	   pWindowsPresentationPclr-namespace:GMap.NET.WindowsPresentation;assembly=GMap.NET.WindowsPresentation     5   	     � Title$  
MainWindow��5	   	   $	��450��6   $	��800��6)   .��5
      � -   5        .+�5   
   .  (GMap.NET.WindowsPresentation.GMapControl  -   5          Name$	 Map��5   *   $
��Left=�67   $	��310��6R   $��
216,99,0,0q�6_   $	��Top=�6s   $	��566��6�   6�   �� 5   
   $��Cursor��6   $
��Left=�6#   $��83��6>   $��	26,27,0,0q�6J   $	��Top=�6]   $	��119��6u   .��5      �� .+�5      	� -   5         	�Name$
 near��5      $��3q�6,   .��67   Nearest Object6G   	� 5      -   5        $ create��5      $��3q�6.   .��69   Create Point6G   	� 5      $��3q�6   .��6)   Drag6/   5      5      �� 5   
   $	��Add��6   $
��Left=�6    $	��110��6;   $��
26,139,0,0q�6H   $	��Top=�6\   $	��119��6t   .��5      �� .+�5      �� -   5         ��Name$ name_box��5      �� 5      -   5         ��Name$ obj_list��5      .@�5      �� -   5         ��Name$ obj_area��5   #   $
��Area��65   �� 5      -   5        $ 	obj_human��5   #   $��Human��66   �� 5      -	   5        $ obj_car��5   #   $	��Car��64   �� 5      -
   5        $ 	obj_route��5   #   $��Route��66   5      �� 5      -   5         ��Name$ Add_but��5      $	��Add��6*   68   �� 5      -   5        $ 	Clear_but��5      $��Clear Points��6,   6C   5      5       �� 5!   
   $
��Find��6   $
��Left=�6!   $��65��6<   $��
26,280,0,0q�6H   $	��Top=�6\   $	��119��6t   .��5"      �� .+�5#      �� -   5        $ find_box��5#      �� 5$      -   5        $ find_but��5$      $
��Find��6+   6:   5%      5&      �� 5'   
   $��Order��6   $
��Left=�6"   $��67��6=   $��
216,13,0,0q�6I   $	��Top=�6]   $	��119��6u   .��5(      �� .+�5)      �� -   5        $ hum_list��5)      5*      �� 5+      -   5        $ 	order_but��5+      $��
Get a taxi��6,   6A   5,      5-      � 5.   
   -   5         �Name$ taxi_progress��5.      $
��Left=�6-   $��22��6H   $��
455,36,0,0q�6T   $	��Top=�6h   $	��251��6�   ���	#FFE6E6E6$y�Hidden=�6�   50      51      