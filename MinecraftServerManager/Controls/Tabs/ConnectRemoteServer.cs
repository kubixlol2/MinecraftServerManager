using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using MinecraftServerManager.Utils.MinecraftRcon;

namespace MinecraftServerManager.Controls
{
    public partial class ConnectRemoteServer : UserControl, IStyleableTab
    {
        private Tabs tabs;

        public ConnectRemoteServer()
        {
            InitializeComponent();
        }

        public new void Load(Tabs _tabs)
        {
            this.tabs = _tabs;
            tabs.AddTab("Podłącz serwer zdalny", this);
        }

        public void SetStyle(Data.Style style)
        {
            Utils.Colors.StyleButton(this.readyButton, style);
            Utils.Colors.StyleTextBox(this.serverName, style);
            Utils.Colors.StyleTextBox(this.serverIP, style);
            Utils.Colors.StyleTextBox(this.ftpPassword, style);
            Utils.Colors.StyleTextBox(this.ftpPort, style);
            Utils.Colors.StyleTextBox(this.ftpUser, style);
            Utils.Colors.StyleTextBox(this.rconPassword, style);
            Utils.Colors.StyleTextBox(this.rconPort, style);
        }

        private void readyButton_Click(object sender, EventArgs e)
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
            if (serverIP.Text == "")
            {
                MessageBox.Show("Podaj adres IP serwera!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ftpUser.Text == "" || ftpPassword.Text == "" || ftpPort.Text == "")
            {
                MessageBox.Show("Podaj dane FTP!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (rconPassword.Text == "" || rconPort.Text == "")
            {
                MessageBox.Show("Podaj dane RCON!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int ftpPortNumber, rconPortNumber;
            if (!int.TryParse(ftpPort.Text, out ftpPortNumber))
            {
                MessageBox.Show("Numer portu FTP musi być liczbą!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (ftpPortNumber < 0 || ftpPortNumber > 65535)
            {
                MessageBox.Show("Numer portu FTP jest poza zakresem!");
            }

            if (!int.TryParse(rconPort.Text, out rconPortNumber))
            {
                MessageBox.Show("Numer portu RCON musi być liczbą!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (rconPortNumber < 0 || rconPortNumber > 65535)
            {
                MessageBox.Show("Numer portu RCON jest poza zakresem!");
            }
            #endregion

            string ip = serverIP.Text;
            int port = ftpPortNumber;
            string user = ftpUser.Text;
            string password = ftpPassword.Text;

            Data.RemoteServer ftpData = new Data.RemoteServer();
            Data.RemoteServerRcon rconData = new Data.RemoteServerRcon();

            try
            {
                ftpData.adress = "ftp://" + ip + ":" + port + "/";
                ftpData.login = user;
                ftpData.password = password;
                ftpData.name = serverName.Text;
                ftpData.engine = engineSelect.SelectedItem.ToString();
                ftpData.version = versionSelect.SelectedItem.ToString();

                Utils.Ftp.directoryListSimple(ftpData, "");

                ftpData.Save();
            }
            catch (WebException)
            {
                MessageBox.Show("Nie udało się połączyć z serwerem FTP! Czy na pewno wpisałeś poprawne dane?", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RconClient rcon = RconClient.INSTANCE;
            rcon.setupStream(serverIP.Text, rconPortNumber, rconPassword.Text);
            if (rcon.isInit)
            {
                rconData.name = serverName.Text;
                rconData.adress = serverIP.Text;
                rconData.password = rconPassword.Text;
                rconData.port = rconPortNumber;
                rconData.Save();
            }
            else
            {
                MessageBox.Show("Nie udało się połączyć z konsolą serwera przez RCON! Czy na pewno wpisałeś poprawne dane?\nPamiętaj o wstawieniu w pliku server.properties następujących linii:\nenable-rcon=true\nrcon.port=Twój port RCON\nrcon.password=Twoje hasło RCON", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            rconData.Save();
            ftpData.Save();

            this.Enabled = false;

            tabs.mainWindow.serversTree.FullRefresh();
        }

        private void ConnectRemoteServer_Resize(object sender, EventArgs e)
        {
            this.serverIP.Size = new Size(this.Width - 91, 27);
            this.serverName.Size = new Size(this.Width - 91, 27);
            this.versionSelect.Size = new Size(this.Width - 91, 27);
            this.engineSelect.Size = new Size(this.Width - 91, 27);
            this.ftpPort.Size = new Size(this.Width - 91, 27);
            this.ftpPassword.Size = new Size(this.Width - 91, 27);
            this.ftpUser.Size = new Size(this.Width - 91, 27);
            this.rconPassword.Size = new Size(this.Width - 91, 27);
            this.rconPort.Size = new Size(this.Width - 91, 27);
            this.readyButton.Size = new Size(this.Width, 30);
        }
    }
}
