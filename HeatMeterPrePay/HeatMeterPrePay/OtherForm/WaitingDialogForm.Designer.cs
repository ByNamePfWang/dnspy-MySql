namespace HeatMeterPrePay.OtherForm
{
	// Token: 0x0200001A RID: 26
	public partial class WaitingDialogForm : global::System.Windows.Forms.Form
	{
		// Token: 0x06000207 RID: 519 RVA: 0x0000B9F4 File Offset: 0x00009BF4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000208 RID: 520 RVA: 0x0000BA14 File Offset: 0x00009C14
		private void InitializeComponent()
		{
			this.loadingCircle1 = new global::CNPOPSOFT.Controls.LoadingCircle();
			this.label1 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this.loadingCircle1.Active = false;
			this.loadingCircle1.Color = global::System.Drawing.Color.DarkGray;
			this.loadingCircle1.InnerCircleRadius = 8;
			this.loadingCircle1.Location = new global::System.Drawing.Point(31, 37);
			this.loadingCircle1.Name = "loadingCircle1";
			this.loadingCircle1.NumberSpoke = 10;
			this.loadingCircle1.OuterCircleRadius = 10;
			this.loadingCircle1.RotationSpeed = 100;
			this.loadingCircle1.Size = new global::System.Drawing.Size(75, 23);
			this.loadingCircle1.SpokeThickness = 4;
			this.loadingCircle1.StylePreset = global::CNPOPSOFT.Controls.LoadingCircle.StylePresets.MacOSX;
			this.loadingCircle1.TabIndex = 0;
			this.loadingCircle1.Text = "loadingCircle1";
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("SimSun", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 134);
			this.label1.Location = new global::System.Drawing.Point(102, 40);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(96, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "正在加载...";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.SystemColors.ControlLightLight;
			base.ClientSize = new global::System.Drawing.Size(261, 90);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.loadingCircle1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.MaximizeBox = false;
			base.Name = "WaitingDialogForm";
			base.ShowInTaskbar = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "WaitingDialogForm";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400011B RID: 283
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400011C RID: 284
		private global::CNPOPSOFT.Controls.LoadingCircle loadingCircle1;

		// Token: 0x0400011D RID: 285
		private global::System.Windows.Forms.Label label1;
	}
}
