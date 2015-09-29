namespace MinecraftServerManager.Dialogs
{
    partial class License
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(License));
            this.licenseText = new System.Windows.Forms.RichTextBox();
            this.decline = new MinecraftServerManager.Controls.Button();
            this.accept = new MinecraftServerManager.Controls.Button();
            this.SuspendLayout();
            // 
            // licenseText
            // 
            this.licenseText.BackColor = System.Drawing.Color.White;
            this.licenseText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.licenseText.Location = new System.Drawing.Point(5, 5);
            this.licenseText.Name = "licenseText";
            this.licenseText.Size = new System.Drawing.Size(684, 352);
            this.licenseText.TabIndex = 0;
            this.licenseText.TabStop = false;
            this.licenseText.Text = "";
            // 
            // decline
            // 
            this.decline.BackColor = System.Drawing.Color.White;
            this.decline.DialogResult = System.Windows.Forms.DialogResult.No;
            this.decline.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.decline.FlatAppearance.BorderSize = 0;
            this.decline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.decline.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.decline.Location = new System.Drawing.Point(8, 382);
            this.decline.Name = "decline";
            this.decline.Size = new System.Drawing.Size(325, 30);
            this.decline.TabIndex = 0;
            this.decline.TabStop = false;
            this.decline.Text = "NIE AKCEPTUJĘ LICENCJI";
            this.decline.UseVisualStyleBackColor = false;
            // 
            // accept
            // 
            this.accept.BackColor = System.Drawing.Color.White;
            this.accept.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.accept.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.accept.FlatAppearance.BorderSize = 0;
            this.accept.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.accept.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.accept.Location = new System.Drawing.Point(367, 382);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(325, 30);
            this.accept.TabIndex = 0;
            this.accept.TabStop = false;
            this.accept.Text = "AKCEPTUJĘ LICENCJĘ";
            this.accept.UseVisualStyleBackColor = false;
            // 
            // License
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gold;
            this.ClientSize = new System.Drawing.Size(700, 420);
            this.Controls.Add(this.accept);
            this.Controls.Add(this.decline);
            this.Controls.Add(this.licenseText);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "License";
            this.Text = "Licencja";
            this.Load += new System.EventHandler(this.License_Load);
            this.Resize += new System.EventHandler(this.License_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox licenseText;
        private MinecraftServerManager.Controls.Button decline;
        private MinecraftServerManager.Controls.Button accept;


    }
}