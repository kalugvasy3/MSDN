   M S B A M L   `   `   `  ����L  HWpfHelperCopyData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:WpfHelperCopyDataWpfHelperCopyData  5   	   (    "WpfHelperCopyData.MainHelperWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1localclr-namespace:WpfHelperCopyData     5   	   -   5          �Name$
  Main��5   7    � Title$. (Select Data From [A]  ->   Insert to [B]��5   	   $	��220��6=   $��1078.415��6J   $	��250��6[   $	��250��6k   $
��1000��6{    � Icon$ AP.ico��6�   .��5      � $��2,0,2,-2q�6   ��5   
   �� 5	       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5
      �  � Color$ 	#FFC3C3C3��6    � Offset$ 0.009Q�61   � 5      $ White��6   $ 0.982Q�6-   5      .+�5   
   �� -   5         ��Name$ txtConnA��5      $��23��6$   $��203,36,60,0q�60    �� TextWrapping$
 Wrap=�6E   $���}Provider=IBMDADB2; Database=**********; Hostname=**********; Protocol=TCPIP; Port=**********; Uid=**********; Pwd=**********;��6Y   $	��Top=�6�   	 � ToolTip$)	 #You can copy form any DataBase ....��6�   .��6J  ��5      �� 5      $ 0.5,1&�6&   $ 0.5,0&�67   .��5      � $ 	#FFCBCACA��6#   $ 0Q�65   � 5      $ 	#FFF9F8F8��6#   $ 1Q�65   5      .��5   "   5      �� 5   
   -   5        $ txtConnB��5      $��23��6$   $��203,57,60,0q�60   $
 Wrap=�6E   $���}Provider=IBMDADB2; Database=**********; Hostname=**********; Protocol=TCPIP; Port=**********; Uid=**********; Pwd=**********;��6Y   $	��Top=�6�   $:	 4You will be able insert data to the DEV/Test only...��6�   .��6Z  ��5      �� 5      $ 0.5,1&�6&   $ 0.5,0&�67   .��5      � $ 	#FFC9C9C9��6#   $ 0Q�65   � 5      $ White��6#   $ 1Q�61   5      .��5   "   5      �� 5   
   -   5        
 ��Name$
 connA��5      $
��Left=�6#   $��	10,39,0,0q�6>   $
��Wrap=�6Q   $"��Connection String [A] (From)��6e   $	��Top=�6�   $	��188��6�   {�.  6�   �� 5   
   -   5        $
 connB��5      $
��Left=�6#   $��	10,60,0,0q�6>   $
��Wrap=�6Q   $ ��Connection String [B] (To)��6e   $	��Top=�6�   $	��188��6�   {�.  6�   �� 5!   
   $)��#Please Use OLE DB Connection String��6   $��Center=�6>   $��406,0,383,0q�6[   $	��Top=�6p   $	��229��6�   �� 5"   
   $��
0,85,206,0q�6   $
��Wrap=�6(   $	��Top=�6<   $��90��6T    J� RenderTransformOrigin$ 0.494,1.042&�6`   $��Right=�6�   $	��478��6�   �� 6�   $:�4You can use this process for copy/scramble  data.   ��6�   �� 6�   �� 6�   $H�BA and B can be same if needed  (Application save data in DataSet).��6�   �� 6K  �� 6W  $-�'(Integrity data is your Responsibility)��6[  �� 6�  �� 6�  $W�QUse below button multiply times for work with multiplies tables or SET of tables.��6�  �� 6�  �� 6  $3�-Use ";" for divide SQL  Select statements... ��6  6I  �� 5$   
   -   5         ��Name$ btnTestA��5$      $��Test A��6#   $��	0,33,10,0q�64   $	��Top=�6G   $��Right=�6_   $��45��6{    �� IsCancel . 6�   �� 5%   
   -   5        $ btnTestB��5%      $��Test B��6#   $��	0,59,10,0q�64   $	��Top=�6G   $��Right=�6_   $��45��6{    . 6�   �� 5'   
   -   5        $ btnBringFromA��5'      $��[Select] SQL Script For A��6(   $��0,180,428,0q�6L   $5	 /Will Open additional window ... Only SELECT ...��6a   $��Right=�6�   $	��159��6�   $��22��6�   $	��Top=�6�   6�   5)      5*      