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
        /// <param name="originalFilePath">画像のパス。</param>
        private static void ConvertToPNG(string originalFilePath)
        {
            Console.WriteLine(originalFilePath);
            var directoryName = Path.GetDirectoryName(originalFilePath);
            var temporaryFileName = $"{Path.GetFileNameWithoutExtension(originalFilePath)}_tmp.png";
            var temporaryFilePath = Path.Combine(directoryName, temporaryFileName);
            using (var bmp = new Bitmap(originalFilePath))
            {
                bmp.Save(temporaryFilePath, ImageFormat.Png);
            }
            File.Delete(originalFilePath);
            var newFileName = $"{Path.GetFileNameWithoutExtension(originalFilePath)}.png";
            var newFilePath = Path.Combine(directoryName, newFileName);
            File.Move(temporaryFilePath, newFilePath);
        }
    }
}
