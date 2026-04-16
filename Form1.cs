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
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path)) return;

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
            PopulateListView(lvwLeftDir, leftFiles, rightFiles);
            PopulateListView(lvwrightDir, rightFiles, leftFiles);
        }

        private void PopulateListView(ListView lv, Dictionary<string, FileInfo> source, Dictionary<string, FileInfo> target)
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
                        else if (f.LastWriteTime > otherFile.LastWriteTime)
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

                foreach (ColumnHeader col in lv.Columns)
                {
                    col.Width = -2;
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
            if (string.IsNullOrEmpty(txtLeftDir.Text) || string.IsNullOrEmpty(txtRightDir.Text))
            {
                MessageBox.Show($"2번 오류: 경로가 비어있음!\n왼쪽: {txtLeftDir.Text}\n오른쪽: {txtRightDir.Text}");
                return;
            }

            // 진단 3: 선택된 파일이 있는지 확인
            if (lvwLeftDir.SelectedItems.Count == 0)
            {
                MessageBox.Show("리스트뷰에서 파일을 선택하지 않았습니다!");
                return;
            }

            foreach (ListViewItem item in lvwLeftDir.SelectedItems)
            {
                string fileName = item.Text;
                string srcPath = Path.Combine(txtLeftDir.Text, fileName);
                string destPath = Path.Combine(txtRightDir.Text, fileName);

                if (CopyFileWithConfirmation(srcPath, destPath))
                {
                    LoadFiles(txtRightDir.Text, false);
                }
            }
            SyncViews();
        }

        private void btnCopyFromRight_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLeftDir.Text) || string.IsNullOrEmpty(txtRightDir.Text)) return;

            foreach (ListViewItem item in lvwrightDir.SelectedItems)
            {
                string fileName = item.Text;
                string srcPath = Path.Combine(txtRightDir.Text, fileName);
                string destPath = Path.Combine(txtLeftDir.Text, fileName);

                if (CopyFileWithConfirmation(srcPath, destPath))
                {
                    LoadFiles(txtLeftDir.Text, true);
                }
            }
            SyncViews();
        }

        private bool CopyFileWithConfirmation(string srcPath, string destPath)
        {
            if (File.Exists(destPath))
            {
                DateTime srcTime = File.GetLastWriteTime(srcPath);
                DateTime destTime = File.GetLastWriteTime(destPath);

                string msg = "대상에 동일한 이름의 파일이 이미 있습니다.\n" +
                     "대상 파일이 더 신규 파일입니다. 덮어쓰시겠습니까?\n\n" +
                     $"원본: {srcPath}\n" +
                     $"대상: {destPath}";
           
                var result = MessageBox.Show(msg, "덮어쓰기 확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No) return false;
            }

            try
            {
                File.Copy(srcPath, destPath, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"복사 실패: {ex.Message}");
                return false;
            }
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) { }
        private void lvwLeftDir_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lvwrightDir_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}