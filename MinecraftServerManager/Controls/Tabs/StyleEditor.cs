using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MinecraftServerManager.Controls
{
    public partial class StyleEditor : UserControl, IStyleableTab
    {
        private Data.Styles styles;

        public List<Style> StyleControls = new List<Style>();

        private Tabs tabs;

        public StyleEditor()
        {
            InitializeComponent();
        }

        public new void Load(Tabs _tabs)
        {
            this.tabs = _tabs;
            foreach (Tab t in tabs.tabs)
            {
                if (t.control is StyleEditor)
                {
                    tabs.SelectTab(t);
                    return;
                }
            }
            if (File.Exists(Utils.Main.DataDirectory + "Styles.xml"))
            {
                styles = Data.Styles.Deserialize();
            }
            else 
            {
                styles = new Data.Styles();
                Data.Style brightGold = new Data.Style();
                brightGold.ForeColor = Color.Black;
                brightGold.ControlBackColor = Color.White;
                brightGold.WindowBackColor = Color.Gold;
                brightGold.Name = "Jasne złoto (Wbudowany)";
                brightGold.Selected = true;
                brightGold.BuiltIn = true;
                Data.Style dark = new Data.Style();
                dark.ForeColor = Color.FromArgb(224, 224, 224);
                dark.ControlBackColor = Color.FromArgb(32, 32, 32);
                dark.WindowBackColor = Color.Black;
                dark.Name = "Ciemny (Wbudowany)";
                dark.BuiltIn = true;
                styles.styles.Add(brightGold);
                styles.styles.Add(dark);
            }
            foreach (Data.Style s in styles.styles) 
            {
                if (s.Selected)
                    this.tabs.mainWindow.SetStyle(s);
                Style style = new Style();
                style.Load(this, s);
            }
            newStyle.Load(this);
            tabs.AddTab("Zarządzaj stylami", this);
        }

        private void StyleEditor_Resize(object sender, EventArgs e)
        {
            this.newStyle.Size = new Size(this.Width, 120);
            foreach (Style s in StyleControls)
            {
                s.Size = new Size(this.Width, 30);
            }
        }

        public void SetStyle(Data.Style style)
        {
            this.newStyle.SetStyle(style);
            foreach (Style s in StyleControls)
            {
                s.SetStyle(style);
            }
        }

        public void RefreshStyles()
        {
            for (int i = 0; i < StyleControls.Count; i++)
            {
                StyleControls[i].Location = new Point(0, 20 + i * 30);
            }
            this.newStyleLabel.Location = new Point(0, 20 + StyleControls.Count * 30);
            this.newStyle.Location = new Point(0, 40 + StyleControls.Count * 30);
        }

        public void SetSelectedStyle(Style style)
        {
            foreach (Style s in this.StyleControls)
            {
                s.SetChecked(false);
                s.style.Selected = false;
            }
            style.SetChecked(true);
            style.style.Selected = true;
            this.tabs.mainWindow.SetStyle(style.style);
        }

        public bool Close()
        {
            styles.Save();
            return true;
        }

        public void DeleteStyle(Data.Style style)
        {
            this.styles.styles.Remove(style);
        }

        public void AddStyle(Data.Style s)
        {
            this.styles.styles.Add(s);
            Style style = new Style();
            style.Load(this, s);
            this.StyleEditor_Resize(null, null);
            RefreshStyles();
        }
    }
}
