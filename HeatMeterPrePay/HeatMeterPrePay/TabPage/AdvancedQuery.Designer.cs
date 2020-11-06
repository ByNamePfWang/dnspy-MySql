namespace HeatMeterPrePay.TabPage
{
	// Token: 0x02000029 RID: 41
	public partial class AdvancedQuery : global::System.Windows.Forms.Form
	{
		// Token: 0x060002A0 RID: 672 RVA: 0x000177B6 File Offset: 0x000159B6
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x000177D8 File Offset: 0x000159D8
		private void InitializeComponent()
		{
			this.typeCB = new global::System.Windows.Forms.ComboBox();
			this.queryItemCB = new global::System.Windows.Forms.ComboBox();
			this.valueTB = new global::System.Windows.Forms.TextBox();
			this.queryStrRTB = new global::System.Windows.Forms.RichTextBox();
			this.valuesCB = new global::System.Windows.Forms.ComboBox();
			this.cancelBtn = new global::System.Windows.Forms.Button();
			this.enterBtn = new global::System.Windows.Forms.Button();
			this.clearBtn = new global::System.Windows.Forms.Button();
			this.orBtn = new global::System.Windows.Forms.Button();
			this.andBtn = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.typeCB.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.typeCB.FormattingEnabled = true;
			this.typeCB.Location = new global::System.Drawing.Point(137, 21);
			this.typeCB.Name = "typeCB";
			this.typeCB.Size = new global::System.Drawing.Size(61, 20);
			this.typeCB.TabIndex = 1;
			this.queryItemCB.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.queryItemCB.FormattingEnabled = true;
			this.queryItemCB.Location = new global::System.Drawing.Point(24, 21);
			this.queryItemCB.Name = "queryItemCB";
			this.queryItemCB.Size = new global::System.Drawing.Size(94, 20);
			this.queryItemCB.TabIndex = 0;
			this.queryItemCB.SelectedIndexChanged += new global::System.EventHandler(this.queryItemCB_SelectedIndexChanged);
			this.valueTB.Location = new global::System.Drawing.Point(226, 20);
			this.valueTB.Name = "valueTB";
			this.valueTB.Size = new global::System.Drawing.Size(133, 21);
			this.valueTB.TabIndex = 2;
			this.queryStrRTB.Location = new global::System.Drawing.Point(18, 57);
			this.queryStrRTB.Name = "queryStrRTB";
			this.queryStrRTB.ReadOnly = true;
			this.queryStrRTB.Size = new global::System.Drawing.Size(259, 210);
			this.queryStrRTB.TabIndex = 2;
			this.queryStrRTB.Text = "";
			this.valuesCB.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.valuesCB.FormattingEnabled = true;
			this.valuesCB.Location = new global::System.Drawing.Point(226, 21);
			this.valuesCB.Name = "valuesCB";
			this.valuesCB.Size = new global::System.Drawing.Size(133, 20);
			this.valuesCB.TabIndex = 1;
			this.valuesCB.Visible = false;
			this.cancelBtn.Image = global::HeatMeterPrePay.Properties.Resources.cancel;
			this.cancelBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.cancelBtn.Location = new global::System.Drawing.Point(295, 244);
			this.cancelBtn.Name = "cancelBtn";
			this.cancelBtn.Size = new global::System.Drawing.Size(75, 23);
			this.cancelBtn.TabIndex = 7;
			this.cancelBtn.Text = "取消";
			this.cancelBtn.UseVisualStyleBackColor = true;
			this.cancelBtn.Click += new global::System.EventHandler(this.cancelBtn_Click);
			this.enterBtn.Image = global::HeatMeterPrePay.Properties.Resources.save;
			this.enterBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.enterBtn.Location = new global::System.Drawing.Point(295, 210);
			this.enterBtn.Name = "enterBtn";
			this.enterBtn.Size = new global::System.Drawing.Size(75, 23);
			this.enterBtn.TabIndex = 6;
			this.enterBtn.Text = "确定";
			this.enterBtn.UseVisualStyleBackColor = true;
			this.enterBtn.Click += new global::System.EventHandler(this.enterBtn_Click);
			this.clearBtn.Image = global::HeatMeterPrePay.Properties.Resources.edit_clear_3_16px_539680_easyicon_net;
			this.clearBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.clearBtn.Location = new global::System.Drawing.Point(295, 128);
			this.clearBtn.Name = "clearBtn";
			this.clearBtn.Size = new global::System.Drawing.Size(75, 23);
			this.clearBtn.TabIndex = 5;
			this.clearBtn.Text = "清除";
			this.clearBtn.UseVisualStyleBackColor = true;
			this.clearBtn.Click += new global::System.EventHandler(this.clearBtn_Click);
			this.orBtn.Image = global::HeatMeterPrePay.Properties.Resources.or;
			this.orBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.orBtn.Location = new global::System.Drawing.Point(295, 92);
			this.orBtn.Name = "orBtn";
			this.orBtn.Size = new global::System.Drawing.Size(75, 23);
			this.orBtn.TabIndex = 4;
			this.orBtn.Text = "或(OR)";
			this.orBtn.TextAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.orBtn.UseVisualStyleBackColor = true;
			this.orBtn.Click += new global::System.EventHandler(this.orBtn_Click);
			this.andBtn.Image = global::HeatMeterPrePay.Properties.Resources.and;
			this.andBtn.ImageAlign = global::System.Drawing.ContentAlignment.MiddleLeft;
			this.andBtn.Location = new global::System.Drawing.Point(295, 57);
			this.andBtn.Name = "andBtn";
			this.andBtn.Size = new global::System.Drawing.Size(75, 23);
			this.andBtn.TabIndex = 3;
			this.andBtn.Text = "与(AND)";
			this.andBtn.TextAlign = global::System.Drawing.ContentAlignment.MiddleRight;
			this.andBtn.UseVisualStyleBackColor = true;
			this.andBtn.Click += new global::System.EventHandler(this.andBtn_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(387, 279);
			base.Controls.Add(this.valuesCB);
			base.Controls.Add(this.cancelBtn);
			base.Controls.Add(this.enterBtn);
			base.Controls.Add(this.clearBtn);
			base.Controls.Add(this.orBtn);
			base.Controls.Add(this.andBtn);
			base.Controls.Add(this.queryStrRTB);
			base.Controls.Add(this.queryItemCB);
			base.Controls.Add(this.typeCB);
			base.Controls.Add(this.valueTB);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.Name = "AdvancedQuery";
			base.ShowIcon = false;
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "高级查询";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040001B0 RID: 432
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040001B1 RID: 433
		private global::System.Windows.Forms.ComboBox typeCB;

		// Token: 0x040001B2 RID: 434
		private global::System.Windows.Forms.ComboBox queryItemCB;

		// Token: 0x040001B3 RID: 435
		private global::System.Windows.Forms.TextBox valueTB;

		// Token: 0x040001B4 RID: 436
		private global::System.Windows.Forms.RichTextBox queryStrRTB;

		// Token: 0x040001B5 RID: 437
		private global::System.Windows.Forms.Button andBtn;

		// Token: 0x040001B6 RID: 438
		private global::System.Windows.Forms.Button orBtn;

		// Token: 0x040001B7 RID: 439
		private global::System.Windows.Forms.Button clearBtn;

		// Token: 0x040001B8 RID: 440
		private global::System.Windows.Forms.Button cancelBtn;

		// Token: 0x040001B9 RID: 441
		private global::System.Windows.Forms.Button enterBtn;

		// Token: 0x040001BA RID: 442
		private global::System.Windows.Forms.ComboBox valuesCB;
	}
}
