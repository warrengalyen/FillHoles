namespace FillHolesTest
{
    partial class FrmTest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTest));
            this.CmdOpen = new System.Windows.Forms.Button();
            this.SrcPic = new System.Windows.Forms.PictureBox();
            this.CmdFillHoleFore = new System.Windows.Forms.Button();
            this.CmdFillHoleBack = new System.Windows.Forms.Button();
            this.DestPic = new System.Windows.Forms.PictureBox();
            this.CmdRestore = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SrcPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DestPic)).BeginInit();
            this.SuspendLayout();
            // 
            // CmdOpen
            // 
            this.CmdOpen.Location = new System.Drawing.Point(12, 18);
            this.CmdOpen.Name = "CmdOpen";
            this.CmdOpen.Size = new System.Drawing.Size(85, 33);
            this.CmdOpen.TabIndex = 36;
            this.CmdOpen.Text = "Open image";
            this.CmdOpen.UseVisualStyleBackColor = true;
            this.CmdOpen.Click += new System.EventHandler(this.CmdOpen_Click);
            // 
            // SrcPic
            // 
            this.SrcPic.Location = new System.Drawing.Point(12, 74);
            this.SrcPic.Name = "SrcPic";
            this.SrcPic.Size = new System.Drawing.Size(521, 413);
            this.SrcPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.SrcPic.TabIndex = 33;
            this.SrcPic.TabStop = false;
            // 
            // CmdFillHoleFore
            // 
            this.CmdFillHoleFore.Location = new System.Drawing.Point(190, 18);
            this.CmdFillHoleFore.Name = "CmdFillHoleFore";
            this.CmdFillHoleFore.Size = new System.Drawing.Size(132, 33);
            this.CmdFillHoleFore.TabIndex = 38;
            this.CmdFillHoleFore.Text = "Fill Foreground holes";
            this.CmdFillHoleFore.UseVisualStyleBackColor = true;
            this.CmdFillHoleFore.Click += new System.EventHandler(this.CmdFillHole_Click);
            // 
            // CmdFillHoleBack
            // 
            this.CmdFillHoleBack.Location = new System.Drawing.Point(328, 18);
            this.CmdFillHoleBack.Name = "CmdFillHoleBack";
            this.CmdFillHoleBack.Size = new System.Drawing.Size(150, 33);
            this.CmdFillHoleBack.TabIndex = 39;
            this.CmdFillHoleBack.Text = "Fill background holes";
            this.CmdFillHoleBack.UseVisualStyleBackColor = true;
            this.CmdFillHoleBack.Click += new System.EventHandler(this.CmdFillHoleBack_Click);
            // 
            // DestPic
            // 
            this.DestPic.Location = new System.Drawing.Point(553, 74);
            this.DestPic.Name = "DestPic";
            this.DestPic.Size = new System.Drawing.Size(521, 413);
            this.DestPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.DestPic.TabIndex = 40;
            this.DestPic.TabStop = false;
            // 
            // CmdRestore
            // 
            this.CmdRestore.Location = new System.Drawing.Point(103, 18);
            this.CmdRestore.Name = "CmdRestore";
            this.CmdRestore.Size = new System.Drawing.Size(68, 33);
            this.CmdRestore.TabIndex = 41;
            this.CmdRestore.Text = "Restore";
            this.CmdRestore.UseVisualStyleBackColor = true;
            this.CmdRestore.Click += new System.EventHandler(this.CmdRestore_Click);
            // 
            // FrmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 498);
            this.Controls.Add(this.CmdRestore);
            this.Controls.Add(this.DestPic);
            this.Controls.Add(this.CmdFillHoleBack);
            this.Controls.Add(this.CmdFillHoleFore);
            this.Controls.Add(this.CmdOpen);
            this.Controls.Add(this.SrcPic);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Histogram-based global binarization algorithm";
            this.Load += new System.EventHandler(this.FrmTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SrcPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DestPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox SrcPic;
        private System.Windows.Forms.Button CmdOpen;
        private System.Windows.Forms.Button CmdFillHoleFore;
        private System.Windows.Forms.Button CmdFillHoleBack;
        private System.Windows.Forms.PictureBox DestPic;
        private System.Windows.Forms.Button CmdRestore;
    }
}