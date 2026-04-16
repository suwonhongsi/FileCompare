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
        // FileInfo 대신 폴더와 파일을 모두 담을 수 있는 FileSystemInfo를 사용합니다.
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
                // GetFileSystemInfos()를 써서 파일과 폴더를 모두 가져옵니다.
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

                    // 폴더면 <DIR>, 파일이면 용량 표시
                    if (f is DirectoryInfo) item.SubItems.Add("<DIR>");
                    else item.SubItems.Add(FormatSize(((FileInfo)f).Length));

                    item.SubItems.Add(f.LastWriteTime.ToString("g"));

                    // 색상 처리 로직
                    if (target.TryGetValue(f.Name, out var other))
                    {
                        if (f.LastWriteTime == other.LastWriteTime) item.ForeColor = Color.Black; // 동일
                        else if (f.LastWriteTime > other.LastWriteTime) item.ForeColor = Color.Red; // 신규(New)
                        else item.ForeColor = Color.Gray; // 오래됨(Old)
                    }
                    else
                    {
                        item.ForeColor = Color.Purple; // 단독 파일/폴더
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
            if (File.Exists(destPath))
            {
                if (!ShowOverwriteDialog(srcPath, destPath)) return false;
            }
            try { File.Copy(srcPath, destPath, true); return true; }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
        }

        private void CopyDirectoryWithConfirmation(string srcDir, string destDir)
        {
            if (Directory.Exists(destDir))
            {
                if (!ShowOverwriteDialog(srcDir, destDir)) return;
            }
            RecursiveCopy(srcDir, destDir);
        }

        private void RecursiveCopy(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);
            foreach (var file in Directory.GetFiles(sourceDir))
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), true);
            foreach (var directory in Directory.GetDirectories(sourceDir))
                RecursiveCopy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }

        private bool ShowOverwriteDialog(string src, string dest)
        {
            string msg = $"대상에 동일한 이름의 항목이 이미 있습니다.\n" +
                         $"대상 항목이 더 신규일 수 있습니다. 덮어쓰시겠습니까?\n\n" +
                         $"원본: {src}\n대상: {dest}";
            return MessageBox.Show(msg, "덮어쓰기 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) { }
        private void lvwLeftDir_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lvwrightDir_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}