﻿namespace Maestro.Editors.WebLayout
{
    partial class WebLayoutEditorCtrl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebLayoutEditorCtrl));
            this.webLayoutSettingsCtrl = new Maestro.Editors.WebLayout.WebLayoutSettingsCtrl();
            this.webLayoutMenusCtrl = new Maestro.Editors.WebLayout.WebLayoutMenusCtrl();
            this.webLayoutCommandsCtrl = new Maestro.Editors.WebLayout.WebLayoutCommandsCtrl();
            this.webLayout3SettingsCtrl = new Maestro.Editors.WebLayout.WebLayout3SettingsCtrl();
            this.SuspendLayout();
            // 
            // webLayoutSettingsCtrl
            // 
            this.webLayoutSettingsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.webLayoutSettingsCtrl, "webLayoutSettingsCtrl");
            this.webLayoutSettingsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.webLayoutSettingsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.webLayoutSettingsCtrl.Name = "webLayoutSettingsCtrl";
            // 
            // webLayoutMenusCtrl
            // 
            this.webLayoutMenusCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.webLayoutMenusCtrl, "webLayoutMenusCtrl");
            this.webLayoutMenusCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.webLayoutMenusCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.webLayoutMenusCtrl.Name = "webLayoutMenusCtrl";
            // 
            // webLayoutCommandsCtrl
            // 
            this.webLayoutCommandsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.webLayoutCommandsCtrl, "webLayoutCommandsCtrl");
            this.webLayoutCommandsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.webLayoutCommandsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.webLayoutCommandsCtrl.Name = "webLayoutCommandsCtrl";
            // 
            // webLayout3SettingsCtrl
            // 
            this.webLayout3SettingsCtrl.ContentBackgroundColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.webLayout3SettingsCtrl, "webLayout3SettingsCtrl");
            this.webLayout3SettingsCtrl.HeaderBackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.webLayout3SettingsCtrl.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.webLayout3SettingsCtrl.Name = "webLayout3SettingsCtrl";
            // 
            // WebLayoutEditorCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.webLayoutCommandsCtrl);
            this.Controls.Add(this.webLayoutMenusCtrl);
            this.Controls.Add(this.webLayout3SettingsCtrl);
            this.Controls.Add(this.webLayoutSettingsCtrl);
            this.Name = "WebLayoutEditorCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private WebLayoutSettingsCtrl webLayoutSettingsCtrl;
        private WebLayoutMenusCtrl webLayoutMenusCtrl;
        private WebLayoutCommandsCtrl webLayoutCommandsCtrl;
        private WebLayout3SettingsCtrl webLayout3SettingsCtrl;

    }
}
