using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileCompare
{
    public partial class Form1 : Form
    {
        private Dictionary<string, FileSystemInfo> leftFiles = new Dictionary<string, FileSystemInfo>();
        private Dictionary<string, FileSystemInfo> rightFiles = new Dictionary<string, FileSystemInfo>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLeftDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text))
                    dlg.SelectedPath = txtLeftDir.Text;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtLeftDir.Text = dlg.SelectedPath;
                    LoadFiles(txtLeftDir.Text, true);
                    SyncViews();
                }
            }
        }

        private void btnRightDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(txtRightDir.Text) && Directory.Exists(txtRightDir.Text))
                    dlg.SelectedPath = txtRightDir.Text;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    txtRightDir.Text = dlg.SelectedPath;
                    LoadFiles(txtRightDir.Text, false);
                    SyncViews();
                }
            }
        }

        private void LoadFiles(string path, bool isLeft)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) return;

            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                var items = di.GetFileSystemInfos().ToDictionary(f => f.Name, StringComparer.OrdinalIgnoreCase);

                if (isLeft) leftFiles = items;
                else rightFiles = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SyncViews()
        {
            PopulateListView(lvwLeftDir, leftFiles, rightFiles);
            PopulateListView(lvwrightDir, rightFiles, leftFiles);
        }

        private void PopulateListView(ListView lv, Dictionary<string, FileSystemInfo> source, Dictionary<string, FileSystemInfo> target)
        {
            lv.BeginUpdate();
            lv.Items.Clear();

            try
            {
                foreach (var f in source.Values.OrderBy(x => x is DirectoryInfo ? 0 : 1).ThenBy(x => x.Name))
                {
                    ListViewItem item = new ListViewItem(f.Name);

                    if (f is DirectoryInfo) item.SubItems.Add("<DIR>");
                    else item.SubItems.Add(FormatSize(((FileInfo)f).Length));

                    item.SubItems.Add(f.LastWriteTime.ToString("g"));

                    if (target.TryGetValue(f.Name, out var other))
                    {
                        // [수정] 1초 미만의 미세한 시간 차이는 동일한 것으로 간주 (파일 시스템 오차 방지)
                        TimeSpan diff = f.LastWriteTime - other.LastWriteTime;
                        if (Math.Abs(diff.TotalSeconds) < 1)
                        {
                            item.ForeColor = Color.Black;
                        }
                        else if (f.LastWriteTime > other.LastWriteTime)
                        {
                            item.ForeColor = Color.Red;
                        }
                        else
                        {
                            item.ForeColor = Color.Gray;
                        }
                    }
                    else
                    {
                        item.ForeColor = Color.Purple;
                    }
                    lv.Items.Add(item);
                }
                foreach (ColumnHeader col in lv.Columns) col.Width = -2;
            }
            finally { lv.EndUpdate(); }
        }

        private string FormatSize(long bytes) => (bytes / 1024f).ToString("N1") + " KB";

        private void btnCopyFromLeft_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLeftDir.Text) || string.IsNullOrEmpty(txtRightDir.Text)) return;

            foreach (ListViewItem item in lvwLeftDir.SelectedItems)
            {
                string srcPath = Path.Combine(txtLeftDir.Text, item.Text);
                string destPath = Path.Combine(txtRightDir.Text, item.Text);

                if (leftFiles[item.Text] is DirectoryInfo) CopyDirectoryWithConfirmation(srcPath, destPath);
                else CopyFileWithConfirmation(srcPath, destPath);
            }
            LoadFiles(txtLeftDir.Text, true); LoadFiles(txtRightDir.Text, false); SyncViews();
        }

        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLeftDir.Text) || string.IsNullOrEmpty(txtRightDir.Text)) return;

            foreach (ListViewItem item in lvwrightDir.SelectedItems)
            {
                string srcPath = Path.Combine(txtRightDir.Text, item.Text);
                string destPath = Path.Combine(txtLeftDir.Text, item.Text);

                if (rightFiles[item.Text] is DirectoryInfo) CopyDirectoryWithConfirmation(srcPath, destPath);
                else CopyFileWithConfirmation(srcPath, destPath);
            }
            LoadFiles(txtLeftDir.Text, true); LoadFiles(txtRightDir.Text, false); SyncViews();
        }

        private bool CopyFileWithConfirmation(string srcPath, string destPath)
        {
            DateTime srcTime = File.GetLastWriteTime(srcPath);

            if (File.Exists(destPath))
            {
                DateTime destTime = File.GetLastWriteTime(destPath);
                if (destTime > srcTime)
                {
                    if (!ShowOverwriteDialog(srcPath, destPath)) return false;
                }
            }
            try
            {
                File.Copy(srcPath, destPath, true);
                // [수정] 복사 후 원본 날짜를 대상 파일에 강제 이식
                File.SetLastWriteTime(destPath, srcTime);
                return true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
        }

        private void CopyDirectoryWithConfirmation(string srcDir, string destDir)
        {
            DateTime srcTime = Directory.GetLastWriteTime(srcDir);

            if (Directory.Exists(destDir))
            {
                DateTime destTime = Directory.GetLastWriteTime(destDir);
                if (destTime > srcTime)
                {
                    if (!ShowOverwriteDialog(srcDir, destDir)) return;
                }
            }
            RecursiveCopy(srcDir, destDir);
        }

        private void RecursiveCopy(string sourceDir, string targetDir)
        {
            // 원본 날짜 보관
            DateTime srcTime = Directory.GetLastWriteTime(sourceDir);

            // 폴더 생성
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(targetDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
                // 파일 날짜 동기화
                File.SetLastWriteTime(destFile, File.GetLastWriteTime(file));
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                RecursiveCopy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
            }

            // [수정] 하위 내용 복사가 모두 끝난 후, 폴더 자체의 날짜도 원본과 동기화
            Directory.SetLastWriteTime(targetDir, srcTime);
        }

        private bool ShowOverwriteDialog(string src, string dest)
        {
            string msg = "대상에 동일한 이름의 파일이 이미 있습니다.\n" +
                         "대상 파일이 더 신규 파일입니다. 덮어쓰시겠습니까?\n\n" +
                         $"원본: {src}\n" +
                         $"대상: {dest}";
            return MessageBox.Show(msg, "덮어쓰기 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) { }
        private void lvwLeftDir_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lvwrightDir_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}