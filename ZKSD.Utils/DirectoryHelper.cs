using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKSD.Utils
{
    public static class DirectoryHelper
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Create(string path, bool isCover = false)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                if (isCover)
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        public static bool Exists(string path)
        {
            return Directory.Exists(path);
        }


        public static List<string> GetDirectoryFiles(string directoryPath, string fileFormatSuffix = null)
        {
            if (string.IsNullOrEmpty(fileFormatSuffix))
            {
                if (Exists(directoryPath))
                {
                    var files = Directory.GetFiles(directoryPath);
                    return files?.ToList();
                }
            }
            else
            {
                if (Exists(directoryPath))
                {
                    var files = Directory.GetFiles(directoryPath, fileFormatSuffix);
                    return files?.ToList();
                }
            }

            return null;
        }

        public static List<string> GetDirectories(string directoryPath)
        {
            if (Exists(directoryPath))
            {
                var files = Directory.GetDirectories(directoryPath);
                return files?.ToList();
            }
            return null;
        }

        public static string GetDirectoryName(string directoryPath)
        {
            if (Exists(directoryPath))
            {
                DirectoryInfo directory = new DirectoryInfo(directoryPath);
                return directory.Name;
            }
            return null;

        }


        public static string GetDirectoryLastPath(string directoryPath, int lastCount)
        {

            if (!string.IsNullOrEmpty(directoryPath))
            {
                DirectoryInfo path = new DirectoryInfo(directoryPath);
                for (int i = 0; i < lastCount; i++)
                {
                    path = path.Parent;
                    if (path == null) return null;

                    if (i == lastCount - 1)
                    {
                        return path.FullName;
                    }
                }
            }
            return null;

        }
        /// <summary>
        /// 拷贝目录里的文件
        /// </summary>
        /// <param name="sourceFilePath">源文件目录</param>
        /// <param name="destFilePath">目地文件目录</param>
        public static void CopyDirs(string sourceFilePath, string destFilePath)
        {
            if (Directory.Exists(sourceFilePath))
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加
                if (destFilePath[destFilePath.Length - 1] != Path.DirectorySeparatorChar)
                    destFilePath += Path.DirectorySeparatorChar;

                // 判断目标目录是否存在如果不存在则新建
                // 得到源目录的文件列表,该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles(sourceFilePath);  
                if (!Directory.Exists(destFilePath))
                    Directory.CreateDirectory(destFilePath);

                string[] fileList = Directory.GetFileSystemEntries(sourceFilePath);

                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件，否则直接Copy文件
                    if (Directory.Exists(file))
                        CopyDirs(file, destFilePath + Path.GetFileName(file));
                    else
                        System.IO.File.Copy(file, destFilePath + Path.GetFileName(file), true);
                }
            }
        }
        public static void CopyFile(string sourceFilePath, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            if (!File.Exists(sourceFilePath))
                throw new Exception($"{sourceFilePath}文件不存在");
            System.IO.File.Copy(sourceFilePath, $"{destFolder}\\{Path.GetFileName(sourceFilePath)}", true);
        }
        public static async Task CopyDirsAsync(string sourceFilePath, string destFilePath, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (sourceFilePath.Contains("\\\\"))
                sourceFilePath = sourceFilePath.Replace("\\\\", "\\");
            if (Directory.Exists(sourceFilePath))
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加
                if (destFilePath[destFilePath.Length - 1] != Path.DirectorySeparatorChar)
                    destFilePath += Path.DirectorySeparatorChar;

                // 判断目标目录是否存在如果不存在则新建
                // 得到源目录的文件列表,该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles(sourceFilePath);  
                if (!Directory.Exists(destFilePath))
                    Directory.CreateDirectory(destFilePath);

                string[] fileList = Directory.GetFileSystemEntries(sourceFilePath);

                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件，否则直接Copy文件
                    if (Directory.Exists(file))
                        await CopyDirsAsync(file, destFilePath + Path.GetFileName(file), cancellationToken);
                    else
                        await CopyFileAsync(file, destFilePath + Path.GetFileName(file), true, cancellationToken: cancellationToken);
                }
            }
            else
            {
                Log.Info($"源路径{sourceFilePath}不存在，复制失败！");
            }
        }
        public static async Task CopyFileAsync(string sourceFileName, string destinationFileName,
            bool overwrite = true, int bufferSize = 0xffff, CancellationToken cancellationToken = default(CancellationToken))
        {

            if (!File.Exists(sourceFileName))
            {
                Log.Info($"源文件{sourceFileName}不存在，复制失败！");
                return;
            }
            using (var sourceFile = File.OpenRead(sourceFileName))
            {
                var dstext = Path.GetExtension(destinationFileName);
                if (string.IsNullOrEmpty(dstext))
                {
                    var srcname = Path.GetFileName(sourceFileName);
                    destinationFileName = Path.Combine(destinationFileName, srcname);
                    var destfolder = Path.GetDirectoryName(destinationFileName);
                    if (!Directory.Exists(destfolder))
                        Directory.CreateDirectory(destfolder);
                }
                if (File.Exists(destinationFileName))
                {
                    using (var destinationFile = File.OpenWrite(destinationFileName))
                    {
                        await sourceFile.CopyToAsync(destinationFile, bufferSize, cancellationToken);
                        Log.Info($"源文件{destinationFileName}复制成功");
                    }
                }
                else
                {
                    using (var destinationFile = File.Create(destinationFileName))
                    {
                        await sourceFile.CopyToAsync(destinationFile, bufferSize, cancellationToken);
                        Log.Info($"源文件{destinationFileName}复制成功");
                    }
                }
            }
        }

        public static string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
    }
}
