   M S B A M L   `   `   `  ���� D  @Feverkill, Version=0.8.0.1, Culture=neutral, PublicKeyToken=null%clr-namespace:Attekinto	Attekinto  [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms     Attekinto.AttekintoWPF   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:Attekinto     RForms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     -     �Name$  MainWindow1�� � Title$ 	Feverkill��$	��760��$	��943�� � AllowsTransparency .  � WindowStyle$
 None=� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=���Z� ��Z�  � DataContext ��  �� RelativeSource �� *Self+.��� -    �Name$ gridBackgnd��$
��Left=�$	��Top=�$	��943��.+��� -   	 ��Name$	 imageBackgnd��$	��729��$
��Fill=�$��Rescources/hatter.png��$	��839��$
��Left=�$	��Top=��� -   
 ��Name$
 button��$��Button��$
��Left=�$��881,657,0,-119q�$	��Top=�$��52��$	��222���� -   $	 	imageHelp��$��831,0,48,712q�$��Rescources/HelpA.png�� � ToolTip$ 
Segítség�� J� IsHitTestVisible .  � Cursor$
 Hand���� -   $	 
imageFoabl��$��830,50,48,660q�$��Rescources/FoablA.png��$ Haladó Ablak megnyitása��$
 Hand���� -   $	 	imageHide��$��839,106,57,610q�$��Rescources/HideA.png��$ 	Elrejtés��$
 Hand���� -   $	 imageFeedback��$��830,320,49,380q�$��Rescources/FeedbackA.png��$ Küldj Visszajelzést!��$
 Hand���� -	    ��Name$ labelCim��$
��Left=�$��	60,34,0,0q�$	��Top=�$��30.667(�$��Arial)�$
��Bold"� J� RenderTransformOrigin$ 0.5,0.5&�$��40��$	��664����Z� ���� *UICim+z�V� .~��� �� ��  �� Angle$
 0.28Q�T� ����  �� EndPoint$ 0.5,1&� � MappingMode$ RelativeToBoundingBox=� �� 
StartPoint$ 0.5,0&� �� RelativeTransform V� .~���  �� CenterY$	 0.5Q� �� CenterX$	 0.5Q���  �� CenterY$	 0.5Q� �� CenterX$	 0.5Q��� $	 -90Q� �� CenterY$	 0.5Q� �� CenterX$	 0.5Q�T� .���  � Color$ Black�� � Offset$ 1Q�� $	 Red���� -
   $	 imageVillFordsz��$��447,141,346,518q�$��Rescources/VillanyA.png��$ Manual Control��$
 Hand���� -   $	 imageVillSema��$��422,327,371,332q�$��Rescources/VillanyA.png��$ Manage Control Schemes��$
 Hand���� -   $	 imageVillRiaszt��$��678,505,115,155q�$��Rescources/VillanyA.png��$ Manage Alerts��$
 Hand���� -    ��Name$ listViewRiaszt��$
��Left=�$	��128��$��254,553,0,0q�$	��485��   
HatterSzin#���     
EloterSzin#���  $ �� IsSynchronizedWithCurrentItem$
 TrueY�$	��Top=�$��Hidden=�$��Hidden=���.  $��0q�$��Segoe UI Black)�$	��Top=���Z�  J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect      ShadowDepth$  7Q�!   
BlurRadius$! 3Q�"   Color$" 	#FF660000��#   RenderingBias$# Quality=�$ �� ItemBindingGroup$ &   System.Windows.Data.BindingGroup  % �� View% ��   myHeaderStyle!& �� ColumnHeaderContainerStyle#& �  .P��� ' �� Width$	' 160������ *LVRHoszenzor+( �� DisplayMemberBinding( �� *Homero+�� $' 20������ *LVRRel+( �� *	Muvelet+�� $' 30������ *LVRFok+( �� *
RiasztPont+�� $	' 140������ *	LVRUzenet+( �� *Uzenet+�� $' 40������ *LVRHangjelzes+( �� *
Hangjelzes+�� $' 60������ *
LVRSpecMuv+( �� *	SpecMuv+�� $' 30������ *LVREbresztes+( �� *
EbresztIdo+) �� 
GroupStyle) (  "System.Windows.Controls.GroupStyle  *   HeaderTemplate* �� .R��� $
��Bold"�$��12(����	#FFFEFF7C���� *Name+�� -   $ listViewSzablistak��$
��Left=�$	��173��$��
65,372,0,0q�$	��421��#���   #���  $
 TrueY�$	��Top=�$��Hidden=�$��Hidden=���.  $��0q�$��Segoe UI Black)�$	��Top=���Z�    $  7Q�$! 3Q�$" 	#FF660000��$# Quality=�$   % �� #& �  .P��� $	' 122������ *LVLNev+( �� *Nev+�� $	' 200������ *LVLHoszenzor+( �� *	Hoszenzor+�� $' 95������ *LVLCsatornak+( �� *	Csatornak+)   * �� .R��� $
��Bold"�$��12(����	#FFFEFF7C���� *Name+�� -   $ listViewFordszamok��$
��Left=�$	��237��$��	65,80,0,0q�$	��421��#���   #���  $
 TrueY�$	��Top=�$��Hidden=�$��Hidden=���.  $��0q�$��Segoe UI Black)�$	��Top=�$
 Hand����Z�    $  7Q�$! 3Q�$" 	#FF660000��$# Quality=�$   % �� #& �  .P��� $	' 310������ *
LVFKimenet+( �� *	Kimenet+�� $' 47������ *LVFFordulatszam+( �� *
Fordszam+�� $' 56������ *LVFVezerles+( �� *
VezTipus+)   * �� .R��� $
��Bold"�$��12(����	#FFFEFF7C���� *Name+ � -   +  �Name$+ frame��$�� ��$
��Left=�$	��103��$��
51,579,0,0q�$	��Top=�$	��189������ $ 0.5,1&�$ RelativeToBoundingBox=�$ 0.5,0&�, �� Opacity$	, 0.7Q�.��� $ DarkRed��� $ Black��$ 0.594Q��� -   $ labelFrissId��$
��Left=�$��
49,639,0,0q�$	��Top=�$��31��$��Segoe UI Black)�#���  $��16(�$
 Hand����Z� ���� *
UIFrissido+   $! 3Q�$  7Q�$# Quality=�$" 	#FF660000���� -   $ 	labelCelh��$
��Left=�$��
49,611,0,0q�$	��Top=�$��31��$��Segoe UI Black)�#���  $��16(�$
 Hand����Z� ���� *UICelhardver+   $! 3Q�$  7Q�$# Quality=�$" 	#FF660000���� -   $ 
labelHiszt��$
��Left=�$��
49,583,0,0q�$	��Top=�$��31��$��Segoe UI Black)�#���  $��16(�$
 Hand����Z� ���� *UIHiszterezis+   $! 3Q�$  7Q�$# Quality=�$" 	#FF660000���� -   $	 imageVillHom��$��664,320,129,339q�$��Rescources/VillanyA.png��$ Sensor Window��$
 Hand���� -   $ listViewHomers��$
��Left=�$	��256��$��
566,81,0,0q�$	��218��#���   #���  $
 TrueY�$	��Top=�$��Hidden=�$��Hidden=���.  $��0q�$��Segoe UI Black)���Z�    $  7Q�$! 3Q�$" 	#FF660000��$# Quality=�$   % �� - �� AllowsColumnReorder- . #& �  .P��� $	' 155������ *
LVHSzenzor+( �� *Név+�� $' 60������ *
LVHErtek+( �� *	Érték+)   * �� .R��� $
��Bold"�$��12(����	#FFFEFF7C���� *Name+�� -   $	 imageTurbine��$	��159��$��546,357,212,244q�$	��185��$ ��Rescources/turbine-512.png��$ 	0.5,0.501&�z�V� .~��� �� �� $
 -180Q�T� . T� Y$. 0.085Q�/ T� X$/ -0.275Q��� -   $	 	imageFan1��$	��159��$��546,359,212,242q�$	��185��$��Rescources/icon-fan.png��$ 	0.5,0.488&�z�V� .~��� �� �� T� $/ -0.212Q�$. 0.005Q��� -   0 ��Name$0 VillFordszRectHelper��$
��Left=�$��38��$��511,168,0,0q�$	��Top=�$��21���� -   $0 VillSemaRectHelper��$
��Left=�$��38��$��486,355,0,0q�$	��Top=�$��22���� -   $0 VillHomRectHelper��$
��Left=�$��37��$��729,347,0,0q�$	��Top=�$��21���� -   $0 VillRiasztRectHelper��$
��Left=�$��36��$��742,532,0,0q�$	��Top=�$��22���� -   $0 OldalTutorRectHelper��$
��Left=�$��32��$��	847,6,0,0q�$	��Top=�$��32���� -   $0 OldalFoablRectHelper��$
��Left=�$��32��$��
846,60,0,0q�$	��Top=�$��32���� -   $0 OldalElrejtRectHelper��$
��Left=�$��32��$��846,112,0,0q�$	��Top=�$��32��1 � 	Resources1 2 �� Color  
BordoBrush  
SargaBrush  GridViewColumnHeaderGripper3 �� 
TargetType4 �� Property5 �� Value6 �� 
TargetType7 �� Padding8 J� SnapsToDevicePixels9 n�Name: �� Triggers; P� Property< P� Value= �� 
TargetName��%x  &	       &	       &	 0     &	 �     &	 �     7  &	 {    7  7  7  7  �� $2 	#FFEC0000���� $2 Black��$	, 0.6Q��� $ 0.5,1&�$ 0.5,0&�$	, 0.4Q�.��� $	 Red��$ 0Q�� $ Black��$	 0.8Q��� $ 0.5,1&�$ 0.5,0&�$	, 0.4Q�.��� $ Yellow��$	 0.0Q�� $ Black��$	 0.8Q��� "3 n�.��� 4 � ��$5 18���� 4 � ��85   �� 4 � ��5 �� "6 n�.R��� #7 z��
������ ._��� $��Center=�$��1��#��z���� "3 ��.��� 4 � ��$5 Center=��� 4 � ��$5 Center=��� 4 � ��85   �� 4 � ��#5 � ���� 4 � ��5 �� "6 ��.R�� .+��� $��HeaderBorder��$��0q�8�� 8�� $7 2,0,2,0q�._��� $��HeaderContent��
���  ��$��0,0,0,1q�#��z��#��z����. #8 z8 n� $9 PART_HeaderGripper��$��Right=�$��0,0,-9,0q�8�� : P� ; �@J�IsMouseOver< . . ��� $= HeaderContent��4 � ��5 �	#FFC3DC00