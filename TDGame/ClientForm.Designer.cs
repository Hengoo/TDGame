namespace TDGame
{
    partial class ClientForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.port = new System.Windows.Forms.TextBox();
            this.ip = new System.Windows.Forms.TextBox();
            this.buttonIpVerbinden = new System.Windows.Forms.Button();
            this.buttonNeuLaden = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonVerbinden = new System.Windows.Forms.Button();
            this.zeichenfenster = new TDGame.Zeichenklasse();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.port);
            this.panel1.Controls.Add(this.ip);
            this.panel1.Controls.Add(this.buttonIpVerbinden);
            this.panel1.Controls.Add(this.buttonNeuLaden);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.buttonVerbinden);
            this.panel1.Location = new System.Drawing.Point(0, 600);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1200, 200);
            this.panel1.TabIndex = 4;
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(175, 142);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(100, 20);
            this.port.TabIndex = 6;
            this.port.Text = "1337";
            // 
            // ip
            // 
            this.ip.Location = new System.Drawing.Point(12, 142);
            this.ip.Name = "ip";
            this.ip.Size = new System.Drawing.Size(157, 20);
            this.ip.TabIndex = 5;
            this.ip.Text = "127.0.0.1";
            // 
            // buttonIpVerbinden
            // 
            this.buttonIpVerbinden.Location = new System.Drawing.Point(279, 139);
            this.buttonIpVerbinden.Name = "buttonIpVerbinden";
            this.buttonIpVerbinden.Size = new System.Drawing.Size(82, 23);
            this.buttonIpVerbinden.TabIndex = 4;
            this.buttonIpVerbinden.Text = "IP Verbinden";
            this.buttonIpVerbinden.UseVisualStyleBackColor = true;
            this.buttonIpVerbinden.Click += new System.EventHandler(this.buttonIpVerbinden_Click);
            // 
            // buttonNeuLaden
            // 
            this.buttonNeuLaden.Location = new System.Drawing.Point(279, 3);
            this.buttonNeuLaden.Margin = new System.Windows.Forms.Padding(2);
            this.buttonNeuLaden.Name = "buttonNeuLaden";
            this.buttonNeuLaden.Size = new System.Drawing.Size(82, 19);
            this.buttonNeuLaden.TabIndex = 2;
            this.buttonNeuLaden.Text = "Neu Laden";
            this.buttonNeuLaden.UseVisualStyleBackColor = true;
            this.buttonNeuLaden.Click += new System.EventHandler(this.buttonNeuLaden_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(11, 3);
            this.listBox1.Margin = new System.Windows.Forms.Padding(2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(264, 121);
            this.listBox1.TabIndex = 1;
            // 
            // buttonVerbinden
            // 
            this.buttonVerbinden.Location = new System.Drawing.Point(279, 26);
            this.buttonVerbinden.Margin = new System.Windows.Forms.Padding(2);
            this.buttonVerbinden.Name = "buttonVerbinden";
            this.buttonVerbinden.Size = new System.Drawing.Size(82, 19);
            this.buttonVerbinden.TabIndex = 0;
            this.buttonVerbinden.Text = "Verbinden";
            this.buttonVerbinden.UseVisualStyleBackColor = true;
            this.buttonVerbinden.Click += new System.EventHandler(this.buttonVerbinden_Click);
            // 
            // zeichenfenster
            // 
            this.zeichenfenster.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.zeichenfenster.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.zeichenfenster.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.zeichenfenster.ForeColor = System.Drawing.Color.Transparent;
            this.zeichenfenster.Location = new System.Drawing.Point(0, 0);
            this.zeichenfenster.Margin = new System.Windows.Forms.Padding(0);
            this.zeichenfenster.Name = "zeichenfenster";
            this.zeichenfenster.Size = new System.Drawing.Size(1200, 800);
            this.zeichenfenster.TabIndex = 3;
            this.zeichenfenster.Text = "zeichenklasse1";
            this.zeichenfenster.Paint += new System.Windows.Forms.PaintEventHandler(this.zeichenfenster_Paint);
            this.zeichenfenster.KeyDown += new System.Windows.Forms.KeyEventHandler(this.zeichenfenster_KeyDown);
            this.zeichenfenster.MouseDown += new System.Windows.Forms.MouseEventHandler(this.zeichenfenster_MouseDown);
            this.zeichenfenster.MouseUp += new System.Windows.Forms.MouseEventHandler(this.zeichenfenster_MouseUp_1);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.zeichenfenster);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "TDGame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        /// <summary>
        /// Das Zeichenfenster in dem das ganze Spiel gezeichnet wird
        /// </summary>
        public Zeichenklasse zeichenfenster;
        /// <summary>
        /// Das Panel in dem die verbindungssuche statfindet und gezeichnet wird
        /// </summary>
        public System.Windows.Forms.Panel panel1;
        /// <summary>
        /// Der Button mit dem die List box Neu geladen wird
        /// </summary>
        private System.Windows.Forms.Button buttonNeuLaden;
        /// <summary>
        /// Listbox für alle gefundenen Server
        /// </summary>
        public System.Windows.Forms.ListBox listBox1;
        /// <summary>
        /// Verbindet den Clienten mit dem in der Listbox markierten Server
        /// </summary>
        private System.Windows.Forms.Button buttonVerbinden;
        private System.Windows.Forms.TextBox ip;
        private System.Windows.Forms.Button buttonIpVerbinden;
        private System.Windows.Forms.TextBox port;







    }
}

