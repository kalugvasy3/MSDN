   M S B A M L   `   `   `  ����  WpfHelperCopyData%    WpfHelperCopyData.SelectSqlCode   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$%  SQLcode - Connection String [A]��5   	   $��479.958��61   $��630.833��6B   $	��400��6R   $	��500��6b    � Icon$ AP.ico��6�   .��5      � $��0,0,0,-2q�6   ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      �  � Color$ 	#FFC3C1C1��6    � Offset$ 0Q�61   � 5	      $ White��6   $ 1Q�6-   5
      .+�5   
   �� -   5         ��Name$ txtSQLSelect��5      $����-- Use SELECT (only) SQL code  ...
-- Data Integrity is Your Responsibility...

-- DO NOT USE  "B" Connection String pointed to PRODUCTION ---
-- You use this application for your owner RISK!!!

-- First table PRIMARY next CHILD and CHILD of CHILD ...
-- You MUST [Run (Bring Data - A] 
-- this operation populate DataSet.

SELECT * FROM tableName ;
SELECT * FROM tableName ;
SELECT * FROM tableName ;��6(   $��	0,0,0,115q�6
   �� 	UndoLimit$
 1000��6   �� AcceptsReturn . $��14(�6C  $
��Auto=�6Q  $
��Auto=�6�  
�������6�  �� 5   
   -   5        	 ��Name$	 btnRun��5      $��Run (Bring Data - A)��6!   $
��Left=�6@   $��	24,0,0,76q�6[   $��Bottom=�6n   $	��128��6�   
 � ToolTip$%
 Data will be saved in DataTable��6�   6�   �� 5   
   -   5         ��Name$ chkSpell��5      $��Spell Check��6%   $
��Left=�6;   $��	24,0,0,23q�6V   $��86��6i   $��30��6t   $��Bottom=�6�   6�   �� 5   
   -   5        $	 
btnInsetTo��5      $��Create "Insert SQL"��6%   $��
0,0,156,76q�6M   $��Bottom=�6a   $��Right=�6�   $	��127��6�   �� 5   
   $
��Wrap=�6   $��20��6(   $��Bottom=�64   $��Right=�6O   $	��393��6k   $��	0,0,10,34q�6w   �� 6�   $I�CYou will be able to save and see generated code on next window ....��6�   �� 6�   �� 6�   �� 6�   6  5      5      