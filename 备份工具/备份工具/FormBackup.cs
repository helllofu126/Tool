using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloFu.BackupTool
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FormBackup : Form
    {
        /// <summary>
        /// 备份工具窗体
        /// </summary>
        public FormBackup()
        {
            InitializeComponent();

            userControlSelectCopyFilePath.LabelListBoxName = "备份文件的路径";
            userControlSelectCopyFileBackup.LabelListBoxName = "保存备份的路径";
        }

        /// <summary>
        /// 方法：将备份的文件压缩成zip文件
        /// </summary>
        /// <param name="sourceFilePathList"></param>
        /// <param name="destinationZipFilePathList"></param>
        /// <param name="backupName"></param>
        private async void ZipFile(List<string> sourceFilePathList, List<string> destinationZipFilePathList, string backupName)
        {
            //开始备份显示
            SetLabelBackup(false);

            // 确保目标ZIP文件所在的目录存在
            foreach (var directoryPath in destinationZipFilePathList)
            {
                //获取ZIP文件的路径
                var zipFilePath = Path.GetFullPath(directoryPath + backupName);

                //获取ZIP文件的目录路径
                var zipDirectoryPath = Path.GetDirectoryName(directoryPath);

                if (!Directory.Exists(zipDirectoryPath))
                {
                    Directory.CreateDirectory(zipDirectoryPath);
                }

                //创建ZIP文件
                await CreateZipFile(sourceFilePathList, zipFilePath);
            }

            SetLabelBackup(true);

            //清空列表
            userControlSelectCopyFilePath.ClearFilePathList();
            userControlSelectCopyFileBackup.ClearFilePathList();
        }

        /// <summary>
        /// 设置labelBackup
        /// </summary>
        /// <param name="isComplete"></param>
        private async void SetLabelBackup(bool isComplete)
        {
            //设置文字颜色
            labelBackup.ForeColor = isComplete ? Color.Red : Color.Blue;
            labelBackup.Text = isComplete ? "备份完成" : "开始备份";
            if (isComplete)
            {
                await Task.Run(() =>
                  {
                      Thread.Sleep(1000);
                  });
                labelBackup.Text = "";
            }

        }

        /// <summary>
        /// 创建压缩文件
        /// </summary>
        /// <param name="sourceFilePathList"></param>
        /// <param name="destinationZipFilePath"></param>
        private async Task<bool> CreateZipFile(List<string> sourceFilePathList, string destinationZipFilePath)
        {
            try
            {
                await Task.Run(() =>
                {
                    // 使用ZipArchive创建或覆盖ZIP文件
                    using (var zipStream = new FileStream(destinationZipFilePath, FileMode.Create, FileAccess.ReadWrite))
                    using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                    {
                        foreach (var sourceFilePath in sourceFilePathList)
                        {
                            // 确保文件存在
                            if (File.Exists(sourceFilePath))
                            {
                                // 为ZIP文件中的每个文件创建一个条目
                                var entryName = Path.GetFileName(sourceFilePath);

                                //添加压缩文件
                                AddFileToZip(archive, sourceFilePath, entryName);
                            }
                            else if (Directory.Exists(sourceFilePath))
                            {

                                //添加目录
                                AddFolderToZip(archive, sourceFilePath, Path.GetFileName(sourceFilePath));
                            }
                        }
                    }

                });
                return true;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
                MessageBox.Show($"权限不足，无法读或写入文件。请确保文件不是只读，并且您有足够的权限。异常详情：{ex.Message}\r\n{ex.StackTrace}");
                return false;
            }
            catch (Exception ex)
            {
                //打印错误
                MessageBox.Show($"在压缩文件时：{ex.Message}\r\n{ex.StackTrace}");
                return false;
            }

        }

        /// <summary>
        /// 添加文件到压缩包
        /// </summary>
        /// <param name="archive"></param>
        /// <param name="filePath"></param>
        /// <param name="entryName"></param>
        private void AddFileToZip(ZipArchive archive, string filePath, string entryName)
        {
            try
            {
                var entry = archive.CreateEntry(entryName);
                using (var entryStream = entry.Open())
                using (var fileStream = File.OpenRead(filePath))
                {
                    fileStream.CopyTo(entryStream);
                }
            }
            catch (Exception ex)
            {
                //打印错误
                MessageBox.Show($"在添加文件到压缩包时：{ex.Message}\r\n{ex.StackTrace}");
            }

        }

        /// <summary>
        /// 添加文件夹到压缩包
        /// </summary>
        /// <param name="archive"></param>
        /// <param name="sourceFolderPath"></param>
        /// <param name="folderName"></param>
        private void AddFolderToZip(ZipArchive archive, string sourceFolderPath, string folderName)
        {
            try
            {
                // 遍历当前文件夹下的所有文件
                foreach (string filePath in Directory.GetFiles(sourceFolderPath))
                {
                    string relativePath = GetRelativePath(sourceFolderPath, filePath);
                    string entryName = Path.Combine(folderName, relativePath);
                    AddFileToZip(archive, filePath, entryName);
                }

                // 递归遍历当前文件夹下的所有子文件夹
                foreach (string directoryPath in Directory.GetDirectories(sourceFolderPath))
                {
                    string subFolderName = Path.GetFileName(directoryPath);
                    string newParentFolderName = Path.Combine(folderName, subFolderName);
                    AddFolderToZip(archive, directoryPath, newParentFolderName);
                }
            }
            catch (Exception ex)
            {
                //打印错误
                MessageBox.Show($"在添加文件夹到压缩包时：{ex.Message}\r\n{ex.StackTrace}");
            }

        }

        /// <summary>
        /// 手动实现相对路径计算
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        private string GetRelativePath(string basePath, string targetPath)
        {
            try
            {
                // 确保末尾是目录分隔符，以便正确处理位于基路径下的文件或目录
                if (!basePath.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                    basePath += Path.DirectorySeparatorChar;

                Uri baseUri = new Uri(basePath);
                Uri targetUri = new Uri(targetPath);

                // 使用Uri类的MakeRelativeUri方法来计算相对路径
                Uri relativeUri = baseUri.MakeRelativeUri(targetUri);
                string relativePath = Uri.UnescapeDataString(relativeUri.ToString());

                // 替换斜杠，以适应当前操作系统的文件系统
                relativePath = relativePath.Replace('/', Path.DirectorySeparatorChar);

                return relativePath;
            }
            catch (Exception ex)
            {
                //打印错误
                MessageBox.Show($"在计算相对路径时：{ex.Message}\r\n{ex.StackTrace}");
                return null;
            }

        }

        private void buttonBackup_Click(object sender, EventArgs e)
        {
            //获取备份名称
            var backupName = "\\" + textBoxBackupFile.Text + ".zip";
            //var backupName = textBoxBackupFile.Text + ".zip";

            // 获取备份文件的路径
            var sourceFilePathList = userControlSelectCopyFilePath.FilePathList;

            // 获取保存备份的路径
            var destinationZipFilePathList = userControlSelectCopyFileBackup.FilePathList;

            ZipFile(sourceFilePathList, destinationZipFilePathList, backupName);
        }
    }
}
