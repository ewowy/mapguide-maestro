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

using OSGeo.MapGuide.MaestroAPI;
using System;

namespace Maestro.Base.Services
{
    /// <summary>
    /// Drag and Drop handler interface
    /// </summary>
    public interface IDragDropHandler
    {
        /// <summary>
        /// Gets a description of the action this handler will take
        /// </summary>
        string HandlerAction { get; }

        /// <summary>
        /// Gets whether this handler can handle the given file extension
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        bool CanHandleFileExtension(string fileExtension);

        /// <summary>
        /// Handles the file drop
        /// </summary>
        /// <param name="conn">The connection this drop affects</param>
        /// <param name="file">The file being dropped</param>
        /// <param name="folderId">The site explorer folder this drop was performed</param>
        /// <returns>true if the drop was successfully handled</returns>
        bool HandleDrop(IServerConnection conn, string file, string folderId);
    }

    internal static class HandlerUtil
    {
        public static bool ExtensionInList(string[] extensions, string fileExtension)
        {
            return Array.FindIndex(extensions, t => t.IndexOf(fileExtension, StringComparison.InvariantCultureIgnoreCase) >= 0) >= 0;
        }
    }
}