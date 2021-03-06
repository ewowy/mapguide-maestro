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

using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;

namespace Maestro.Editors.WatermarkDefinition
{
    /// <summary>
    /// Editor control for Watermark Definitions
    /// </summary>
    public partial class WatermarkEditorCtrl : EditorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WatermarkEditorCtrl()
        {
            InitializeComponent();
        }

        private IWatermarkDefinition _wm;

        /// <summary>
        /// Sets the initial state of this editor and sets up any databinding
        /// within such that user interface changes will propagate back to the
        /// model.
        /// </summary>
        /// <param name="service"></param>
        public override void Bind(IEditorService service)
        {
            _wm = (IWatermarkDefinition)service.GetEditedResource();
            _wm.Content.RemoveSchemaAttributes(); //Sanity

            wmContent.Bind(service);
            wmSettings.Bind(service);
        }
    }
}