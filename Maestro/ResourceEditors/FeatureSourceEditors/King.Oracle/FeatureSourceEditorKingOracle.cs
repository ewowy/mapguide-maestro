#region Disclaimer / License
// Copyright (C) 2006, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors
{
	/// <summary>
	/// Summary description for FeatureSourceEditorKingOracle.
	/// </summary>
	public class FeatureSourceEditorKingOracle : System.Windows.Forms.UserControl, ResourceEditor
	{
		private System.ComponentModel.IContainer components;

		private EditorInterface m_editor;
		private OSGeo.MapGuide.MaestroAPI.FeatureSource m_feature;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private ResourceEditors.FeatureSourceEditors.ODBC.Credentials credentials;
		private System.Windows.Forms.ToolTip toolTips;
		private System.Windows.Forms.TextBox Schema;
		private System.Windows.Forms.TextBox FDOClass;
		private System.Windows.Forms.TextBox Service;
		private bool m_isUpdating = false;
		private Globalizator.Globalizator m_globalizor;

		public FeatureSourceEditorKingOracle()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			m_globalizor = new Globalizator.Globalizator(this);
		}

		public FeatureSourceEditorKingOracle(EditorInterface editor, OSGeo.MapGuide.MaestroAPI.FeatureSource feature)
			: this()
		{
			m_editor = editor;
			m_feature = feature;

			UpdateDisplay();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Schema = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.FDOClass = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.Service = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.credentials = new ResourceEditors.FeatureSourceEditors.ODBC.Credentials();
			this.toolTips = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// Schema
			// 
			this.Schema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Schema.Location = new System.Drawing.Point(112, 40);
			this.Schema.Name = "Schema";
			this.Schema.Size = new System.Drawing.Size(200, 20);
			this.Schema.TabIndex = 42;
			this.Schema.Text = "";
			this.Schema.TextChanged += new System.EventHandler(this.Schema_TextChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 16);
			this.label2.TabIndex = 41;
			this.label2.Text = "Schema";
			// 
			// FDOClass
			// 
			this.FDOClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.FDOClass.Location = new System.Drawing.Point(112, 72);
			this.FDOClass.Name = "FDOClass";
			this.FDOClass.Size = new System.Drawing.Size(200, 20);
			this.FDOClass.TabIndex = 40;
			this.FDOClass.Text = "";
			this.FDOClass.TextChanged += new System.EventHandler(this.FDOClass_TextChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.TabIndex = 39;
			this.label3.Text = "King FDO Class";
			// 
			// Service
			// 
			this.Service.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.Service.Location = new System.Drawing.Point(112, 8);
			this.Service.Name = "Service";
			this.Service.Size = new System.Drawing.Size(200, 20);
			this.Service.TabIndex = 38;
			this.Service.Text = "";
			this.Service.TextChanged += new System.EventHandler(this.Service_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 16);
			this.label1.TabIndex = 37;
			this.label1.Text = "Service";
			// 
			// credentials
			// 
			this.credentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.credentials.Location = new System.Drawing.Point(8, 104);
			this.credentials.Name = "credentials";
			this.credentials.Size = new System.Drawing.Size(304, 152);
			this.credentials.TabIndex = 36;
			this.credentials.CredentialsChanged += new ResourceEditors.FeatureSourceEditors.ODBC.Credentials.CredentialsChangedDelegate(this.credentials_CredentialsChanged);
			// 
			// FeatureSourceEditorKingOracle
			// 
			this.AutoScroll = true;
			this.AutoScrollMinSize = new System.Drawing.Size(320, 264);
			this.Controls.Add(this.Schema);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.FDOClass);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.Service);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.credentials);
			this.Name = "FeatureSourceEditorKingOracle";
			this.Size = new System.Drawing.Size(320, 264);
			this.ResumeLayout(false);

		}
		#endregion

		public object Resource
		{
			get { return m_feature; }
			set 
			{
				m_feature = (OSGeo.MapGuide.MaestroAPI.FeatureSource)value;
				UpdateDisplay();
			}
		}

		public string ResourceId
		{
			get { return m_feature.ResourceId; }
			set { m_feature.ResourceId = value; }
		}

		public bool Preview()
		{
			return false;
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
				if (m_feature == null || m_feature.Parameter == null)
					return;

				credentials.SetCredentials(m_feature.Parameter["Username"], m_feature.Parameter["Password"]);

				Service.Text = m_feature.Parameter["Service"];
				Schema.Text = m_feature.Parameter["OracleSchema"];
				FDOClass.Text = m_feature.Parameter["KingFdoClass"];
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		public bool Save(string savename)
		{
			return false;
		}

		private void credentials_CredentialsChanged(string username, string password)
		{
			if (m_feature == null || m_isUpdating)
				return;

			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_feature.Parameter["Username"] = username;
			m_feature.Parameter["Password"] = password;
			m_editor.HasChanged();
		}

		private void Service_TextChanged(object sender, System.EventArgs e)
		{
			if (m_feature == null || m_isUpdating)
				return;

			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_feature.Parameter["Service"] = Service.Text;
			m_editor.HasChanged();
		}

		private void Schema_TextChanged(object sender, System.EventArgs e)
		{
			if (m_feature == null || m_isUpdating)
				return;

			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_feature.Parameter["OracleSchema"] = Schema.Text;
			m_editor.HasChanged();
		}

		private void FDOClass_TextChanged(object sender, System.EventArgs e)
		{
			if (m_feature == null || m_isUpdating)
				return;

			if (m_feature.Parameter == null)
				m_feature.Parameter = new OSGeo.MapGuide.MaestroAPI.NameValuePairTypeCollection();

			m_feature.Parameter["KingFdoClass"] = FDOClass.Text;
			m_editor.HasChanged();
		}
	}
}
