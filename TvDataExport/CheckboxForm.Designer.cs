namespace TvDataExport
{
    partial class CheckboxForm
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
            components = new System.ComponentModel.Container();
            flowLayoutPanelIni = new FlowLayoutPanel();
            btnSave = new Button();
            btnReload = new Button();
            txtNewItem = new TextBox();
            btnAdd = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            removeToolStripMenuItem = new ToolStripMenuItem();
            tabControlSettings = new TabControl();
            tabPageIni = new TabPage();
            tabPageModel = new TabPage();
            flowLayoutPanelModel = new FlowLayoutPanel();
            contextMenuStrip1.SuspendLayout();
            tabControlSettings.SuspendLayout();
            tabPageIni.SuspendLayout();
            tabPageModel.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanelIni
            // 
            flowLayoutPanelIni.AutoScroll = true;
            flowLayoutPanelIni.BackColor = SystemColors.ControlLightLight;
            flowLayoutPanelIni.Dock = DockStyle.Fill;
            flowLayoutPanelIni.Location = new Point(3, 3);
            flowLayoutPanelIni.Name = "flowLayoutPanelIni";
            flowLayoutPanelIni.Size = new Size(248, 326);
            flowLayoutPanelIni.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSave.Location = new Point(46, 436);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(107, 23);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnReload
            // 
            btnReload.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnReload.Location = new Point(175, 436);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(113, 23);
            btnReload.TabIndex = 2;
            btnReload.Text = "Reload";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // txtNewItem
            // 
            txtNewItem.Location = new Point(46, 41);
            txtNewItem.Name = "txtNewItem";
            txtNewItem.Size = new Size(107, 23);
            txtNewItem.TabIndex = 3;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(175, 40);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(113, 23);
            btnAdd.TabIndex = 4;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { removeToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(119, 26);
            // 
            // removeToolStripMenuItem
            // 
            removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            removeToolStripMenuItem.Size = new Size(118, 22);
            removeToolStripMenuItem.Text = "Удалить";
            removeToolStripMenuItem.Click += RemoveToolStripMenuItem_Click;
            // 
            // TabControlSettings
            // 
            tabControlSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControlSettings.Controls.Add(tabPageIni);
            tabControlSettings.Controls.Add(tabPageModel);
            tabControlSettings.Location = new Point(36, 70);
            tabControlSettings.Name = "TabControlSettings";
            tabControlSettings.SelectedIndex = 0;
            tabControlSettings.Size = new Size(262, 360);
            tabControlSettings.TabIndex = 5;
            // 
            // tabPageIni
            // 
            tabPageIni.BackColor = SystemColors.Control;
            tabPageIni.Controls.Add(flowLayoutPanelIni);
            tabPageIni.Location = new Point(4, 24);
            tabPageIni.Name = "tabPageIni";
            tabPageIni.Padding = new Padding(3);
            tabPageIni.Size = new Size(254, 332);
            tabPageIni.TabIndex = 0;
            tabPageIni.Text = "Ini settings";
            // 
            // tabPageModel
            // 
            tabPageModel.BackColor = SystemColors.Control;
            tabPageModel.Controls.Add(flowLayoutPanelModel);
            tabPageModel.Location = new Point(4, 24);
            tabPageModel.Name = "tabPageModel";
            tabPageModel.Padding = new Padding(3);
            tabPageModel.Size = new Size(254, 332);
            tabPageModel.TabIndex = 2;
            tabPageModel.Text = "Model settings";
            // 
            // flowLayoutPanelModel
            // 
            flowLayoutPanelModel.AutoScroll = true;
            flowLayoutPanelModel.BackColor = SystemColors.ControlLightLight;
            flowLayoutPanelModel.Location = new Point(3, 3);
            flowLayoutPanelModel.Name = "flowLayoutPanelModel";
            flowLayoutPanelModel.Size = new Size(248, 326);
            flowLayoutPanelModel.TabIndex = 5;
            // 
            // CheckboxForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(339, 483);
            Controls.Add(btnSave);
            Controls.Add(btnAdd);
            Controls.Add(tabControlSettings);
            Controls.Add(btnReload);
            Controls.Add(txtNewItem);
            MinimumSize = new Size(355, 522);
            Name = "CheckboxForm";
            Text = "Settings";
            contextMenuStrip1.ResumeLayout(false);
            tabControlSettings.ResumeLayout(false);
            tabPageIni.ResumeLayout(false);
            tabPageModel.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanelIni;
        private Button btnSave;
        private Button btnReload;
        private TextBox txtNewItem;
        private Button btnAdd;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem removeToolStripMenuItem;
        private TabControl tabControlSettings;
        private TabPage tabPageIni;
        private TabPage tabPageModel;
        private FlowLayoutPanel flowLayoutPanelModel;
    }
}