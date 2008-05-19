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
	/// Summary description for PlaceHolderEditor.
	/// </summary>
	public class PlaceHolderEditor : System.Windows.Forms.UserControl, ResourceEditor
	{
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private EditorInterface m_editor;
		private object m_item;
		private bool m_isUpdating;
		private Globalizator.Globalizator m_globalizor = null;

		public PlaceHolderEditor(EditorInterface editor)
			: this()
		{
			m_editor = editor;
			m_item = null;
			UpdateDisplay();
		}

		public PlaceHolderEditor(EditorInterface editor, string resourceID)
			: this()
		{
			m_editor = editor;
			//TODO: This does not work with non-identified objects...
			m_item = editor.CurrentConnection.GetResource(resourceID);
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			try
			{
				m_isUpdating = true;
			}
			finally
			{
				m_isUpdating = false;
			}
		}

		private PlaceHolderEditor()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			m_globalizor = new  Globalizator.Globalizator(this);

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
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(48, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(296, 104);
			this.label1.TabIndex = 0;
			this.label1.Text = "This is a placeholder for the actual page";
			// 
			// PlaceHolderEditor
			// 
			this.Controls.Add(this.label1);
			this.Name = "PlaceHolderEditor";
			this.Size = new System.Drawing.Size(400, 320);
			this.ResumeLayout(false);

		}
		#endregion

		public string ResourceId
		{
			get 
			{
				if (m_item == null)
					return "";
				else
				{
					System.Reflection.PropertyInfo pi = m_item.GetType().GetProperty("ResourceId");
					if (pi != null)
						return (string)pi.GetValue(m_item, null);
					else
                        return ""; 
				}
			}
			set 
			{ 
				if (m_item == null)
					return;
				else
				{
					System.Reflection.PropertyInfo pi = m_item.GetType().GetProperty("ResourceId");
					if (pi != null)
						pi.SetValue(m_item, value, null);
					else
						return; 
				}
			}
		}

		public bool Preview()
		{
			return false;
		}

		public object Resource
		{
			get { return m_item; }
			set 
			{
				m_item = value;
				UpdateDisplay();
			}
		}

		public bool Save(string savename)
		{
			return false;
		}
	}
}
