namespace SpeechCoding
{
    partial class frmCustomWord
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
            this.tabCustomWords = new System.Windows.Forms.TabControl();
            this.tabIncludeWords = new System.Windows.Forms.TabPage();
            this.txtIncludeWords = new System.Windows.Forms.TextBox();
            this.tabExcludeWords = new System.Windows.Forms.TabPage();
            this.txtExcludeWords = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tabCustomWords.SuspendLayout();
            this.tabIncludeWords.SuspendLayout();
            this.tabExcludeWords.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCustomWords
            // 
            this.tabCustomWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCustomWords.Controls.Add(this.tabIncludeWords);
            this.tabCustomWords.Controls.Add(this.tabExcludeWords);
            this.tabCustomWords.Location = new System.Drawing.Point(0, 5);
            this.tabCustomWords.Name = "tabCustomWords";
            this.tabCustomWords.SelectedIndex = 0;
            this.tabCustomWords.Size = new System.Drawing.Size(786, 414);
            this.tabCustomWords.TabIndex = 0;
            // 
            // tabIncludeWords
            // 
            this.tabIncludeWords.Controls.Add(this.txtIncludeWords);
            this.tabIncludeWords.Location = new System.Drawing.Point(4, 22);
            this.tabIncludeWords.Name = "tabIncludeWords";
            this.tabIncludeWords.Padding = new System.Windows.Forms.Padding(3);
            this.tabIncludeWords.Size = new System.Drawing.Size(778, 388);
            this.tabIncludeWords.TabIndex = 0;
            this.tabIncludeWords.Text = "Include words";
            this.tabIncludeWords.UseVisualStyleBackColor = true;
            // 
            // txtIncludeWords
            // 
            this.txtIncludeWords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIncludeWords.Location = new System.Drawing.Point(3, 3);
            this.txtIncludeWords.Multiline = true;
            this.txtIncludeWords.Name = "txtIncludeWords";
            this.txtIncludeWords.Size = new System.Drawing.Size(772, 382);
            this.txtIncludeWords.TabIndex = 0;
            // 
            // tabExcludeWords
            // 
            this.tabExcludeWords.Controls.Add(this.txtExcludeWords);
            this.tabExcludeWords.Location = new System.Drawing.Point(4, 22);
            this.tabExcludeWords.Name = "tabExcludeWords";
            this.tabExcludeWords.Padding = new System.Windows.Forms.Padding(3);
            this.tabExcludeWords.Size = new System.Drawing.Size(778, 388);
            this.tabExcludeWords.TabIndex = 1;
            this.tabExcludeWords.Text = "Exclude words";
            this.tabExcludeWords.UseVisualStyleBackColor = true;
            // 
            // txtExcludeWords
            // 
            this.txtExcludeWords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExcludeWords.Location = new System.Drawing.Point(3, 3);
            this.txtExcludeWords.Multiline = true;
            this.txtExcludeWords.Name = "txtExcludeWords";
            this.txtExcludeWords.Size = new System.Drawing.Size(772, 382);
            this.txtExcludeWords.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(701, 421);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(620, 421);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 7;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmCustomWord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 450);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tabCustomWords);
            this.Controls.Add(this.btnOK);
            this.Name = "frmCustomWord";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Custom words";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmCustomWord_Load);
            this.tabCustomWords.ResumeLayout(false);
            this.tabIncludeWords.ResumeLayout(false);
            this.tabIncludeWords.PerformLayout();
            this.tabExcludeWords.ResumeLayout(false);
            this.tabExcludeWords.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabCustomWords;
        private System.Windows.Forms.TabPage tabIncludeWords;
        private System.Windows.Forms.TextBox txtIncludeWords;
        private System.Windows.Forms.TabPage tabExcludeWords;
        private System.Windows.Forms.TextBox txtExcludeWords;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}