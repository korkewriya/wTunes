namespace wTunes
{
    partial class wTunes
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.listView_pl = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox_Device = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // listView_pl
            // 
            this.listView_pl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_pl.HideSelection = false;
            this.listView_pl.Location = new System.Drawing.Point(12, 52);
            this.listView_pl.Name = "listView_pl";
            this.listView_pl.Size = new System.Drawing.Size(358, 289);
            this.listView_pl.TabIndex = 0;
            this.listView_pl.UseCompatibleStateImageBehavior = false;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(150, 353);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 31);
            this.button1.TabIndex = 1;
            this.button1.Text = "実行";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox_Device
            // 
            this.comboBox_Device.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Device.FormattingEnabled = true;
            this.comboBox_Device.Location = new System.Drawing.Point(12, 15);
            this.comboBox_Device.Name = "comboBox_Device";
            this.comboBox_Device.Size = new System.Drawing.Size(358, 23);
            this.comboBox_Device.TabIndex = 2;
            this.comboBox_Device.SelectionChangeCommitted += new System.EventHandler(this.comboBox_Device_SelectionChangeCommitted);
            // 
            // wTunes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 393);
            this.Controls.Add(this.comboBox_Device);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView_pl);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 9999);
            this.MinimumSize = new System.Drawing.Size(400, 440);
            this.Name = "wTunes";
            this.Text = "wTunes";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_pl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox_Device;
    }
}

