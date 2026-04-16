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
        private Dictionary<string, FileInfo> leftFiles = new Dictionary<string, FileInfo>();
        private Dictionary<string, FileInfo> rightFiles = new Dictionary<string, FileInfo>();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnLeftDir_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) && Directory.Exists(txtLeftDir.Text))
                {
                    dlg.SelectedPath = txtLeftDir.Text;
                }

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
                {
                    dlg.SelectedPath = txtRightDir.Text;
                }

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
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (isLeft)
                    leftFiles = di.GetFiles().ToDictionary(f => f.Name, StringComparer.OrdinalIgnoreCase);
                else
                    rightFiles = di.GetFiles().ToDictionary(f => f.Name, StringComparer.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SyncViews()
        {
            PopulateListView(lvwLeftDir, leftFiles, rightFiles, true);
            PopulateListView(lvwrightDir, rightFiles, leftFiles, false);
        }

        private void PopulateListView(ListView lv, Dictionary<string, FileInfo> source, Dictionary<string, FileInfo> target, bool isLeft)
        {
            lv.BeginUpdate();
            lv.Items.Clear();

            try
            {
                foreach (var f in source.Values.OrderBy(x => x.Name))
                {
                    ListViewItem item = new ListViewItem(f.Name);
                    item.SubItems.Add(FormatSize(f.Length));
                    item.SubItems.Add(f.LastWriteTime.ToString("g"));

                    if (target.TryGetValue(f.Name, out var otherFile))
                    {
                        if (f.LastWriteTime == otherFile.LastWriteTime)
                        {
                            item.ForeColor = Color.Black;
                        }
                        else
                        {
                            item.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        item.ForeColor = isLeft ? Color.Blue : Color.Green;
                    }

                    lv.Items.Add(item);
                }

                for (int i = 0; i < lv.Columns.Count; i++)
                {
                    lv.AutoResizeColumn(i, ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
            finally
            {
                lv.EndUpdate();
            }
        }

        private string FormatSize(long bytes)
        {
            return (bytes / 1024f).ToString("N1") + " KB";
        }

        private void btnCopyFromLeft_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRightDir.Text)) return;

            foreach (ListViewItem item in lvwLeftDir.SelectedItems)
            {
                string fileName = item.Text;
                if (leftFiles.TryGetValue(fileName, out var srcFile))
                {
                    string destPath = Path.Combine(txtRightDir.Text, fileName);
                    CopyFileWithConfirmation(srcFile.FullName, destPath);
                }
            }
            LoadFiles(txtRightDir.Text, false);
            SyncViews();
        }

        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLeftDir.Text)) return;

            foreach (ListViewItem item in lvwrightDir.SelectedItems)
            {
                string fileName = item.Text;
                if (rightFiles.TryGetValue(fileName, out var srcFile))
                {
                    string destPath = Path.Combine(txtLeftDir.Text, fileName);
                    CopyFileWithConfirmation(srcFile.FullName, destPath);
                }
            }
            LoadFiles(txtLeftDir.Text, true);
            SyncViews();
        }

        private void CopyFileWithConfirmation(string srcPath, string destPath)
        {
            if (File.Exists(destPath))
            {
                DateTime srcTime = File.GetLastWriteTime(srcPath);
                DateTime destTime = File.GetLastWriteTime(destPath);

                if (srcTime < destTime)
                {
                    var result = MessageBox.Show(Path.GetFileName(destPath) + " 파일이 대상 폴더에서 더 최신입니다. 덮어쓰시겠습니까?", "확인", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No) return;
                }
            }
            File.Copy(srcPath, destPath, true);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) { }
        private void lvwLeftDir_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lvwrightDir_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}