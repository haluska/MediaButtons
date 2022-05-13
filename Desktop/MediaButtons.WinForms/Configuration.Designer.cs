namespace MediaButtons
{
    partial class Configuration
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
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lvPorts = new System.Windows.Forms.ListView();
			this.colPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colPNPDevice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colDetails = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSave.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSave.Location = new System.Drawing.Point(727, 430);
			this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(158, 58);
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(13, 430);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(158, 58);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// lvPorts
			// 
			this.lvPorts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPort,
            this.colType,
            this.colPNPDevice,
            this.colDetails});
			this.lvPorts.FullRowSelect = true;
			this.lvPorts.HideSelection = false;
			this.lvPorts.Location = new System.Drawing.Point(13, 75);
			this.lvPorts.Name = "lvPorts";
			this.lvPorts.Size = new System.Drawing.Size(872, 245);
			this.lvPorts.TabIndex = 3;
			this.lvPorts.UseCompatibleStateImageBehavior = false;
			this.lvPorts.View = System.Windows.Forms.View.Details;
			this.lvPorts.SelectedIndexChanged += new System.EventHandler(this.lvPorts_SelectedIndexChanged);
			// 
			// colPort
			// 
			this.colPort.Text = "Port";
			// 
			// colType
			// 
			this.colType.Text = "Type";
			this.colType.Width = 108;
			// 
			// colPNPDevice
			// 
			this.colPNPDevice.Text = "PNP Device Name";
			this.colPNPDevice.Width = 484;
			// 
			// colDetails
			// 
			this.colDetails.Text = "Details";
			this.colDetails.Width = 203;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
			this.label1.Location = new System.Drawing.Point(8, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(292, 28);
			this.label1.TabIndex = 4;
			this.label1.Text = "Select a valid COM port:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
			this.label2.Location = new System.Drawing.Point(592, 362);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(177, 28);
			this.label2.TabIndex = 5;
			this.label2.Text = "Selected Port:";
			// 
			// txtPort
			// 
			this.txtPort.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F);
			this.txtPort.Location = new System.Drawing.Point(775, 359);
			this.txtPort.Name = "txtPort";
			this.txtPort.ReadOnly = true;
			this.txtPort.Size = new System.Drawing.Size(110, 35);
			this.txtPort.TabIndex = 6;
			this.txtPort.Text = "NONE";
			this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// Configuration
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.ClientSize = new System.Drawing.Size(898, 502);
			this.Controls.Add(this.txtPort);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lvPorts);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = global::MediaButtons.Properties.Resources.AppIcon;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.Name = "Configuration";
			this.Text = "Configuration";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SaveSettings);
			this.Load += new System.EventHandler(this.Configuration_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ListView lvPorts;
		private System.Windows.Forms.ColumnHeader colPort;
		private System.Windows.Forms.ColumnHeader colPNPDevice;
		private System.Windows.Forms.ColumnHeader colDetails;
		private System.Windows.Forms.ColumnHeader colType;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtPort;
	}
}

