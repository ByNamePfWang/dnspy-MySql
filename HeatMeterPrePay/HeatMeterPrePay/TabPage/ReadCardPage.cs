using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using HeatMeterPrePay.CardEntity;
using HeatMeterPrePay.Properties;
using HeatMeterPrePay.Util;
using HeatMeterPrePay.Widget;

namespace HeatMeterPrePay.TabPage
{
    // Token: 0x02000036 RID: 54
    public class ReadCardPage : UserControl
    {
        // Token: 0x06000389 RID: 905 RVA: 0x00028DC2 File Offset: 0x00026FC2
        public ReadCardPage()
        {
            this.InitializeComponent();
        }

        // Token: 0x0600038A RID: 906 RVA: 0x00028DD0 File Offset: 0x00026FD0
        public void setParentForm(MainForm form)
        {
            this.parentForm = form;
        }

        // Token: 0x0600038B RID: 907 RVA: 0x00028DDC File Offset: 0x00026FDC
        private void readCardBtn_Click(object sender, EventArgs e)
        {
            if (this.parentForm.isEmptyCard())
            {
                this.messageRichTextBox.Text = "空白卡";
                return;
            }
            uint[] array = this.parentForm.readCard();
            if (array != null)
            {
                string text = "";
                uint cardType = this.parentForm.getCardType(array[0]);
                uint num = cardType;
                switch (num)
                {
                    case 1U:
                        {
                            ConsumeCardEntity consumeCardEntity = new ConsumeCardEntity();
                            consumeCardEntity.parseEntity(array);
                            string value = string.Concat(consumeCardEntity.UserId);
                            DbUtil dbUtil = new DbUtil();
                            dbUtil.AddParameter("userId", value);
                            DataRow dataRow = dbUtil.ExecuteRow("SELECT * FROM metersTable WHERE meterId=@userId");
                            if (dataRow == null)
                            {
                                WMMessageBox.Show(this, "没有找到相应的表信息！");
                            }
                            else
                            {
                                dbUtil.AddParameter("permanentUserId", dataRow["permanentUserId"].ToString());
                                DataRow dataRow2 = dbUtil.ExecuteRow("SELECT * FROM usersTable WHERE permanentUserId=@permanentUserId");
                                if (dataRow2 != null)
                                {
                                    text = string.Concat(new string[]
                                    {
                                "设备号：",
                                dataRow2["userId"].ToString(),
                                "\n姓名：",
                                dataRow2["username"].ToString(),
                                "\n联系方式：",
                                dataRow2["phoneNum"].ToString(),
                                "\n证件号码：",
                                dataRow2["identityId"].ToString(),
                                "\n地址：",
                                dataRow2["address"].ToString(),
                                "\n用户面积：",
                                dataRow2["userArea"].ToString(),
                                "\n人口数：",
                                dataRow2["userPersons"].ToString()
                                    });
                                }
                            }
                            text = text + "\n\n" + consumeCardEntity.ToString();
                            goto IL_2A5;
                        }
                    case 2U:
                        {
                            TransCardEntity transCardEntity = new TransCardEntity();
                            transCardEntity.parseEntity(array);
                            text = transCardEntity.ToString();
                            goto IL_2A5;
                        }
                    case 3U:
                        {
                            RefundCardEntity refundCardEntity = new RefundCardEntity();
                            refundCardEntity.parseEntity(array);
                            text = refundCardEntity.ToString();
                            goto IL_2A5;
                        }
                    case 4U:
                        {
                            SettingCardEntity settingCardEntity = new SettingCardEntity();
                            settingCardEntity.parseEntity(array);
                            text = settingCardEntity.ToString();
                            goto IL_2A5;
                        }
                    case 5U:
                        {
                            ClearCardEntity clearCardEntity = new ClearCardEntity();
                            clearCardEntity.parseEntity(array);
                            text = clearCardEntity.ToString();
                            goto IL_2A5;
                        }
                    case 6U:
                        break;
                    case 7U:
                    case 8U:
                        {
                            ForceValveOperationCardEntity forceValveOperationCardEntity = new ForceValveOperationCardEntity();
                            forceValveOperationCardEntity.parseEntity(array);
                            text = forceValveOperationCardEntity.ToString();
                            goto IL_2A5;
                        }
                    default:
                        if (num != 31U)
                        {
                            goto IL_2A5;
                        }
                        break;
                }
                CheckCardEntityV3 checkCardEntityV = new CheckCardEntityV3();
                checkCardEntityV.parseEntity(array);
                text = checkCardEntityV.ToString();
                IL_2A5:
                this.messageRichTextBox.Text = text;
            }
        }

        // Token: 0x0600038C RID: 908 RVA: 0x0002909C File Offset: 0x0002729C
        private string parseUserCard(uint[] datas)
        {
            return "";
        }

        // Token: 0x0600038D RID: 909 RVA: 0x000290B0 File Offset: 0x000272B0
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Token: 0x0600038E RID: 910 RVA: 0x000290D0 File Offset: 0x000272D0
        private void InitializeComponent()
        {
            this.readCardBtn = new Button();
            this.label19 = new Label();
            this.messageRichTextBox = new RichTextBox();
            this.label36 = new Label();
            base.SuspendLayout();
            this.readCardBtn.Image = Resources.read;
            this.readCardBtn.ImageAlign = ContentAlignment.MiddleLeft;
            this.readCardBtn.Location = new Point(305, 526);
            this.readCardBtn.Name = "readCardBtn";
            this.readCardBtn.Size = new Size(87, 29);
            this.readCardBtn.TabIndex = 11;
            this.readCardBtn.Text = "读卡";
            this.readCardBtn.UseVisualStyleBackColor = true;
            this.readCardBtn.Click += this.readCardBtn_Click;
            this.label19.AutoSize = true;
            this.label19.Font = new Font("SimSun", 15f, FontStyle.Bold, GraphicsUnit.Point, 134);
            this.label19.Location = new Point(16, 16);
            this.label19.Name = "label19";
            this.label19.Size = new Size(51, 20);
            this.label19.TabIndex = 12;
            this.label19.Text = "读卡";
            this.messageRichTextBox.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.messageRichTextBox.Location = new Point(20, 67);
            this.messageRichTextBox.Name = "messageRichTextBox";
            this.messageRichTextBox.ReadOnly = true;
            this.messageRichTextBox.Size = new Size(641, 417);
            this.messageRichTextBox.TabIndex = 13;
            this.messageRichTextBox.Text = "";
            this.label36.AutoSize = true;
            this.label36.Font = new Font("SimSun", 12f, FontStyle.Regular, GraphicsUnit.Point, 134);
            this.label36.ForeColor = SystemColors.Highlight;
            this.label36.Location = new Point(88, 19);
            this.label36.Name = "label36";
            this.label36.Size = new Size(136, 16);
            this.label36.TabIndex = 33;
            this.label36.Text = "读出卡片所有数据";
            this.label36.Visible = false;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.Controls.Add(this.label36);
            base.Controls.Add(this.messageRichTextBox);
            base.Controls.Add(this.readCardBtn);
            base.Controls.Add(this.label19);
            base.Name = "ReadCardPage";
            base.Size = new Size(701, 584);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        // Token: 0x040002E1 RID: 737
        private MainForm parentForm;

        // Token: 0x040002E2 RID: 738
        private IContainer components;

        // Token: 0x040002E3 RID: 739
        private Button readCardBtn;

        // Token: 0x040002E4 RID: 740
        private Label label19;

        // Token: 0x040002E5 RID: 741
        private RichTextBox messageRichTextBox;

        // Token: 0x040002E6 RID: 742
        private Label label36;
    }
}
