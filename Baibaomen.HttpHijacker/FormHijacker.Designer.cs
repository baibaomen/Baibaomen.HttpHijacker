namespace Baibaomen.HttpHijacker
{
    partial class FormHijacker
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbMsg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnHijack = new System.Windows.Forms.Button();
            this.lstSessions = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbMsg
            // 
            this.lbMsg.AutoSize = true;
            this.lbMsg.ForeColor = System.Drawing.Color.RoyalBlue;
            this.lbMsg.Location = new System.Drawing.Point(13, 10);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(88, 13);
            this.lbMsg.TabIndex = 7;
            this.lbMsg.Text = "正在启动监听...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "监听到的HTTP会话列表：";
            // 
            // btnHijack
            // 
            this.btnHijack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHijack.Location = new System.Drawing.Point(360, 278);
            this.btnHijack.Name = "btnHijack";
            this.btnHijack.Size = new System.Drawing.Size(139, 49);
            this.btnHijack.TabIndex = 5;
            this.btnHijack.Text = "劫持选中HTTP会话";
            this.btnHijack.UseVisualStyleBackColor = true;
            this.btnHijack.Click += new System.EventHandler(this.btnHijack_Click);
            // 
            // lstSessions
            // 
            this.lstSessions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSessions.FormattingEnabled = true;
            this.lstSessions.Location = new System.Drawing.Point(13, 63);
            this.lstSessions.Name = "lstSessions";
            this.lstSessions.Size = new System.Drawing.Size(486, 199);
            this.lstSessions.TabIndex = 4;
            // 
            // FormHijacker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 339);
            this.Controls.Add(this.lbMsg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnHijack);
            this.Controls.Add(this.lstSessions);
            this.Name = "FormHijacker";
            this.Text = "Http Hijacker";
            this.Load += new System.EventHandler(this.FormHijacker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnHijack;
        private System.Windows.Forms.ListBox lstSessions;
    }
}

