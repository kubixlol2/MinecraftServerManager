namespace MinecraftServerManager
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.serversTreeLabel = new System.Windows.Forms.Label();
            this.hideMenusTimer = new System.Windows.Forms.Timer(this.components);
            this.treeTabsContainer = new System.Windows.Forms.SplitContainer();
            this.settingsMenu = new MinecraftServerManager.Controls.Button();
            this.serversTree = new MinecraftServerManager.Controls.ServersTreeView();
            this.tabs = new MinecraftServerManager.Controls.Tabs();
            this.addServerMenu = new MinecraftServerManager.Controls.Button();
            ((System.ComponentModel.ISupportInitialize)(this.treeTabsContainer)).BeginInit();
            this.treeTabsContainer.Panel1.SuspendLayout();
            this.treeTabsContainer.Panel2.SuspendLayout();
            this.treeTabsContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // serversTreeLabel
            // 
            this.serversTreeLabel.BackColor = System.Drawing.Color.White;
            this.serversTreeLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.serversTreeLabel.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.serversTreeLabel.ForeColor = System.Drawing.Color.Black;
            this.serversTreeLabel.Location = new System.Drawing.Point(0, 0);
            this.serversTreeLabel.MinimumSize = new System.Drawing.Size(150, 0);
            this.serversTreeLabel.Name = "serversTreeLabel";
            this.serversTreeLabel.Size = new System.Drawing.Size(151, 20);
            this.serversTreeLabel.TabIndex = 0;
            this.serversTreeLabel.Text = "TWOJE SERWERY:";
            this.serversTreeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hideMenusTimer
            // 
            this.hideMenusTimer.Enabled = true;
            this.hideMenusTimer.Interval = 50;
            this.hideMenusTimer.Tick += new System.EventHandler(this.hideMenusTimer_Tick);
            // 
            // treeTabsContainer
            // 
            this.treeTabsContainer.ForeColor = System.Drawing.SystemColors.Control;
            this.treeTabsContainer.Location = new System.Drawing.Point(5, 30);
            this.treeTabsContainer.Name = "treeTabsContainer";
            // 
            // treeTabsContainer.Panel1
            // 
            this.treeTabsContainer.Panel1.Controls.Add(this.serversTree);
            this.treeTabsContainer.Panel1.Controls.Add(this.serversTreeLabel);
            this.treeTabsContainer.Panel1MinSize = 150;
            // 
            // treeTabsContainer.Panel2
            // 
            this.treeTabsContainer.Panel2.Controls.Add(this.tabs);
            this.treeTabsContainer.Panel2MinSize = 500;
            this.treeTabsContainer.Size = new System.Drawing.Size(837, 383);
            this.treeTabsContainer.SplitterDistance = 151;
            this.treeTabsContainer.TabIndex = 0;
            this.treeTabsContainer.TabStop = false;
            this.treeTabsContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.treeTabsContainer_SplitterMoved);
            // 
            // settingsMenu
            // 
            this.settingsMenu.FlatAppearance.BorderSize = 0;
            this.settingsMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsMenu.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.settingsMenu.Image = global::MinecraftServerManager.Properties.Resources.MenuSettings;
            this.settingsMenu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.settingsMenu.Location = new System.Drawing.Point(140, 0);
            this.settingsMenu.Margin = new System.Windows.Forms.Padding(0);
            this.settingsMenu.Name = "settingsMenu";
            this.settingsMenu.Size = new System.Drawing.Size(120, 30);
            this.settingsMenu.TabIndex = 1;
            this.settingsMenu.TabStop = false;
            this.settingsMenu.Text = "USTAWIENIA";
            this.settingsMenu.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.settingsMenu.UseVisualStyleBackColor = true;
            this.settingsMenu.MouseEnter += new System.EventHandler(this.settingsMenu_MouseEnter);
            // 
            // serversTree
            // 
            this.serversTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.serversTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serversTree.ImageIndex = 0;
            this.serversTree.Location = new System.Drawing.Point(0, 20);
            this.serversTree.MinimumSize = new System.Drawing.Size(150, 0);
            this.serversTree.Name = "serversTree";
            this.serversTree.SelectedImageIndex = 0;
            this.serversTree.Size = new System.Drawing.Size(151, 363);
            this.serversTree.TabIndex = 0;
            this.serversTree.TabStop = false;
            // 
            // tabs
            // 
            this.tabs.BackColor = System.Drawing.Color.Gold;
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.ForeColor = System.Drawing.Color.Black;
            this.tabs.Location = new System.Drawing.Point(0, 0);
            this.tabs.Name = "tabs";
            this.tabs.Size = new System.Drawing.Size(682, 383);
            this.tabs.TabIndex = 0;
            this.tabs.TabStop = false;
            // 
            // addServerMenu
            // 
            this.addServerMenu.FlatAppearance.BorderSize = 0;
            this.addServerMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addServerMenu.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.addServerMenu.Image = global::MinecraftServerManager.Properties.Resources.MenuAdd;
            this.addServerMenu.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addServerMenu.Location = new System.Drawing.Point(0, 0);
            this.addServerMenu.Margin = new System.Windows.Forms.Padding(0);
            this.addServerMenu.Name = "addServerMenu";
            this.addServerMenu.Size = new System.Drawing.Size(140, 30);
            this.addServerMenu.TabIndex = 0;
            this.addServerMenu.TabStop = false;
            this.addServerMenu.Text = "DODAJ SERWER";
            this.addServerMenu.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.addServerMenu.UseVisualStyleBackColor = true;
            this.addServerMenu.MouseEnter += new System.EventHandler(this.addServerMenu_MouseEnter);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gold;
            this.ClientSize = new System.Drawing.Size(850, 461);
            this.Controls.Add(this.settingsMenu);
            this.Controls.Add(this.treeTabsContainer);
            this.Controls.Add(this.addServerMenu);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 450);
            this.Name = "MainWindow";
            this.Text = "Minecraft - menedżer serwera";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.MainWindow_PreviewKeyDown);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            this.treeTabsContainer.Panel1.ResumeLayout(false);
            this.treeTabsContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeTabsContainer)).EndInit();
            this.treeTabsContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label serversTreeLabel;
        public Controls.ServersTreeView serversTree = null;
        private Controls.Tabs tabs;
        private MinecraftServerManager.Controls.Button addServerMenu;
        private System.Windows.Forms.Timer hideMenusTimer;
        private System.Windows.Forms.SplitContainer treeTabsContainer;
        private MinecraftServerManager.Controls.Button settingsMenu;
    }
}

