﻿#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource.Comparison;
using OSGeo.MapGuide.ObjectModels;
using System;
using System.Windows.Forms;

namespace Maestro.Editors.Diff
{
    /// <summary>
    /// A dialog that lets the user pick two resources for XML content comparison
    /// </summary>
    public partial class CompareResourceDialog : Form
    {
        private CompareResourceDialog()
        {
            InitializeComponent();
        }

        private readonly IServerConnection _conn;

        /// <summary>
        /// Creates a new instance of CompareResourceDialog
        /// </summary>
        /// <param name="conn">The server connection</param>
        public CompareResourceDialog(IServerConnection conn)
            : this()
        {
            _conn = conn;
        }

        /// <summary>
        /// Gets the source resource
        /// </summary>
        public string Source
        {
            get { return txtSource.Text; }
            set { txtSource.Text = value; }
        }

        /// <summary>
        /// Gets the target resource
        /// </summary>
        public string Target
        {
            get { return txtTarget.Text; }
            set { txtTarget.Text = value; }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

        private void btnCompare_Click(object sender, EventArgs e)
        {
            bool bValid = true;
            if (string.IsNullOrEmpty(txtSource.Text))
            {
                errorProvider.SetError(txtSource, Strings.Required);
                bValid = false;
            }
            else
            {
                errorProvider.SetError(txtSource, null);
            }
            if (string.IsNullOrEmpty(txtTarget.Text))
            {
                errorProvider.SetError(txtTarget, Strings.Required);
                bValid = false;
            }
            else
            {
                errorProvider.SetError(txtTarget, null);
            }

            if (bValid)
            {
                var set = XmlCompareUtil.PrepareForComparison(_conn.ResourceService,
                                                              this.Source,
                                                              this.Target);

                double time = 0;
                DiffEngine de = new DiffEngine();
                time = de.ProcessDiff(set.Source, set.Target, DiffEngineLevel.SlowPerfect);

                var rep = de.DiffReport();
                using (TextDiffView dlg = new TextDiffView(set.Source, set.Target, rep, time))
                {
                    dlg.SetLabels(this.Source, this.Target);
                    dlg.ShowDialog();
                    this.Close();
                }
            }
        }

        private void btnSource_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_conn, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtSource.Text = picker.ResourceID;
                }
            }
        }

        private void btnTarget_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Source))
            {
                MessageBox.Show(Strings.SelectSourceResource);
                return;
            }

            var resType = ResourceIdentifier.GetResourceTypeAsString(this.Source);

            using (var picker = new ResourcePicker(_conn, resType, ResourcePickerMode.OpenResource))
            {
                picker.SetStartingPoint(ResourceIdentifier.GetParentFolder(this.Source));
                if (picker.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtTarget.Text = picker.ResourceID;
                }
            }
        }
    }
}