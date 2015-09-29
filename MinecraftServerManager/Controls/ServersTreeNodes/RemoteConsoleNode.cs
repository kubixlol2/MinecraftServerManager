using System;
using System.Windows.Forms;

namespace MinecraftServerManager.Controls.ServersTreeNodes
{

    public class RemoteConsoleNode : TreeNode
    {
        public new RemoteServerNode Parent { get; private set; }

        public RemoteConsoleNode(RemoteServerNode parent)
            : base("Konsola serwera")
        {
            this.ImageIndex = ServersTreeView.ConsoleIcon;
            this.SelectedImageIndex = this.ImageIndex;
            this.Parent = parent;
            this.Parent.Nodes.Add(this);
        }
    }

}
