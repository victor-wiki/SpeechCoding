using Newtonsoft.Json;
using SpeechCoding.Properties;
using SpeechCodingHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SpeechCoding
{
    public partial class frmMain : Form
    {
        private Config config = ConfigManager.GetConfig();
        private Setting setting = SettingManager.GetSetting();
        private List<string> loadedFilePaths = new List<string>();
        private SpeechCodingProcessor processor;
        private bool isLoadingProject = false;
        private string logFilePath = LogHelper.DefaultLogFilePath;

        public frmMain()
        {
            InitializeComponent();
            Label.CheckForIllegalCrossThreadCalls = false;
            Button.CheckForIllegalCrossThreadCalls = false;

            UiHelper.SetUI(this);

            this.SetPosition();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LogHelper.EnableDebug = true;

            ProjectManager.Init();

            this.InitControlState();
            this.InitProcessor();
        }

        private void InitControlState()
        {
            this.btnStart.Enabled = true;
            this.btnStart.Image = Resources.microphone_ready;
            this.btnStop.Enabled = false;
            this.SetLogVisible();
        }

        private void SetLogVisible()
        {
            this.btnLog.Visible = File.Exists(this.logFilePath);
        }

        private void InitProcessor()
        {
            this.processor = new SpeechCodingProcessor(this.setting);
            this.processor.OnSpeechSateChanged += this.SpeechSateChanged;
            this.processor.OnSpeechMessageReceived += this.SpeechMessageReceived;

            this.processor.Init();
        }

        private void SetPosition()
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - this.Size.Width, workingArea.Bottom - this.Size.Height);
        }

        private void SpeechSateChanged(SpeechState state)
        {
            if (state == SpeechState.Stopped || this.processor.RecordEnded)
            {
                this.Invoke(new Action(() => { this.btnStop.Enabled = false; }));
            }
            else
            {
                this.Invoke(new Action(() => { this.btnStop.Enabled = true; }));
            }

            if (this.processor.RecordEnded)
            {
                this.lblMessage.Text = "ready";
            }

            this.Invoke(new Action(() => { this.btnStart.Image = state == SpeechState.Stopped ? Resources.microphone_ready : Resources.microphone_recording; }));
        }

        private void SpeechMessageReceived(MessageType type, MessageCode code, string message)
        {
            this.lblMessage.Text = message;
            this.toolTip1.ToolTipIcon = (ToolTipIcon)(int)type;
            this.toolTip1.SetToolTip(this.lblMessage, message);

            if (type != MessageType.Information)
            {
                this.Log(type, message);

                if (code == MessageCode.LanguageNotSpecified)
                {
                    this.ShowSettingDialog();
                }
                else if (code == MessageCode.NoAudioInputDevice)
                {
                    MessageBox.Show(message);
                    Application.Exit();
                }
            }

            this.SetLogVisible();
        }

        private void RememberFiles(IEnumerable<string> filePaths)
        {
            foreach (string path in filePaths)
            {
                if (!this.loadedFilePaths.Contains(path))
                {
                    this.loadedFilePaths.Add(path);
                }
            }
        }

        private void LoadFiles(IEnumerable<string> filePaths)
        {
            if (filePaths.Count() > 0)
            {
                this.RememberFiles(filePaths);

                List<ObjectInfo> objInfos = this.processor.GetObjectInfos(filePaths);

                List<string> lstWord = new List<string>();

                IEnumerable<string> objectWords = this.processor.GetWords(objInfos);
                IEnumerable<string> textWords = this.processor.GetPlainTextWords(filePaths);

                lstWord.AddRange(objectWords);
                lstWord.AddRange(textWords);

                string[] words = lstWord.Except(this.processor.BaseWords).Distinct().ToArray();

                this.processor.LoadGrammer(GrammarType.General, words);
            }
            else
            {
                this.processor.LoadGrammer(GrammarType.General, new string[] { });
            }
        }

        private void frmMain_DragDrop(object sender, DragEventArgs e)
        {
            this.SetDropFiles(e);
        }

        private void frmMain_DragOver(object sender, DragEventArgs e)
        {
            this.SetDragEffect(e);
        }

        private void toolStrip1_DragDrop(object sender, DragEventArgs e)
        {
            this.SetDropFiles(e);
        }

        private void SetDropFiles(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop.ToString());
                this.LoadFiles(filePaths);
            }
        }

        private void toolStrip1_DragEnter(object sender, DragEventArgs e)
        {
            this.SetDragEffect(e);
        }

        private void SetDragEffect(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ShowSettingDialog();
        }

        private void ShowSettingDialog()
        {
            string oldLanguage = this.setting.PreferredLanguage;

            frmSetting frmSetting = new frmSetting();
            DialogResult result = frmSetting.ShowDialog();
            if (result == DialogResult.OK)
            {
                UiHelper.SetUI(this);
               
                this.setting = SettingManager.GetSetting();

                if (this.setting.PreferredLanguage != oldLanguage)
                {
                    this.loadedFilePaths.Clear();
                    this.LoadFiles(Enumerable.Empty<string>());
                }

                this.processor.Dispose();

                this.InitProcessor();
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.setting.PreferredLanguage))
            {
                MessageBox.Show("Please specify preferred language first.");
                return;
            }

            if (this.processor != null)
            {
                await this.processor.StartRecord();
            }
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            await this.processor.StopRecord();
        }

        private void chkControl_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterGrammer();
        }

        private void chkKeyword_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterGrammer();
        }

        private void chkCustom_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterGrammer();
        }

        private void chkGeneral_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterGrammer();
        }

        private void FilterGrammer()
        {
            if (!this.isLoadingProject)
            {
                this.processor.FilterGrammar(this.GetCurrentGrammarType());
            }
        }

        private GrammarType GetCurrentGrammarType()
        {
            GrammarType grammarType = GrammarType.Unknown;
            if (this.chkControl.Checked)
            {
                grammarType = grammarType | GrammarType.Control;
            }
            if (this.chkKeyword.Checked)
            {
                grammarType = grammarType | GrammarType.Keyword;
            }
            if (this.chkCustom.Checked)
            {
                grammarType = grammarType | GrammarType.Custom;
            }

            if (this.chkGeneral.Checked)
            {
                grammarType = grammarType | GrammarType.General;
            }

            return grammarType;
        }

        private void btnInformation_Click(object sender, EventArgs e)
        {
            frmInformation frmInformation = new frmInformation() { FilePaths = this.loadedFilePaths, GrammarWords = this.processor.GrammarWords };
            frmInformation.OnUnloadFiles += FrmInformation_OnUnloadFiles;
            frmInformation.ShowDialog();
        }

        private void FrmInformation_OnUnloadFiles(IEnumerable<string> filePaths)
        {
            foreach (string filePath in filePaths)
            {
                this.loadedFilePaths.Remove(filePath);
            }

            this.LoadFiles(this.loadedFilePaths);
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            if (File.Exists(this.logFilePath))
            {
                string tool = AppSettingManager.Setting.LogFileOpenTool ?? "notepad.exe";

                Process.Start(tool, this.logFilePath);
            }
        }

        private void Log(MessageType type, string message)
        {
            LogHelper.LogInfo(this.logFilePath, type, message);
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.dlgLoadFile.FileName = "";

            string filter = "all files|*.*";
            if (!string.IsNullOrEmpty(this.setting.PreferredLanguage))
            {
                List<string> extList = new List<string>();
                List<string> extensions = this.processor.RelativeFileExtensions;
                extensions.ForEach(item =>
                {
                    extList.Add($"{item.Trim('.')} file|*{item}");
                });

                filter = string.Join("|", extList) + "|" + filter;
            }

            this.dlgLoadFile.Filter = filter;

            DialogResult result = this.dlgLoadFile.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.LoadFiles(this.dlgLoadFile.FileNames);
            }
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectInfo projectInfo = new ProjectInfo() { GrammarType = this.GetCurrentGrammarType(), FilePaths = this.loadedFilePaths };

            this.dlgSaveRecent.FileName = "";
            this.dlgSaveRecent.InitialDirectory = ProjectManager.DefaultSaveFolder;
            DialogResult result = this.dlgSaveRecent.ShowDialog();

            if (result == DialogResult.OK)
            {
                ProjectManager.SaveProject(projectInfo, this.dlgSaveRecent.FileName);
            }
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = this.dlgOpenProject.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filePath = this.dlgOpenProject.FileName;
                string content = File.ReadAllText(filePath);

                ProjectInfo projectInfo = (ProjectInfo)JsonConvert.DeserializeObject(content, typeof(ProjectInfo));

                this.LoadProject(projectInfo);
            }
        }

        private void LoadProject(ProjectInfo projectInfo)
        {
            this.isLoadingProject = true;

            this.processor.Dispose();

            this.InitControlState();

            this.loadedFilePaths.Clear();
            this.SetLogVisible();

            GrammarType grammarType = projectInfo.GrammarType;

            this.chkControl.Checked = (grammarType & GrammarType.Control) == GrammarType.Control;
            this.chkKeyword.Checked = (grammarType & GrammarType.Keyword) == GrammarType.Keyword;
            this.chkCustom.Checked = (grammarType & GrammarType.Custom) == GrammarType.Custom;
            this.chkGeneral.Checked = (grammarType & GrammarType.General) == GrammarType.General;

            this.InitProcessor();

            this.LoadFiles(projectInfo.FilePaths.Where(item => File.Exists(item)));
            this.isLoadingProject = false;

            this.FilterGrammer();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.processor.OnSpeechSateChanged -= this.SpeechSateChanged;
            this.processor.OnSpeechMessageReceived -= this.SpeechMessageReceived;
            this.processor.Dispose();
        }
    }
}
