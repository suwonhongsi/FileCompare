namespace FileCompare
{
    partial class Form1
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer1 = new SplitContainer();
            panel3 = new Panel();
            lvwLeftDir = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            panel2 = new Panel();
            btnLeftDir = new Button();
            txtLeftDir = new TextBox();
            panel1 = new Panel();
            btnCopyFromLeft = new Button();
            lblAppName = new Label();
            panel6 = new Panel();
            lvwrightDir = new ListView();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            panel5 = new Panel();
            btnRightDir = new Button();
            txtRightDir = new TextBox();
            panel4 = new Panel();
            btnCopyFromRight = new Button();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(22, 27);
            splitContainer1.Margin = new Padding(3, 4, 3, 4);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(panel3);
            splitContainer1.Panel1.Controls.Add(panel2);
            splitContainer1.Panel1.Controls.Add(panel1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel6);
            splitContainer1.Panel2.Controls.Add(panel5);
            splitContainer1.Panel2.Controls.Add(panel4);
            splitContainer1.Size = new Size(1632, 979);
            splitContainer1.SplitterDistance = 816;
            splitContainer1.TabIndex = 0;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
            // 
            // panel3
            // 
            panel3.Controls.Add(lvwLeftDir);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 267);
            panel3.Margin = new Padding(3, 4, 3, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(816, 712);
            panel3.TabIndex = 3;
            // 
            // lvwLeftDir
            // 
            lvwLeftDir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvwLeftDir.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3 });
            lvwLeftDir.FullRowSelect = true;
            lvwLeftDir.GridLines = true;
            lvwLeftDir.Location = new Point(17, 32);
            lvwLeftDir.Margin = new Padding(3, 4, 3, 4);
            lvwLeftDir.Name = "lvwLeftDir";
            lvwLeftDir.Size = new Size(779, 652);
            lvwLeftDir.TabIndex = 0;
            lvwLeftDir.UseCompatibleStateImageBehavior = false;
            lvwLeftDir.View = View.Details;
            lvwLeftDir.SelectedIndexChanged += lvwLeftDir_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "이름";
            columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "크기";
            columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "수정일";
            columnHeader3.Width = 160;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnLeftDir);
            panel2.Controls.Add(txtLeftDir);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 127);
            panel2.Margin = new Padding(3, 4, 3, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(816, 852);
            panel2.TabIndex = 2;
            // 
            // btnLeftDir
            // 
            btnLeftDir.Anchor = AnchorStyles.Right;
            btnLeftDir.BackColor = Color.White;
            btnLeftDir.Font = new Font("맑은 고딕", 10.875F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnLeftDir.Location = new Point(635, 22);
            btnLeftDir.Margin = new Padding(3, 4, 3, 4);
            btnLeftDir.Name = "btnLeftDir";
            btnLeftDir.Size = new Size(172, 98);
            btnLeftDir.TabIndex = 1;
            btnLeftDir.Text = "폴더선택";
            btnLeftDir.UseVisualStyleBackColor = false;
            btnLeftDir.Click += btnLeftDir_Click;
            // 
            // txtLeftDir
            // 
            txtLeftDir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtLeftDir.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            txtLeftDir.Location = new Point(3, 53);
            txtLeftDir.Margin = new Padding(3, 4, 3, 4);
            txtLeftDir.Name = "txtLeftDir";
            txtLeftDir.Size = new Size(612, 50);
            txtLeftDir.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnCopyFromLeft);
            panel1.Controls.Add(lblAppName);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(816, 127);
            panel1.TabIndex = 1;
            // 
            // btnCopyFromLeft
            // 
            btnCopyFromLeft.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCopyFromLeft.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnCopyFromLeft.Location = new Point(666, 9);
            btnCopyFromLeft.Margin = new Padding(3, 4, 3, 4);
            btnCopyFromLeft.Name = "btnCopyFromLeft";
            btnCopyFromLeft.Size = new Size(130, 85);
            btnCopyFromLeft.TabIndex = 1;
            btnCopyFromLeft.Text = ">>>";
            btnCopyFromLeft.UseVisualStyleBackColor = true;
            btnCopyFromLeft.Click += btnCopyFromLeft_Click;
            // 
            // lblAppName
            // 
            lblAppName.AutoSize = true;
            lblAppName.Font = new Font("맑은 고딕", 19.875F, FontStyle.Bold, GraphicsUnit.Point, 129);
            lblAppName.ForeColor = Color.Blue;
            lblAppName.Location = new Point(19, 23);
            lblAppName.Name = "lblAppName";
            lblAppName.Size = new Size(364, 71);
            lblAppName.TabIndex = 0;
            lblAppName.Text = "File Compare";
            // 
            // panel6
            // 
            panel6.Controls.Add(lvwrightDir);
            panel6.Dock = DockStyle.Bottom;
            panel6.Location = new Point(0, 267);
            panel6.Margin = new Padding(3, 4, 3, 4);
            panel6.Name = "panel6";
            panel6.Size = new Size(812, 712);
            panel6.TabIndex = 2;
            // 
            // lvwrightDir
            // 
            lvwrightDir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lvwrightDir.Columns.AddRange(new ColumnHeader[] { columnHeader4, columnHeader5, columnHeader6 });
            lvwrightDir.FullRowSelect = true;
            lvwrightDir.GridLines = true;
            lvwrightDir.Location = new Point(20, 32);
            lvwrightDir.Margin = new Padding(3, 4, 3, 4);
            lvwrightDir.Name = "lvwrightDir";
            lvwrightDir.Size = new Size(767, 652);
            lvwrightDir.TabIndex = 0;
            lvwrightDir.UseCompatibleStateImageBehavior = false;
            lvwrightDir.View = View.Details;
            lvwrightDir.SelectedIndexChanged += lvwrightDir_SelectedIndexChanged;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "이름";
            columnHeader4.Width = 300;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "크기";
            columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "수정일";
            columnHeader6.Width = 160;
            // 
            // panel5
            // 
            panel5.Controls.Add(btnRightDir);
            panel5.Controls.Add(txtRightDir);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 127);
            panel5.Margin = new Padding(3, 4, 3, 4);
            panel5.Name = "panel5";
            panel5.Size = new Size(812, 852);
            panel5.TabIndex = 1;
            // 
            // btnRightDir
            // 
            btnRightDir.Anchor = AnchorStyles.Right;
            btnRightDir.Font = new Font("맑은 고딕", 10.875F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnRightDir.Location = new Point(630, 23);
            btnRightDir.Margin = new Padding(3, 4, 3, 4);
            btnRightDir.Name = "btnRightDir";
            btnRightDir.Size = new Size(164, 98);
            btnRightDir.TabIndex = 1;
            btnRightDir.Text = "폴더선택";
            btnRightDir.UseVisualStyleBackColor = true;
            btnRightDir.Click += btnRightDir_Click;
            // 
            // txtRightDir
            // 
            txtRightDir.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtRightDir.Font = new Font("맑은 고딕", 12F, FontStyle.Bold, GraphicsUnit.Point, 129);
            txtRightDir.Location = new Point(17, 53);
            txtRightDir.Margin = new Padding(3, 4, 3, 4);
            txtRightDir.Name = "txtRightDir";
            txtRightDir.Size = new Size(598, 50);
            txtRightDir.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnCopyFromRight);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Margin = new Padding(3, 4, 3, 4);
            panel4.Name = "panel4";
            panel4.Size = new Size(812, 127);
            panel4.TabIndex = 0;
            // 
            // btnCopyFromRight
            // 
            btnCopyFromRight.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCopyFromRight.Font = new Font("맑은 고딕", 9F, FontStyle.Bold, GraphicsUnit.Point, 129);
            btnCopyFromRight.Location = new Point(17, 9);
            btnCopyFromRight.Margin = new Padding(3, 4, 3, 4);
            btnCopyFromRight.Name = "btnCopyFromRight";
            btnCopyFromRight.Size = new Size(130, 85);
            btnCopyFromRight.TabIndex = 1;
            btnCopyFromRight.Text = "<<<";
            btnCopyFromRight.UseVisualStyleBackColor = true;
            btnCopyFromRight.Click += btnCopyFromRight_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1676, 1033);
            Controls.Add(splitContainer1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Padding = new Padding(22, 27, 22, 27);
            Text = "Form1";
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel6.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panel4.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtLeftDir;
        private System.Windows.Forms.Button btnCopyFromLeft;
        private System.Windows.Forms.TextBox txtRightDir;
        private System.Windows.Forms.Button btnCopyFromRight;
        private System.Windows.Forms.ListView lvwLeftDir;
        private System.Windows.Forms.Button btnLeftDir;
        private System.Windows.Forms.ListView lvwrightDir;
        private System.Windows.Forms.Button btnRightDir;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
    }
}

