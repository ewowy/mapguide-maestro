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
using System;
using System.Windows.Forms;

namespace Maestro.Editors.Common.Expression
{
    internal partial class ExpressionParseErrorDialog : Form
    {
        private ExpressionParseErrorDialog()
        {
            InitializeComponent();
        }

        private readonly FdoExpressionValidationException _ex;
        private readonly IExpressionErrorSource _source;

        public ExpressionParseErrorDialog(FdoExpressionValidationException ex, IExpressionErrorSource source)
            : this()
        {
            _ex = ex;
            _source = source;
            txtErrorDetails.Text = _ex.Message;
        }

        private void btnClose_Click(object sender, EventArgs e) => this.DialogResult = System.Windows.Forms.DialogResult.OK;

        private void btnGotoError_Click(object sender, EventArgs e)
        {
            _source.HighlightToken(_ex.Token);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
