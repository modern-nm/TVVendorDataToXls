namespace TvDataExport
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            exportButton = new Button();
            textBox1 = new TextBox();
            pathButton = new Button();
            folderBrowserDialog1 = new FolderBrowserDialog();
            groupBox1 = new GroupBox();
            exportIniRadioButton = new RadioButton();
            exportPanelRadioButton = new RadioButton();
            exportModelRadioButton = new RadioButton();
            logRichTextBox = new RichTextBox();
            progressBar1 = new ProgressBar();
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // exportButton
            // 
            exportButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            exportButton.Location = new Point(347, 410);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(456, 95);
            exportButton.TabIndex = 1;
            exportButton.Text = "Export";
            exportButton.UseVisualStyleBackColor = true;
            exportButton.Click += exportButton_Click;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(347, 176);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(326, 23);
            textBox1.TabIndex = 2;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // pathButton
            // 
            pathButton.AccessibleName = "";
            pathButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pathButton.Location = new Point(693, 175);
            pathButton.Name = "pathButton";
            pathButton.Size = new Size(110, 24);
            pathButton.TabIndex = 3;
            pathButton.Text = "Select folder";
            pathButton.UseVisualStyleBackColor = true;
            pathButton.Click += pathButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(exportIniRadioButton);
            groupBox1.Controls.Add(exportPanelRadioButton);
            groupBox1.Controls.Add(exportModelRadioButton);
            groupBox1.Location = new Point(431, 59);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(200, 100);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Choose option";
            // 
            // exportIniRadioButton
            // 
            exportIniRadioButton.AutoSize = true;
            exportIniRadioButton.Checked = true;
            exportIniRadioButton.Location = new Point(22, 72);
            exportIniRadioButton.Name = "exportIniRadioButton";
            exportIniRadioButton.Size = new Size(81, 19);
            exportIniRadioButton.TabIndex = 2;
            exportIniRadioButton.TabStop = true;
            exportIniRadioButton.Text = "read as .ini";
            exportIniRadioButton.UseVisualStyleBackColor = true;
            // 
            // exportPanelRadioButton
            // 
            exportPanelRadioButton.AutoSize = true;
            exportPanelRadioButton.Location = new Point(22, 47);
            exportPanelRadioButton.Name = "exportPanelRadioButton";
            exportPanelRadioButton.Size = new Size(122, 19);
            exportPanelRadioButton.TabIndex = 1;
            exportPanelRadioButton.Text = "read as .json panel";
            exportPanelRadioButton.UseVisualStyleBackColor = true;
            // 
            // exportModelRadioButton
            // 
            exportModelRadioButton.AutoSize = true;
            exportModelRadioButton.Location = new Point(22, 22);
            exportModelRadioButton.Name = "exportModelRadioButton";
            exportModelRadioButton.Size = new Size(127, 19);
            exportModelRadioButton.TabIndex = 0;
            exportModelRadioButton.Text = "read as .json model";
            exportModelRadioButton.UseVisualStyleBackColor = true;
            // 
            // logRichTextBox
            // 
            logRichTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            logRichTextBox.Location = new Point(12, 81);
            logRichTextBox.Name = "logRichTextBox";
            logRichTextBox.Size = new Size(284, 424);
            logRichTextBox.TabIndex = 3;
            logRichTextBox.Text = "";
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(347, 357);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(456, 23);
            progressBar1.TabIndex = 5;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button1.Location = new Point(728, 59);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 6;
            button1.Text = "Settings";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button2.Location = new Point(758, 88);
            button2.Name = "button2";
            button2.Size = new Size(45, 23);
            button2.TabIndex = 7;
            button2.Text = "Help";
            button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(12, 53);
            label1.Name = "label1";
            label1.Size = new Size(43, 25);
            label1.TabIndex = 8;
            label1.Text = "Log";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(881, 517);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(progressBar1);
            Controls.Add(logRichTextBox);
            Controls.Add(groupBox1);
            Controls.Add(pathButton);
            Controls.Add(textBox1);
            Controls.Add(exportButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "TvDataExport";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button exportButton;
        private TextBox textBox1;
        private Button pathButton;
        private FolderBrowserDialog folderBrowserDialog1;
        private GroupBox groupBox1;
        private RadioButton exportIniRadioButton;
        private RadioButton exportPanelRadioButton;
        private RadioButton exportModelRadioButton;
        private RichTextBox logRichTextBox;
        private ProgressBar progressBar1;
        private Button button1;
        private Button button2;
        private Label label1;
    }
}
