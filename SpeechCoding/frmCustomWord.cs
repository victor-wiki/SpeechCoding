using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SpeechCodingHandler;

namespace SpeechCoding
{
    public partial class frmCustomWord : Form
    {
        public string CustomIncludeWords { get; private set; }

        public string CustomExcludeWords { get; private set; }

        public frmCustomWord()
        {
            InitializeComponent();

            UiHelper.SetUI(this);
        }

        private void frmCustomWord_Load(object sender, EventArgs e)
        {
            if(File.Exists(SettingManager.CustomIncludeWordsFilePath))
            {
                this.txtIncludeWords.Text = File.ReadAllText(SettingManager.CustomIncludeWordsFilePath);
            }

            if(File.Exists(SettingManager.CustomExcludeWordsFilePath))
            {
                this.txtExcludeWords.Text = File.ReadAllText(SettingManager.CustomExcludeWordsFilePath);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.CustomIncludeWords = this.txtIncludeWords.Text;
            this.CustomExcludeWords = this.txtExcludeWords.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
