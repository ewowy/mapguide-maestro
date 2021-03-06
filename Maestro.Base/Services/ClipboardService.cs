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

using Maestro.Base.UI;
using Maestro.Shared.UI;
using OSGeo.MapGuide.ObjectModels;

namespace Maestro.Base.Services
{
    /// <summary>
    /// Clipboard service. This simulates the behaviour of a clipboard, it does
    /// not actually put and retrieve data from the system clipboard.
    /// </summary>
    public class ClipboardService : ServiceBase
    {
        /// <summary>
        /// Initializes this service
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        private object _item;

        /// <summary>
        /// Gets whether the clipboard currently has any content
        /// </summary>
        /// <returns></returns>
        public bool HasContent()
        {
            lock (_clipLock)
            {
                return _item != null;
            }
        }

        /// <summary>
        /// Puts the given item into the clipboard
        /// </summary>
        /// <param name="item"></param>
        public void Put(object item)
        {
            Check.ArgumentNotNull(item, nameof(item));
            lock (_clipLock)
            {
                _item = item;
            }
        }

        /// <summary>
        /// Gets the current item from the clipboard
        /// </summary>
        /// <returns></returns>
        public object Get()
        {
            if (!HasContent())
                return null;

            lock (_clipLock)
            {
                return _item;
            }
        }

        private readonly object _clipLock = new object();

        internal RepositoryItem.ClipboardAction GetClipboardState(string resId)
        {
            Check.ArgumentNotEmpty(resId, nameof(resId));
            var state = RepositoryItem.ClipboardAction.None;
            object o = null;

            var riArr = o as RepositoryItem[];
            var ri = o as RepositoryItem;

            lock (_clipLock)
            {
                o = _item;
            }
            if (o == null)
            {
                state = RepositoryItem.ClipboardAction.None;
            }
            else if (riArr != null)
            {
                foreach (var r in riArr)
                {
                    if (resId.Equals(r.ResourceId))
                    {
                        state = r.ClipboardState;
                        break;
                    }
                }
            }
            else if (ri != null)
            {
                if (resId.Equals(ri.ResourceId))
                {
                    state = ri.ClipboardState;
                }
            }
            return state;
        }
    }
}