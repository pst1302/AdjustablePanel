﻿using System;
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

        int curIndex;

        bool scrolled = false;
        bool colChanged = true;
        bool resized = false;

        startIndexAndEndIndex removedIndex = new startIndexAndEndIndex(0,0);

        /*
         * ********************************************************** 구조체 선언부 **********************************************************
         * 
         */
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
        /*
         ********************************************************** 구조체 선언 끝 ********************************************************** 
         */

        /*
         ********************************************************** Form init() ************************************************************
         */
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

        /*
         ********************************************************** 이벤트 리스너 **********************************************************
         */
        private void clickedButton(Object sender, EventArgs e)
        {
            //removeAllControls((Panel)((Button)sender).Parent);
        }

        private void LocationRefresh(Panel parent , int Col)
        {
            colChanged = true;

            parent.AutoScrollPosition = new Point(0, 0);
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
                //calculateEntirePanel(parent, Col);
                postLocations(parent, Col);
            }
            resized = false;
        }

        private void scrollChangeLocationRefresh(Panel parent, int Col,int scrollMove)
        {
            scrolled = true;
            if (resized == false && colChanged == false)
            {
                //calculateEntirePanel(parent, Col);
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

        /*
         * ********************************************************** 현재 인덱스 계산 부분 **********************************************************
         */
        private indexAndRemain calculateCurIndex(Panel parent, int Col)
        {
            int curPosIndex;
            int remainHeight;


            remainHeight = (Math.Abs(parent.AutoScrollPosition.Y) % (defaultHeight + parent.Controls[0].Margin.Bottom));

            curPosIndex = (Math.Abs(parent.AutoScrollPosition.Y) / (defaultHeight + parent.Controls[0].Margin.Bottom) * Col);

            if (curPosIndex < 0)
                curPosIndex = 0;

            indexAndRemain result = new indexAndRemain(curPosIndex, remainHeight);

            return result;
        }

        /*
         ********************************************************** 현재 인덱스부터 어디까지 그릴지 계산 하는 함수 **********************************************************
         */
        private int calculateHowMuchPaint(Panel parent, int Col)
        {
            int howMuchPaint = Convert.ToInt32(Math.Ceiling((double)(parent.Height * 2) / (defaultHeight + parent.Controls[0].Margin.Bottom) * Col));
            return howMuchPaint;
        }

        /*
         ********************************************************** 다시 그릴지 계산하는 함수 ********************************************************** 
         */
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

        /*
         * ********************************************************** 배열 인덱스 넘어가는지 확인해주는 함수 **********************************************************
         */
        private int calculateMaxIndex(int index)
        {
            if (index >= ITEMS)
                return ITEMS;
            else
                return index;
        }

        /*
         ********************************************************** 현재 인덱스 읽어서 지우고 다시 그리는 부분 **********************************************************
         */
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

            System.Diagnostics.Debug.WriteLine(parent.AutoScrollMinSize);
            
            removedIndex.start = curIndex;
            removedIndex.end = curIndex + maxCtrInDisplay;
        }


        private void removeAllControls(Panel parent)
        {
            for (int i = 0; i < parent.Controls.Count; i++)
                parent.Controls[i].Location = new Point(0, INVISIBLE);
        }

        /*
         ********************************************************** 다시 그리는 코어 함수 **********************************************************
         */
        private void reArrangeControls(Panel parent,int Col,int widthUnit,int heightUnit,int curIndex,int curRemain,int maxCtrInDisplay) 
        {
            int location_y = -curRemain;
            int location_x = 0;
            int j = 0;

            parent.Controls[curIndex].Location = new Point(location_x, location_y);

            parent.Controls[curIndex].Width = widthUnit - (parent.Controls[curIndex].Margin.Left * 2);
            parent.Controls[curIndex].Height = heightUnit - (parent.Controls[curIndex].Margin.Bottom * 2);

            for (int i = curIndex + 1; i < calculateMaxIndex(curIndex + maxCtrInDisplay); i++)
            {
                Control SubItem = parent.Controls[i];

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

                SubItem.Location = new Point(location_x, location_y);
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
            scrollChangeLocationRefresh((Panel)sender, ColNum,e.NewValue-e.OldValue);
        }

        private void panView_WheelChanged(object sender, MouseEventArgs e)
        {
            scrollChangeLocationRefresh((Panel)sender, ColNum,Math.Abs(e.Delta));
        }
    }
}
