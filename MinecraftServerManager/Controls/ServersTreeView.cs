using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections;
using System.Drawing;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.FileIO;
using MinecraftServerManager.Controls.ServersTreeNodes;

namespace MinecraftServerManager.Controls
{
    public class ServersTreeView : TreeView
    {
        private ImageList _imageList = new ImageList();
        private List<Image> _imageListCopy = new List<Image>();
        private Hashtable _systemIcons = new Hashtable();
        private Tabs tabs;
        private ContextMenuStrip serverContextMenu, directoryContextMenu, fileContextMenu,
            remoteServerContextMenu, remoteDirectoryContextMenu, remoteFileContextMenu,
            openContextMenu;

        public static readonly int FolderOpenIcon = 0, FolderCloseIcon = 1, LocalServerIcon = 2, RemoteServerIcon = 3, ConsoleIcon = 4, PropertiesIcon = 5;


        #region Main

        public ServersTreeView()
        {
            this.ImageList = _imageList;
            this.MouseDown += new MouseEventHandler(FileSystemTreeView_MouseDown);
            this.BeforeExpand += new TreeViewCancelEventHandler(FileSystemTreeView_BeforeExpand);
            this.BeforeCollapse += new TreeViewCancelEventHandler(FileSystemTreeView_BeforeCollapse);
            this.DoubleClick += new EventHandler(openMenu_Click);

            this.InitializeComponent();
        }

        public void Load(Tabs _tabs)
        {
            this.tabs = _tabs;
            FullRefresh();
        }

        public void FullRefresh()
        {
            _systemIcons.Clear();
            _imageList.Images.Clear();
            Nodes.Clear();

            _imageList.Images.Add(Properties.Resources.FolderOpenIcon);
            _systemIcons.Add(ServersTreeView.FolderOpenIcon, 0);
            _imageList.Images.Add(Properties.Resources.FolderCloseIcon);
            _systemIcons.Add(ServersTreeView.FolderCloseIcon, 0);
            _imageList.Images.Add(Properties.Resources.LocalIcon);
            _systemIcons.Add(ServersTreeView.LocalServerIcon, 0);
            _imageList.Images.Add(Properties.Resources.RemoteIcon);
            _systemIcons.Add(ServersTreeView.RemoteServerIcon, 0);
            _imageList.Images.Add(Properties.Resources.ConsoleIcon);
            _systemIcons.Add(ServersTreeView.ConsoleIcon, 0);
            _imageList.Images.Add(Properties.Resources.MenuSettings);
            _systemIcons.Add(ServersTreeView.PropertiesIcon, 0);

            foreach (string server in Directory.GetDirectories(Utils.Main.ServersDirectory))
            {
                if (File.Exists(server + Path.DirectorySeparatorChar + "ServerCreatorData.xml")) //old file name
                {
                    File.Move(server + Path.DirectorySeparatorChar + "ServerCreatorData.xml", server + Path.DirectorySeparatorChar + "ServerManagerData.xml");
                }
                if (File.Exists(server + Path.DirectorySeparatorChar + "ServerManagerData.xml"))
                {
                    Data.Server serverData = Data.Server.Deserialize(server + Path.DirectorySeparatorChar + "ServerManagerData.xml");

                    ServerNode node = new ServerNode(this, new DirectoryInfo(server), serverData);

                    node.Expand();
                }
            }
            foreach (string importedServer in Directory.GetFiles(Utils.Main.ImportDirectory))
            {
                Data.Server serverData = Data.Server.Deserialize(importedServer);

                ServerNode node = new ServerNode(this, new DirectoryInfo(serverData.path), serverData);
                node.Expand();
            }

            foreach (string remoteServer in Directory.GetDirectories(Utils.Main.RemoteDirectory))
            {
                if (File.Exists(remoteServer + Path.DirectorySeparatorChar + "MainData.xml"))
                {
                    Data.RemoteServer serverData = Data.RemoteServer.Deserialize(remoteServer + Path.DirectorySeparatorChar + "MainData.xml");

                    RemoteServerNode node = new RemoteServerNode(this, serverData);
                }
            }
        }

        public Tabs GetTabs()
        {
            return tabs;
        }

        private void FileSystemTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNode node = this.GetNodeAt(e.X, e.Y);
            this.SelectedNode = node;

            if (e.Button == MouseButtons.Right)
            {
                if (this.SelectedNode is ServerNode)
                {
                    serverContextMenu.Show(this, e.X, e.Y);
                }
                else if (this.SelectedNode is DirectoryNode)
                {
                    directoryContextMenu.Show(this, e.X, e.Y);
                }
                else if (this.SelectedNode is FileNode)
                {
                    fileContextMenu.Show(this, e.X, e.Y);
                }
                else if (this.SelectedNode is RemoteServerNode)
                {
                    remoteServerContextMenu.Show(this, e.X, e.Y);
                }
                else if (this.SelectedNode is RemoteDirectoryNode)
                {
                    remoteDirectoryContextMenu.Show(this, e.X, e.Y);
                }
                else if (this.SelectedNode is RemoteFileNode)
                {
                    remoteFileContextMenu.Show(this, e.X, e.Y);
                }
                else if (this.SelectedNode is ConsoleNode || this.SelectedNode is RemoteConsoleNode)
                {
                    openContextMenu.Show(this, e.X, e.Y);
                } else if (this.SelectedNode is PropertiesNode)
                {
                    openContextMenu.Show(this, e.X, e.Y);
                }
            }
        }

        private void FileSystemTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is ServerNode)
            {
                ((DirectoryNode)e.Node).Refresh();
            }
            else if (e.Node is RemoteServerNode)
            {
                ((RemoteDirectoryNode)e.Node).Refresh();
            }
            else if (e.Node is RemoteDirectoryNode)
            {
                ((RemoteDirectoryNode)e.Node).Refresh();
                e.Node.ImageIndex = FolderOpenIcon;
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
            else if (e.Node is DirectoryNode)
            {
                ((DirectoryNode)e.Node).Refresh();
                e.Node.ImageIndex = FolderOpenIcon;
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        private void FileSystemTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is ServerNode)
            { 
            }
            else if (e.Node is RemoteServerNode)
            {
            }
            else if (e.Node is RemoteDirectoryNode)
            {
                e.Node.ImageIndex = FolderCloseIcon;
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
            else if (e.Node is DirectoryNode)
            {
                e.Node.ImageIndex = FolderCloseIcon;
                e.Node.SelectedImageIndex = e.Node.ImageIndex;
            }
        }

        public int GetLocalFileIconImageIndex(string path)
        {
            string extension = Path.GetExtension(path);

            if (_systemIcons.ContainsKey(extension) == false)
            {
                if (Utils.Main.IsLinux)
                    _imageList.Images.Add(Icon.ExtractAssociatedIcon(path));
                else
                    _imageList.Images.Add(Utils.Icons.GetSmallFileIcon(path));
                _systemIcons.Add(extension, _imageList.Images.Count - 1);
            }

            return (int)_systemIcons[Path.GetExtension(path)];
        }

        public int GetRemoteFileIconImageIndex(string path)
        {
            string extension = Path.GetExtension(path);

            if (_systemIcons.ContainsKey(extension) == false)
            {
                path = Utils.Main.TempDirectory + "get_icon" + extension;
                File.Create(path).Close();
                _imageList.Images.Add(Utils.Icons.GetSmallFileIcon(path));
                _systemIcons.Add(extension, _imageList.Images.Count - 1);
            }

            return (int)_systemIcons[Path.GetExtension(path)];
        }

        private void InitializeComponent()
        {
            this.serverContextMenu = new ContextMenuStrip();
            this.directoryContextMenu = new ContextMenuStrip();
            this.remoteServerContextMenu = new ContextMenuStrip();
            this.remoteDirectoryContextMenu = new ContextMenuStrip();
            this.remoteFileContextMenu = new ContextMenuStrip();
            this.fileContextMenu = new ContextMenuStrip();
            this.openContextMenu = new ContextMenuStrip();
            this.SuspendLayout();

            this.serverContextMenu.Items.Add(CreateRenameMenuItem());
            this.serverContextMenu.Items.Add(CreateRemoveMenuItem());
            this.serverContextMenu.Items.Add(CreateCopyClipboardMenuItem());
            this.serverContextMenu.Items.Add(CreatePasteClipboardMenuItem());
            this.serverContextMenu.Items.Add(CreateNewFileMenuItem());
            this.serverContextMenu.Items.Add(CreateNewDirectoryMenuItem());
            this.serverContextMenu.Items.Add(CreateCopyFileMenuItem());
            this.serverContextMenu.Items.Add(CreateCopyDirectoryMenuItem());
            this.serverContextMenu.Items.Add(CreateMoveFileMenuItem());
            this.serverContextMenu.Items.Add(CreateMoveDirectoryMenuItem());
            this.serverContextMenu.Items.Add(CreateExploreMenuItem());

            this.directoryContextMenu.Items.Add(CreateRenameMenuItem());
            this.directoryContextMenu.Items.Add(CreateRemoveMenuItem());
            this.directoryContextMenu.Items.Add(CreateCopyClipboardMenuItem());
            this.directoryContextMenu.Items.Add(CreatePasteClipboardMenuItem());
            this.directoryContextMenu.Items.Add(CreateNewFileMenuItem());
            this.directoryContextMenu.Items.Add(CreateNewDirectoryMenuItem());
            this.directoryContextMenu.Items.Add(CreateCopyFileMenuItem());
            this.directoryContextMenu.Items.Add(CreateCopyDirectoryMenuItem());
            this.directoryContextMenu.Items.Add(CreateMoveFileMenuItem());
            this.directoryContextMenu.Items.Add(CreateMoveDirectoryMenuItem());
            this.directoryContextMenu.Items.Add(CreateExploreMenuItem());

            this.remoteServerContextMenu.Items.Add(CreateRenameMenuItem());
            this.remoteServerContextMenu.Items.Add(CreateRemoveMenuItem());
            this.remoteServerContextMenu.Items.Add(CreateNewFileMenuItem());
            this.remoteServerContextMenu.Items.Add(CreateNewDirectoryMenuItem());
            this.remoteServerContextMenu.Items.Add(CreateCopyFileMenuItem());
            this.remoteServerContextMenu.Items.Add(CreateCopyDirectoryMenuItem());

            this.remoteDirectoryContextMenu.Items.Add(CreateRenameMenuItem());
            this.remoteDirectoryContextMenu.Items.Add(CreateRemoveMenuItem());
            this.remoteDirectoryContextMenu.Items.Add(CreateNewFileMenuItem());
            this.remoteDirectoryContextMenu.Items.Add(CreateNewDirectoryMenuItem());
            this.remoteDirectoryContextMenu.Items.Add(CreateCopyFileMenuItem());
            this.remoteDirectoryContextMenu.Items.Add(CreateCopyDirectoryMenuItem());

            this.remoteFileContextMenu.Items.Add(CreateOpenMenuItem());
            this.remoteFileContextMenu.Items.Add(CreateRenameMenuItem());
            this.remoteFileContextMenu.Items.Add(CreateRemoveMenuItem());

            this.openContextMenu.Items.Add(CreateOpenMenuItem());

            this.fileContextMenu.Items.Add(CreateOpenMenuItem());
            this.fileContextMenu.Items.Add(CreateRenameMenuItem());
            this.fileContextMenu.Items.Add(CreateRemoveMenuItem());
            this.fileContextMenu.Items.Add(CreateCopyClipboardMenuItem());

            this.ResumeLayout(false);
        }

        public void SetStyle(Data.Style style)
        {
            this.BackColor = style.ControlBackColor;
            this.ForeColor = style.ForeColor;

            _imageList.Images[0] = Utils.Icons.AddColor(Properties.Resources.FolderOpenIcon, this.ForeColor);
            _imageList.Images[1] = Utils.Icons.AddColor(Properties.Resources.FolderCloseIcon, this.ForeColor);
            _imageList.Images[2] = Utils.Icons.AddColor(Properties.Resources.LocalIcon, this.ForeColor);
            _imageList.Images[3] = Utils.Icons.AddColor(Properties.Resources.RemoteIcon, this.ForeColor);
            _imageList.Images[4] = Utils.Icons.AddColor(Properties.Resources.ConsoleIcon, this.ForeColor);
        }

        #endregion

        #region MenuEvents

        private void uploadDirectory(Data.RemoteServer data, string localDirectory, string remoteDirectory)
        {
            foreach (string directory in Directory.GetDirectories(localDirectory))
            {
                string dirname = remoteDirectory + "/" + Path.GetFileName(directory) + "/";
                Utils.Ftp.createDirectory(data, dirname);
                uploadDirectory(data, directory, dirname);
            }
            foreach (string file in Directory.GetFiles(localDirectory))
            {
                new Dialogs.FtpUploader().Upload(data, file, remoteDirectory + "/" + Path.GetFileName(file));
            }
        }

        private void newFileMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is DirectoryNode)
            {
                DirectoryNode i = (DirectoryNode)base.SelectedNode;
                string newName = Dialogs.TextInput.ShowDialog("Nowy plik", "Podaj nazwę nowego pliku:", "", "Utwórz plik", "Anuluj");
                if (newName != "")
                {
                    string path = i.GetDirectory().FullName + Path.DirectorySeparatorChar + newName;
                    if (System.IO.File.Exists(path))
                    {
                        MessageBox.Show("Plik o podanej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try
                    {
                        File.Create(path).Close();
                        TextEditor te = new TextEditor();
                        te.Load(new FileInfo(path), this.tabs);
                        new FakeChildNode(i);
                    }
                    catch (System.ArgumentException)
                    {
                        MessageBox.Show("Podana nazwa pliku jest nieprawidłowa!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (base.SelectedNode is RemoteDirectoryNode)
            {
                RemoteDirectoryNode i = (RemoteDirectoryNode)base.SelectedNode;
                string newName = Dialogs.TextInput.ShowDialog("Nowy plik", "Podaj nazwę nowego pliku:", "", "Utwórz plik", "Anuluj");
                if (newName != "")
                {
                    File.Create(Utils.Main.TempDirectory + newName).Close();
                    Utils.Ftp.upload(i.data, i.directory + newName, Utils.Main.TempDirectory + newName);
                    i.Refresh();
                }
            }
        }

        private void newDirectoryMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is DirectoryNode)
            {
                DirectoryNode i = (DirectoryNode)base.SelectedNode;
                string newName = Dialogs.TextInput.ShowDialog("Nowy folder", "Podaj nazwę nowego folderu:", "", "Utwórz folder", "Anuluj");
                if (newName != "")
                {
                    string path = i.GetDirectory().FullName + Path.DirectorySeparatorChar + newName;
                    if (Directory.Exists(path))
                    {
                        MessageBox.Show("Folder o podanej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try
                    {
                        Directory.CreateDirectory(path);
                        new FakeChildNode(i);
                    }
                    catch (System.ArgumentException)
                    {
                        MessageBox.Show("Podana nazwa folderu jest nieprawidłowa!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (base.SelectedNode is RemoteDirectoryNode)
            {
                RemoteDirectoryNode node = (RemoteDirectoryNode)base.SelectedNode;
                string newName = Dialogs.TextInput.ShowDialog("Nowy folder", "Podaj nazwę nowego folderu:", "", "Utwórz folder", "Anuluj");
                if (newName != "")
                {
                    Utils.Ftp.createDirectory(node.data, node.directory + newName);
                    node.Refresh();
                }
            }
        }

        private void removeMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is FileNode)
            {
                FileNode node = (FileNode)base.SelectedNode;
                FileInfo file = node.GetFile();
                DialogResult result = MessageBox.Show(
                    "Czy na pewno chcesz usunąć plik \"" + file.Name +
                    "\"?\n Tej operacji nie można będzie cofnąć!",
                    "Uwaga!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    file.Delete();
                    node.Remove();
                }
            }
            else if (base.SelectedNode is DirectoryNode)
            {
                DirectoryNode node = (DirectoryNode)base.SelectedNode;
                DirectoryInfo directory = node.GetDirectory();
                string message;
                if (base.SelectedNode is ServerNode)
                    message = "Czy na pewno chcesz usunąć serwer \"" + directory.Name + "\"?\n Tej operacji nie można będzie cofnąć!";
                else
                    message = "Czy na pewno chcesz usunąć folder \"" + directory.Name + "\"?\n Tej operacji nie można będzie cofnąć!";
                DialogResult result = MessageBox.Show(
                message, "Uwaga!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (directory.Exists)
                            new Computer().FileSystem.DeleteDirectory(directory.FullName, UIOption.AllDialogs, RecycleOption.SendToRecycleBin);
                        if (base.SelectedNode is ServerNode && ((ServerNode)base.SelectedNode).GetServerData().isImported)
                            File.Delete(((ServerNode)base.SelectedNode).GetServerData().GetFile());
                        node.Destroy();
                    }
                    catch (OperationCanceledException) { }
                }
            }
            else if (base.SelectedNode is RemoteServerNode)
            {
                RemoteServerNode node = (RemoteServerNode)base.SelectedNode;
                DialogResult result = MessageBox.Show(
                "Czy na pewno chcesz usunąć serwer zdalny \"" + node.GetServerData().name + "\" z listy swoich serwerów?", "Uwaga!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    Directory.Delete(Utils.Main.RemoteDirectory + node.GetServerData().name, true);
                    node.Destroy();
                }
            }
            else if (base.SelectedNode is RemoteDirectoryNode)
            {
                RemoteDirectoryNode node = (RemoteDirectoryNode)base.SelectedNode;
                DialogResult result = MessageBox.Show(
                "Czy na pewno chcesz usunąć folder \"" + node.Text + "\"?\n Tej operacji nie będzie można cofnąć!", "Uwaga!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                Utils.Ftp.deleteDirectory(node.data, node.directory);
                node.Destroy();
            }
            else if (base.SelectedNode is RemoteFileNode)
            {
                RemoteFileNode node = (RemoteFileNode)base.SelectedNode;
                DialogResult result = MessageBox.Show(
                "Czy na pewno chcesz usunąć plik \"" + node.Text + "\"?\n Tej operacji nie będzie można cofnąć!", "Uwaga!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                Utils.Ftp.deleteFile(node.data, node.GetFile());
                node.Remove();
            }
        }

        private void exploreMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode != null)
            {
                if (base.SelectedNode is DirectoryNode)
                {
                    DirectoryNode node = (DirectoryNode)base.SelectedNode;
                    Process.Start("explorer.exe", node.GetDirectory().FullName);
                }
            }
        }

        private void renameMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is FileNode)
            {
                FileNode node = (FileNode)base.SelectedNode;
                FileInfo file = node.GetFile();
                string newName = Dialogs.TextInput.ShowDialog("Zmiana nazwy", "Podaj nową nazwę pliku:", file.Name, "Zmień nazwę", "Anuluj");
                if (newName != "" && (file.DirectoryName + Path.DirectorySeparatorChar + newName) != file.FullName)
                {
                    try
                    {
                        file.MoveTo(file.DirectoryName + Path.DirectorySeparatorChar + newName);
                    }
                    catch (NotSupportedException)
                    {
                        MessageBox.Show("Podana nazwa pliku jest nieprawidłowa!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Plik o podanej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    ((DirectoryNode)node.Parent).Refresh();
                }
            }
            else if (base.SelectedNode is ServerNode)
            {
                ServerNode node = (ServerNode)base.SelectedNode;
                DirectoryInfo directory = node.GetDirectory();
                string newName = Dialogs.TextInput.ShowDialog("Zmiana nazwy", "Podaj nową nazwę serwera:", directory.Name, "Zmień nazwę", "Anuluj");
                if (newName != "" && (directory.Parent.FullName + Path.DirectorySeparatorChar + newName + Path.DirectorySeparatorChar) != directory.FullName)
                {
                    try
                    {
                        directory.MoveTo(directory.Parent.FullName + Path.DirectorySeparatorChar + newName);
                        node.GetServerData().name = newName;
                        node.GetServerData().Save();
                        FullRefresh();
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("Podana nazwa serwera jest nieprawidłowa!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Serwer o podanej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (base.SelectedNode is DirectoryNode)
            {
                DirectoryNode node = (DirectoryNode)base.SelectedNode;
                DirectoryInfo directory = node.GetDirectory();
                string newName = Dialogs.TextInput.ShowDialog("Zmiana nazwy", "Podaj nową nazwę folderu:", directory.Name, "Zmień nazwę", "Anuluj");
                if (newName != "" && (directory.Parent.FullName + Path.DirectorySeparatorChar + newName) != directory.FullName)
                {
                    try
                    {
                        directory.MoveTo(directory.Parent.FullName + Path.DirectorySeparatorChar + newName);
                        ((DirectoryNode)node.Parent).Refresh();
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show("Podana nazwa folderu jest nieprawidłowa!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("Folder o podanej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else if (base.SelectedNode is RemoteServerNode)
            {
                RemoteServerNode node = (RemoteServerNode)base.SelectedNode;
                string newName = Dialogs.TextInput.ShowDialog("Zmiana nazwy", "Podaj nową nazwę serwera:", node.GetServerData().name, "Zmień nazwę", "Anuluj");
                Data.RemoteServer data = new Data.RemoteServer();
                data.name = newName;
                if (!Directory.Exists(data.GetDirectory()))
                {
                    Directory.Move(node.GetServerData().GetDirectory(), data.GetDirectory());
                    node.GetServerData().name = newName;
                    node.GetServerData().Save();
                    FullRefresh();
                }
            }
            else if (base.SelectedNode is RemoteDirectoryNode)
            {
                RemoteDirectoryNode node = (RemoteDirectoryNode)base.SelectedNode;
                string directory = Path.GetFileName(Utils.Strings.CutLastChars(node.directory, 1));
                string newName = Dialogs.TextInput.ShowDialog("Zmiana nazwy", "Podaj nową nazwę folderu:", directory, "Zmień nazwę", "Anuluj");
                if (newName != "" && newName != directory)
                {
                    Utils.Ftp.rename(node.data, node.directory, newName);
                    ((RemoteDirectoryNode)node.Parent).Refresh();
                }
            }
            else if (base.SelectedNode is RemoteFileNode)
            {
                RemoteFileNode node = (RemoteFileNode)base.SelectedNode;
                string file = Path.GetFileName(node.GetFile());
                string newName = Dialogs.TextInput.ShowDialog("Zmiana nazwy", "Podaj nową nazwę pliku:", file, "Zmień nazwę", "Anuluj");
                if (newName != "" && newName !=  file)
                {
                    Utils.Ftp.rename(node.data, node.GetFile(), newName);
                    ((RemoteDirectoryNode)node.Parent).Refresh();
                }
            }
        }

        private void openMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is FileNode)
            {
                FileNode i = (FileNode)base.SelectedNode;
                TextEditor te = new TextEditor();
                te.Load(i.GetFile(), this.tabs);
            }
            else if (base.SelectedNode is RemoteFileNode)
            {
                RemoteFileNode i = (RemoteFileNode)base.SelectedNode;
                TextEditor te = new TextEditor();
                te.Load(i, this.tabs);
            }
            else if (base.SelectedNode is ConsoleNode)
            {
                ConsoleNode node = (ConsoleNode)base.SelectedNode;
                Console console = new Console();
                console.Load(node.Parent.GetServerData(), this.tabs);
            }
            else if (base.SelectedNode is RemoteConsoleNode)
            {
                RemoteConsoleNode node = (RemoteConsoleNode)base.SelectedNode;
                RemoteConsole console = new RemoteConsole();
                console.Load(node.Parent.data, node.Parent.GetServerData().name, this.tabs);
            } else if (base.SelectedNode is PropertiesNode)
            {
                PropertiesNode node = (PropertiesNode)base.SelectedNode;
                PropertiesEditor editor = new PropertiesEditor();
                editor.Load(this.tabs);
            }
        }

        private void moveFileMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is DirectoryNode)
            {
                DirectoryNode node = (DirectoryNode)base.SelectedNode;
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Multiselect = true;
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    string[] fileNames = openFile.FileNames;
                    for (int j = 0; j < fileNames.Length; j++)
                    {
                        string filename = fileNames[j];
                        string newName = node.GetDirectory().FullName + Path.DirectorySeparatorChar + Path.GetFileName(filename);
                        if (File.Exists(newName))
                        {
                            MessageBox.Show("Plik o podanej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        File.Move(filename, newName);
                        node.Refresh();
                    }
                }
            }
        }

        private void copyFileMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is DirectoryNode)
            {
                DirectoryNode node = (DirectoryNode)base.SelectedNode;
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Multiselect = true;
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    string[] fileNames = openFile.FileNames;
                    for (int j = 0; j < fileNames.Length; j++)
                    {
                        string filename = fileNames[j];
                        string newName = node.GetDirectory().FullName + Path.DirectorySeparatorChar + Path.GetFileName(filename);
                        if (File.Exists(newName))
                        {
                            MessageBox.Show("Plik o podanej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        try
                        {
                            new Computer().FileSystem.CopyFile(filename, newName, UIOption.AllDialogs);
                        }
                        catch (OperationCanceledException) { }
                        node.Refresh();
                    }
                }
            }
            else if (base.SelectedNode is RemoteDirectoryNode)
            {
                RemoteDirectoryNode node = (RemoteDirectoryNode)base.SelectedNode;
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Multiselect = true;
                if (openFile.ShowDialog() == DialogResult.OK)
                {
                    string[] fileNames = openFile.FileNames;
                    for (int j = 0; j < fileNames.Length; j++)
                    {
                        string filename = fileNames[j];
                        string newName = node.directory + Path.GetFileName(filename);
                        new Dialogs.FtpUploader().Upload(node.data, filename, newName);
                        node.Refresh();
                    }
                }
            }
        }

        private void copyClipboardMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is DirectoryNode)
            {
                DirectoryNode node = (DirectoryNode)base.SelectedNode;
                StringCollection path = new StringCollection();
                path.Add(node.GetDirectory().FullName);
                Clipboard.SetFileDropList(path);
            }
            else if (base.SelectedNode is FileNode)
            {
                FileNode node = (FileNode)base.SelectedNode;
                StringCollection path = new StringCollection();
                path.Add(node.GetFile().FullName);
                Clipboard.SetFileDropList(path);
            }
        }

        private void pasteClipboardMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is DirectoryNode)
            {
                DirectoryNode node = (DirectoryNode)base.SelectedNode;
                StringCollection paths = Clipboard.GetFileDropList();
                foreach (string path in paths)
                {
                    if (Directory.Exists(path))
                    {
                        string newName = node.GetDirectory().FullName + Path.DirectorySeparatorChar + Path.GetFileName(path);
                        try
                        {
                            new Computer().FileSystem.CopyDirectory(path, newName, UIOption.AllDialogs);
                        }
                        catch (InvalidOperationException)
                        {
                            MessageBox.Show("Nie można ukończyć operacji z powodu błędu!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (OperationCanceledException) { }
                    }
                    if (File.Exists(path))
                    {
                        string newName = node.GetDirectory().FullName + Path.DirectorySeparatorChar + Path.GetFileName(path);
                        try
                        {
                            new Computer().FileSystem.CopyFile(path, newName, UIOption.AllDialogs);
                        }
                        catch (OperationCanceledException) { }
                    }
                }
            }
        }

        private void copyDirectoryMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is DirectoryNode)
            {
                DirectoryNode node = (DirectoryNode)base.SelectedNode;
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string newName = node.GetDirectory().FullName + Path.DirectorySeparatorChar + Path.GetFileName(fbd.SelectedPath);
                    if (Directory.Exists(newName))
                    {
                        MessageBox.Show("Folder o podanej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    try
                    {
                        new Computer().FileSystem.CopyDirectory(fbd.SelectedPath, newName, UIOption.AllDialogs);
                    }
                    catch (OperationCanceledException) { }
                }
                node.Refresh();
            } 
            else if (base.SelectedNode is RemoteDirectoryNode) 
            {
                RemoteDirectoryNode node = (RemoteDirectoryNode)base.SelectedNode;
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string remoteName = node.directory + Path.GetFileName(fbd.SelectedPath);
                    Utils.Ftp.createDirectory(node.data, remoteName);
                    uploadDirectory(node.data, fbd.SelectedPath, remoteName);
                }
                node.Refresh();
            }
        }

        private void moveDirectoryMenu_Click(object sender, System.EventArgs e)
        {
            if (base.SelectedNode is DirectoryNode)
            {
                DirectoryNode node = (DirectoryNode)base.SelectedNode;
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string newName = node.GetDirectory().FullName + Path.DirectorySeparatorChar + Path.GetFileName(fbd.SelectedPath);
                    if (Directory.Exists(newName))
                    {
                        MessageBox.Show("Folder o podanej nazwie już istnieje!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    Directory.Move(fbd.SelectedPath, newName);
                }
                node.Refresh();

            }
        }

        #endregion

        #region MenuFactory
        private ToolStripMenuItem CreateRemoveMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Usuń";
            menuItem.Click += new EventHandler(removeMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreateExploreMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Otwórz w eksploratorze plików";
            menuItem.Click += new EventHandler(this.exploreMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreateRenameMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Zmień nazwę";
            menuItem.Click += new EventHandler(this.renameMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreateOpenMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Otwórz";
            menuItem.Click += new EventHandler(this.openMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreateNewFileMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Nowy plik";
            menuItem.Click += new EventHandler(this.newFileMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreateNewDirectoryMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Nowy folder";
            menuItem.Click += new EventHandler(this.newDirectoryMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreateCopyFileMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Kopiuj plik";
            menuItem.Click += new EventHandler(this.copyFileMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreateMoveFileMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Przenieś plik";
            menuItem.Click += new EventHandler(this.moveFileMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreateCopyDirectoryMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Kopiuj folder";
            menuItem.Click += new EventHandler(this.copyDirectoryMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreateMoveDirectoryMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Przenieś folder";
            menuItem.Click += new EventHandler(this.moveDirectoryMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreateCopyClipboardMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Kopiuj do schowka";
            menuItem.Click += new EventHandler(this.copyClipboardMenu_Click);
            return menuItem;
        }

        private ToolStripMenuItem CreatePasteClipboardMenuItem()
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Text = "Wklej ze schowka";
            menuItem.Click += new EventHandler(this.pasteClipboardMenu_Click);
            return menuItem;
        }

        #endregion
    }
}
