using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpeechCodingHandler;
using WindowsInput.Native;

namespace SpeechCoding
{
    public partial class frmAddControlWord : Form
    {
        public ControlWordSetting ControlWordSetting { get; set; }

        public frmAddControlWord()
        {
            InitializeComponent();

            UiHelper.SetUI(this);
        }

        private void frmAddControlWord_Load(object sender, EventArgs e)
        {
            this.InitControls(); 
        }

        private void InitControls()
        {
            var controls = Enum.GetNames(typeof(VirtualKeyCode));
            this.cboControl.Items.AddRange(controls.OrderBy(item=>item).ToArray());
        }

        private void rbKeybord_CheckedChanged(object sender, EventArgs e)
        {
            this.SetControlState();
        }

        private void rbText_CheckedChanged(object sender, EventArgs e)
        {
            this.SetControlState();
        }

        private void SetControlState()
        {           
            this.cboControl.DropDownStyle = this.rbKeybord.Checked ? ComboBoxStyle.DropDownList : ComboBoxStyle.DropDown;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string control = this.cboControl.Text.Trim();
            string word = this.txtWord.Text.Trim();

            if(string.IsNullOrEmpty(control) || string.IsNullOrEmpty(word))
            {
                MessageBox.Show("Control and Word can't be empty.");
                return;
            }

            this.ControlWordSetting = new ControlWordSetting() { Control = control, Word = word, IsKeyboard= this.rbKeybord.Checked, UseCodeTemplate=this.chkUseCodeTemplate.Checked };

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
