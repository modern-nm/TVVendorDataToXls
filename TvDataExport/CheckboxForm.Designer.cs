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
            flowLayoutPanel1 = new FlowLayoutPanel();
            btnSave = new Button();
            btnReload = new Button();
            txtNewItem = new TextBox();
            btnAdd = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            removeToolStripMenuItem = new ToolStripMenuItem();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Location = new Point(63, 84);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(242, 213);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(63, 303);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnReload
            // 
            btnReload.Location = new Point(230, 303);
            btnReload.Name = "btnReload";
            btnReload.Size = new Size(75, 23);
            btnReload.TabIndex = 2;
            btnReload.Text = "Reload";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // txtNewItem
            // 
            txtNewItem.Location = new Point(63, 55);
            txtNewItem.Name = "txtNewItem";
            txtNewItem.Size = new Size(100, 23);
            txtNewItem.TabIndex = 3;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(230, 54);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
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
            // CheckboxForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(406, 338);
            Controls.Add(btnAdd);
            Controls.Add(txtNewItem);
            Controls.Add(btnReload);
            Controls.Add(btnSave);
            Controls.Add(flowLayoutPanel1);
            Name = "CheckboxForm";
            Text = "Settings";
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnSave;
        private Button btnReload;
        private TextBox txtNewItem;
        private Button btnAdd;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem removeToolStripMenuItem;
    }
}