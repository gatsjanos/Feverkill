   M S B A M L   `   `   `  ���� D  @Szelcsend, Version=0.8.0.1, Culture=neutral, PublicKeyToken=null%clr-namespace:Attekinto	Attekinto  [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms     Attekinto.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:Attekinto     RForms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     -     �Name$  MainWindow1�� � Title$ 
Szélcsend��$	��760��$	��943�� � AllowsTransparency .  � WindowStyle$
 None=� � 
ResizeMode$ NoResize=���Z� ��Z�  � DataContext ��  �� RelativeSource �� *Self+.��� -    �Name$ gridBackgnd��$
��Left=�$	��Top=�$	��943��.+��� -    ��Name$ imageBackgnd��$	��729��$
��Fill=�$��Rescources/hatter.png��$	��839��$
��Left=�$	��Top=��� -   	 ��Name$	 button��$��Button��$
��Left=�$��839,185,0,0q�$	��Top=�$��52��$	��222���� -   $ 	imageHelp��$��831,0,48,712q�$��Rescources/HelpA.png��
 � ToolTip$
 
Segítség�� J� IsHitTestVisible . �� -   $ 	imageHide��$��839,106,57,610q�$��Rescources/HideA.png��$
 	Elrejtés���� -   $ 
imageFoabl��$��830,50,48,660q�$��Rescources/FoablA.png��$
 Haladó Ablak megnyitása���� -    ��Name$ labelCim��$
��Left=�$��61.4,33.4,0,0q�$	��Top=�$��30.667(�$��Arial)�$
��Bold"� J� RenderTransformOrigin$ 0.5,0.5&����� *UICim+��Z� z�V� .~��� �� ��  �� Angle$
 0.28Q�T� ����  �� EndPoint$ 0.5,1&� � MappingMode$ RelativeToBoundingBox=� �� 
StartPoint$ 0.5,0&� �� RelativeTransform V� .~���  �� CenterY$	 0.5Q� �� CenterX$	 0.5Q���  �� CenterY$	 0.5Q� �� CenterX$	 0.5Q��� $	 -90Q� �� CenterY$	 0.5Q� �� CenterX$	 0.5Q�T� .���  � Color$ Black�� � Offset$ 1Q�� $	 Red���� -	   $ imageVillFordsz��$��447,141,346,518q�$��Rescources/VillanyA.png��$
 Klikk�� � Cursor$
 Hand���� -
   $ imageVillSzabl��$��422,327,371,332q�$��Rescources/VillanyA.png��$
 Klikk��$
 Hand���� -   $ imageVillRiaszt��$��678,505,115,155q�$��Rescources/VillanyA.png��$
 Klikk��$
 Hand���� -    ��Name$ listViewRiaszt��$
��Left=�$	��128��$��254,553,0,0q�$	��485��   
HatterSzin#���     
EloterSzin#���  $ �� IsSynchronizedWithCurrentItem$
 TrueY�$	��Top=�$��Hidden=�$��Hidden=���.  $��0q�$��Segoe UI Black)�$	��Top=���Z�  J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     ShadowDepth$ 7Q�    
BlurRadius$  3Q�!   Color$! 	#FF660000��"   RenderingBias$" Quality=�# �� ItemBindingGroup# &   System.Windows.Data.BindingGroup  $ �� View$ ��   myHeaderStyle!% �� ColumnHeaderContainerStyle#% �  .P��� & �� Width$	& 160������ *LVRHoszenzor+' �� DisplayMemberBinding' �� *Homero+�� $& 20������ *LVRRel+' �� *	Muvelet+�� $& 30������ *LVRFok+' �� *
RiasztPont+�� $	& 140������ *	LVRUzenet+' �� *Uzenet+�� $& 40������ *LVRHangjelzes+' �� *
Hangjelzes+�� $& 60������ *
LVRSpecMuv+' �� *	SpecMuv+�� $& 30������ *LVREbresztes+' �� *
EbresztIdo+�� -   $ listViewSzablistak��$
��Left=�$	��173��$��
65,372,0,0q�$	��421��#���   #���  $
 TrueY�$	��Top=�$��Hidden=�$��Hidden=���.  $��0q�$��Segoe UI Black)�$	��Top=���Z�    $ 7Q�$  3Q�$! 	#FF660000��$" Quality=�#   $ �� #% �  .P��� $	& 122������ *LVLNev+' �� *Nev+�� $	& 200������ *LVLHoszenzor+' �� *	Hoszenzor+�� $& 95������ *LVLCsatornak+' �� *	Csatornak+( �� 
GroupStyle( (  "System.Windows.Controls.GroupStyle  )   HeaderTemplate) �� .R��� $
��Bold"�$��12(����	#FFFEFF7C���� *Name+�� -   $ listViewFordszamok��$
��Left=�$	��237��$��	65,80,0,0q�$	��421��#���   #���  $
 TrueY�$	��Top=�$��Hidden=�$��Hidden=���.  $��0q�$��Segoe UI Black)�$	��Top=���Z�    $ 7Q�$  3Q�$! 	#FF660000��$" Quality=�#   $ �� #% �  .P��� $	& 310������ *
LVFKimenet+' �� *	Kimenet+�� $& 47������ *LVFFordulatszam+' �� *
Fordszam+�� $& 56������ *LVFVezerles+' �� *
VezTipus+(   ) �� .R��� $
��Bold"�$��12(����	#FFFEFF7C���� *Name+ � -   *  �Name$* frame��$�� ��$
��Left=�$	��103��$��
51,579,0,0q�$	��Top=�$	��189������ $ 0.5,1&�$ RelativeToBoundingBox=�$ 0.5,0&�+ �� Opacity$	+ 0.7Q�.��� $ DarkRed��� $ Black��$ 0.594Q��� -   $ labelFrissId��$
��Left=�$��
49,639,0,0q�$	��Top=�$��48��$��Segoe UI Black)�#���  $��16(�$
 Hand������ *
UIFrissido+��Z�    $  3Q�$ 7Q�$" Quality=�$! 	#FF660000���� -   $ 	labelCelh��$
��Left=�$��
49,611,0,0q�$	��Top=�$��48��$��Segoe UI Black)�#���  $��16(�$
 Hand������ *UICelhardver+��Z�    $  3Q�$ 7Q�$" Quality=�$! 	#FF660000���� -   $ 
labelHiszt��$
��Left=�$��
49,583,0,0q�$	��Top=�$��48��$��Segoe UI Black)�#���  $��16(�$
 Hand������ *UIHiszterezis+��Z�    $  3Q�$ 7Q�$" Quality=�$! 	#FF660000���� -   $ imageVillHom��$��664,320,129,339q�$��Rescources/VillanyA.png��$
 Klikk��$
 Hand���� -   $ listViewHomers��$
��Left=�$	��256��$��
566,81,0,0q�$	��218��#���   #���  $
 TrueY�$	��Top=�$��Hidden=�$��Hidden=���.  $��0q�$��Segoe UI Black)���Z�    $ 7Q�$  3Q�$! 	#FF660000��$" Quality=�#   $ �� , �� AllowsColumnReorder, . #% �  .P��� $	& 155������ *
LVHSzenzor+' �� *Név+�� $& 60������ *
LVHErtek+' �� *	Érték+(   ) �� .R��� $
��Bold"�$��12(����	#FFFEFF7C���� *Name+�� -   $ imageTurbine��$	��159��$��546,357,212,244q�$	��185��$ ��Rescources/turbine-512.png��$ 	0.5,0.501&�z�V� .~��� �� �� $
 -180Q�T� - T� Y$- 0.085Q�. T� X$. -0.275Q��� -   $ 	imageFan1��$	��159��$��546,359,212,242q�$	��185��$��Rescources/icon-fan.png��$ 	0.5,0.488&�z�V� .~��� �� �� T� $. -0.212Q�$- 0.005Q�/ � 	Resources/ 0 �� Color  
BordoBrush  
SargaBrush  GridViewColumnHeaderGripper1 �� 
TargetType2 �� Property3 �� Value4 �� 
TargetType5 �� Padding6 J� SnapsToDevicePixels7 n�Name8 �� Triggers9 P� Property: P� Value; �� 
TargetName��%x  &	       &	       &	 0     &	 �     &	 �     7  &	 {    7  7  7  7  �� $0 	#FFEC0000���� $0 Black��$	+ 0.6Q��� $ 0.5,1&�$ 0.5,0&�$	+ 0.4Q�.��� $	 Red��$ 0Q�� $ Black��$	 0.8Q��� $ 0.5,1&�$ 0.5,0&�$	+ 0.4Q�.��� $ Yellow��$	 0.0Q�� $ Black��$	 0.8Q��� "1 n�.��� 2 � ��$3 18���� 2 � ��83   �� 2 � ��3 �� "4 n�.R��� #5 z��
������ ._��� $��Center=�$��1��#��z���� "1 ��.��� 2 � ��$3 Center=��� 2 � ��$3 Center=��� 2 � ��83   �� 2 � ��#3 � ���� 2 � ��3 �� "4 ��.R�� .+��� $��HeaderBorder��$��0q�8�� 8�� $5 2,0,2,0q�._��� $��HeaderContent��
���  ��$��0,0,0,1q�#��z��#��z����. #6 z6 n� $7 PART_HeaderGripper��$��Right=�$��0,0,-9,0q�8�� 8 P� 9 �@J�IsMouseOver: . . ��� $; HeaderContent��2 � ��3 �	#FFC3DC00