using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace wTunes
{
    public static class Tools
    {
        // iTunesXMLのパスを、有効なWindowsパスへ変換する
        public static string MakeValidPath(string path)
        {
            Uri u = new Uri(path);
            if (u.IsFile)
            {
                string windowsPath = u.LocalPath + Uri.UnescapeDataString(u.Fragment);
                Regex rgx = new Regex(@"^\\\\localhost\\");
                return rgx.Replace(windowsPath, "");
            }
            return path;
        }

        public static string RemoveInvalidChars(string filename, char replace = '_')
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Concat(filename.Select(c => invalidChars.Contains(c) ? replace : c));
        }

        public static string FormatSize(long amt, int rounding)
        {
            /// <summary>
            /// ByteをKB, MB, GB...のような他の形式に変換する
            /// KB, MB, GB, TB, PB, EB, ZB or YB
            /// 第１引数:long型
            /// 第２引数：小数点第何位まで表示するか
            /// </summary>

            if (amt >= Math.Pow(2, 80)) return Math.Round(amt
                / Math.Pow(2, 70), rounding).ToString() + " YB"; //yettabyte
            if (amt >= Math.Pow(2, 70)) return Math.Round(amt
                / Math.Pow(2, 70), rounding).ToString() + " ZB"; //zettabyte
            if (amt >= Math.Pow(2, 60)) return Math.Round(amt
                / Math.Pow(2, 60), rounding).ToString() + " EB"; //exabyte
            if (amt >= Math.Pow(2, 50)) return Math.Round(amt
                / Math.Pow(2, 50), rounding).ToString() + " PB"; //petabyte
            if (amt >= Math.Pow(2, 40)) return Math.Round(amt
                / Math.Pow(2, 40), rounding).ToString() + " TB"; //terabyte
            if (amt >= Math.Pow(2, 30)) return Math.Round(amt
                / Math.Pow(2, 30), rounding).ToString() + " GB"; //gigabyte
            if (amt >= Math.Pow(2, 20)) return Math.Round(amt
                / Math.Pow(2, 20), rounding).ToString() + " MB"; //megabyte
            if (amt >= Math.Pow(2, 10)) return Math.Round(amt
                / Math.Pow(2, 10), rounding).ToString() + " KB"; //kilobyte

            return amt.ToString() + " Bytes"; //byte
        }

        public static int MillisecondToSecond(string mSec)
        {
            int j;
            if (Int32.TryParse(mSec, out j))
                return Int32.Parse(mSec) / 1000;
            return 0;
        }

        public static void CopyCreateDir(string src, string dst)
        {
            string dirName = Path.GetDirectoryName(dst);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            if (!File.Exists(dst))
            {
                try
                {
                    File.Copy(src, dst);
                }
                catch (FileNotFoundException)
                {
                }
            }
            else if(!FileCompare(src, dst))
            {
                try
                {
                    File.Delete(dst);
                    File.Copy(src, dst);
                }
                catch (FileNotFoundException)
                {
                }
            }
        }

        public static void SaveToBinaryFile(object obj, string path)
        {
            FileStream fs = new FileStream(path,
                                        FileMode.Create,
                                        FileAccess.Write);
            BinaryFormatter bf = new BinaryFormatter();
            //シリアル化して書き込む
            bf.Serialize(fs, obj);
            fs.Close();
        }

        public static object LoadFromBinaryFile(string path)
        {
            FileStream fs = new FileStream(path,
                                        FileMode.Open,
                                        FileAccess.Read);
            BinaryFormatter f = new BinaryFormatter();
            //読み込んで逆シリアル化する
            object obj = f.Deserialize(fs);
            fs.Close();

            return obj;
        }

        public static void RemoveBlankFolder(string path)
        {
            foreach (var dir in Directory.GetDirectories(path))
            {
                RemoveBlankFolder(dir);
            }

            if (Directory.GetDirectories(path).Length == 0 && Directory.GetFiles(path).Length == 0)
            {
                Directory.Delete(path);
            }

            return;
        }

        private static bool FileCompare(string file1, string file2)
        {
            if (file1 == file2)
            {
                return true;
            }

            var fi1 = new FileInfo(file1);
            var fi2 = new FileInfo(file2);
            if(fi1.Length == fi2.Length)
            {
                return true;
            }

            FileStream fs1 = new FileStream(file1, FileMode.Open);
            FileStream fs2 = new FileStream(file2, FileMode.Open);
            int byte1;
            int byte2;
            bool ret = false;

            try
            {
                do
                {
                    byte1 = fs1.ReadByte();
                    byte2 = fs2.ReadByte();
                }
                while ((byte1 == byte2) && (byte1 != -1));

                if (byte1 == byte2)
                {
                    ret = true;
                }
            }
            catch
            {

            }
            finally
            {
                fs1.Close();
                fs2.Close();
            }
            return ret;
        }
    }
}
