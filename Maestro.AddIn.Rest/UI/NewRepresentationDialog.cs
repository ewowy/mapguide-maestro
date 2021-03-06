﻿#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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
using Maestro.AddIn.Rest.Model;
using Maestro.AddIn.Rest.UI.Representation;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Forms;

namespace Maestro.AddIn.Rest.UI
{
    public partial class NewRepresentationDialog : Form
    {
        private NewRepresentationDialog()
        {
            InitializeComponent();
        }

        private readonly string _rep;
        private readonly IRepresentationCtrl _ctrl;
        private readonly dynamic _config;
        private RestSourceContext _context;

        public NewRepresentationDialog(string rep, dynamic config, RestSourceContext context)
            : this()
        {
            _context = context;
            _config = config;
            _rep = rep;
            switch(rep)
            {
                case "xml":
                case "geojson":
                    _ctrl = new CruddableRepresentationCtrl(rep, context);
                    break;
                case "csv":
                    _ctrl = new CsvRepresentationCtrl(context);
                    break;
                case "image":
                    _ctrl = new ImageRepresentationCtrl(context);
                    break;
                case "template":
                    _ctrl = new TemplateRepresentationCtrl(context);
                    break;
            }

            if (_ctrl != null)
            {
                _ctrl.Dock = DockStyle.Fill;
                panelSettings.Controls.Add((Control)_ctrl);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

        private void btnSave_Click(object sender, EventArgs e)
        {
            dynamic repr = null;
            if (!((IDictionary<string, object>)_config).ContainsKey(nameof(_config.Representations))) 
            {
                _config.Representations = new ExpandoObject();   
            }
            repr = _config.Representations;

            dynamic conf = _ctrl.GetOptions();
            ((IDictionary<string, object>)repr)[_rep] = conf;

            this.DialogResult = DialogResult.OK;
        }
    }
}
