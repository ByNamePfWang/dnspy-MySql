using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.Util;
using System.Text;
using Newtonsoft.Json.Linq;
using GwInfoPay.Pay.HeMaPay;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
    // Token: 0x02000037 RID: 55
    public class RefundFailedPage : UserControl
    {
        // Token: 0x0600038F RID: 911 RVA: 0x000293E2 File Offset: 0x000275E2
        public RefundFailedPage()
        {
            this.InitializeComponent();
        }
        private DbUtil db = new DbUtil();
        private Button button1;
        private GroupBox groupBox1;
        private Label label1;
        private Button button3;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox_refund_request_no;
        private Button button2;
        private DataGridView allRefundFailedDGV;
        // Token: 0x06000390 RID: 912 RVA: 0x000293FB File Offset: 0x000275FB
        public void setParentForm(MainForm form)
        {
            this.parentForm = form;
        }

        private void RefundFailedPage_Load(object sender, EventArgs e)
        {
            this.textBox_refund_request_no.Visible = false;
            cleanData();
            initDGV();
        }

        /// <summary>
        /// 已退款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string out_order_no = this.textBox1.Text;
            string amount = this.textBox2.Text;
            string order_create_time = strToTime(this.textBox3.Text);
            if (string.IsNullOrWhiteSpace(out_order_no) || string.IsNullOrWhiteSpace(amount) || string.IsNullOrWhiteSpace(order_create_time))
            {
                WMMessageBox.Show(this, "请选择退款错误的数据!");
                return;
            }
            JObject json = new JObject();
            json.Add("operator", MainForm.getStaffId());
            json.Add("operatorTime", DateTime.Now.ToString());
            refundOK(out_order_no, json.ToString());
            cleanData();
            initDGV();
            WMMessageBox.Show(this, "操作成功!");
            return;

        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            string refund_request_no = Guid.NewGuid().ToString();
            this.textBox_refund_request_no.Text = refund_request_no;
            string out_order_no = this.textBox1.Text;
            string amount = this.textBox2.Text;
            string order_create_time = strToTime(this.textBox3.Text);
            if (string.IsNullOrWhiteSpace(out_order_no) || string.IsNullOrWhiteSpace(amount) || string.IsNullOrWhiteSpace(order_create_time))
            {
                WMMessageBox.Show(this, "请选择退款错误的数据!");
                return;
            }
            double refund_fee = double.Parse(amount);
            string refundResult = HeMaPay.Refund(out_order_no, refund_request_no, refund_fee, order_create_time);
            if (string.IsNullOrWhiteSpace(refundResult))
            {
                RefundDB(out_order_no, refundResult, "0");
                WMMessageBox.Show(this, "退款失败,请稍后再试!");
                return;
            }
            else
            {
                JObject retRefundObj = JObject.Parse(refundResult);
                if (retRefundObj["code"].ToString().Equals("200"))
                {
                    JObject retRefundDetailObj = JObject.Parse(retRefundObj["data"].ToString());
                    if (retRefundDetailObj["sub_code"].ToString().Equals("REFUND_SUCCESS"))
                    {
                        refundOK(out_order_no, refundResult);
                        cleanData();
                        initDGV();
                        WMMessageBox.Show(this, "退款成功!");
                        return;
                    }
                    else
                    {
                        RefundDB(out_order_no, refundResult, "0");
                        showCauseOfError(retRefundDetailObj);
                        WMMessageBox.Show(this, "退款失败!");
                        return;
                    }
                }
                else
                {
                    RefundDB(out_order_no, refundResult, "0");
                    showCauseOfError(retRefundObj);
                    WMMessageBox.Show(this, "退款失败!");
                    return;
                }
            }
        }

        private void showCauseOfError(JObject retRefundObj)
        {
            this.groupBox1.Text = "退款错误原因";
            this.label1.Text = retRefundObj.ToString();
        }

        private void refundOK(string out_order_no, string ret = "")
        {
            DbUtil dbUtil = new DbUtil();
            dbUtil.AddParameter("refund_status", "1");
            dbUtil.AddParameter("refund", ret);
            dbUtil.AddParameter("out_order_no", out_order_no);
            int num = dbUtil.ExecuteNonQuery("UPDATE he_ma_pay SET refund_status = @refund_status, refund = @refund WHERE out_order_no =@out_order_no");
            if (num > 0)
            {
                dbUtil.AddParameter("out_order_no", out_order_no);
                dbUtil.ExecuteNonQuery("DELETE FROM he_ma_pay_refund_failed WHERE out_order_no=@out_order_no");
            }
        }

        private void RefundDB(string out_order_no, string ret, string retStatus)
        {
            DbUtil dbUtil = new DbUtil();
            dbUtil.AddParameter("out_order_no", out_order_no);
            dbUtil.AddParameter("refund", ret);
            dbUtil.AddParameter("refund_status", retStatus);
            dbUtil.ExecuteNonQuery("UPDATE he_ma_pay_refund_failed SET refund_status=@refund_status,refund=@refund WHERE out_order_no=@out_order_no");
        }

        private void cleanData()
        {
            this.textBox1.Text = "";
            this.textBox2.Text = "";
            this.textBox3.Text = "";
            this.textBox_refund_request_no.Text = "";
            this.groupBox1.Text = "";
            this.label1.Text = "";
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("订单号"),
                new DataColumn("支付类型"),
                new DataColumn("金额"),
                new DataColumn("创建时间"),
                new DataColumn("订单状态"),
                new DataColumn("失败原因")
            });
            this.allRefundFailedDGV.DataSource = dataTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cleanData();
            initDGV();
        }

        private void initDGV()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("订单号"),
                new DataColumn("支付类型"),
                new DataColumn("金额"),
                new DataColumn("创建时间"),
                new DataColumn("订单状态"),
                new DataColumn("失败原因")
            });

            DataTable dataTable2 = this.db.ExecuteQuery(string.Concat(new string[]
            {
                    "SELECT * FROM he_ma_pay_refund_failed ORDER BY create_time ASC"
            }));
            if (dataTable2 != null)
            {
                for (int i = 0; i < dataTable2.Rows.Count; i++)
                {
                    DataRow dataRow = dataTable2.Rows[i];
                    dataTable.Rows.Add(new object[]
                    {
                            dataRow["out_order_no"],
                            payToStr(dataRow["pay"].ToString()),
                            dataRow["total_amount"].ToString(),
                            timeToStr(dataRow["create_time"].ToString()),
                            statusToStr(dataRow["status"].ToString()),
                            dataRow["refund"].ToString()
                    });

                }
            }
            this.allRefundFailedDGV.DataSource = dataTable;
        }

        private void allRefundFailedDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow currentRow = this.allRefundFailedDGV.CurrentRow;
            if (currentRow != null)
            {
                string out_order_no = (string)currentRow.Cells[0].Value;
                string amount = (string)currentRow.Cells[2].Value;
                this.textBox1.Text = out_order_no;
                this.textBox2.Text = amount;
                this.textBox3.Text = (string)currentRow.Cells[3].Value;
                this.groupBox1.Text = "退款错误原因";
                this.label1.Text = (string)currentRow.Cells[5].Value;
            }
        }

        private string strToTime(string str)
        {
            char[] array = str.ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                string c = array[i].ToString();

                if (!c.Equals(" ") && !c.Equals("-") && !c.Equals(":"))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private string timeToStr(string time)
        {
            char[] array = time.ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append(array[i]);
                if (i == 3 || i == 5)
                {
                    sb.Append("-");
                }
                if (i == 7)
                {
                    sb.Append(" ");
                }
                if (i == 9 || i == 11)
                {
                    sb.Append(":");
                }
            }
            return sb.ToString();
        }

        private string statusToStr(string str)
        {
            if (str.Equals("-1"))
            {
                return "关闭";
            }
            if (str.Equals("0"))
            {
                return "下单成功";
            }
            if (str.Equals("1"))
            {
                return "待支付";
            }
            if (str.Equals("2"))
            {
                return "支付成功";
            }
            return "未 知";
        }
        private string payToStr(string str)
        {
            if (str.Equals("ALIPAY"))
            {
                return "支付宝";
            }
            if (str.Equals("WECHAT"))
            {
                return "微  信";
            }
            return "未  知";
        }

        // Token: 0x0600039A RID: 922 RVA: 0x00029FF7 File Offset: 0x000281F7
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Token: 0x0600039B RID: 923 RVA: 0x0002A018 File Offset: 0x00028218
        private void InitializeComponent()
        {
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.allRefundFailedDGV = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox_refund_request_no = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.allRefundFailedDGV)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label19.Location = new System.Drawing.Point(14, 16);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(135, 20);
            this.label19.TabIndex = 20;
            this.label19.Text = "自助退款失败";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.allRefundFailedDGV);
            this.groupBox2.Location = new System.Drawing.Point(12, 55);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(676, 229);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "明细列表";
            // 
            // allRefundFailedDGV
            // 
            this.allRefundFailedDGV.AllowUserToAddRows = false;
            this.allRefundFailedDGV.BackgroundColor = System.Drawing.SystemColors.Control;
            this.allRefundFailedDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.allRefundFailedDGV.Location = new System.Drawing.Point(6, 16);
            this.allRefundFailedDGV.Name = "allRefundFailedDGV";
            this.allRefundFailedDGV.ReadOnly = true;
            this.allRefundFailedDGV.RowTemplate.Height = 23;
            this.allRefundFailedDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.allRefundFailedDGV.Size = new System.Drawing.Size(664, 207);
            this.allRefundFailedDGV.TabIndex = 22;
            this.allRefundFailedDGV.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.allRefundFailedDGV_CellClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(549, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 25;
            this.button1.Text = "刷  新";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(18, 378);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(664, 170);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(652, 150);
            this.label1.TabIndex = 0;
            this.label1.Text = "   ";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.DarkRed;
            this.button3.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(626, 332);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 28);
            this.button3.TabIndex = 28;
            this.button3.Text = "退款";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 344);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "订单号:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(240, 344);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 29;
            this.label3.Text = "金额:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(334, 344);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 30;
            this.label4.Text = "下单时间:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(59, 339);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(175, 21);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(271, 339);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(55, 21);
            this.textBox2.TabIndex = 31;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(390, 339);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(127, 21);
            this.textBox3.TabIndex = 32;
            // 
            // textBox_refund_request_no
            // 
            this.textBox_refund_request_no.Location = new System.Drawing.Point(336, 301);
            this.textBox_refund_request_no.Name = "textBox_refund_request_no";
            this.textBox_refund_request_no.Size = new System.Drawing.Size(100, 21);
            this.textBox_refund_request_no.TabIndex = 33;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Orange;
            this.button2.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(539, 332);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 28);
            this.button2.TabIndex = 34;
            this.button2.Text = "已退款";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // RefundFailedPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox_refund_request_no);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label19);
            this.Name = "RefundFailedPage";
            this.Size = new System.Drawing.Size(701, 584);
            this.Load += new System.EventHandler(this.RefundFailedPage_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.allRefundFailedDGV)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        private double balance;

        // Token: 0x040002E9 RID: 745
        private MainForm parentForm;

        // Token: 0x040002EB RID: 747
        private ConsumeCardEntity cce;

        // Token: 0x040002EC RID: 748
        private IContainer components;
        private GroupBox groupBox2;

        // Token: 0x0400030D RID: 781
        private Label label19;

    }
}
