using CsvHelper;
using SpeechCodingHandler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpeechCoding
{
    public partial class frmControlWordSetting : Form
    {
        public Setting Setting { get; set; } = new Setting();

        public List<ControlWordSetting> ControlWordSettings { get { return this.controlWordSettings; } }

        private List<ControlWordSetting> controlWordSettings = new List<ControlWordSetting>();

        public frmControlWordSetting()
        {
            InitializeComponent();

            UiHelper.SetUI(this);
        }

        private void frmBuildOpenToolSetting_Load(object sender, EventArgs e)
        {
            Config config = ConfigManager.GetConfig();

            foreach (ControlWordSetting controlWord in this.Setting.ControlWords)
            {
                this.controlWordSettings.Add(controlWord);
            }

            if (this.controlWordSettings.Count == 0)
            {
                this.controlWordSettings.AddRange(config.ControlWords);
            }

            this.dataGridView1.AutoGenerateColumns = false;
            this.controlWordSettings.ForEach(item =>
            {
                this.AddDataGridViewRow(item);
            });
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddControlWord frmAddControlWord = new frmAddControlWord();
            DialogResult result = frmAddControlWord.ShowDialog();
            if (result == DialogResult.OK)
            {
                ControlWordSetting controlWordSetting = frmAddControlWord.ControlWordSetting;
                foreach (DataGridViewRow row in this.dataGridView1.Rows)
                {
                    string control = row.Cells["Control"].Value?.ToString();
                    if (control == controlWordSetting.Control)
                    {
                        MessageBox.Show($"Control \"{control}\" has already existed.");
                        return;
                    }
                }

                this.controlWordSettings.Add(controlWordSetting);

                this.AddDataGridViewRow(controlWordSetting);

                this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.RowCount - 1;
            }
        }

        private void AddDataGridViewRow(ControlWordSetting record)
        {
            bool isKeyboard = record.IsKeyboard && KeyboardHelper.IsKeyboard(record.Control);

            this.dataGridView1.Rows.Add(record.Control, record.Word, isKeyboard, record.UseCodeTemplate);
        }

        private void UpdateDataGridViewRow(ControlWordSetting record)
        {
            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                string control = row.Cells[nameof(ControlWordSetting.Control)].Value?.ToString();
                if (!string.IsNullOrEmpty(control) && control.ToLower() == record.Control?.ToLower())
                {
                    row.Cells[nameof(ControlWordSetting.Word)].Value = record.Word?.ToLower();
                    row.Cells[nameof(ControlWordSetting.IsKeyboard)].Value = record.IsKeyboard && KeyboardHelper.IsKeyboard(control);
                    row.Cells[nameof(ControlWordSetting.UseCodeTemplate)].Value = record.UseCodeTemplate;
                    break;
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int count = this.dataGridView1.SelectedRows.Count;
            if (count == 0)
            {
                MessageBox.Show("Please select row by clicking row header.");
                return;
            }

            for (int i = count - 1; i >= 0; i--)
            {
                int rowIndex = this.dataGridView1.SelectedRows[i].Index;
                string control = this.dataGridView1.Rows[rowIndex].Cells["Control"]?.Value?.ToString();
                ControlWordSetting controlWordSetting = this.controlWordSettings.FirstOrDefault(item => item.Control == control);

                if (controlWordSetting != null)
                {
                    this.controlWordSettings.Remove(controlWordSetting);
                }
                this.dataGridView1.Rows.RemoveAt(rowIndex);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            string columnName = this.dataGridView1.Columns[columnIndex].Name;

            string control = this.dataGridView1.Rows[rowIndex].Cells["Control"]?.Value?.ToString();
            ControlWordSetting controlWordSetting = this.controlWordSettings.FirstOrDefault(item => item.Control == control);

            if (controlWordSetting != null)
            {
                string value = this.dataGridView1.Rows[rowIndex].Cells[columnName]?.Value?.ToString();

                if (columnName == nameof(ControlWordSetting.Word))
                {
                    controlWordSetting.Word = value;
                }
                else if (columnName == nameof(ControlWordSetting.IsKeyboard))
                {
                    if (KeyboardHelper.IsKeyboard(control))
                    {
                        controlWordSetting.IsKeyboard = Convert.ToBoolean(value);
                    }
                    else
                    {
                        MessageBox.Show($"Control \"{control}\" is invalid keyboard.");
                        controlWordSetting.IsKeyboard = false;                      
                        this.dataGridView1.CurrentCell = this.dataGridView1.Rows[rowIndex].Cells[columnName];
                        this.dataGridView1.Rows[rowIndex].Cells[columnName].Value = false;
                    }
                }
                else if (columnName == nameof(ControlWordSetting.UseCodeTemplate))
                {
                    controlWordSetting.UseCodeTemplate = Convert.ToBoolean(value);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();
            this.controlWordSettings.Clear();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.FileName = "";
            DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                CsvReader reader = new CsvReader(new StringReader(File.ReadAllText(this.openFileDialog1.FileName)), CultureInfo.CurrentCulture);
                reader.Configuration.HasHeaderRecord = this.chkIncludeHeader.Checked;

                List<ControlWordSetting> records = reader.GetRecords<ControlWordSetting>().ToList();
                foreach (ControlWordSetting record in records)
                {
                    if (!string.IsNullOrEmpty(record.Control))
                    {
                        ControlWordSetting oldSetting = this.controlWordSettings.FirstOrDefault(item => item.Control?.ToLower() == record.Control.ToLower());
                        if (oldSetting != null)
                        {
                            oldSetting.Word = record.Word;
                            oldSetting.UseCodeTemplate = record.UseCodeTemplate;
                            this.UpdateDataGridViewRow(record);
                        }
                        else
                        {
                            this.controlWordSettings.Add(record);
                            this.AddDataGridViewRow(record);
                        }
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DialogResult result = this.saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string filePath = this.saveFileDialog1.FileName;
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    CsvWriter writer = new CsvWriter(sw, CultureInfo.CurrentCulture);
                    writer.Configuration.HasHeaderRecord = this.chkIncludeHeader.Checked;

                    writer.WriteRecords<ControlWordSetting>(this.controlWordSettings);

                    MessageBox.Show("Exported.");
                }
            }
        }
    }
}
