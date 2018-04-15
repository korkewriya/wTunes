using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace wTunes
{
    public class M3UPlayList
    {
        List<string> m3u = new List<string>();
        List<Dictionary<string, string>> songList;

        public M3UPlayList(List<Dictionary<string, string>> songList)
        {
            this.songList = songList;
        }

        public List<Dictionary<string, string>> MakePlayList(string savePath)
        {
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            using (StreamWriter writer = new StreamWriter(savePath, false, sjisEnc))
            {
                writer.WriteLine("#EXTM3U");
                foreach (var song in songList)
                {
                    string songPath = MakeSongPath(song);
                    string meta = string.Format("#EXINF:{0},{1}", Tools.MillisecondToSecond(song["Total Time"]),
                                                                  song["Name"]);
                    song["dest"] = songPath;
                    writer.WriteLine(meta);
                    writer.WriteLine(songPath);
                }
            }
            return songList;
        }

        private string MakeSongPath(Dictionary<string, string> song)
        {
            string filename = Path.GetFileName(Tools.MakeValidPath(song.GetOrDefault("Location", "不明")));
            string path = Tools.RemoveInvalidChars(song.GetOrDefault("Artist", "不明なアーティスト")) + "\\"
                        + Tools.RemoveInvalidChars(song.GetOrDefault("Album", "不明なアルバム")) + "\\"
                        + Tools.RemoveInvalidChars(filename);
            return path;
        }


    }
}
