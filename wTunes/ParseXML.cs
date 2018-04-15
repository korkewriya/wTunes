using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wTunes
{
    public class ParseXML
    {
        private XDocument xdoc;

        public ParseXML(string xmlFile)
        {
            xdoc = XDocument.Load(xmlFile);
        }

        // iTunesライブラリに登録された曲の詳細情報を取得する
        public List<Dictionary<string, string>> GetSongList()
        {
            var songList = new List<Dictionary<string, string>>();

            var xelements = xdoc.Root.Elements();
            foreach(var elem in xelements)
            {
                var songs = elem.Element("dict").Elements("dict");
                foreach(var song in songs) {
                    var keys = song.Elements("key");
                    var subDict = new Dictionary<string, string>();
                    foreach (var key in keys)
                    {
                        subDict[key.Value] = key.ElementsAfterSelf().FirstOrDefault().Value;
                    }
                    songList.Add(subDict);
                }
            }

            return songList;
        }

        public List<Playlist> GetPlayLists()
        {
            var playLists = new List<Playlist>();

            var xelements = xdoc.Root.Elements();
            foreach (var elem in xelements)     // 最上位要素
            {
                var playlists = elem.Element("array").Elements("dict"); // array -> dictでプレイリスト一覧
                foreach (var pl in playlists)
                {
                    var keys = pl.Elements("key");
                    string plName = "";
                    string plId = "";
                    foreach(var key in keys)
                    {
                        if (key.Value == "Name")
                            plName = key.ElementsAfterSelf().FirstOrDefault().Value;
                        else if (key.Value == "Playlist Persistent ID")
                            plId = key.ElementsAfterSelf().FirstOrDefault().Value;
                    }
                    if(plName != "" && plId != "")
                    {
                        var tempPlaylist = new Playlist(plName, plId, false);
                        playLists.Add(tempPlaylist);
                    }
                }
            }
            return playLists;
        }

        public List<string> GetPlayListDetails(string ppid)
        {
            var list = new List<string>();

            var xelements = xdoc.Root.Elements();
            foreach (var elem in xelements)     // 最上位要素
            {
                var playlists = elem.Element("array").Elements("dict"); // array -> dictでプレイリスト一覧
                foreach (var pl in playlists)
                {
                    var keys = pl.Elements("key");
                    foreach (var key in keys)
                    {
                        if (key.Value == "Playlist Persistent ID" && key.ElementsAfterSelf().FirstOrDefault().Value == ppid)
                        {
                            // TrackIDの取得
                            var songArray = pl.Element("array");
                            if (songArray != null)
                            {
                                var songLists = songArray.Elements("dict");
                                foreach (var song in songLists)
                                {
                                    var songID = song.Element("integer").Value;
                                    list.Add(songID);
                                }
                            }
                            return list;
                        }
                    }
                }
            }
            return list;
        }
    }
}
