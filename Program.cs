using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace btp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("ディレクトリが存在しません。");
                return;
            }
            RecursivelySearchDirectories(directory: args[0]);
        }

        /// <summary>
        /// ディレクトリを再帰的に探索します。
        /// </summary>
        /// <param name="directory">探索するディレクトリ。</param>
        private static void RecursivelySearchDirectories(string directory)
        {
            Directory
                .GetFiles(directory)
                .ToList()
                .ForEach(ConvertToPNG);
            Directory
                .GetDirectories(directory)
                .ToList()
                .ForEach(RecursivelySearchDirectories);
        }

        /// <summary>
        /// 画像を PNG ファイルに変換します。
        /// </summary>
        /// <param name="path">画像のパス。</param>
        private static void ConvertToPNG(string path)
        {
            Console.WriteLine(path);
            var directoryName = Path.GetDirectoryName(path);
            var fileName = $"{Path.GetFileNameWithoutExtension(path)}.png";
            using var bmp = new Bitmap(path);
            bmp.Save(Path.Combine(directoryName, fileName), ImageFormat.Png);
        }
    }
}
