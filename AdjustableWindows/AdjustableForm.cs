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

        const int INVISIBLE = -1000000000;
        const int ITEMS = 100;

        // 기본 높이 설정
        int defaultHeight = 200;

        Point[] locations = new Point[ITEMS];

        int curIndex = 0;

        bool scrolled = false;
        bool colChanged = true;
        bool resized = false;

        startIndexAndEndIndex removedIndex = new startIndexAndEndIndex(0,0);

        private struct indexAndRemain
        {
            public int index, remain;

            public indexAndRemain(int index, int remain)
            {
                this.index = index;
                this.remain = remain;
            }
        }

        private struct startIndexAndEndIndex
        {
            public int start, end;

            public startIndexAndEndIndex(int start, int end)
            {
                this.start = start;
                this.end = end;
            }
        }

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


        private void clickedButton(Object sender, EventArgs e)
        {
            MessageBox.Show(((Button)sender).Text);
        }

        private void LocationRefresh(Panel parent , int Col)
        {
            colChanged = true;

            //parent.AutoScrollPosition = new Point(0, 0);
            if (resized == false && scrolled == false)
            {
                calculateEntirePanel(parent, Col);
                postLocations(parent, Col);
            }
            colChanged = false;
        }

        private void SizeChangeLocationRefresh(Panel parent , int Col)
        {
            resized = true;
            if (colChanged == false && scrolled == false)
            {
                calculateEntirePanel(parent, Col);
                postLocations(parent, Col);
            }
            resized = false;
        }

        private void scrollChangeLocationRefresh(Panel parent, int Col,int scrollMove)
        {
            scrolled = true;
            if (resized == false && colChanged == false)
            {
                calculateEntirePanel(parent, Col);
                postLocations(parent, Col);
            }
            scrolled = false;
        }

        private int calculateEntirePanel(Panel parent, int Col)
        {
            int entirePanel;

            entirePanel = Convert.ToInt32(Math.Ceiling((double)(defaultHeight + parent.Controls[0].Margin.Bottom) * parent.Controls.Count / Col));

            parent.AutoScrollMinSize = new Size(0, entirePanel);

            return entirePanel;
        }
        
        private indexAndRemain calculateCurIndex(Panel parent, int Col)
        {
            int curPosIndex;
            int remainHeight;

            remainHeight = (Math.Abs(parent.AutoScrollPosition.Y) % (defaultHeight + parent.Controls[0].Margin.Bottom));

            //if (remainHeight != 0)
            //{
                curPosIndex = (Math.Abs(parent.AutoScrollPosition.Y) / (defaultHeight + parent.Controls[0].Margin.Bottom) * Col);
            //}
                /*
            else
            {
                curPosIndex = (Math.Abs(parent.AutoScrollPosition.Y) / (defaultHeight + parent.Controls[0].Margin.Bottom) * Col);
            }*/

            indexAndRemain result = new indexAndRemain(curPosIndex, remainHeight);

            return result;
        }

        private int calculateHowMuchPaint(Panel parent, int Col)
        {
            return Convert.ToInt32(Math.Ceiling((double)(parent.Height * 2) / (defaultHeight + parent.Controls[0].Margin.Bottom) * Col));
        }
        
        private bool howToChanged(Panel parent, int Col)
        {
            int newIndex;

            newIndex = calculateCurIndex(parent,Col).index;

            if (newIndex == curIndex)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int calculateMaxIndex(int index)
        {
            if (index >= ITEMS)
                return ITEMS;
            else
                return index;
        }
  
        private void postLocations(Panel parent, int Col)
        {

            int width = parent.Width;

            int widthUnit = width / Col;
            int heightUnit = defaultHeight + parent.Controls[0].Margin.Bottom;

            int maxCtrInDisplay;

            int curRemain;

            if (howToChanged(parent, Col) == true)
            {
                for (int i = removedIndex.start; i < calculateMaxIndex(removedIndex.end); i++)
                {
                    Control subItem = parent.Controls[i];

                    subItem.Location = new Point(0, INVISIBLE);
                }
            }
            indexAndRemain ir = calculateCurIndex(parent, Col);
            curIndex = ir.index;
            curRemain = ir.remain;
            maxCtrInDisplay = calculateHowMuchPaint(parent,Col);

            reArrangeControls(parent,Col,widthUnit,heightUnit,curIndex, curRemain, maxCtrInDisplay);

            removedIndex.start = curIndex;
            removedIndex.end = curIndex + maxCtrInDisplay;
        }

        private void reArrangeControls(Panel parent,int Col,int widthUnit,int heightUnit,int curIndex,int curRemain,int maxCtrInDisplay) 
        {
            int location_y = -curRemain;
            int location_x = 0;
            int j = 0;

            for (int i = curIndex; i < calculateMaxIndex(curIndex + maxCtrInDisplay); i++)
            {
                Control SubItem = parent.Controls[i];

                SubItem.Location = new Point(location_x, location_y);

                SubItem.Width = widthUnit - (SubItem.Margin.Left * 2);
                SubItem.Height = heightUnit - (SubItem.Margin.Bottom * 2);

                if (j < Col - 1)
                {
                    location_x += widthUnit;
                    j++;
                }
                else
                {
                    location_x = 0;
                    location_y += heightUnit;
                    j = 0;
                }
            }
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
            //scrollChangeLocationRefresh((Panel)sender, ColNum,e.NewValue-e.OldValue);
        }

        private void panView_WheelChanged(object sender, MouseEventArgs e)
        {
            scrollChangeLocationRefresh((Panel)sender, ColNum,Math.Abs(e.Delta));
        }
    }
}
