   M S B A M L   `   `   `  ����L  HWpfHelperCopyData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:WpfHelperCopyDataWpfHelperCopyData  5   	   (    "WpfHelperCopyData.MainHelperWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1localclr-namespace:WpfHelperCopyData     5   	   -   5          �Name$
  Main��5   7    � Title$. (Select Data From [A]  ->   Insert to [B]��5   	   $��1078.415��6>   $	��350��6O   $	��250��6_   $
��1000��6o    � Icon$ AP.ico��6�   $	��350��6�   .��5      � ��5   
   �� 5	       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5
      �  � Color$ 	#FFC3C3C3��6    � Offset$ 0.009Q�61   � 5      $ White��6   $ 0.982Q�6-   5      .+�5   
   �� -   5         ��Name$ txtConnA��5      $��23��6$   $��203,60,60,0q�60    �� TextWrapping$
 Wrap=�6E   $.��(Connection String A - insert here please��6Y   $	��Top=�6�   	 � ToolTip$)	 #You can copy form any DataBase ....��6�   .��6�   5      �� 5   
   -   5        $ txtConnB��5      $��23��6$   $��203,134,60,0q�60   $
 Wrap=�6F   $.��(Connection String B - insert here please��6Z   $	��Top=�6�   $:	 4You will be able insert data to the DEV/Test only...��6�   .��6  5      �� 5   
   -   5        
 ��Name$
 connA��5      $
��Left=�6#   $��	10,63,0,0q�6>   $
��Wrap=�6Q   $"��Connection String [A] (From)��6e   $	��Top=�6�   $	��188��6�   {�.  6�   �� 5   
   -   5        $
 connB��5      $
��Left=�6#   $��
10,137,0,0q�6>   $
��Wrap=�6R   $ ��Connection String [B] (To)��6f   $	��Top=�6�   $	��188��6�   {�.  6�   �� 5   
   $M��GPlease Use  DB Connection String According with Selected Data Provider.��6   $��Center=�6b   $��432,0,159,0q�6   $	��Top=�6�   $	��479��6�   {�.  6�   �� 5   
   $��0,178,214,0q�6   $
��Wrap=�6)   $	��Top=�6=   $��90��6U    J� RenderTransformOrigin$ 0.494,1.042&�6a   $��Right=�6�   $	��478��6�   �� 6�   $:�4You can use this process for copy/scramble  data.   ��6�   �� 6�   �� 6�   $H�BA and B can be same if needed  (Application save data in DataSet).��6   �� 6L  �� 6X  $-�'(Integrity data is your Responsibility)��6\  �� 6�  �� 6�  $W�QUse below button multiply times for work with multiplies tables or SET of tables.��6�  �� 6�  �� 6  $3�-Use ";" for divide SQL  Select statements... ��6  6J  �� 5   
   -   5         ��Name$ btnTestA��5      $��Test A��6#   $��	0,38,10,0q�64   $	��Top=�6G   $��Right=�6_   $��45��6{    �� IsCancel . $��45��6�   �� 5   
   -   5        $ btnTestB��5      $��Test B��6#   $��
0,112,10,0q�64   $	��Top=�6H   $��Right=�6`   $��45��6|    . $��46��6�   �� 5   
   -   5        $ btnBringFromA��5      $��[Select] SQL Script For A��6(   $��0,273,435,0q�6L   $5	 /Will Open additional window ... Only SELECT ...��6a   $��Right=�6�   $	��159��6�   $��22��6�   $	��Top=�6�   6�   �� 5   
   -	   5         ��Name$ comboProvidersA��5      $��203,38,60,0q�6,   $	��Top=�6A   $/	 )Db Providers Name installed on your PC...��6Y   6�   �� 5   
   -
   5        $ comboProvidersB��5      $��203,112,60,0q�6,   $	��Top=�6B   $/	 )Db Providers Name installed on your PC...��6Z   6�   �� 5   
   -   5        $
 	txtStatus��5      $
��Wrap=�6'   $��Status��6;   $��23��6I   $��Bottom=�6U   $
��10,0q�6p   5       5!      