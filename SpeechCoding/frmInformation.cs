using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using SpeechCodingHandler;
using System.Text.RegularExpressions;

namespace SpeechCoding
{
    public delegate void UnloadFilesHandler(IEnumerable<string> filePaths);

    public partial class frmInformation : Form
    {
        public event UnloadFilesHandler OnUnloadFiles;

        public Dictionary<GrammarType, string[]> GrammarWords { get; set; } = new Dictionary<GrammarType, string[]>();
        public List<string> FilePaths { get; set; } = new List<string>();
        public frmInformation()
        {
            InitializeComponent();

            UiHelper.SetUI(this);
        }

        private void frmInformation_Load(object sender, EventArgs e)
        {
            this.InitControls();
        }

        private void InitControls()
        {
            var types = Enum.GetNames(typeof(GrammarType));
            foreach (var type in types)
            {
                if (type != GrammarType.Unknown.ToString())
                {
                    this.cboType.Items.Add(type);
                }
            }

            this.LoadWords(this.cboType.Text);

            foreach (var path in this.FilePaths)
            {
                FileInfo file = new FileInfo(path);
                ListViewItem li = new ListViewItem();
                li.Tag = file;
                li.Name = file.Name;
                li.Text = file.Name;
                li.SubItems.Add(file.FullName);

                this.lvFiles.Items.Add(li);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lvFiles_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.lvFiles.FocusedItem.Bounds.Contains(e.Location))
                {
                    this.contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }

        private void tsmiOpenExplorer_Click(object sender, EventArgs e)
        {
            if (this.lvFiles.FocusedItem != null)
            {
                this.OpenInExplorer((this.lvFiles.FocusedItem.Tag as FileInfo)?.FullName);
            }
        }

        private void OpenInExplorer(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                Utility.OpenInExplorer(filePath);
            }
        }

        private void cboType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadWords(this.cboType.Text);
        }

        private void LoadWords(string type)
        {
            var words = Enumerable.Empty<string>();
            if (string.IsNullOrEmpty(type))
            {
                words = this.GrammarWords.Values.SelectMany(item => item);
            }
            else if (this.GrammarWords.Keys.Select(item => item.ToString()).Contains(type))
            {
                words = this.GrammarWords.Where(item => item.Key.ToString() == type).FirstOrDefault().Value.AsEnumerable<string>();
            }

            this.txtWords.Text = string.Join(Environment.NewLine, words.OrderBy(item => item));
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            this.Find();
        }

        private void Find()
        {
            this.txtWords.Select(0, 0);

            string content = this.txtWords.Text.Trim();
            string keyword = this.txtKeyword.Text.Trim();

            if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(keyword))
            {
                string[] words = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int count = 0;
                bool found = false;

                foreach (string word in words)
                {
                    if (word.ToLower() == keyword.ToLower())
                    {
                        found = true;
                        break;
                    }
                    count += word.Length + Environment.NewLine.Length;
                }

                if (found)
                {
                    this.txtWords.SelectionStart = count;
                    this.txtWords.SelectionLength = keyword.Length;
                    this.txtWords.Select();
                    this.txtWords.ScrollToCaret();
                }
                else
                {
                    MessageBox.Show("Not found.");
                }
            }
        }

        private void txtKeyword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Find();
            }
        }

        private void unloadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.lvFiles.SelectedItems != null)
            {
                List<string> filePaths = new List<string>();
                int count = this.lvFiles.SelectedIndices.Count;
                for (int i = this.lvFiles.SelectedIndices[count - 1]; i >= 0; i--)
                {
                    ListViewItem item = this.lvFiles.Items[this.lvFiles.SelectedIndices[i]];
                    FileInfo file = item.Tag as FileInfo;
                    filePaths.Add(file.FullName);

                    this.lvFiles.Items.RemoveAt(i); 
                }               

                if (this.OnUnloadFiles != null)
                {
                    this.OnUnloadFiles(filePaths);
                }
            }
            else
            {
                MessageBox.Show("Please select row.");
            }
        }
    }
}
