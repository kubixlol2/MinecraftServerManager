using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.FileIO;

namespace MinecraftServerManager.Controls
{
    public partial class NewLocalServer : UserControl, IStyleableTab
    {
        private Data.Server ServerData;
        private string ServerDirectory;
        private string EngineFile;
        private Data.Style style;
        private Tabs tabs;
        private WebClient Client; 

        public NewLocalServer()
        {
            InitializeComponent();
            this.NewLocalServer_Resize(null, null);
        }

        public new void Load(Tabs _tabs)
        {
            this.tabs = _tabs;
            tabs.AddTab("Nowy serwer lokalny", this);
        }

        public bool Close()
        {
            if (Client == null || Client.IsBusy == false)
                return true;
            DialogResult dr = MessageBox.Show("Trwa pobieranie silnika serwera. Czy chcesz je anulować?", "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (dr == DialogResult.Yes)
            {
                Client.CancelAsync();
                return true;
            }
            return false;
        }

        private void NewLocalServer_Resize(object sender, EventArgs e)
        {
            this.serverName.Size = new Size(this.Width - 70, 27);
            this.engineSelect.Size = new Size(this.Width - 70, 27);
            this.versionSelect.Size = new Size(this.Width - 70, 27);
            this.readyButton.Width = this.Width;
        }

        public void SetStyle(Data.Style _style)
        {
            style = _style;
            this.readyButton.SetStyle(style);
            Utils.Colors.StyleComboBox(this.engineSelect, style);
            Utils.Colors.StyleComboBox(this.versionSelect, style);
            Utils.Colors.StyleTextBox(this.serverName, style);
        }

        private void readyButton_Click(object sender, EventArgs e)
        {
            if (engineSelect.SelectedItem == null)
            {
                MessageBox.Show("Wybierz silnik!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.versionSelect.SelectedItem == null)
            {
                MessageBox.Show("Wybierz wersję!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!eulaCheckBox.Checked)
            {
                MessageBox.Show("Musisz zaakceptować postanowienia Mojang EULA!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (serverName.Text == "")
            {
                MessageBox.Show("Podaj nazwę serwera!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string engine = engineSelect.SelectedItem.ToString(), version = this.versionSelect.SelectedItem.ToString();
            ServerData = new Data.Server();
            ServerData.name = serverName.Text;
            ServerData.version = version;
            ServerData.engine = engine;
            ServerDirectory = ServerData.GetDirectory();
            if (Directory.Exists(ServerDirectory))
            {
                MessageBox.Show("Serwer o takiej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Directory.CreateDirectory(ServerDirectory);
            string engineDirectory = Utils.Main.EnginesDirectory + engine + Path.DirectorySeparatorChar;
            Directory.CreateDirectory(engineDirectory);
            EngineFile = engineDirectory + version + ".jar";
            if (File.Exists(EngineFile))
            {
                DownloadProgressCompleted(null, new AsyncCompletedEventArgs(null, false, null));
            }
            else
            {
                string url = "http://mcservermanager.tk/info/servers/" + engine.ToLower() + "-" + Utils.Main.EngineVersions + ".jar";
                if (engine == "Vanilla")
                    url = "https://s3.amazonaws.com/Minecraft.Download/versions/" + version + "/minecraft_server." + version + ".jar";
                Client = new WebClient();
                Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChanged);
                Client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadProgressCompleted);
                Client.DownloadFileAsync(new Uri(url), EngineFile);
            }

            this.Enabled = false;
        }

        private void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes;
            string megaBytesReceived = (e.BytesReceived / (1024.0 * 1024.0)).ToString("0.00") + "MB";
            string megaBytesTotal = (e.TotalBytesToReceive / (1024.0 * 1024.0)).ToString("0.00") + "MB";
            readyButton.Text = "Pobieranie silnika serwera: " + megaBytesReceived + " / " + megaBytesTotal;
            this.readyButton.Progress = (float)percentage;
        }

        private void DownloadProgressCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled && File.Exists(ServerDirectory + "server.jar"))
            {
                Directory.Delete(ServerDirectory, true);
                return;
            }
            if (e.Error != null)
            {
                MessageBox.Show("Wystąpił błąd podczas pobierania silnika serwera!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Directory.Delete(ServerDirectory, true);
                File.Delete(EngineFile);
                return;
            }
            ServerData.Save();
            StreamWriter file = File.CreateText(ServerDirectory + "eula.txt");
            file.Write("eula=true");
            file.Close();
            this.readyButton.Text = "Pobieranie zakończone";
            new Computer().FileSystem.CopyFile(EngineFile, ServerData.GetDirectory() + "server.jar", UIOption.AllDialogs);
            this.tabs.mainWindow.serversTree.FullRefresh();
        }
    }
}
