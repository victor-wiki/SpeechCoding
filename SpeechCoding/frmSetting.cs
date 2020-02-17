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
using SpeechCodingHandler;

namespace SpeechCoding
{
    public partial class frmSetting : Form
    {
        private AppSetting appSetting;
        private Setting setting;
        private List<LanguageSetting> languageSettings = new List<LanguageSetting>();
        private List<ControlWordSetting> controlWordSettings = new List<ControlWordSetting>();

        private string customIncludeWords = "";
        private string customExcludeWords = "";
        private bool hasChangedCustomWords = false;

        public frmSetting()
        {
            InitializeComponent();

            UiHelper.SetUI(this);

            this.dgvGrammarPriority.AutoGenerateColumns = false;

            this.GrammarType.ValueType = typeof(GrammarType);
            this.GrammarPriority.ValueType = typeof(GrammarPriority);
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            this.appSetting = AppSettingManager.GetSetting();
            this.chkStartRecordWhenStartup.Checked = this.appSetting.FormTopMost;
            this.txtLogFileOpenTool.Text = appSetting.LogFileOpenTool;

            this.setting = SettingManager.GetSetting();

            this.chkStartRecordWhenStartup.Checked = this.setting.StartRecordWhenStartup;

            Config config = ConfigManager.GetConfig();

            if (!SettingManager.IsSetted())
            {
                this.controlWordSettings = config.ControlWords;
            }
            else
            {
                this.controlWordSettings = setting.ControlWords;
            }          

            this.cboLanguage.Items.Add("");
            foreach (string language in config.Languages)
            {
                this.cboLanguage.Items.Add(language);
            }
            this.cboLanguage.Text = setting.PreferredLanguage;

            var grammarTypes = Enum.GetNames(typeof(GrammarType));

            List<GrammarPrioritySetting> grammarSettings = new List<GrammarPrioritySetting>();

            foreach (string grammarType in grammarTypes)
            {
                if (grammarType != SpeechCodingHandler.GrammarType.Unknown.ToString())
                {
                    GrammarPrioritySetting gs = new GrammarPrioritySetting() { GrammarType = (GrammarType)Enum.Parse(typeof(GrammarType), grammarType) };
                    GrammarPrioritySetting oldGs = setting.GrammarPriorities.FirstOrDefault(item => item.GrammarType.ToString() == grammarType);
                    if (oldGs != null)
                    {
                        gs.GrammarPriority = oldGs.GrammarPriority;
                    }
                    grammarSettings.Add(gs);
                }
            }

            this.GrammarType.DataSource = Enum.GetValues(typeof(GrammarType)).Cast<GrammarType>().Where(item => item != (int)SpeechCodingHandler.GrammarType.Unknown).ToArray();
            this.GrammarPriority.DataSource = Enum.GetValues(typeof(GrammarPriority));

            this.dgvGrammarPriority.DataSource = grammarSettings;           
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.appSetting.FormTopMost = this.chkStartRecordWhenStartup.Checked;
            this.appSetting.LogFileOpenTool = this.txtLogFileOpenTool.Text;

            AppSettingManager.SaveSetting(this.appSetting);

            this.setting.StartRecordWhenStartup = this.chkStartRecordWhenStartup.Checked;
            this.setting.PreferredLanguage = this.cboLanguage.Text;           

            this.setting.ControlWords = this.controlWordSettings;

            List<GrammarPrioritySetting> grammarSettings = new List<GrammarPrioritySetting>();
            foreach (DataGridViewRow row in this.dgvGrammarPriority.Rows)
            {
                GrammarPrioritySetting gs = new GrammarPrioritySetting();
                gs.GrammarType = (GrammarType)Convert.ToInt32(row.Cells["GrammarType"].Value);
                gs.GrammarPriority = (GrammarPriority)Convert.ToInt32(row.Cells["GrammarPriority"].Value);

                grammarSettings.Add(gs);
            }

            this.setting.GrammarPriorities = grammarSettings;
            this.setting.LanguageSettings = this.languageSettings;

            SettingManager.SaveSetting(this.setting);

            if (this.hasChangedCustomWords)
            {
                if (!Directory.Exists(SettingManager.CustomWordFolder))
                {
                    Directory.CreateDirectory(SettingManager.CustomWordFolder);
                }

                File.WriteAllText(SettingManager.CustomIncludeWordsFilePath, this.customIncludeWords);
                File.WriteAllText(SettingManager.CustomExcludeWordsFilePath, this.customExcludeWords);
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnConfigControlWords_Click(object sender, EventArgs e)
        {
            frmControlWordSetting frmControlWordSetting = new frmControlWordSetting();
            frmControlWordSetting.Setting = this.setting;
            DialogResult result = frmControlWordSetting.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.controlWordSettings = frmControlWordSetting.ControlWordSettings;
            }
        }

        private void btnConfigCustomWords_Click(object sender, EventArgs e)
        {
            frmCustomWord frmCustomWord = new frmCustomWord();
            DialogResult result = frmCustomWord.ShowDialog();
            if (result == DialogResult.OK)
            {
                hasChangedCustomWords = true;
                this.customIncludeWords = frmCustomWord.CustomIncludeWords;
                this.customExcludeWords = frmCustomWord.CustomExcludeWords;
            }
        }

        private void btnLanguageSetting_Click(object sender, EventArgs e)
        {
            frmLanguageSetting frmLanguageSetting = new frmLanguageSetting();
            frmLanguageSetting.Setting = this.setting;
            DialogResult result = frmLanguageSetting.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.languageSettings = frmLanguageSetting.LanguageSettings;
            }
        }

        private void btnLogFileOpenTool_Click(object sender, EventArgs e)
        {
            DialogResult result = this.dlgLogFileOpenTool.ShowDialog();
            if(result == DialogResult.OK)
            {
                this.txtLogFileOpenTool.Text = this.dlgLogFileOpenTool.FileName;
            }
        }
    }
}
