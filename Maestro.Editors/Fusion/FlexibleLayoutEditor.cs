﻿#region Disclaimer / License

// Copyright (C) 2010, Jackie Ng
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

using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;

namespace Maestro.Editors.Fusion
{
    /// <summary>
    /// Editor control for Fusion Flexible Layouts (Application Definitions)
    /// </summary>
    public partial class FlexibleLayoutEditor : EditorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlexibleLayoutEditor"/> class.
        /// </summary>
        public FlexibleLayoutEditor()
        {
            InitializeComponent();
        }

        private IApplicationDefinition _flexLayout;

        /// <summary>
        /// Sets the initial state of this editor and sets up any databinding
        /// within such that user interface changes will propagate back to the
        /// model.
        /// </summary>
        /// <param name="service"></param>
        public override void Bind(IEditorService service)
        {
            _flexLayout = (IApplicationDefinition)service.GetEditedResource();

            settingsCtrl.Bind(service);
            mapsCtrl.Bind(service);
            widgetsCtrl.Bind(service);
        }
    }
}