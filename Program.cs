using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
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
            RecursivelySearchDirectories(directoryName: args[0]);
        }

        private static readonly string[] TargetExtensions = { ".bmp", ".png" };

        /// <summary>
        /// ディレクトリを再帰的に探索します。
        /// </summary>
        /// <param name="directoryName">探索するディレクトリ。</param>
        private static void RecursivelySearchDirectories(string directoryName)
        {
            Directory
                .GetFiles(directoryName)
                .Where(IsTargetExtensionFile)
                .ToList()
                .ForEach(ConvertToPNG);
            Directory
                .GetDirectories(directoryName)
                .ToList()
                .ForEach(RecursivelySearchDirectories);
        }

        /// <summary>
        /// 指定されたファイルの拡張子が対象の拡張子かどうか示す値を取得します。
        /// </summary>
        /// <param name="fileName">ファイル名。</param>
        /// <returns>対象の拡張子の場合 true。</returns>
        private static bool IsTargetExtensionFile(string fileName)
        {
            return TargetExtensions
                .Any(o => o == Path.GetExtension(fileName).ToLower(CultureInfo.CurrentCulture));
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
