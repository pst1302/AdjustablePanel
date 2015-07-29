using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdjustableWindows
{
    public partial class AdjustableForm : Form
    {
        // 현재 컬럼 개수
        int ColNum = 1;

        Boolean sizeLock = false;
        Boolean colChangeLock = false;

        const int INVISIBLE = -1000000000;
        const int ITEMS = 100;

        // 기본 높이 설정
        int defaultHeight = 200;
        int curIndex = 0;

        Point[] locations = new Point[ITEMS];

        public AdjustableForm()
        {
            InitializeComponent();

            // 더블 버퍼링 설정
            this.DoubleBuffered = true;

            // 컨트롤 생성후 넣는 부분
            for (int i = 0; i < ITEMS; i++)
            {
                Button SampleData = new Button();
                SampleData.Text = (i + 1).ToString();
                SampleData.Margin = new Padding(10);
                SampleData.Location = new Point(0, INVISIBLE);
                panView.Controls.Add(SampleData);

            }

            // AutoScroll 설정
            panView.AutoScroll = true;
            cbmCol.Text = ColNum.ToString();
        }


        private void LocationRefresh(Panel parent , int Col)
        {
            /* 행 변경하는 이벤트 중에 사이즈를 조정하는 이벤트가 계속 발생하기 때문에 Lock을 걸어주는 부분
             * 
             */
            if (colChangeLock == false)
            {
                //parent.VerticalScroll.Value = 0;
                //setControlsLocation(parent,Col);
                initLocation(parent, Col);
                postLocations(parent, Col);
            }
        }

        private void SizeChangeLocationRefresh(Panel parent , int Col)
        {
            //colChangeLock = true;
            //isSize = true;
            if (sizeLock == false)
            {
                //setControlsLocation(parent, Col);
                initLocation(parent, Col);
                postLocations(parent, Col);
            }
           // colChangeLock = false;
            //isSize = false;
            
        }

        private void scrollChangeLocationRefresh(Panel parent, int Col)
        {
            //setControlsLocation(parent, Col);
            initLocation(parent, Col);
            postLocations(parent, Col);
        }


        /*
         * *************************************************************************************************************
        // Sub컨트롤의 생성하고 위치 조정하고 패널에 넣는 메소드
        public void setControlsLocation(Panel parent,int Col)
        {
            if (parent.Controls.Count == 0)
                return;

            // Col 열의 개수
            // 행의 개수는 자동
            
            Point curP = new Point(0,0);

            // 새롭게 넣는 컨트롤의 위치를 저장하는 변수
            int location_x = 0;
            int location_y = 0;

            // 행에 갯수에 따라 계산된 한개의 컨트롤의 크기를 저장하는 변수
            int widthUnit = 0;
            int heightUnit = 0;

            int startHeight = parent.Width;
            //int curIndex = 1;

            // 패널의 크기를 저장하는 Width, Height
            int Width = parent.Width;
            int Height = parent.Height;

            int j = 0;  // 반복문용 변수

            // 컨트롤의 Location 속성에 넣을 구조체 선언
            Point location = new Point(location_x, location_y);

            // 현재 스크롤된 위치 저장
            int curY = Math.Abs(parent.AutoScrollPosition.Y);
            
            // Panel의 너비를 행으로 나눈 값 -> 한개의 컨트롤의 너비
            widthUnit = Width / Col;
            // 높이는 디폴트
            heightUnit = defaultHeight + parent.Controls[0].Margin.Bottom;

            location.X = location_x;
            location.Y = location_y - curY;
            parent.Controls[0].Location = location;

            parent.Controls[0].Width = widthUnit - (parent.Controls[0].Margin.Left * 2);
            parent.Controls[0].Height = heightUnit - (parent.Controls[0].Margin.Bottom * 2);


            // 여기서 정렬
            for (int i = 1; i < parent.Controls.Count; i++)
            {
                Control SubItem = parent.Controls[i];

                // 현재 위치 밑 화면까지만 그리기
                if (SubItem.Location.Y < (0 - parent.Height))
                {
                    SubItem.Location = new Point(0, INVISIBLE);
                }

                if (SubItem.Location.Y >= (parent.Height * 2))
                {
                    SubItem.Location = new Point(0, INVISIBLE);
                    break;
                }

                // 한개의 컨트롤에서 양쪽 마진만큼 뺀것이 한개의 컨트롤의 너비
                SubItem.Width = widthUnit - (SubItem.Margin.Left * 2);
                SubItem.Height = heightUnit - (SubItem.Margin.Bottom * 2);

                // 전달 받은 행의 갯수만큼 오른쪽으로 위치 변경
                if (j < Col - 1)
                {
                    // 한개 컨트롤의 너비 + 마진 다음 컨트롤의 위치
                    location_x += (SubItem.Width + SubItem.Margin.Right * 2);
                    j++;
                }
                // 전달 받은 행만큼 오른쪽으로 이동하면 다음줄로 넘어감
                else
                {
                    location_x = 0;
                    location_y += heightUnit;
                    j = 0;

                }

                // Location 설정
                location.X = location_x;
                location.Y = location_y - curY;
                SubItem.Location = location;
            }
        }
         * *************************************************************************************************************************************
        */


        private void initLocation(Panel parent,int Col)
        {
            int location_x = 0;
            int location_y = 0;

            int width = parent.Width;
            int height = parent.Height;

            int widthUnit = width / Col;
            int heightUnit = defaultHeight + parent.Controls[0].Margin.Bottom;

            int j = 0;

            for (int i = curIndex; i < locations.Length; i++)
            {
                locations[i] = new Point(location_x, location_y - Math.Abs(parent.AutoScrollPosition.Y));

                // 전달 받은 행의 갯수만큼 오른쪽으로 위치 변경
                if (j < Col - 1)
                {
                    // 한개 컨트롤의 너비 + 마진 다음 컨트롤의 위치
                    location_x += widthUnit;
                    j++;
                }
                // 전달 받은 행만큼 오른쪽으로 이동하면 다음줄로 넘어감
                else
                {
                    location_x = 0;
                    location_y += heightUnit;
                    j = 0;
                }
            }
        }

        private void postLocations(Panel parent, int Col)
        {

            int width = parent.Width;

            int widthUnit = width / Col;
            int heightUnit = defaultHeight + parent.Controls[0].Margin.Bottom;

            for (int i = 0; i < parent.Controls.Count; i++)
            {
                Control SubItem = parent.Controls[i];

                if (locations[i].Y < (0 - parent.Height))
                {
                    locations[i].Y = INVISIBLE;
                }

                if (locations[i].Y >= (parent.Height * 2))
                {
                    locations[i].Y = INVISIBLE;
                    SubItem.Location = locations[i];
                    for (int j = i; j < locations.Length; j++)
                    {
                        locations[j].Y = INVISIBLE;
                    }
                    break;
                }
                SubItem.Location = locations[i];

                SubItem.Width = widthUnit - (SubItem.Margin.Left * 2);
                SubItem.Height = heightUnit - (SubItem.Margin.Bottom * 2);
            }
        }

        // 컨트롤의 Height 변경
        private void setContorlsHeight(Panel parent, int size)
        {
            if (parent.Controls.Count == 0)
                return;

            for (int i = 0; i < parent.Controls.Count; i++)
            {
                defaultHeight = size;
            }
            LocationRefresh(parent, ColNum);
        }
     
        // cbm열 변경시 호출되는 이벤트
        private void cbmCol_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;

            if (cmb.SelectedIndex == -1)
                return;

            ColNum = Convert.ToInt32(cmb.SelectedItem);

            LocationRefresh(panView, ColNum);
        }

        // 사이즈 변경할시 호출되는 이벤트
        private void panView_SizeChanged(object sender, EventArgs e)
        {
            SizeChangeLocationRefresh((Panel)sender, ColNum);
        }

        private void panView_ScrollChanged(object sender, ScrollEventArgs e)
        {
            //scrollChangeLocationRefresh((Panel)sender, ColNum);
        }

        private void panView_WheelChanged(object sender, MouseEventArgs e)
        {
            scrollChangeLocationRefresh((Panel)sender, ColNum);
        }
    }
}
