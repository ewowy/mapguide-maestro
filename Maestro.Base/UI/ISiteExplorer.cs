﻿#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
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
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.MapGuide.MaestroAPI;

namespace Maestro.Base.UI
{
    public interface ISiteExplorer : IViewContent
    {
        /// <summary>
        /// The name of the active <see cref="IServerConnection"/> associated with this site explorer. The active connection
        /// is the connection whose node or child nodes is currently selected
        /// </summary>
        string ConnectionName { get; }

        /// <summary>
        /// Gets the array of connection names.
        /// </summary>
        string[] ConnectionNames { get; }

        /// <summary>
        /// Performs a full refresh of the tree model
        /// </summary>
        void FullRefresh();

        /// <summary>
        /// Refreshes the tree model
        /// </summary>
        void RefreshModel(string connectionName);

        /// <summary>
        /// Refreshes the tree model from the specified resource id 
        /// </summary>
        /// <param name="resId"></param>
        void RefreshModel(string connectionName, string resId);

        /// <summary>
        /// Expands the node indicated by the specified id
        /// </summary>
        /// <param name="folderId"></param>
        void ExpandNode(string connectionName, string folderId);

        /// <summary>
        /// Selects the node indicated by the specified id
        /// </summary>
        /// <param name="resourceId"></param>
        void SelectNode(string connectionName, string resourceId);

        /// <summary>
        /// Flags the node indicated by the specified action
        /// </summary>
        /// <param name="resourceId"></param>
        /// <param name="action"></param>
        void FlagNode(string connectionName, string resourceId, NodeFlagAction action);

        /// <summary>
        /// Gets the items currently selected
        /// </summary>
        RepositoryItem[] SelectedItems { get; }
    }

    public enum NodeFlagAction
    {
        /*
        /// <summary>
        /// Indicate that the node has been cut and placed on the clipboard
        /// </summary>
        IndicateCut,
        /// <summary>
        /// Indicate that the node has been copied and placed on the clipboard
        /// </summary>
        IndicateCopy,*/
        /// <summary>
        /// Highlight the affected node with a pre-defined back color to indicate open
        /// </summary>
        HighlightOpen,
        /// <summary>
        /// Highlight the affected node with a pre-defined back color to indicate dirty state
        /// </summary>
        HighlightDirty,
        /// <summary>
        /// Reset node to default styles
        /// </summary>
        None
    }
}

//This is a compatibility shim. Most existing consumers of these APIs operate based on the active connection anyway

namespace Maestro.Base
{
    using UI;

    internal static class SiteExplorerShims
    {
        public static void RefreshModel(this ISiteExplorer explorer)
        {
            Check.NotNull(explorer, "explorer");
            explorer.RefreshModel(explorer.ConnectionName);
        }

        public static void ExpandNode(this ISiteExplorer explorer, string resId)
        {
            Check.NotNull(explorer, "explorer");
            explorer.ExpandNode(explorer.ConnectionName, resId);
        }

        public static void FlagNode(this ISiteExplorer explorer, string resId, NodeFlagAction action)
        {
            Check.NotNull(explorer, "explorer");
            explorer.FlagNode(explorer.ConnectionName, resId, action);
        }

        public static void SelectNode(this ISiteExplorer explorer, string resId)
        {
            Check.NotNull(explorer, "explorer");
            explorer.SelectNode(explorer.ConnectionName, resId);
        }
    }
}