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
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Maestro.Editors.SymbolDefinition
{
    [ToolboxItem(false)]
    internal partial class ParametersCtrl : EditorBindableCollapsiblePanel
    {
        public ParametersCtrl()
        {
            InitializeComponent();
            grdParameters.AutoGenerateColumns = false;
            _params = new BindingList<IParameter>();
            grdParameters.DataSource = _params;
        }

        private IEditorService _edSvc;
        private BindingList<IParameter> _params;
        private ISimpleSymbolDefinition _sym;

        public override void Bind(IEditorService service)
        {
            _edSvc = service;
            _params.Clear();
            _sym = (ISimpleSymbolDefinition)service.GetEditedResource();
            foreach (var p in _sym.ParameterDefinition.Parameter)
            {
                _params.Add(p);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var p = _sym.CreateParameter();
            _params.Add(p);
            _sym.ParameterDefinition.AddParameter(p);
            var diag = new SymbolParameterDialog(_sym.ResourceVersion, p, _edSvc);
            diag.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grdParameters.SelectedRows.Count == 1)
            {
                var p = (IParameter)grdParameters.SelectedRows[0].DataBoundItem;
                var diag = new SymbolParameterDialog(_sym.ResourceVersion, p, _edSvc);
                diag.ShowDialog();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grdParameters.SelectedRows.Count == 1)
            {
                var p = (IParameter)grdParameters.SelectedRows[0].DataBoundItem;
                _params.Remove(p);
                _sym.ParameterDefinition.RemoveParameter(p);
                _edSvc.MarkDirty();
            }
        }

        private void grdParameters_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = grdParameters.Rows[e.RowIndex];
                row.Selected = true;
                btnEdit.Enabled = btnDelete.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = btnDelete.Enabled = false;
            }
        }
    }
}