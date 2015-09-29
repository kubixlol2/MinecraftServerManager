using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MinecraftServerManager.Controls
{
    public partial class NewStyle : UserControl
    {
        private StyleEditor styleEditor;

        public NewStyle()
        {
            InitializeComponent();
        }

        private void foreColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                foreColor.BackColor = cd.Color;
                foreColorLabel.ForeColor = Utils.Colors.Invert(cd.Color);
            }
        }

        private void controlBackColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                controlBackColor.BackColor = cd.Color;
                controlBackColorLabel.ForeColor = Utils.Colors.Invert(cd.Color);
            }
        }

        private void windowBackColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                windowBackColor.BackColor = cd.Color;
                windowBackColorLabel.ForeColor = Utils.Colors.Invert(cd.Color);
            }
        }

        private void NewStyle_Resize(object sender, EventArgs e)
        {
            this.nameText.Size = new Size(this.Width - 140, 30);
            this.foreColor.Size = new Size(this.Width - 140, 20);
            this.controlBackColor.Size = new Size(this.Width - 140, 20);
            this.windowBackColor.Size = new Size(this.Width - 140, 20);
            this.save.Size = new Size(this.Width, 30);
            this.foreColorLabel.Location = new Point((this.Width - 140 - this.foreColorLabel.Width) / 2, 0);
            this.controlBackColorLabel.Location = new Point((this.Width - 140 - this.controlBackColorLabel.Width) / 2, 0);
            this.windowBackColorLabel.Location = new Point((this.Width - 140 - this.windowBackColorLabel.Width) / 2, 0);
        }

        private void foreColorLabel_Click(object sender, EventArgs e)
        {
            foreColor_Click(sender, e);
        }

        private void controlBackColorLabel_Click(object sender, EventArgs e)
        {
            controlBackColor_Click(sender, e);
        }

        private void windowBackColorLabel_Click(object sender, EventArgs e)
        {
            windowBackColor_Click(sender, e);
        }


        public void SetStyle(Data.Style style)
        {
            Utils.Colors.StyleButton(this.save, style);
            this.nameText.ForeColor = style.ForeColor;
            this.nameText.BackColor = style.ControlBackColor;
            this.foreColor.BackColor = style.ForeColor;
            this.controlBackColor.BackColor = style.ControlBackColor;
            this.windowBackColor.BackColor = style.WindowBackColor;
            this.foreColorLabel.ForeColor = Utils.Colors.Invert(this.foreColor.BackColor);
            this.controlBackColorLabel.ForeColor = Utils.Colors.Invert(this.controlBackColor.BackColor);
            this.windowBackColorLabel.ForeColor = Utils.Colors.Invert(this.windowBackColor.BackColor);
        }

        public new void Load(StyleEditor _styleEditor)
        {
            this.styleEditor = _styleEditor;
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (this.nameText.Text == String.Empty)
            {
                MessageBox.Show("Nazwa stylu nie może być pusta!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Data.Style newStyle = new Data.Style();
            newStyle.Name = nameText.Text;
            newStyle.ForeColor = foreColor.BackColor;
            newStyle.ControlBackColor = controlBackColor.BackColor;
            newStyle.WindowBackColor = windowBackColor.BackColor;
            styleEditor.AddStyle(newStyle);
        }
    }
}
