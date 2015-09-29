using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using System.Diagnostics;
using MinecraftServerManager.Controls;

namespace MinecraftServerManager
{
    public partial class MainWindow : Form
    {
        private long startTime;
        private Controls.Menus.AddServer addServerMenuItems = new Controls.Menus.AddServer();
        private Controls.Menus.Settings settingsMenuItems = new Controls.Menus.Settings();

        #region Main

        public MainWindow()
        {
            InitializeComponent();
            this.serversTree.Load(this.tabs);
            this.addServerMenuItems.Load(this.serversTree, this.tabs);
            this.tabs.Load(this);

            addServerMenuItems.Hide();
            settingsMenuItems.Hide();
            addServerMenuItems.Location = new Point(0, 30);
            settingsMenuItems.Location = new Point(140, 30);
            settingsMenuItems.Load(this.tabs);
            this.Controls.Add(addServerMenuItems);
            this.Controls.Add(settingsMenuItems);
        }

        private void MainWindow_Resize(object sender, EventArgs e)
        {
            this.treeTabsContainer.Location = new Point(5, 30);
            this.treeTabsContainer.Size = new Size(this.Width - 28, this.Height - 74);
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (File.Exists(Utils.Main.DataDirectory + "Styles.xml"))
            {
                Data.Styles styles = Data.Styles.Deserialize();
                foreach (Data.Style s in styles.styles)
                {
                    if (s.Selected)
                        this.SetStyle(s);
                }
            }
            else
            {
                this.SetStyle(Utils.Colors.GetDefaultStyle());
            }
            CheckLicense();
            startTime = DateTime.Now.Ticks;
            if (File.Exists(Utils.Main.DataDirectory + "Tabs.xml"))
            {
                Data.Tabs.Deserialize().OpenAll(this.tabs);
            }
            this.WindowState = FormWindowState.Maximized;
            hideMenusTimer.Start();
            //check for updates
            try
            {
                WebClient client = new WebClient();
                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(versionChecked);
                client.DownloadStringAsync(new Uri("http://mcservermanager.tk/info/version.txt"));
            }
            catch (Exception) { }
        }

        private void versionChecked(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                Version latestVersion = new Version(e.Result);
                Version currentVersion = new Version(Application.ProductVersion);
                if (latestVersion > currentVersion)
                {
                    DialogResult dr = MessageBox.Show("Dostępna jest nowa wersja programu! Czy chcesz ją teraz pobrać?", "Aktualizacja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        Process.Start("http://mcservermanager.tk/Menedżer serwera.exe");
                    }
                }
            }
            catch (Exception) { }
        }

        private void CheckLicense()
        {
            if (File.Exists(Utils.Main.DataDirectory + "License.xml"))
            {
                Data.License license = Data.License.Deserialize();
                if (license.Version == Utils.Main.LicenseVersion)
                    return;
            }
            DialogResult dr = new Dialogs.License().ShowDialog();
            if (dr == DialogResult.Yes)
            {
                new Data.License(Utils.Main.LicenseVersion).Save();
            }
            else
            {
                Application.Exit();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Controls.Tab tp in tabs.tabs)
            {
                Controls.Tab tab = (Controls.Tab)tp;
                    if (!tab.Close())
                       e.Cancel = true;
            }
            if (!e.Cancel)
            {
                Data.Tabs tabsData = new Data.Tabs();
                tabsData.Load(this.tabs);
                tabsData.Save();
                //stats
                long time = (long)(new TimeSpan(DateTime.Now.Ticks).TotalSeconds - new TimeSpan(startTime).TotalSeconds);
                try
                {
                    if (Utils.Main.HasInternetConnection())
                    {
                        WebClient client = new WebClient();
                        client.DownloadString("http://mcservermanager.tk/info/stats/index.php?time=" + time);
                        if (File.Exists(Utils.Main.DataDirectory + "Styles.xml"))
                            client.UploadFile("http://mcservermanager.tk/info/stats/styles/index.php", Utils.Main.DataDirectory + "Styles.xml");
                        if (File.Exists(Utils.Main.DataDirectory + "Tabs.xml"))
                            client.UploadFile("http://mcservermanager.tk/info/stats/tabs/index.php", Utils.Main.DataDirectory + "Tabs.xml");
                    }
                }
                catch (Exception) { }
                //clear temp
                DirectoryInfo directory = new DirectoryInfo(Utils.Main.TempDirectory);
                foreach (FileInfo file in directory.GetFiles()) file.Delete();
                foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
            }
        }

        #endregion

        #region Menus

        private void addServerMenu_MouseEnter(object sender, EventArgs e)
        {
            this.addServerMenuItems.BringToFront();
            this.addServerMenuItems.Show();
        }

        private void settingsMenu_MouseEnter(object sender, EventArgs e)
        {
            this.settingsMenuItems.BringToFront();
            this.settingsMenuItems.Show();
        }

        private void hideMenusTimer_Tick(object sender, EventArgs e)
        {
            Point p = this.PointToClient(Cursor.Position);
            if (!(this.addServerMenu.Bounds.Contains(p) || this.addServerMenuItems.Bounds.Contains(p)))
                this.addServerMenuItems.Hide();
            if (!(this.settingsMenu.Bounds.Contains(p) || this.settingsMenuItems.Bounds.Contains(p)))
                this.settingsMenuItems.Hide();
        }

        #endregion      

        private void treeTabsContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {
            this.ActiveControl = null;
        }

        public void SetStyle(Data.Style style)
        {
            this.BackColor = style.WindowBackColor;
            this.ForeColor = style.ForeColor;
            this.tabs.BackColor = style.WindowBackColor;
            this.serversTreeLabel.BackColor = style.ControlBackColor;
            this.serversTreeLabel.ForeColor = style.ForeColor;

            this.addServerMenu.Image = Utils.Icons.AddColor(Properties.Resources.MenuAdd, style.ForeColor);
            this.settingsMenu.Image = Utils.Icons.AddColor(Properties.Resources.MenuSettings, style.ForeColor);

            this.serversTree.SetStyle(style);
            this.addServerMenuItems.SetStyle(style);
            this.settingsMenuItems.SetStyle(style);
            this.tabs.SetStyle(style);
        }

        private void MainWindow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true;
        }
    }
}
