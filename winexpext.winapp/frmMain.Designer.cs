namespace winexpext.winapp
{
	partial class frmMain
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			splitContainer1 = new SplitContainer();
			grpActionLog = new GroupBox();
			textBox1 = new TextBox();
			btnClose = new Button();
			tmrAutoClose = new System.Windows.Forms.Timer(components);
			btnShowLog = new Button();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			grpActionLog.SuspendLayout();
			SuspendLayout();
			// 
			// splitContainer1
			// 
			splitContainer1.Dock = DockStyle.Fill;
			splitContainer1.Location = new Point(0, 0);
			splitContainer1.Name = "splitContainer1";
			splitContainer1.Orientation = Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.Controls.Add(grpActionLog);
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(btnShowLog);
			splitContainer1.Panel2.Controls.Add(btnClose);
			splitContainer1.Size = new Size(815, 531);
			splitContainer1.SplitterDistance = 479;
			splitContainer1.TabIndex = 0;
			// 
			// grpActionLog
			// 
			grpActionLog.Controls.Add(textBox1);
			grpActionLog.Dock = DockStyle.Fill;
			grpActionLog.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			grpActionLog.Location = new Point(0, 0);
			grpActionLog.Name = "grpActionLog";
			grpActionLog.Size = new Size(815, 479);
			grpActionLog.TabIndex = 0;
			grpActionLog.TabStop = false;
			grpActionLog.Text = "groupBox1";
			// 
			// textBox1
			// 
			textBox1.BackColor = Color.FromArgb(64, 64, 64);
			textBox1.Dock = DockStyle.Fill;
			textBox1.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
			textBox1.ForeColor = Color.White;
			textBox1.Location = new Point(3, 25);
			textBox1.Multiline = true;
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(809, 451);
			textBox1.TabIndex = 0;
			// 
			// btnClose
			// 
			btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnClose.Location = new Point(710, 3);
			btnClose.Name = "btnClose";
			btnClose.Size = new Size(93, 33);
			btnClose.TabIndex = 0;
			btnClose.Text = "&Close";
			btnClose.UseVisualStyleBackColor = true;
			btnClose.Click += btnClose_Click;
			// 
			// btnShowLog
			// 
			btnShowLog.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			btnShowLog.Location = new Point(598, 3);
			btnShowLog.Name = "btnShowLog";
			btnShowLog.Size = new Size(93, 33);
			btnShowLog.TabIndex = 1;
			btnShowLog.Text = "&Show Log";
			btnShowLog.UseVisualStyleBackColor = true;
			// 
			// frmMain
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(815, 531);
			Controls.Add(splitContainer1);
			Name = "frmMain";
			Text = "Windows Explorer Extensions";
			FormClosing += frmMain_FormClosing;
			FormClosed += frmMain_FormClosed;
			Load += frmMain_Load;
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			grpActionLog.ResumeLayout(false);
			grpActionLog.PerformLayout();
			ResumeLayout(false);
		}

		#endregion

		private SplitContainer splitContainer1;
		private Button btnClose;
		private GroupBox grpActionLog;
		private TextBox textBox1;
		private System.Windows.Forms.Timer tmrAutoClose;
		private Button btnShowLog;
	}
}
