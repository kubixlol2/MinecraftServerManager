using System;
using System.Windows.Forms;

namespace MinecraftServerManager.Controls.ServersTreeNodes
{
    public class PropertiesNode : TreeNode
    {
        public new ServerNode Parent { get; private set; }

        public PropertiesNode(ServerNode parent)
            : base("Ustawienia")
        {
            this.ImageIndex = ServersTreeView.PropertiesIcon;
            this.SelectedImageIndex = this.ImageIndex;
            this.Parent = parent;
            this.Parent.Nodes.Add(this);
        }
    }
}
