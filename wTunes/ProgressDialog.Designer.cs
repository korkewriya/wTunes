namespace wTunes
{
    partial class ProgressDialog
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
            this.ProgressBarControl = new System.Windows.Forms.ProgressBar();
            this.lbl_Detail = new System.Windows.Forms.Label();
            this.lbl_Count = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ProgressBarControl
            // 
            this.ProgressBarControl.Location = new System.Drawing.Point(15, 49);
            this.ProgressBarControl.Name = "ProgressBarControl";
            this.ProgressBarControl.Size = new System.Drawing.Size(505, 23);
            this.ProgressBarControl.TabIndex = 0;
            // 
            // lbl_Detail
            // 
            this.lbl_Detail.Location = new System.Drawing.Point(12, 18);
            this.lbl_Detail.Name = "lbl_Detail";
            this.lbl_Detail.Size = new System.Drawing.Size(496, 26);
            this.lbl_Detail.TabIndex = 1;
            this.lbl_Detail.Text = "準備中です…";
            // 
            // lbl_Count
            // 
            this.lbl_Count.Location = new System.Drawing.Point(357, 77);
            this.lbl_Count.Name = "lbl_Count";
            this.lbl_Count.Size = new System.Drawing.Size(163, 23);
            this.lbl_Count.TabIndex = 2;
            this.lbl_Count.Text = "0 / 0";
            this.lbl_Count.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // ProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 154);
            this.ControlBox = false;
            this.Controls.Add(this.lbl_Count);
            this.Controls.Add(this.lbl_Detail);
            this.Controls.Add(this.ProgressBarControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "楽曲を転送中";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar ProgressBarControl;
        private System.Windows.Forms.Label lbl_Detail;
        private System.Windows.Forms.Label lbl_Count;
    }
}