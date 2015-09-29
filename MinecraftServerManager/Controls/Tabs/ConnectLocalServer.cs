using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftServerManager.Controls
{
    public partial class ConnectLocalServer : UserControl, IStyleableTab
    {
        private Tabs tabs;

        public ConnectLocalServer()
        {
            InitializeComponent();
        }

        public new void Load(Tabs _tabs)
        {
            this.tabs = _tabs;
            tabs.AddTab("Podłącz serwer lokalny", this);
        }

        public void SetStyle(Data.Style style)
        {
            Utils.Colors.StyleButton(this.selectDirectoryButton, style);
            Utils.Colors.StyleButton(this.selectJarButton, style);
            Utils.Colors.StyleButton(this.readyButton, style);
            Utils.Colors.StyleTextBox(this.serverName, style);
        }

        private void selectDirectoryButton_Click(object sender, EventArgs e)
        {
            DialogResult result = selectDirectory.ShowDialog();
            if (result == DialogResult.OK)
                selectDirectoryButton.Text = "Wybrano folder: " + Path.GetFileName(selectDirectory.SelectedPath);
        }

        private void selectJarButton_Click(object sender, EventArgs e)
        {
            selectJar.InitialDirectory = selectDirectory.SelectedPath;
            DialogResult result = selectJar.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (Path.GetDirectoryName(selectJar.FileName) != selectDirectory.SelectedPath)
                    MessageBox.Show("Silnik serwera musi znajdować się w wybranym folderze!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    selectJarButton.Text = "Wybrano plik: " + Path.GetFileName(selectJar.FileName);
            }
        }

        private void next_Click(object sender, EventArgs e)
        {
            #region validation
            if (serverName.Text == "")
            {
                MessageBox.Show("Podaj nazwę serwera!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            if (selectDirectory.SelectedPath == "")
            {
                MessageBox.Show("Wybierz folder!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (selectJar.FileName == "")
            {
                MessageBox.Show("Wybierz plik .jar!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            #endregion

            Data.Server data = new Data.Server();
            data.engine = engineSelect.SelectedItem.ToString();
            data.name = serverName.Text;
            data.version = versionSelect.SelectedItem.ToString();
            data.path = selectDirectory.SelectedPath;
            data.jarPath = selectJar.FileName;
            data.isImported = true;

            if (File.Exists(data.GetFile()))
            {
                MessageBox.Show("Serwer o podanej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            data.Save();

            this.Enabled = false;

            tabs.mainWindow.serversTree.FullRefresh();
        }

        private void ConnectLocalServer_Resize(object sender, EventArgs e)
        {
            this.selectDirectoryButton.Size = new Size(this.Width - 200, 30);
            this.selectJarButton.Size = new Size(this.Width - 200, 30);
            this.readyButton.Size = new Size(this.Width, 30);
            this.serverName.Size = new Size(this.Width - 70, 27);
            this.versionSelect.Size = new Size(this.Width - 70, 27);
            this.engineSelect.Size = new Size(this.Width - 70, 27);
        }
    }
}
