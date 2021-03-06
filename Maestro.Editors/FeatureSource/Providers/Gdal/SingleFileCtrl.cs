﻿#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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

using Maestro.Editors.Common;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Maestro.Editors.FeatureSource.Providers.Gdal
{
    [ToolboxItem(false)]
    internal partial class SingleFileCtrl : EditorBase
    {
        public SingleFileCtrl()
        {
            InitializeComponent();
        }

        private IFeatureSource _fs;
        private IServerConnection _conn;

        private bool _init = false;

        public override void Bind(IEditorService service)
        {
            try
            {
                _init = true;
                service.RegisterCustomNotifier(this);
                _fs = (IFeatureSource)service.GetEditedResource();
                _conn = service.CurrentConnection;

                InitDefaults();
            }
            finally
            {
                _init = false;
            }
        }

        internal void InitDefaults()
        {
            txtPath.Text = _fs.GetConnectionProperty("DefaultRasterFileLocation"); //NOXLATE
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            if (_init)
                return;

            _fs.SetConnectionProperty("DefaultRasterFileLocation", txtPath.Text); //NOXLATE
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = openFileDialog.FileName;
            }
        }

        private void btnBrowseDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnBrowseAliasFile_Click(object sender, EventArgs e)
        {
            using (var picker = new UnmanagedFileBrowser(_conn.ResourceService))
            {
                picker.SelectFoldersOnly = false;
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = picker.SelectedItem;
                }
            }
        }

        private void btnBrowseAliasDir_Click(object sender, EventArgs e)
        {
            using (var picker = new UnmanagedFileBrowser(_conn.ResourceService))
            {
                picker.SelectFoldersOnly = true;
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = picker.SelectedItem;
                }
            }
        }
    }
}