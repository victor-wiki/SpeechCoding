namespace SpeechCoding
{
    partial class frmAddControlWord
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
            this.rbKeybord = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.rbCustom = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cboControl = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWord = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkUseCodeTemplate = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // rbKeybord
            // 
            this.rbKeybord.AutoSize = true;
            this.rbKeybord.Checked = true;
            this.rbKeybord.Location = new System.Drawing.Point(81, 21);
            this.rbKeybord.Name = "rbKeybord";
            this.rbKeybord.Size = new System.Drawing.Size(71, 16);
            this.rbKeybord.TabIndex = 0;
            this.rbKeybord.TabStop = true;
            this.rbKeybord.Text = "Keyboard";
            this.rbKeybord.UseVisualStyleBackColor = true;
            this.rbKeybord.CheckedChanged += new System.EventHandler(this.rbKeybord_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Type:";
            // 
            // rbCustom
            // 
            this.rbCustom.AutoSize = true;
            this.rbCustom.Location = new System.Drawing.Point(168, 21);
            this.rbCustom.Name = "rbCustom";
            this.rbCustom.Size = new System.Drawing.Size(59, 16);
            this.rbCustom.TabIndex = 2;
            this.rbCustom.Text = "Custom";
            this.rbCustom.UseVisualStyleBackColor = true;
            this.rbCustom.CheckedChanged += new System.EventHandler(this.rbText_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Control:";
            // 
            // cboControl
            // 
            this.cboControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboControl.FormattingEnabled = true;
            this.cboControl.Location = new System.Drawing.Point(79, 47);
            this.cboControl.Name = "cboControl";
            this.cboControl.Size = new System.Drawing.Size(125, 20);
            this.cboControl.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Word:";
            // 
            // txtWord
            // 
            this.txtWord.Location = new System.Drawing.Point(79, 80);
            this.txtWord.Name = "txtWord";
            this.txtWord.Size = new System.Drawing.Size(125, 21);
            this.txtWord.TabIndex = 6;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(37, 152);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(133, 152);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkUseCodeTemplate
            // 
            this.chkUseCodeTemplate.AutoSize = true;
            this.chkUseCodeTemplate.Location = new System.Drawing.Point(78, 119);
            this.chkUseCodeTemplate.Name = "chkUseCodeTemplate";
            this.chkUseCodeTemplate.Size = new System.Drawing.Size(126, 16);
            this.chkUseCodeTemplate.TabIndex = 9;
            this.chkUseCodeTemplate.Text = "Use code template";
            this.chkUseCodeTemplate.UseVisualStyleBackColor = true;
            // 
            // frmAddControlWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(250, 187);
            this.Controls.Add(this.chkUseCodeTemplate);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtWord);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboControl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbCustom);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rbKeybord);
            this.MinimizeBox = false;
            this.Name = "frmAddControlWord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add control word";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAddControlWord_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rbKeybord;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbCustom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboControl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWord;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkUseCodeTemplate;
    }
}