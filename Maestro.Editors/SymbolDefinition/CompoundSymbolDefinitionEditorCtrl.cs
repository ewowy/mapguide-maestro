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

using Maestro.Editors.Generic;
using Maestro.Shared.UI;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.SymbolDefinition;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Maestro.Editors.SymbolDefinition
{
    /// <summary>
    /// A resource editor for Compound Symbol Definitions
    /// </summary>
    [ToolboxItem(true)]
    public partial class CompoundSymbolDefinitionEditorCtrl : EditorBase
    {
        private const int IDX_SYMBOL = 0;
        private const int IDX_REFERENCE = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompoundSymbolDefinitionEditorCtrl"/> class.
        /// </summary>
        public CompoundSymbolDefinitionEditorCtrl()
        {
            InitializeComponent();
        }

        private IEditorService _edSvc;
        private ICompoundSymbolDefinition _compSym;

        /// <summary>
        /// Sets the initial state of this editor and sets up any databinding
        /// within such that user interface changes will propagate back to the
        /// model.
        /// </summary>
        /// <param name="service"></param>
        public override void Bind(IEditorService service)
        {
            service.RegisterCustomNotifier(this);
            _edSvc = service;
            _compSym = (ICompoundSymbolDefinition)_edSvc.GetEditedResource();
            _compSym.PurgeSimpleSymbolAttributes();

            TextBoxBinder.BindText(txtName, _compSym, "Name");
            TextBoxBinder.BindText(txtDescription, _compSym, "Description");

            foreach (var symRef in _compSym.SimpleSymbol)
            {
                AddSymbolReference(symRef);
            }
        }

        private void RemoveSymbolReference(ISimpleSymbolReferenceBase symRef)
        {
            ListViewItem remove = null;
            foreach (ListViewItem li in lstSymbols.Items)
            {
                if (li.Tag == symRef)
                {
                    remove = li;
                    break;
                }
            }

            if (remove != null)
            {
                lstSymbols.Items.Remove(remove);
                _compSym.RemoveSimpleSymbol(symRef);
            }
        }

        private void AddSymbolReference(ISimpleSymbolReferenceBase symRef)
        {
            var li = new ListViewItem();
            if (symRef.Type == SimpleSymbolReferenceType.Library)
            {
                var sref = (ISimpleSymbolLibraryReference)symRef;
                li.Text = sref.ResourceId;
                li.ImageIndex = 1;
                li.Tag = symRef;
                lstSymbols.Items.Add(li);
            }
            else if (symRef.Type == SimpleSymbolReferenceType.Inline)
            {
                var inline = (ISimpleSymbolInlineReference)symRef;
                li.Text = inline.SimpleSymbolDefinition.Name;
                PropertyChangedEventHandler symChange = (s, e) =>
                {
                    if (e.PropertyName == "Name" && li != null)
                        li.Text = inline.SimpleSymbolDefinition.Name;
                };
                var simpleSym = inline.SimpleSymbolDefinition;
                simpleSym.PropertyChanged += WeakEventHandler.Wrap<PropertyChangedEventHandler>(symChange, (eh) => simpleSym.PropertyChanged -= eh);
                li.ImageIndex = 0;
                li.Tag = symRef;
                lstSymbols.Items.Add(li);
            }
            else
            {
                throw new ArgumentException("Unknown symbol reference type");
            }
        }

        private void symbolReferenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edSvc.CurrentConnection,
                                                   ResourceTypes.SymbolDefinition.ToString(),
                                                   ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    LastSelectedFolder.FolderId = picker.SelectedFolder;
                    var symRef = _compSym.CreateSymbolReference(picker.ResourceID);
                    AddSymbolReference(symRef);
                }
            }
        }

        private void simpleSymbolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Must create simple symbol of the same version
            var sym = ObjectFactory.CreateSimpleSymbol(_compSym.ResourceVersion, "", "");
            var symRef = _compSym.CreateSimpleSymbol(sym);
            AddSymbolReference(symRef);
            _compSym.AddSimpleSymbol(symRef);
        }

        private void lstSymbols_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = (lstSymbols.SelectedItems.Count == 1);
            if (btnDelete.Enabled)
            {
                var symRef = lstSymbols.SelectedItems[0].Tag as ISimpleSymbolReferenceBase;
                if (symRef != null)
                {
                    splitContainer1.Panel2.Controls.Clear();
                    if (symRef.Type == SimpleSymbolReferenceType.Inline)
                    {
                        var ctrl = new SimpleSymbolDefinitionEditorCtrl();
                        ctrl.Dock = DockStyle.Fill;
                        splitContainer1.Panel2.Controls.Add(ctrl);
                        var inline = (ISimpleSymbolInlineReference)symRef;
                        ctrl.Bind(new SymbolEditorService(_edSvc, inline.SimpleSymbolDefinition));
                    }
                    else if (symRef.Type == SimpleSymbolReferenceType.Library)
                    {
                        var ctrl = new SimpleSymbolReferenceCtrl(_edSvc, (ISimpleSymbolLibraryReference)symRef);
                        ctrl.Dock = DockStyle.Fill;
                        splitContainer1.Panel2.Controls.Add(ctrl);
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstSymbols.SelectedItems.Count == 1)
            {
                RemoveSymbolReference((ISimpleSymbolReferenceBase)lstSymbols.SelectedItems[0].Tag);
            }
            splitContainer1.Panel2.Controls.Clear();
        }
    }
}