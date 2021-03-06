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

using ICSharpCode.Core;
using Maestro.Shared.UI;
using System;
using System.Collections.Generic;

namespace Maestro.Base.Services
{
    /// <summary>
    /// Manages view content
    /// </summary>
    public class ViewContentManager : ViewContentManagerBase
    {
        /// <summary>
        /// Initializes this instance
        /// </summary>
        public override void Initialize()
        {
            _singletonInstances = new List<IViewContent>();
            _singletonViewContentTypes = new Dictionary<string, Type>();

            List<IViewContent> views = AddInTree.BuildItems<IViewContent>("/Maestro/Shell/SingleViewContent", null);
            _singletonInstances.AddRange(views);

            foreach (var v in views)
            {
                var type = v.GetType();
                _singletonViewContentTypes.Add(type.Name, type);
            }

            LoggingService.Info(Strings.Service_Init_ViewContent_Manager);
        }

        /// <summary>
        /// Gets the workbench implementation
        /// </summary>
        /// <returns></returns>
        protected override WorkbenchBase GetWorkbench()
        {
            return Workbench.Instance;
        }
    }
}