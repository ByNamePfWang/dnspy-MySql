namespace HeatMeterPrePay
{
	// Token: 0x02000002 RID: 2
	public partial class AboutForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002076 File Offset: 0x00000276
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002098 File Offset: 0x00000298
		private void InitializeComponent()
		{
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.versionLabel = new global::System.Windows.Forms.Label();
			this.button1 = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("SimSun", 14.25f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label1.Location = new global::System.Drawing.Point(109, 26);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(0, 19);
			this.label1.TabIndex = 0;
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(145, 73);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(149, 12);
			this.label2.TabIndex = 0;
			this.label2.Text = "CopyRight ©2016 版权所有";
			this.versionLabel.AutoSize = true;
			this.versionLabel.Location = new global::System.Drawing.Point(152, 113);
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.Size = new global::System.Drawing.Size(131, 12);
			this.versionLabel.TabIndex = 0;
			this.versionLabel.Text = "Version:1.0(20161122)";
			this.button1.Location = new global::System.Drawing.Point(174, 158);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "确定";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(439, 204);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.versionLabel);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "AboutForm";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "关于系统";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000001 RID: 1
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000002 RID: 2
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000003 RID: 3
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000004 RID: 4
		private global::System.Windows.Forms.Label versionLabel;

		// Token: 0x04000005 RID: 5
		private global::System.Windows.Forms.Button button1;
	}
}
