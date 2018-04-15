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
        public class ItemSet
        {
            // DisplayMemberとValueMemberにはプロパティで指定する仕組み
            public String ItemDisp { get; set; }
            public String ItemValue { get; set; }

            // プロパティをコンストラクタでセット
            public ItemSet(String v, String s)
            {
                ItemDisp = s;
                ItemValue = v;
            }
        }

        enum WINDOW_MESSAGES : uint
        {
            WM_DEVICECHANGE = 0x0219,
        }

        protected override void WndProc(ref Message m)
        {
            switch ((WINDOW_MESSAGES)m.Msg)
            {
                case WINDOW_MESSAGES.WM_DEVICECHANGE:
                    ReloadDevice();
                    break;
            }
            base.WndProc(ref m);
        }

        private void ReloadDevice()
        {
            comboBox_Device.DataSource = CheckLocalDrive();
            comboBox_Device.DisplayMember = "ItemDisp";
            comboBox_Device.ValueMember = "ItemValue";

            int walkmanID = comboBox_Device.FindString("WALKMAN");
            if (walkmanID != -1)
            {
                comboBox_Device.SelectedIndex = walkmanID;
            }

            if (comboBox_Device.SelectedValue != null)
            {
                savePath = Path.Combine(comboBox_Device.SelectedValue.ToString(), "Music");
            }
            LoadPlaylistCondition();
        }

        private string GetMusicExcel()
        {
            string musicDir = System.Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            string iTunesXml = Path.Combine(musicDir, "iTunes", "iTunes Music Library.xml");
            Console.WriteLine(iTunesXml);
            if (File.Exists(iTunesXml))
                return iTunesXml;
            return null;
        }

        private void ReadiTunesXML()
        {
            xml = new ParseXML(iTunes);
            songList = xml.GetSongList();
            playLists = xml.GetPlayLists();
        }

        private List<ItemSet> CheckLocalDrive()
        {
            var driveList = new List<ItemSet>();

            try
            {
                string[] drives = Directory.GetLogicalDrives();
                foreach (string s in drives)
                {
                    DriveInfo drive = new DriveInfo(s);
                    if (drive.DriveType == DriveType.Removable)
                    {
                        string showText = string.Format("{0}({1}) {2}/{3}",
                                                        drive.VolumeLabel, drive.Name,
                                                        Tools.FormatSize(drive.TotalSize - drive.AvailableFreeSpace, 2),
                                                        Tools.FormatSize(drive.TotalSize, 2));
                        driveList.Add(new ItemSet(drive.Name, showText));
                    }
                }
            }
            catch(System.IO.IOException)
            {
                return CheckLocalDrive();
            }

            return driveList;
        }

        private void SavePlaylistCondition()
        {
            var data = Path.Combine(savePath, "playList");
            foreach (ListViewItem playlist in listView_pl.CheckedItems)
            {
                foreach (var pl in playLists)
                {
                    if (pl.name == playlist.Text)
                    {
                        pl.isSelected = true;
                    }
                }
            }
            Tools.SaveToBinaryFile(playLists, data);
        }

        private void LoadPlaylistCondition()
        {
            var data = Path.Combine(savePath, "playList");
            if (File.Exists(data))
            {
                var oldPlaylist = (List<Playlist>)Tools.LoadFromBinaryFile(data);
                var checkedItem = oldPlaylist.Where(pl => pl.isSelected == true)
                                             .ToList();

                foreach (var pl in checkedItem)
                {
                    foreach (ListViewItem playlist in listView_pl.Items)
                    {
                        if (pl.name == playlist.Text)
                        {
                            playlist.Checked = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (ListViewItem playlist in listView_pl.Items)
                {
                    playlist.Checked = false;
                }
            }
        }

        private void RemovePlaylists()
        {
            foreach (var file in Directory.GetFiles(savePath, @"*.m3u"))
            {
                File.Delete(file);
            }
        }

        private async Task<ProgressValue> TransferSongs(Dictionary<string, string> song)
        {
            ProgressValue values = new ProgressValue();

            int max = transferSongList.Count;
            await Task.Run(() =>
            {
                var src = Tools.MakeValidPath(song["Location"]);
                var name = song["Name"];
                var artist = song.GetOrDefault("Artist", "不明なアーティスト");
                var dst = Path.Combine(savePath, song["dest"]);

                if (!File.Exists(src))
                {
                    errorLog += string.Format("エラー：{0}が見つかりません。\r\n", src);
                }
                else
                {
                    Tools.CopyCreateDir(src, dst);
                }
            });

            values.artist = song.GetOrDefault("Artist", "不明なアーティスト");
            values.songname = song["Name"];
            values.count = ++transferCounter;

            return values;
        }

        private async Task<ProgressValue> MakePlaylist(ListViewItem playlist)
        {
            ProgressValue values = new ProgressValue();

            await Task.Run(() =>
            {
                var plId = playLists.Where(pl => pl.name == playlist.Text)
                                    .FirstOrDefault()
                                    .id;
                var songIDs = xml.GetPlayListDetails(plId);
                var transferPlaylist = new List<Dictionary<string, string>>();

                foreach (var songID in songIDs)
                {
                    var song = songList.Where(s => s["Track ID"] == songID)
                                        .FirstOrDefault();
                    var src = Tools.MakeValidPath(song["Location"]);
                    if (File.Exists(src))
                    {
                        transferPlaylist.Add(song);
                    }
                    else
                    {
                        errorLog += string.Format("エラー：{0}が見つかりません。\r\n", src);
                    }
                }

                var m3u = new M3UPlayList(transferPlaylist);
                transferSongList.AddRange(m3u.MakePlayList(Path.Combine(savePath,
                                                            Tools.RemoveInvalidChars(playlist.Text, ' ') + ".m3u")));
            });

            values.count = -1;
            values.message = playlist.Text + " プレイリストの作成中…";

            return values;
        }
    }

    public struct ProgressValue
    {
        public int count;
        public string artist;
        public string songname;
        public string message;
    }

    [Serializable]
    public class Playlist
    {
        public string name { get; set; }
        public string id { get; set; }
        public bool isSelected { get; set; }

        public Playlist(string _name, string _id, bool _isSelected)
        {
            name = _name;
            id = _id;
            isSelected = _isSelected;
        }
    }

    internal static class DialogExt 
    {
        public static async Task<DialogResult> ShowDialogAsync(this Form @this)
        {
            await Task.Yield();
            if (@this.IsDisposed)
                return DialogResult.OK;
            return @this.ShowDialog();
        }
    }
}