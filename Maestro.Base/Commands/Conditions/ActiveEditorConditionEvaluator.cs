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
using Maestro.Base.Editor;

namespace Maestro.Base.Commands.Conditions
{
    internal class ActiveEditorConditionEvaluator : IConditionEvaluator
    {
        static class ConditionProperties
        {
            public const string CANPREVIEW = nameof(CANPREVIEW);
            public const string CANVALIDATE = nameof(CANVALIDATE);
            public const string CANSAVE = nameof(CANSAVE);
            public const string CANPROFILE = nameof(CANPROFILE);
            public const string CANEDITASXML = nameof(CANEDITASXML);
        }

        public bool IsValid(object caller, Condition condition)
        {
            var wb = Workbench.Instance;
            if (wb != null)
            {
                var cnt = wb.ActiveDocumentView;
                var ed = cnt as IEditorViewContent;
                string prop = condition.Properties["property"]; //NOXLATE
                if (!string.IsNullOrEmpty(prop))
                {
                    prop = prop.ToUpper();
                    switch (prop)
                    {
                        case ConditionProperties.CANPREVIEW:
                            return ed != null && ed.CanBePreviewed;

                        case ConditionProperties.CANVALIDATE:
                            return ed != null && ed.CanBeValidated;

                        case ConditionProperties.CANSAVE:
                            return ed != null && !ed.IsNew && ed.IsDirty;

                        case ConditionProperties.CANPROFILE:
                            return ed != null && ed.CanProfile;

                        case ConditionProperties.CANEDITASXML:
                            return ed != null && ed.CanEditAsXml;

                        default:
                            return false;
                    }
                }
                else //No property, then just see if active doc is an editor
                {
                    return ed != null;
                }
            }
            return false;
        }
    }
}