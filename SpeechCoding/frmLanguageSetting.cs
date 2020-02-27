using SpeechCodingHandler;
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

namespace SpeechCoding
{
    public partial class frmLanguageSetting : Form
    {
        public Setting Setting { get; set; } = new Setting();

        public List<LanguageSetting> LanguageSettings { get { return this.languageSettings; } }

        private List<LanguageSetting> languageSettings = new List<LanguageSetting>();

        public frmLanguageSetting()
        {
            InitializeComponent();

            UiHelper.SetUI(this);
        }

        private void frmBuildOpenToolSetting_Load(object sender, EventArgs e)
        {
            Config config = ConfigManager.GetConfig();

            foreach (string language in config.Languages)
            {
                LanguageSetting setting = new LanguageSetting() { Language = language };
                LanguageSetting oldSetting = this.Setting.LanguageSettings.FirstOrDefault(item => item.Language == language);
                if (oldSetting != null)
                {
                    setting.RelativeFileExtension = oldSetting.RelativeFileExtension;                                       
                }

                LanguageInterpreter interpreter = LanguageHelper.GetInterpreter(language);

                setting.RelativeFileExtension = StringHelper.GetNotEmptyValue(oldSetting?.RelativeFileExtension, interpreter?.RelativeFileExtension);

                languageSettings.Add(setting);
            }

            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.DataSource = languageSettings;            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                string language = row.Cells["Language"].Value.ToString();
                LanguageSetting setting = this.languageSettings.FirstOrDefault(item => item.Language == language);
                if (setting != null)
                {
                    setting.RelativeFileExtension = row.Cells["RelativeFileExtension"].Value?.ToString();                   
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }       
    }
}
