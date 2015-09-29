using System;
using System.Drawing;
using System.Windows.Forms;

namespace MinecraftServerManager.Dialogs
{
    public partial class License : Form
    {
        public License()
        {
            InitializeComponent();
        }

        private void License_Load(object sender, System.EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.licenseText.Rtf = Properties.Resources.License;
        }

        private void License_Resize(object sender, EventArgs e)
        {
            this.accept.Size = new Size((this.Width - 28) / 2, 30);
            this.decline.Size = new Size((this.Width - 28) / 2, 30);
            this.decline.Location = new Point(5, this.Height - 74);
            this.accept.Location = new Point(5 + (this.Width - 28) / 2, this.Height - 74);
            this.licenseText.Size = new Size(this.Width - 28, this.Height - 79);
        }
    }
}
