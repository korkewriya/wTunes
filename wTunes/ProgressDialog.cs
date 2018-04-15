using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wTunes
{
    public partial class ProgressDialog : Form
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        public void updateProgressBar(ProgressValue data, int maxProcess)
        {
            if(0 <= data.count)  // 楽曲転送用
            {
                ProgressBarControl.Value = (int)(data.count * 100 / (float)maxProcess);
                lbl_Detail.Text = string.Format("{0} / {1} を転送中…", data.artist, data.songname);
                lbl_Count.Text = string.Format("{0} / {1}", data.count + 1, maxProcess);
            }
            else  // その他用途
            {
                ProgressBarControl.Value = 0;
                lbl_Detail.Text = data.message;
                lbl_Count.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            wTunes.processCanceled = true;
        }
    }
}
