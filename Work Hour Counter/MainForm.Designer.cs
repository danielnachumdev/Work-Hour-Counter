
namespace Work_Hour_Counter
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.OpenButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SaveAsButton = new System.Windows.Forms.Button();
            this.PathLabel = new System.Windows.Forms.Label();
            this.ExportButton = new System.Windows.Forms.Button();
            this.DayCheckBox = new System.Windows.Forms.CheckBox();
            this.WeekCheckBox = new System.Windows.Forms.CheckBox();
            this.MonthCheckBox = new System.Windows.Forms.CheckBox();
            this.YearCheckBox = new System.Windows.Forms.CheckBox();
            this.CreaditLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(236, 68);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(112, 41);
            this.OpenButton.TabIndex = 0;
            this.OpenButton.Text = "Open";
            this.OpenButton.UseVisualStyleBackColor = true;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(236, 168);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 58);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // SaveAsButton
            // 
            this.SaveAsButton.Location = new System.Drawing.Point(356, 219);
            this.SaveAsButton.Name = "SaveAsButton";
            this.SaveAsButton.Size = new System.Drawing.Size(116, 57);
            this.SaveAsButton.TabIndex = 2;
            this.SaveAsButton.Text = "Save As";
            this.SaveAsButton.UseVisualStyleBackColor = true;
            this.SaveAsButton.Click += new System.EventHandler(this.SaveAsButton_Click);
            // 
            // PathLabel
            // 
            this.PathLabel.AutoSize = true;
            this.PathLabel.Location = new System.Drawing.Point(614, 121);
            this.PathLabel.Name = "PathLabel";
            this.PathLabel.Size = new System.Drawing.Size(80, 20);
            this.PathLabel.TabIndex = 3;
            this.PathLabel.Text = "pathLabel";
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(235, 299);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(99, 54);
            this.ExportButton.TabIndex = 4;
            this.ExportButton.Text = "Export";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // DayCheckBox
            // 
            this.DayCheckBox.AutoSize = true;
            this.DayCheckBox.Checked = true;
            this.DayCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DayCheckBox.Location = new System.Drawing.Point(417, 316);
            this.DayCheckBox.Name = "DayCheckBox";
            this.DayCheckBox.Size = new System.Drawing.Size(63, 24);
            this.DayCheckBox.TabIndex = 5;
            this.DayCheckBox.Text = "Day";
            this.DayCheckBox.UseVisualStyleBackColor = true;
            // 
            // WeekCheckBox
            // 
            this.WeekCheckBox.AutoSize = true;
            this.WeekCheckBox.Enabled = false;
            this.WeekCheckBox.Location = new System.Drawing.Point(429, 347);
            this.WeekCheckBox.Name = "WeekCheckBox";
            this.WeekCheckBox.Size = new System.Drawing.Size(76, 24);
            this.WeekCheckBox.TabIndex = 6;
            this.WeekCheckBox.Text = "Week";
            this.WeekCheckBox.UseVisualStyleBackColor = true;
            // 
            // MonthCheckBox
            // 
            this.MonthCheckBox.AutoSize = true;
            this.MonthCheckBox.Enabled = false;
            this.MonthCheckBox.Location = new System.Drawing.Point(429, 378);
            this.MonthCheckBox.Name = "MonthCheckBox";
            this.MonthCheckBox.Size = new System.Drawing.Size(80, 24);
            this.MonthCheckBox.TabIndex = 7;
            this.MonthCheckBox.Text = "Month";
            this.MonthCheckBox.UseVisualStyleBackColor = true;
            // 
            // YearCheckBox
            // 
            this.YearCheckBox.AutoSize = true;
            this.YearCheckBox.Enabled = false;
            this.YearCheckBox.Location = new System.Drawing.Point(417, 409);
            this.YearCheckBox.Name = "YearCheckBox";
            this.YearCheckBox.Size = new System.Drawing.Size(69, 24);
            this.YearCheckBox.TabIndex = 8;
            this.YearCheckBox.Text = "Year";
            this.YearCheckBox.UseVisualStyleBackColor = true;
            // 
            // CreaditLabel
            // 
            this.CreaditLabel.Location = new System.Drawing.Point(642, 386);
            this.CreaditLabel.Name = "CreaditLabel";
            this.CreaditLabel.Size = new System.Drawing.Size(146, 47);
            this.CreaditLabel.TabIndex = 9;
            this.CreaditLabel.Text = "All rights reserved: Daniel Nachum";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CreaditLabel);
            this.Controls.Add(this.YearCheckBox);
            this.Controls.Add(this.MonthCheckBox);
            this.Controls.Add(this.WeekCheckBox);
            this.Controls.Add(this.DayCheckBox);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.PathLabel);
            this.Controls.Add(this.SaveAsButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.OpenButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Work Hour Counter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button SaveAsButton;
        private System.Windows.Forms.Label PathLabel;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.CheckBox DayCheckBox;
        private System.Windows.Forms.CheckBox WeekCheckBox;
        private System.Windows.Forms.CheckBox MonthCheckBox;
        private System.Windows.Forms.CheckBox YearCheckBox;
        private System.Windows.Forms.Label CreaditLabel;
    }
}

