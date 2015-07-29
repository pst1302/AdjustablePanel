using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdjustableWindows
{
    class AdjustablePanel<T> where T : Button
    {
        private Panel pn;
        private int low;
        private bool lockPainting;
        T[] contentsArray;

        public AdjustablePanel ()
        {
            pn = new Panel();
            pn.Paint += paintHandler;
            lockPainting = false;
        }
        
        /*
         *  Dock 설정
         */
        public void setDock(DockStyle dStyle) {
            pn.Dock = dStyle;
        }

        /*
         *  패널의 위치 설정
         */

        public void setPanelLocation(Point startLocation)
        {
            pn.Location = startLocation;
        }

        /*
         *  크기 설정
         */
        public void setSize(Size size)
        {
            pn.Size = size;
        }

        /*
         *  안에 집어넣을 컨트롤 설정 컨트롤을 상속받은 것이어야 함
         */
        public void setContents(T[] contents)
        {
            contentsArray = contents;
        }

        /*
         *  스크롤바가 나오게할 크기 설정
         */
        public void minimumScrollSize(Size size)
        {
            pn.AutoScrollMinSize = size;
        }

        /*
         *  패널의 Paint 이벤트 핸들러
         */
        private void paintHandler(Object sender,EventArgs e) {

            if (lockPainting == false)
            {
                setLocation();
            }
        }

        /*
         *  패널안에 컨트롤들의 위치를 low에 따라서 재조정 
         */
        private void setLocation()
        {

            int location_x = 30;
            int location_y = 30;
            int j = 0;

            for (int i = 0; i < contentsArray.Length; i++)
            {

                contentsArray[i].Location = new Point(location_x, location_y);

                if (j < low)
                {
                    location_x += 110;
                    j++;
                }
                else
                {
                    location_x = 30;
                    location_y += 110;
                    j = 0;
                }
            }

            pn.Controls.AddRange(contentsArray);

            lockPainting = true;
        }

        /*
         *  클릭 이벤트 혹은 변경 이벤트에 따라 low를 변경해주는 메소드
         */
        public void updating(int low)
        {
            this.low = low;
            pn.Refresh();
            lockPainting = false;
        }

        /*
         *  패널 읽기
         */
        public Panel getPanel()
        {
            return pn;
        }
    }
}
