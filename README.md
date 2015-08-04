# AdjustablePanel
in c#

제작자 : 이 길복

Controls을 집어 넣을 수 있는 Panel입니다.

기능1. 행의 갯수를 조정할 수 있으며 값을 계산해 너비에 딱 맞게 조절해주는 기능을 가지고 있습니다.

기능2. UI 가상화를 통해 성능이 향상됩니다. -> 목표 1000개의 Controls

기능3. Panel을 상속받는 클래스화 시켜서 사용하기 편하게 설계


How to Use -> ColumnGridPanel을 도구상자에 추가해 사용 

Methods 1. col_Changed(int col) -> 행의 갯수를 바뀔때 사용
Methods 2. input_Control(Control item) -> Control을 추가할 때 사용
Methods 3. setControlsHeight(int height) -> Control의 높이를 변경 할 때 사용

