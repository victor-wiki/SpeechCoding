namespace SpeechCoding
{
    partial class frmSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSetting));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cboLanguage = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfigControlWords = new System.Windows.Forms.Button();
            this.btnConfigCustomWords = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvGrammarPriority = new System.Windows.Forms.DataGridView();
            this.GrammarType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.GrammarPriority = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.btnLanguageSetting = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.chkStartRecordWhenStartup = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnLogFileOpenTool = new System.Windows.Forms.Button();
            this.txtLogFileOpenTool = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dlgLogFileOpenTool = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrammarPriority)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(290, 383);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "Save";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(371, 383);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 12);
            this.label5.TabIndex = 15;
            this.label5.Text = "Preferred language:";
            // 
            // cboLanguage
            // 
            this.cboLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLanguage.FormattingEnabled = true;
            this.cboLanguage.Location = new System.Drawing.Point(135, 63);
            this.cboLanguage.Name = "cboLanguage";
            this.cboLanguage.Size = new System.Drawing.Size(75, 20);
            this.cboLanguage.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 17;
            this.label1.Text = "Control words:";
            // 
            // btnConfigControlWords
            // 
            this.btnConfigControlWords.Location = new System.Drawing.Point(134, 121);
            this.btnConfigControlWords.Name = "btnConfigControlWords";
            this.btnConfigControlWords.Size = new System.Drawing.Size(75, 23);
            this.btnConfigControlWords.TabIndex = 18;
            this.btnConfigControlWords.Text = "Config";
            this.btnConfigControlWords.UseVisualStyleBackColor = true;
            this.btnConfigControlWords.Click += new System.EventHandler(this.btnConfigControlWords_Click);
            // 
            // btnConfigCustomWords
            // 
            this.btnConfigCustomWords.Location = new System.Drawing.Point(134, 155);
            this.btnConfigCustomWords.Name = "btnConfigCustomWords";
            this.btnConfigCustomWords.Size = new System.Drawing.Size(75, 23);
            this.btnConfigCustomWords.TabIndex = 20;
            this.btnConfigCustomWords.Text = "Config";
            this.btnConfigCustomWords.UseVisualStyleBackColor = true;
            this.btnConfigCustomWords.Click += new System.EventHandler(this.btnConfigCustomWords_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "Custom words:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "Grammar priority:";
            // 
            // dgvGrammarPriority
            // 
            this.dgvGrammarPriority.AllowUserToAddRows = false;
            this.dgvGrammarPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGrammarPriority.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrammarPriority.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GrammarType,
            this.GrammarPriority});
            this.dgvGrammarPriority.Location = new System.Drawing.Point(135, 187);
            this.dgvGrammarPriority.Name = "dgvGrammarPriority";
            this.dgvGrammarPriority.RowHeadersVisible = false;
            this.dgvGrammarPriority.RowHeadersWidth = 20;
            this.dgvGrammarPriority.RowTemplate.Height = 23;
            this.dgvGrammarPriority.Size = new System.Drawing.Size(311, 140);
            this.dgvGrammarPriority.TabIndex = 22;
            // 
            // GrammarType
            // 
            this.GrammarType.DataPropertyName = "GrammarType";
            this.GrammarType.FillWeight = 150F;
            this.GrammarType.Frozen = true;
            this.GrammarType.HeaderText = "Grammar type";
            this.GrammarType.Name = "GrammarType";
            this.GrammarType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.GrammarType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // GrammarPriority
            // 
            this.GrammarPriority.DataPropertyName = "GrammarPriority";
            this.GrammarPriority.FillWeight = 150F;
            this.GrammarPriority.HeaderText = "Priority";
            this.GrammarPriority.Name = "GrammarPriority";
            // 
            // btnLanguageSetting
            // 
            this.btnLanguageSetting.Location = new System.Drawing.Point(134, 89);
            this.btnLanguageSetting.Name = "btnLanguageSetting";
            this.btnLanguageSetting.Size = new System.Drawing.Size(75, 23);
            this.btnLanguageSetting.TabIndex = 24;
            this.btnLanguageSetting.Text = "Config";
            this.btnLanguageSetting.UseVisualStyleBackColor = true;
            this.btnLanguageSetting.Click += new System.EventHandler(this.btnLanguageSetting_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "Language settings:";
            // 
            // chkStartRecordWhenStartup
            // 
            this.chkStartRecordWhenStartup.AutoSize = true;
            this.chkStartRecordWhenStartup.Location = new System.Drawing.Point(13, 38);
            this.chkStartRecordWhenStartup.Name = "chkStartRecordWhenStartup";
            this.chkStartRecordWhenStartup.Size = new System.Drawing.Size(174, 16);
            this.chkStartRecordWhenStartup.TabIndex = 26;
            this.chkStartRecordWhenStartup.Text = "Start record when startup";
            this.chkStartRecordWhenStartup.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(13, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(264, 16);
            this.checkBox1.TabIndex = 27;
            this.checkBox1.Text = "Top this application form on other forms";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnLogFileOpenTool
            // 
            this.btnLogFileOpenTool.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogFileOpenTool.Location = new System.Drawing.Point(410, 336);
            this.btnLogFileOpenTool.Name = "btnLogFileOpenTool";
            this.btnLogFileOpenTool.Size = new System.Drawing.Size(36, 23);
            this.btnLogFileOpenTool.TabIndex = 30;
            this.btnLogFileOpenTool.Text = "...";
            this.btnLogFileOpenTool.UseVisualStyleBackColor = true;
            this.btnLogFileOpenTool.Click += new System.EventHandler(this.btnLogFileOpenTool_Click);
            // 
            // txtLogFileOpenTool
            // 
            this.txtLogFileOpenTool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogFileOpenTool.Location = new System.Drawing.Point(134, 338);
            this.txtLogFileOpenTool.Name = "txtLogFileOpenTool";
            this.txtLogFileOpenTool.Size = new System.Drawing.Size(270, 21);
            this.txtLogFileOpenTool.TabIndex = 29;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 341);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "Log file open tool:";
            // 
            // dlgLogFileOpenTool
            // 
            this.dlgLogFileOpenTool.Filter = "exe file|*.exe|all files|*.*";
            // 
            // frmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 413);
            this.Controls.Add(this.btnLogFileOpenTool);
            this.Controls.Add(this.txtLogFileOpenTool);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.chkStartRecordWhenStartup);
            this.Controls.Add(this.btnLanguageSetting);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dgvGrammarPriority);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnConfigCustomWords);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnConfigControlWords);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboLanguage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setting";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrammarPriority)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboLanguage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConfigControlWords;
        private System.Windows.Forms.Button btnConfigCustomWords;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvGrammarPriority;
        private System.Windows.Forms.DataGridViewComboBoxColumn GrammarType;
        private System.Windows.Forms.DataGridViewComboBoxColumn GrammarPriority;
        private System.Windows.Forms.Button btnLanguageSetting;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkStartRecordWhenStartup;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnLogFileOpenTool;
        private System.Windows.Forms.TextBox txtLogFileOpenTool;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.OpenFileDialog dlgLogFileOpenTool;
    }
}