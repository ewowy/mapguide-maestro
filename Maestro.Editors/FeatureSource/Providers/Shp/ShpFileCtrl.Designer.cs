﻿namespace Maestro.Editors.FeatureSource.Providers.Shp
{
    partial class ShpFileCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShpFileCtrl));
            this.btnTest = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.btnToOGR = new System.Windows.Forms.Button();
            this.contentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // resDataCtrl
            // 
            this.resDataCtrl.MarkEnabled = false;
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.btnToOGR);
            this.contentPanel.Controls.Add(this.txtStatus);
            this.contentPanel.Controls.Add(this.btnTest);
            this.contentPanel.Controls.SetChildIndex(this.resDataCtrl, 0);
            this.contentPanel.Controls.SetChildIndex(this.rdManaged, 0);
            this.contentPanel.Controls.SetChildIndex(this.rdUnmanaged, 0);
            this.contentPanel.Controls.SetChildIndex(this.btnTest, 0);
            this.contentPanel.Controls.SetChildIndex(this.txtStatus, 0);
            this.contentPanel.Controls.SetChildIndex(this.btnToOGR, 0);
            // 
            // btnTest
            // 
            resources.ApplyResources(this.btnTest, "btnTest");
            this.btnTest.Name = "btnTest";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // txtStatus
            // 
            resources.ApplyResources(this.txtStatus, "txtStatus");
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            // 
            // btnToOGR
            // 
            resources.ApplyResources(this.btnToOGR, "btnToOGR");
            this.btnToOGR.Name = "btnToOGR";
            this.btnToOGR.UseVisualStyleBackColor = true;
            this.btnToOGR.Click += new System.EventHandler(this.btnToOGR_Click);
            // 
            // ShpFileCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Name = "ShpFileCtrl";
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Button btnToOGR;
    }
}
