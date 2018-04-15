using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Forms;

namespace wTunes
{
    public partial class wTunes : Form
    {
        List<Dictionary<string, string>> songList;
        List<Playlist> playLists;
        List<Dictionary<string, string>> transferSongList;
        ParseXML xml;
        string iTunes;

        string errorLog;
        string savePath = "";

        int transferCounter = 0;

        public static bool processCanceled = false;

        public wTunes()
        {
            InitializeComponent();

            iTunes = GetMusicExcel();
            if (iTunes == null)
            {
                MessageBox.Show("iTunes Music Library.xmlが見つかりません。",
                                "エラー",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            ReadiTunesXML();

            listView_pl.View = View.Details;
            listView_pl.CheckBoxes = true;
            listView_pl.Columns.Add("プレイリスト", 240, HorizontalAlignment.Left);

            foreach (var pl in playLists)
            {
                listView_pl.Items.Add(pl.name);
            }

            // デバイス一覧の表示
            ReloadDevice();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            errorLog = "";

            if(comboBox_Device.SelectedValue == null)
            {
                MessageBox.Show("Walkmanを接続してください。",
                                "エラー",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            ReadiTunesXML();

            savePath = Path.Combine(comboBox_Device.SelectedValue.ToString(), "Music");
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            transferSongList = new List<Dictionary<string, string>>();

            // 進捗フォームの作成
            var progressFormPlaylist = new ProgressDialog();
            var progressFormTaskPlaylist = progressFormPlaylist.ShowDialogAsync();

            // iTunesプレイリスト単位で、m3uプレイリストの作成
            RemovePlaylists();  // 予め古いプレイリストを削除しておく
            foreach (ListViewItem playlist in listView_pl.CheckedItems)
            {
                var progress = await MakePlaylist(playlist);
                progressFormPlaylist.updateProgressBar(progress, -1);
            }
            progressFormPlaylist.Close();
            progressFormPlaylist = null;
            await progressFormTaskPlaylist;

            // 重複を削除
            transferSongList = transferSongList.Distinct().ToList();


            // 進捗フォームの作成
            var progressFormTransfer = new ProgressDialog();
            var progressFormTaskTransfer = progressFormTransfer.ShowDialogAsync();

            // 楽曲の転送
            transferCounter = 0;
            int songCount = transferSongList.Count;
            foreach (var song in transferSongList)
            {
                var result = await TransferSongs(song);
                progressFormTransfer.updateProgressBar(result, songCount);
            }
            progressFormTransfer.Close();
            progressFormTransfer = null;
            await progressFormTaskTransfer;


            // 旧データの整理
            var data = Path.Combine(savePath, "iTunesData");
            if (File.Exists(data))
            {
                var oldSongs = (List<Dictionary<string, string>>)Tools.LoadFromBinaryFile(data);

                var removeSongs = new List<Dictionary<string, string>>();
                var persistentIDs = transferSongList.Select(song => song["Persistent ID"]).ToList();
                foreach (var song in oldSongs)
                {
                    if(!persistentIDs.Contains(song["Persistent ID"]))
                    {
                        removeSongs.Add(song);
                    }
                }

                foreach (var song in removeSongs)
                {
                    var dst = Path.Combine(savePath, song["dest"]);
                    if (File.Exists(dst))
                    {
                        File.Delete(dst);
                    }
                }
            }

            //空フォルダの削除
            Tools.RemoveBlankFolder(savePath);

            Tools.SaveToBinaryFile(transferSongList, data);
            SavePlaylistCondition();

            MessageBox.Show("楽曲の転送が完了しました。",
                            "メッセージ",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

            // エラーログの出力
            if (errorLog != "")
            {
                string logPath = Path.Combine(savePath, "error.log");
                using (StreamWriter sw = new StreamWriter(logPath, false))
                {
                    sw.Write(errorLog);
                }
                MessageBox.Show("楽曲の転送でエラーが発生しています。",
                                "エラー",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                System.Diagnostics.Process.Start(logPath);
            }

            ReloadDevice();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = Properties.Settings.Default.F1State;
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
            this.Location = Properties.Settings.Default.F1Location;
            this.Size = Properties.Settings.Default.F1Size;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.F1State = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
            {
                // ウインドウステートがNormalな場合には位置（location）とサイズ（size）を記憶する。
                Properties.Settings.Default.F1Location = this.Location;
                Properties.Settings.Default.F1Size = this.Size;
            }
            else
            {
                // もし最小化（minimized）や最大化（maximized）の場合には、RestoreBoundsを記憶する。
                Properties.Settings.Default.F1Location = this.RestoreBounds.Location;
                Properties.Settings.Default.F1Size = this.RestoreBounds.Size;
            }

            // ここで設定を保存する
            Properties.Settings.Default.Save();
        }

        private void comboBox_Device_SelectionChangeCommitted(object sender, EventArgs e)
        {
            savePath = Path.Combine(comboBox_Device.SelectedValue.ToString(), "Music");
            LoadPlaylistCondition();
        }
    }
}
