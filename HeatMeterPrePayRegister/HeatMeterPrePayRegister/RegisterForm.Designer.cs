namespace HeatMeterPrePayRegister
{
    partial class RegisterForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.registerBtn = new System.Windows.Forms.Button();
            this.registerStringTB = new System.Windows.Forms.TextBox();
            this.hardwareInfoTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelll = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // registerBtn
            // 
            this.registerBtn.Location = new System.Drawing.Point(323, 206);
            this.registerBtn.Name = "registerBtn";
            this.registerBtn.Size = new System.Drawing.Size(75, 23);
            this.registerBtn.TabIndex = 14;
            this.registerBtn.Text = "生成";
            this.registerBtn.UseVisualStyleBackColor = true;
            this.registerBtn.Click += new System.EventHandler(this.registerBtn_Click);
            // 
            // registerStringTB
            // 
            this.registerStringTB.Location = new System.Drawing.Point(86, 130);
            this.registerStringTB.Name = "registerStringTB";
            this.registerStringTB.Size = new System.Drawing.Size(312, 21);
            this.registerStringTB.TabIndex = 13;
            this.registerStringTB.Text = "1000";
            // 
            // hardwareInfoTB
            // 
            this.hardwareInfoTB.Location = new System.Drawing.Point(86, 90);
            this.hardwareInfoTB.Name = "hardwareInfoTB";
            this.hardwareInfoTB.Size = new System.Drawing.Size(312, 21);
            this.hardwareInfoTB.TabIndex = 12;
            this.hardwareInfoTB.Text = "1000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "注册号";
            // 
            // labelll
            // 
            this.labelll.AutoSize = true;
            this.labelll.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelll.Location = new System.Drawing.Point(127, 41);
            this.labelll.Name = "labelll";
            this.labelll.Size = new System.Drawing.Size(149, 19);
            this.labelll.TabIndex = 9;
            this.labelll.Text = "软件注册码生成";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "序列号";
            // 
            // RegisterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 285);
            this.Controls.Add(this.registerBtn);
            this.Controls.Add(this.registerStringTB);
            this.Controls.Add(this.hardwareInfoTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelll);
            this.Controls.Add(this.label1);
            this.Name = "RegisterForm";
            this.Text = "预付费热量表管理软件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button registerBtn;
        private System.Windows.Forms.TextBox registerStringTB;
        private System.Windows.Forms.TextBox hardwareInfoTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelll;
        private System.Windows.Forms.Label label1;
    }
}

