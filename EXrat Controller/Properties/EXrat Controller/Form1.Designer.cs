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
            this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
            this.Device = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Username = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.IP = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Screen_Width = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Screen_Height = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.Is_Admin = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
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
            this.objectListView1.Location = new System.Drawing.Point(12, 12);
            this.objectListView1.Name = "objectListView1";
            this.objectListView1.Size = new System.Drawing.Size(852, 313);
            this.objectListView1.StateImageList = this.imageList1;
            this.objectListView1.TabIndex = 0;
            this.objectListView1.UseCompatibleStateImageBehavior = false;
            this.objectListView1.View = System.Windows.Forms.View.Details;
            // 
            // Device
            // 
            this.Device.AspectName = "Device";
            this.Device.Text = "Device";
            this.Device.Width = 96;
            // 
            // Username
            // 
            this.Username.AspectName = "Username";
            this.Username.Text = "Username";
            this.Username.Width = 143;
            // 
            // IP
            // 
            this.IP.AspectName = "IP";
            this.IP.Text = "IP";
            this.IP.Width = 174;
            // 
            // Screen_Width
            // 
            this.Screen_Width.AspectName = "Screen Width";
            this.Screen_Width.ButtonSizing = BrightIdeasSoftware.OLVColumn.ButtonSizingMode.CellBounds;
            this.Screen_Width.Text = "Screen Width";
            this.Screen_Width.Width = 90;
            // 
            // Screen_Height
            // 
            this.Screen_Height.AspectName = "Screen Height";
            this.Screen_Height.Text = "Screen Height";
            this.Screen_Height.Width = 99;
            // 
            // Is_Admin
            // 
            this.Is_Admin.AspectName = "Is Admin";
            this.Is_Admin.Text = "IsAdmin";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(32, 32);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 337);
            this.Controls.Add(this.objectListView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
            this.ResumeLayout(false);

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
    }
}

