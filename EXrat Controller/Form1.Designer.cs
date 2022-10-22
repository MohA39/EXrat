namespace EXrat_Controller
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.Device = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Username = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.IP = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Screen_Width = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Screen_Height = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Is_Admin = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ratToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.drawingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawTextToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.drawImageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mouseDrawToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.blackScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textToVoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.messageBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setWallpaperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backlightOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setAllTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reverseTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.downloadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.processlistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bSODToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.controlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cDTrayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // objectListView1
            // 
            this.objectListView1.AllColumns.Add(this.Device);
            this.objectListView1.AllColumns.Add(this.Username);
            this.objectListView1.AllColumns.Add(this.IP);
            this.objectListView1.AllColumns.Add(this.Screen_Width);
            this.objectListView1.AllColumns.Add(this.Screen_Height);
            this.objectListView1.AllColumns.Add(this.Is_Admin);
            this.objectListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.objectListView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.objectListView1.CellEditUseWholeCell = false;
            this.objectListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Device,
            this.Username,
            this.IP,
            this.Screen_Width,
            this.Screen_Height,
            this.Is_Admin});
            this.objectListView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.objectListView1.FullRowSelect = true;
            this.objectListView1.Location = new System.Drawing.Point(0, 24);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.Size = new System.Drawing.Size(928, 312);
            this.objectListView1.SmallImageList = this.imageList1;
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            this.objectListView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.objectListView1_ItemSelectionChanged);
            this.objectListView1.SizeChanged += new System.EventHandler(this.ObjectListView1_SizeChanged);
            // 
            // Device
            // 
            this.Device.AspectName = "Device";
            this.Device.Text = "Device";
            this.Device.Width = 76;
            // 
            // Username
            // 
            this.Username.AspectName = "Username";
            this.Username.Text = "Username";
            this.Username.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Username.Width = 146;
            // 
            // IP
            // 
            this.IP.AspectName = "IP";
            this.IP.Text = "IP";
            this.IP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.IP.Width = 174;
            // 
            // Screen_Width
            // 
            this.Screen_Width.AspectName = "ScreenWidth";
            this.Screen_Width.ButtonSizing = BrightIdeasSoftware.OLVColumn.ButtonSizingMode.CellBounds;
            this.Screen_Width.Text = "Screen Width";
            this.Screen_Width.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Screen_Width.Width = 180;
            // 
            // Screen_Height
            // 
            this.Screen_Height.AspectName = "ScreenHeight";
            this.Screen_Height.Text = "Screen Height";
            this.Screen_Height.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Screen_Height.Width = 180;
            // 
            // Is_Admin
            // 
            this.Is_Admin.AspectName = "IsAdmin";
            this.Is_Admin.Text = "Administrator";
            this.Is_Admin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Is_Admin.Width = 172;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "download.png");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ratToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(928, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ratToolStripMenuItem
            // 
            this.ratToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateToolStripMenuItem});
            this.ratToolStripMenuItem.Name = "ratToolStripMenuItem";
            this.ratToolStripMenuItem.Size = new System.Drawing.Size(36, 20);
            this.ratToolStripMenuItem.Text = "Rat";
            // 
            // generateToolStripMenuItem
            // 
            this.generateToolStripMenuItem.Name = "generateToolStripMenuItem";
            this.generateToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.generateToolStripMenuItem.Text = "Generate";
            this.generateToolStripMenuItem.Click += new System.EventHandler(this.GenerateToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.controlToolStripMenuItem,
            this.filesToolStripMenuItem,
            this.downloadFileToolStripMenuItem,
            this.processlistToolStripMenuItem,
            this.messageBoxToolStripMenuItem,
            this.backlightOffToolStripMenuItem,
            this.setAllTextToolStripMenuItem,
            this.reverseTextToolStripMenuItem,
            this.cDTrayToolStripMenuItem,
            this.bSODToolStripMenuItem,
            this.textToVoiceToolStripMenuItem,
            this.setWallpaperToolStripMenuItem,
            this.drawingToolStripMenuItem,
            this.restartToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(148, 334);
            // 
            // drawingToolStripMenuItem
            // 
            this.drawingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawTextToolStripMenuItem1,
            this.drawImageToolStripMenuItem1,
            this.mouseDrawToolStripMenuItem1,
            this.blackScreenToolStripMenuItem});
            this.drawingToolStripMenuItem.Name = "drawingToolStripMenuItem";
            this.drawingToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.drawingToolStripMenuItem.Text = "Drawing";
            // 
            // drawTextToolStripMenuItem1
            // 
            this.drawTextToolStripMenuItem1.Name = "drawTextToolStripMenuItem1";
            this.drawTextToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.drawTextToolStripMenuItem1.Text = "Draw text";
            this.drawTextToolStripMenuItem1.Click += new System.EventHandler(this.drawTextToolStripMenuItem_Click);
            // 
            // drawImageToolStripMenuItem1
            // 
            this.drawImageToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sizeToolStripMenuItem,
            this.noSizeToolStripMenuItem});
            this.drawImageToolStripMenuItem1.Name = "drawImageToolStripMenuItem1";
            this.drawImageToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.drawImageToolStripMenuItem1.Text = "Draw image";
            // 
            // sizeToolStripMenuItem
            // 
            this.sizeToolStripMenuItem.Name = "sizeToolStripMenuItem";
            this.sizeToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.sizeToolStripMenuItem.Text = "Size";
            this.sizeToolStripMenuItem.Click += new System.EventHandler(this.withSizeToolStripMenuItem_Click);
            // 
            // noSizeToolStripMenuItem
            // 
            this.noSizeToolStripMenuItem.Name = "noSizeToolStripMenuItem";
            this.noSizeToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.noSizeToolStripMenuItem.Text = "No size";
            this.noSizeToolStripMenuItem.Click += new System.EventHandler(this.withoutSizeToolStripMenuItem_Click);
            // 
            // mouseDrawToolStripMenuItem1
            // 
            this.mouseDrawToolStripMenuItem1.Name = "mouseDrawToolStripMenuItem1";
            this.mouseDrawToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.mouseDrawToolStripMenuItem1.Text = "Mouse draw";
            this.mouseDrawToolStripMenuItem1.Click += new System.EventHandler(this.mouseDrawToolStripMenuItem_Click);
            // 
            // blackScreenToolStripMenuItem
            // 
            this.blackScreenToolStripMenuItem.Name = "blackScreenToolStripMenuItem";
            this.blackScreenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.blackScreenToolStripMenuItem.Text = "Black screen";
            this.blackScreenToolStripMenuItem.Click += new System.EventHandler(this.setWallpaperToolStripMenuItem_Click);
            // 
            // textToVoiceToolStripMenuItem
            // 
            this.textToVoiceToolStripMenuItem.Name = "textToVoiceToolStripMenuItem";
            this.textToVoiceToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.textToVoiceToolStripMenuItem.Text = "Text To Voice";
            this.textToVoiceToolStripMenuItem.Click += new System.EventHandler(this.TextToVoiceToolStripMenuItem_Click);
            // 
            // messageBoxToolStripMenuItem
            // 
            this.messageBoxToolStripMenuItem.Name = "messageBoxToolStripMenuItem";
            this.messageBoxToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.messageBoxToolStripMenuItem.Text = "MessageBox";
            this.messageBoxToolStripMenuItem.Click += new System.EventHandler(this.messageBoxToolStripMenuItem_Click);
            // 
            // setWallpaperToolStripMenuItem
            // 
            this.setWallpaperToolStripMenuItem.Name = "setWallpaperToolStripMenuItem";
            this.setWallpaperToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.setWallpaperToolStripMenuItem.Text = "Set wallpaper";
            this.setWallpaperToolStripMenuItem.Click += new System.EventHandler(this.setWallpaperToolStripMenuItem_Click);
            // 
            // backlightOffToolStripMenuItem
            // 
            this.backlightOffToolStripMenuItem.Name = "backlightOffToolStripMenuItem";
            this.backlightOffToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.backlightOffToolStripMenuItem.Text = "Backlight off";
            this.backlightOffToolStripMenuItem.Click += new System.EventHandler(this.backlightOffToolStripMenuItem_Click);
            // 
            // setAllTextToolStripMenuItem
            // 
            this.setAllTextToolStripMenuItem.Name = "setAllTextToolStripMenuItem";
            this.setAllTextToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.setAllTextToolStripMenuItem.Text = "Set all text";
            this.setAllTextToolStripMenuItem.Click += new System.EventHandler(this.setAllTextToolStripMenuItem_Click);
            // 
            // reverseTextToolStripMenuItem
            // 
            this.reverseTextToolStripMenuItem.Name = "reverseTextToolStripMenuItem";
            this.reverseTextToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.reverseTextToolStripMenuItem.Text = "Reverse text";
            this.reverseTextToolStripMenuItem.Click += new System.EventHandler(this.reverseTextToolStripMenuItem_Click);
            // 
            // downloadFileToolStripMenuItem
            // 
            this.downloadFileToolStripMenuItem.Name = "downloadFileToolStripMenuItem";
            this.downloadFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.downloadFileToolStripMenuItem.Text = "Download file";
            this.downloadFileToolStripMenuItem.Click += new System.EventHandler(this.downloadFileToolStripMenuItem_Click);
            // 
            // processlistToolStripMenuItem
            // 
            this.processlistToolStripMenuItem.Name = "processlistToolStripMenuItem";
            this.processlistToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.processlistToolStripMenuItem.Text = "Processlist";
            this.processlistToolStripMenuItem.Click += new System.EventHandler(this.processlistToolStripMenuItem_Click);
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.filesToolStripMenuItem.Text = "File explorer";
            this.filesToolStripMenuItem.Click += new System.EventHandler(this.filesToolStripMenuItem_Click);
            // 
            // bSODToolStripMenuItem
            // 
            this.bSODToolStripMenuItem.Name = "bSODToolStripMenuItem";
            this.bSODToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.bSODToolStripMenuItem.Text = "BSOD";
            this.bSODToolStripMenuItem.Click += new System.EventHandler(this.bSODToolStripMenuItem_Click);
            // 
            // controlToolStripMenuItem
            // 
            this.controlToolStripMenuItem.Name = "controlToolStripMenuItem";
            this.controlToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.controlToolStripMenuItem.Text = "Control";
            this.controlToolStripMenuItem.Click += new System.EventHandler(this.controlToolStripMenuItem_Click);
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.restartToolStripMenuItem.Text = "Restart";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // cDTrayToolStripMenuItem
            // 
            this.cDTrayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.cDTrayToolStripMenuItem.Name = "cDTrayToolStripMenuItem";
            this.cDTrayToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.cDTrayToolStripMenuItem.Text = "CD tray";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openCDTrayToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeCDTrayToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 337);
            this.Controls.Add(this.objectListView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "EXrat Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView objectListView1;
        private BrightIdeasSoftware.OLVColumn Device;
        private BrightIdeasSoftware.OLVColumn Username;
        private BrightIdeasSoftware.OLVColumn IP;
        private BrightIdeasSoftware.OLVColumn Screen_Width;
        private BrightIdeasSoftware.OLVColumn Screen_Height;
        private BrightIdeasSoftware.OLVColumn Is_Admin;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ratToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem textToVoiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem messageBoxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setWallpaperToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backlightOffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAllTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reverseTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem downloadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem processlistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bSODToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem controlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawImageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem drawTextToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mouseDrawToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem blackScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cDTrayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    }
}

