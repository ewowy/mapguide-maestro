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

using OSGeo.MapGuide.ObjectModels.Common;
using System.Collections.Generic;

namespace OSGeo.MapGuide.ObjectModels.DrawingSource
{
    /// <summary>
    /// A DWF-based Drawing Source
    /// </summary>
    public interface IDrawingSource : IResource
    {
        /// <summary>
        /// Gets or sets the name of the source (dwf file).
        /// </summary>
        /// <value>The name of the source.</value>
        string SourceName { get; set; }

        /// <summary>
        /// Gets or sets the coordinate space.
        /// </summary>
        /// <value>The coordinate space.</value>
        string CoordinateSpace { get; set; }

        /// <summary>
        /// Removes all sheets.
        /// </summary>
        void RemoveAllSheets();

        /// <summary>
        /// Gets the sheets.
        /// </summary>
        /// <value>The sheets.</value>
        IEnumerable<IDrawingSourceSheet> Sheet { get; }

        /// <summary>
        /// Adds the sheet.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        void AddSheet(IDrawingSourceSheet sheet);

        /// <summary>
        /// Removes the sheet.
        /// </summary>
        /// <param name="sheet">The sheet.</param>
        void RemoveSheet(IDrawingSourceSheet sheet);

        /// <summary>
        /// Creates the sheet.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="minx">The minx.</param>
        /// <param name="miny">The miny.</param>
        /// <param name="maxx">The maxx.</param>
        /// <param name="maxy">The maxy.</param>
        /// <returns></returns>
        IDrawingSourceSheet CreateSheet(string name, double minx, double miny, double maxx, double maxy);
    }

    /// <summary>
    /// Represents a sheet (DWF section)
    /// </summary>
    public interface IDrawingSourceSheet
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the extent.
        /// </summary>
        /// <value>The extent.</value>
        IEnvelope Extent { get; set; }
    }
}