namespace AdjustableWindows
{
    partial class AdjustableForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbmCol = new System.Windows.Forms.ComboBox();
            this.panView = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbmCol);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1096, 46);
            this.panel1.TabIndex = 1;
            // 
            // cbmCol
            // 
            this.cbmCol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbmCol.FormattingEnabled = true;
            this.cbmCol.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cbmCol.Location = new System.Drawing.Point(12, 12);
            this.cbmCol.Name = "cbmCol";
            this.cbmCol.Size = new System.Drawing.Size(179, 20);
            this.cbmCol.TabIndex = 2;
            this.cbmCol.SelectedIndexChanged += new System.EventHandler(this.cbmCol_SelectedIndexChanged);
            // 
            // panView
            // 
            this.panView.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panView.Location = new System.Drawing.Point(0, 46);
            this.panView.Name = "panView";
            this.panView.Size = new System.Drawing.Size(1096, 414);
            this.panView.TabIndex = 2;
            this.panView.Scroll += new System.Windows.Forms.ScrollEventHandler(this.panView_ScrollChanged);
            this.panView.SizeChanged += new System.EventHandler(this.panView_SizeChanged);
            this.panView.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panView_WheelChanged);
            // 
            // AdjustableForm
            // 
            this.ClientSize = new System.Drawing.Size(1096, 460);
            this.Controls.Add(this.panView);
            this.Controls.Add(this.panel1);
            this.Name = "AdjustableForm";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbmCol;
        private System.Windows.Forms.Panel panView;
    }
}

