﻿#region Disclaimer / License

// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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

using Maestro.Editors.Common.Expression;
using Maestro.Editors.LayerDefinition.Vector.Thematics;
using Maestro.Shared.UI;
using OSGeo.FDO.Expressions;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Exceptions;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.ObjectModels.Capabilities;
using OSGeo.MapGuide.ObjectModels.FeatureSource;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Maestro.Editors.Common
{
    /*
     * Intellisense overview:
     *
     * The intellisense of this expression editor consists of the following parts:
     *  - An ImageListBox which is filled with auto-complete suggestions
     *  - A System.Windows.Forms.ToolTip which is shown when an auto-complete choice is highlighted (but not selected)
     *
     * In order to invoke intellisense, we listen for the KeyUp and KeyDown events
     * on the textbox to determine what actions to take. Some actions include:
     *
     * Key Up:
     *  - Comma: Show auto-complete with all suggestions
     *  - Quotes (Single or Double): Insert an extra quote of that type
     *  - Up/Down: Move the auto-complete selection up/down one item if the auto-complete box is visible.
     *  - Backspace: Invoke auto-complete with suggestions if there is a context buffer, otherwise hide auto-complete.
     *  - Alt + Right: Invoke auto-complete with all suggestions
     *  - Alphanumeric (no modifiers): Invoke auto-complete with suggestions
     *
     * Key Down:
     *  - Escape: Hide auto-complete
     *  - Enter: Hide auto-complete
     *
     * As part of the loading process, a full list of auto-complete items (functions/properties) is constructed (sorted by name)
     * Everytime intellisense is invoked, this list is queried for possible suggestions.
     *
     * In order to determine what items to suggest, the editor builds a context buffer from the current position of the caret
     * in the textbox. The context buffer algorithm is as follows:
     *
     *  1 - Start from caret position
     *  2 - Can we move back one char?
     *    2.1 - Get this char.
     *    2.2 - If alpha numeric, goto 2.
     *  3 - Get the string that represents the uninterrupted alphanumeric string sequence that ends at the caret position
     *  4 - Get the list of completable items that starts with this alphanumeric string
     *  5 - Add these items to the auto-complete box.
     *  6 - Show the auto-complete box
     */

    /// <summary>
    /// An expression editor dialog
    /// </summary>
    public partial class MonoCompatibleExpressionEditor : Form, IExpressionEditor, ITextInserter, IExpressionErrorSource
    {
        private ClassDefinition _cls;

        private IFeatureService _featSvc;
        private IEditorService _edSvc;
        private string m_featureSource = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionEditor"/> class.
        /// </summary>
        internal MonoCompatibleExpressionEditor()
        {
            InitializeComponent();
            InitAutoComplete();
        }

        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>The expression.</value>
        public string Expression
        {
            get { return ExpressionText.Text; }
            set { ExpressionText.Text = value; }
        }

        private IFdoProviderCapabilities _caps;
        private ExpressionEditorMode _mode;

        /// <summary>
        /// Initializes the expression editor
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="cls"></param>
        /// <param name="featureSourceId"></param>
        /// <param name="mode"></param>
        public void Initialize(IServerConnection conn, ClassDefinition cls, string featureSourceId, ExpressionEditorMode mode)
        {
            IFeatureSource fs = (IFeatureSource)conn.ResourceService.GetResource(featureSourceId);
            IFdoProviderCapabilities caps = conn.FeatureService.GetProviderCapabilities(fs.Provider);

            //This is normally set by the Editor Service, but we don't have that so do it here
            _featSvc = conn.FeatureService;

            this.Initialize(null, caps, cls, featureSourceId, mode, false);
        }

        /// <summary>
        /// Initializes the dialog.
        /// </summary>
        /// <param name="edSvc">The editor service.</param>
        /// <param name="caps">The provider capabilities.</param>
        /// <param name="cls">The class definition.</param>
        /// <param name="featureSourceId">The features source id.</param>
        /// <param name="mode">The editor mode</param>
        /// <param name="attachStylizationFunctions">If true, Stylization FDO functions will be included</param>
        public void Initialize(IEditorService edSvc, IFdoProviderCapabilities caps, ClassDefinition cls, string featureSourceId, ExpressionEditorMode mode, bool attachStylizationFunctions)
        {
            try
            {
                _caps = caps;
                _mode = mode;
                _cls = cls;
                _edSvc = edSvc;
                if (_edSvc != null)
                {
                    _featSvc = _edSvc.CurrentConnection.FeatureService;
                    insertThemeExpressionToolStripMenuItem.Enabled = true;
                }
                else
                {
                    insertThemeExpressionToolStripMenuItem.Enabled = false;
                }
                m_featureSource = featureSourceId;

                insertThemeExpressionToolStripMenuItem.Enabled = attachStylizationFunctions;

                //TODO: Perhaps add column type and indication of primary key
                SortedList<string, PropertyDefinition> sortedCols = new SortedList<string, PropertyDefinition>();
                foreach (var col in _cls.Properties)
                {
                    sortedCols.Add(col.Name, col);
                }

                ColumnName.Items.Clear();
                ColumnName.Tag = sortedCols;

                foreach (var col in sortedCols.Values)
                {
                    string name = col.Name;
                    ToolStripButton btn = new ToolStripButton();
                    btn.Name = name;
                    btn.Text = name;
                    btn.Click += delegate
                    {
                        InsertText(name);
                    };
                    btnProperties.DropDown.Items.Add(btn);

                    ColumnName.Items.Add(name);
                }

                if (ColumnName.Items.Count > 0)
                    ColumnName.SelectedIndex = 0;

                LoadCompletableProperties(_cls.Properties);

                //TODO: Figure out how to translate the enums into something usefull

                //Functions
                var sortedFuncs = new SortedList<string, IFdoFunctionDefintion>();
                foreach (var func in caps.Expression.SupportedFunctions)
                {
                    sortedFuncs.Add(func.Name, func);
                }

                if (attachStylizationFunctions)
                {
                    foreach (var func in Utility.GetStylizationFunctions())
                    {
                        sortedFuncs.Add(func.Name, func);
                    }
                }

                foreach (var func in sortedFuncs.Values)
                {
                    string name = func.Name;
                    string desc = func.Description;

                    ToolStripItemCollection parent = btnFunctions.DropDown.Items;
                    var sigs = ExpressionEditor.MakeUniqueSignatures(func);
                    if (sigs.Length > 1)
                    {
                        ToolStripMenuItem btn = new ToolStripMenuItem();
                        btn.Name = string.Format(Strings.MultiSigFunction, name, sigs.Length);
                        btn.Text = string.Format(Strings.MultiSigFunction, name, sigs.Length);
                        btn.ToolTipText = desc;

                        btnFunctions.DropDown.Items.Add(btn);
                        parent = btn.DropDown.Items;
                    }

                    foreach (var sig in sigs)
                    {
                        ToolStripMenuItem btn = new ToolStripMenuItem();
                        btn.Name = name;
                        btn.ToolTipText = desc;
                        
                        List<string> args = new List<string>();
                        foreach (var argDef in sig.Arguments)
                        {
                            args.Add(argDef.Name.Trim());
                        }
                        string expr = $"{name}({FdoExpressionCompletionDataProvider.StringifyFunctionArgs(args)})"; //NOXLATE
                        btn.Text = $"{expr} : {sig.ReturnType}"; //NOXLATE
                        btn.Click += (s, e) =>
                        {
                            InsertText(expr);
                        };
                        parent.Add(btn);
                    }
                }
                LoadCompletableFunctions(caps.Expression.SupportedFunctions);
                if (attachStylizationFunctions)
                    LoadCompletableFunctions(Utility.GetStylizationFunctions());

                //Spatial Operators
                foreach (var op in caps.Filter.SpatialOperations)
                {
                    string name = op.ToUpper();
                    ToolStripButton btn = new ToolStripButton();
                    btn.Name = btn.Text = btn.ToolTipText = op;
                    btn.Click += delegate
                    {
                        InsertFilter(name);
                    };
                    btnSpatial.DropDown.Items.Add(btn);
                }

                //Distance Operators
                foreach (var op in caps.Filter.DistanceOperations)
                {
                    string name = op.ToUpper();
                    ToolStripButton btn = new ToolStripButton();
                    btn.Name = btn.Text = btn.ToolTipText = op;
                    btn.Click += delegate
                    {
                        InsertFilter(name);
                    };
                    btnDistance.DropDown.Items.Add(btn);
                }

                //Conditional Operators
                foreach (var op in caps.Filter.ConditionTypes)
                {
                    string name = op.ToUpper();
                    ToolStripButton btn = new ToolStripButton();
                    btn.Name = btn.Text = btn.ToolTipText = op;
                    btn.Click += delegate
                    {
                        InsertFilter(name);
                    };
                    btnCondition.DropDown.Items.Add(btn);
                }

                /*try
                {
                    /*FdoProviderCapabilities cap = m_connection.GetProviderCapabilities(m_providername);
                    foreach (FdoProviderCapabilitiesFilterType cmd in cap.Filter.Condition)
                        FunctionCombo.Items.Add(cmd.ToString());

                    FunctionLabel.Enabled = FunctionCombo.Enabled = true;
                }
                catch
                {
                    FunctionLabel.Enabled = FunctionCombo.Enabled = false;
                }*/
            }
            catch
            {
            }
        }

        private void InsertText(string exprText)
        {
            int index = ExpressionText.SelectionStart;
            if (ExpressionText.SelectionLength > 0)
            {
                ExpressionText.SelectedText = exprText;
                ExpressionText.SelectionStart = index;
            }
            else
            {
                if (index > 0)
                {
                    string text = ExpressionText.Text;
                    ExpressionText.Text = text.Insert(index, exprText);
                    ExpressionText.SelectionStart = index;
                }
                else
                {
                    ExpressionText.Text = exprText;
                    ExpressionText.SelectionStart = index;
                }
            }
        }

        private void InsertFilter(string op)
        {
            if (!string.IsNullOrEmpty(op))
            {
                string exprText = $"<geometry property> {op} GeomFromText('<FGF geometry text>')"; //LOCALIZEME
                InsertText(exprText);
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private SortedList<string, List<AutoCompleteItem>> _autoCompleteItems = new SortedList<string, List<AutoCompleteItem>>();
        private ImageListBox _autoBox;

        private enum AutoCompleteItemType : int
        {
            Property = 0,
            Function = 1,
        }

        /// <summary>
        /// Base auto-complete item
        /// </summary>
        private abstract class AutoCompleteItem
        {
            public abstract AutoCompleteItemType Type { get; }

            public abstract string Name { get; }

            public abstract string ToolTipText { get; }

            public abstract string AutoCompleteText { get; }
        }

        /// <summary>
        /// Property auto-complete item
        /// </summary>
        private class PropertyItem : AutoCompleteItem
        {
            private PropertyDefinition _propDef;

            public PropertyItem(PropertyDefinition pd)
            {
                _propDef = pd;
            }

            public override AutoCompleteItemType Type
            {
                get { return AutoCompleteItemType.Property; }
            }

            public override string Name
            {
                get { return _propDef.Name; }
            }

            private string _ttText;

            public override string ToolTipText
            {
                get
                {
                    if (string.IsNullOrEmpty(_ttText))
                    {
                        _ttText = string.Format(Strings.PropertyTooltip, _propDef.Name, _propDef.Type.ToString());
                    }
                    return _ttText;
                }
            }

            public override string AutoCompleteText
            {
                get { return this.Name; }
            }
        }

        /// <summary>
        /// Function auto-complete item
        /// </summary>
        private class FunctionItem : AutoCompleteItem
        {
            private IFdoFunctionDefintion _func;
            private IFdoFunctionDefintionSignature _sig;

            private readonly string _insertExpr;

            private FunctionItem(IFdoFunctionDefintion fd, IFdoFunctionDefintionSignature sig)
            {
                _func = fd;
                _sig = sig;
            }

            public FunctionItem(IFdoFunctionDefintion fd, IFdoFunctionDefintionSignature sig, string insertExpr)
                : this(fd, sig)
            {
                _insertExpr = insertExpr;
            }

            public override AutoCompleteItemType Type
            {
                get { return AutoCompleteItemType.Function; }
            }

            public override string Name
            {
                get { return _insertExpr; } // _func.Name; }
            }

            private string _ttText;

            public override string ToolTipText
            {
                get
                {
                    if (string.IsNullOrEmpty(_ttText))
                    {
                        string argDesc = FdoExpressionCompletionDataProvider.DescribeSignature(_sig);
                        _ttText = string.Format(Strings.ExprEditorFunctionDesc, _insertExpr, _func.Description, argDesc, _sig.ReturnType, Environment.NewLine);
                    }
                    //_ttText = string.Format(Strings.FunctionTooltip, GetReturnTypeString(), _func.Name, GetArgumentString(), _func.Description);

                    return _ttText;
                }
            }

            private string _argStr;

            private string GetArgumentString()
            {
                if (string.IsNullOrEmpty(_argStr))
                {
                    List<string> tokens = new List<string>();
                    foreach (var argDef in _sig.Arguments)
                    {
                        tokens.Add("[" + argDef.Name.Trim() + "]");
                    }
                    _argStr = string.Join(", ", tokens.ToArray());
                }
                return _argStr;
            }

            private string GetReturnTypeString()
            {
                return _sig.ReturnType;
            }

            public override string AutoCompleteText
            {
                get
                {
                    return _insertExpr; //return this.Name + "(" + GetArgumentString() + ")";
                }
            }
        }

        private void InitAutoComplete()
        {
            _autoBox = new ImageListBox();
            _autoBox.Visible = false;
            _autoBox.ImageList = new ImageList();
            _autoBox.ImageList.Images.Add(Properties.Resources.property);  //Property
            _autoBox.ImageList.Images.Add(Properties.Resources.block); //Function
            _autoBox.DoubleClick += new EventHandler(OnAutoCompleteDoubleClick);
            _autoBox.SelectedIndexChanged += new EventHandler(OnAutoCompleteSelectedIndexChanged);
            _autoBox.KeyDown += new KeyEventHandler(OnAutoCompleteKeyDown);
            _autoBox.KeyUp += new KeyEventHandler(OnAutoCompleteKeyUp);
            _autoBox.ValueMember = "Name";
            _autoBox.Font = new Font(FontFamily.GenericMonospace, 10.0f);
            ExpressionText.Controls.Add(_autoBox);
        }

        private void OnAutoCompleteKeyDown(object sender, KeyEventArgs e)
        {
            ExpressionText.Focus();
        }

        private void OnAutoCompleteKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Enter)
            {
                PutAutoCompleteSuggestion();
                _autoBox.Hide();
                _autoCompleteTooltip.Hide(this);
            }
        }

        private void OnAutoCompleteSelectedIndexChanged(object sender, EventArgs e)
        {
            ExpressionText.Focus();
            if (_autoBox.Visible && _autoBox.SelectedIndex >= 0 && _autoBox.Items.Count > 0)
            {
                string tt = ((_autoBox.SelectedItem as ImageListBoxItem).Tag as AutoCompleteItem).ToolTipText;
                Point pt = GetCaretPoint();
                pt.X += _autoBox.Width + 10;
                pt.Y += 65;

                _autoCompleteTooltip.Show(tt, this, pt.X, pt.Y);
            }
        }

        private void OnAutoCompleteDoubleClick(object sender, EventArgs e)
        {
            PutAutoCompleteSuggestion();
            _autoBox.Hide();
            _autoCompleteTooltip.Hide(this);
        }

        private void MoveAutoCompleteSelectionDown()
        {
            if (_autoBox.SelectedIndex < 0)
            {
                _autoBox.SelectedIndex = 0;
            }
            else
            {
                int idx = _autoBox.SelectedIndex;
                if ((idx + 1) <= _autoBox.Items.Count - 1)
                {
                    _autoBox.SelectedIndex = idx + 1;
                }
            }
        }

        private void MoveAutoCompleteSelectionUp()
        {
            if (_autoBox.SelectedIndex < 0)
            {
                _autoBox.SelectedIndex = 0;
            }
            else
            {
                int idx = _autoBox.SelectedIndex;
                if ((idx - 1) >= 0)
                {
                    _autoBox.SelectedIndex = idx - 1;
                }
            }
        }

        private void LoadCompletableProperties(IEnumerable<PropertyDefinition> cols)
        {
            foreach (var col in cols)
            {
                if (!_autoCompleteItems.ContainsKey(col.Name))
                    _autoCompleteItems[col.Name] = new List<AutoCompleteItem>();
                _autoCompleteItems[col.Name].Add(new PropertyItem(col));
            }
        }

        private void LoadCompletableFunctions(IEnumerable<IFdoFunctionDefintion> funcs)
        {
            foreach (var func in funcs)
            {
                var sigs = ExpressionEditor.MakeUniqueSignatures(func);
                foreach (var sig in sigs)
                {
                    var args = new List<string>();
                    foreach (var argDef in sig.Arguments)
                    {
                        args.Add(argDef.Name.Trim());
                    }
                    string expr = $"{func.Name}({FdoExpressionCompletionDataProvider.StringifyFunctionArgs(args)})"; //NOXLATE
                    if (!_autoCompleteItems.ContainsKey(func.Name))
                        _autoCompleteItems[func.Name] = new List<AutoCompleteItem>();
                    _autoCompleteItems[func.Name].Add(new FunctionItem(func, sig, expr));
                }
            }
        }

        private void PutAutoCompleteSuggestion()
        {
            if (_autoBox.SelectedItems.Count == 1)
            {
                int pos = ExpressionText.SelectionStart;
                string context;
                char? c = GetContextBuffer(out context);

                AutoCompleteItem aci = (_autoBox.SelectedItem as ImageListBoxItem).Tag as AutoCompleteItem;

                string fullText = aci.AutoCompleteText;

                int start = pos - context.Length;
                int newPos = start + fullText.Length;
                int selLength = -1;

                //if it's a function, highlight the parameter (or the first parameter if there is multiple arguments
                if (aci.Type == AutoCompleteItemType.Function)
                {
                    newPos = start + aci.Name.Length + 1; //Position the caret just after the opening bracket

                    //Has at least two arguments
                    int idx = fullText.IndexOf(",");
                    if (idx > 0)
                        selLength = idx - aci.Name.Length - 1;
                    else
                        selLength = fullText.IndexOf(")") - fullText.IndexOf("(") - 1;
                }

                string prefix = ExpressionText.Text.Substring(0, start);
                string suffix = ExpressionText.Text.Substring(pos, ExpressionText.Text.Length - pos);

                ExpressionText.Text = prefix + fullText + suffix;
                ExpressionText.SelectionStart = newPos;
                if (selLength > 0)
                {
                    ExpressionText.SelectionLength = selLength;
                }
                ExpressionText.ScrollToCaret();
            }
        }

        private Point GetCaretPoint()
        {
            Point pt = ExpressionText.GetPositionFromCharIndex(ExpressionText.SelectionStart);
            pt.Y += (int)Math.Ceiling(ExpressionText.Font.GetHeight()) + 2;
            pt.X += 2; // for Courier, may need a better method
            return pt;
        }

        private char? GetContextBuffer(out string buffer)
        {
            buffer = string.Empty;
            int caretPos = ExpressionText.SelectionStart;
            int currentPos = caretPos;
            char? res = null;
            if (caretPos > 0)
            {
                //Walk backwards
                caretPos--;
                char c = ExpressionText.Text[caretPos];
                while (Char.IsLetterOrDigit(c))
                {
                    caretPos--;

                    if (caretPos < 0)
                        break;

                    c = ExpressionText.Text[caretPos];
                }

                if (caretPos > 0)
                {
                    res = ExpressionText.Text[caretPos];
                }
                buffer = ExpressionText.Text.Substring(caretPos + 1, currentPos - caretPos - 1);
            }
            return res;
        }

        private void HandleKeyDown(KeyEventArgs e)
        {
            Keys code = e.KeyCode;
            switch (code)
            {
                case Keys.Escape:
                    if (_autoBox.Visible)
                    {
                        e.SuppressKeyPress = true;
                        _autoBox.Hide();
                        _autoCompleteTooltip.Hide(this);
                    }

                    break;
                case Keys.Up:
                case Keys.Down:
                    if (_autoBox.Visible)
                    {
                        e.SuppressKeyPress = true;
                    }

                    break;
                case Keys.Enter: // || Keys.Return
                    if (_autoBox.Visible && _autoBox.SelectedItems.Count == 1)
                    {
                        e.SuppressKeyPress = true;
                        PutAutoCompleteSuggestion();
                        _autoBox.Hide();
                        _autoCompleteTooltip.Hide(this);
                    }

                    break;
            }
        }

        private void HandleKeyUp(KeyEventArgs e)
        {
            Keys code = e.KeyCode;
            if (code == Keys.Oemcomma || code == Keys.OemOpenBrackets)
            {
                Complete(string.Empty);
            }
            else if (code == Keys.OemQuotes)
            {
                if (e.Modifiers == Keys.Shift)  // "
                    InsertText("\"");
                else                            // '
                    InsertText("'");
            }
            else if (code == Keys.D9 && e.Modifiers == Keys.Shift) // (
            {
                InsertText(")");
            }
            else if (code == Keys.Up || code == Keys.Down)
            {
                if (_autoBox.Visible)
                {
                    if (code == Keys.Up)
                    {
                        MoveAutoCompleteSelectionUp();
                    }
                    else
                    {
                        MoveAutoCompleteSelectionDown();
                    }
                }
            }
            else if (code == Keys.Back)
            {
                string context;
                char? c = GetContextBuffer(out context);
                if (!string.IsNullOrEmpty(context))
                {
                    Complete(context);
                }
                else
                {
                    if (_autoBox.Visible)
                    {
                        _autoBox.Hide();
                        _autoCompleteTooltip.Hide(this);
                    }
                }
            }
            else if (e.Modifiers == Keys.Alt && e.KeyCode == Keys.Right)
            {
                string context;
                char? c = GetContextBuffer(out context);
                Complete(context);
            }
            else
            {
                if (e.Modifiers == Keys.None)
                {
                    bool alpha = (code >= Keys.A && code <= Keys.Z);
                    bool numeric = (code >= Keys.D0 && code <= Keys.D9) || (code >= Keys.NumPad0 && code <= Keys.NumPad9);
                    if (alpha || numeric || e.KeyData == Keys.ShiftKey)
                    {
                        string context;
                        char? c = GetContextBuffer(out context);
                        Complete(context);
                    }
                }
            }
        }

        private List<AutoCompleteItem> GetItemsStartingWith(string text)
        {
            List<AutoCompleteItem> ati = new List<AutoCompleteItem>();
            foreach (string key in _autoCompleteItems.Keys)
            {
                if (key.ToLower().StartsWith(text.Trim().ToLower()))
                {
                    ati.AddRange(_autoCompleteItems[key]);
                }
            }
            return ati;
        }

        private void Complete(string text)
        {
            List<AutoCompleteItem> items = GetItemsStartingWith(text);
            _autoBox.Items.Clear();

            int width = 0;
            foreach (AutoCompleteItem it in items)
            {
                ImageListBoxItem litem = new ImageListBoxItem();
                litem.Text = it.Name;
                litem.ImageIndex = (int)it.Type;
                litem.Tag = it;

                _autoBox.Items.Add(litem);
                int length = TextRenderer.MeasureText(it.Name, _autoBox.Font).Width + 30; //For icon size
                if (length > width)
                    width = length;
            }
            _autoBox.Width = width;

            if (!_autoBox.Visible)
            {
                if (_autoBox.Items.Count > 0)
                {
                    _autoBox.BringToFront();
                    _autoBox.Show();
                }
            }

            Point pt = GetCaretPoint();

            _autoBox.Location = pt;
        }

        private void ExpressionText_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyDown(e);
        }

        private void ExpressionText_KeyUp(object sender, KeyEventArgs e)
        {
            HandleKeyUp(e);
        }

        private void ColumnName_Click(object sender, EventArgs e)
        {
        }

        private void ColumnName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ColumnValue.Enabled = false;
            LookupValues.Enabled = ColumnName.SelectedIndex >= 0;
        }

        private void LookupValues_Click(object sender, EventArgs e)
        {
            //Use UNIQUE() method first. This should work in most cases
            using (new WaitCursor(this))
            {
                string filter = null;
                var expr = $"UNIQUE({ColumnName.Text})";
                bool bFallback = false;
                ColumnValue.Items.Clear();
                ColumnValue.Tag = null;
                try
                {
                    using (var rdr = _featSvc.AggregateQueryFeatureSource(m_featureSource, _cls.QualifiedName, filter, new System.Collections.Specialized.NameValueCollection() {
                            { "UNIQ_VALS", expr }
                        }))
                    {
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            if (rdr.GetName(i) == "UNIQ_VALS")
                            {
                                ColumnName.Tag = rdr.GetFieldType(i);
                            }
                        }
                        while (rdr.ReadNext())
                        {
                            if (!rdr.IsNull("UNIQ_VALS"))
                            {
                                object value = rdr["UNIQ_VALS"];
                                ColumnValue.Items.Add(value);
                            }
                        }
                        rdr.Close();
                    }
                }
                catch
                {
                    ColumnValue.Items.Clear();
                    bFallback = true;
                }
                if (!bFallback)
                {
                    ColumnValue.Enabled = true;
                    ColumnValue.SelectedIndex = -1;
                    ColumnValue.DroppedDown = true;
                    return;
                }

                try
                {
                    SortedList<string, PropertyDefinition> cols = (SortedList<string, PropertyDefinition>)ColumnName.Tag;
                    PropertyDefinition col = cols[ColumnName.Text];

                    bool retry = true;
                    Exception rawEx = null;

                    SortedList<string, string> values = new SortedList<string, string>();
                    bool hasNull = false;

                    while (retry)
                    {
                        try
                        {
                            retry = false;
                            using (var rd = _featSvc.QueryFeatureSource(m_featureSource, _cls.QualifiedName, filter, new string[] { ColumnName.Text }))
                            {
                                while (rd.ReadNext())
                                {
                                    if (rd.IsNull(ColumnName.Text))
                                        hasNull = true;
                                    else
                                        values[Convert.ToString(rd[ColumnName.Text], System.Globalization.CultureInfo.InvariantCulture)] = null;
                                }
                                rd.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            if (filter == null && ex.Message.IndexOf("MgNullPropertyValueException") >= 0)
                            {
                                hasNull = true;
                                rawEx = ex;
                                retry = true;
                                filter = ColumnName.Text + " != NULL";
                            }
                            else if (rawEx != null)
                                throw rawEx;
                            else
                                throw;
                        }
                    }

                    ColumnValue.Items.Clear();
                    if (hasNull)
                        ColumnValue.Items.Add("NULL");

                    foreach (string s in values.Keys)
                        ColumnValue.Items.Add(s);

                    ColumnValue.Tag = col.Type;

                    if (ColumnValue.Items.Count == 0)
                        MessageBox.Show(this, Strings.NoColumnValuesError, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        ColumnValue.Enabled = true;
                        ColumnValue.SelectedIndex = -1;
                        ColumnValue.DroppedDown = true;
                    }
                }
                catch (Exception ex)
                {
                    string msg = NestedExceptionMessageProcessor.GetFullMessage(ex);
                    MessageBox.Show(this, string.Format(Strings.ColumnValueError, msg), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ColumnValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ColumnValue.SelectedIndex >= 0)
            {
                var tag = ColumnValue.Tag;
                if (tag != null)
                {
                    if (ColumnValue.Tag as Type == typeof(string) && (ColumnValue.SelectedIndex != 0 || ColumnValue.Text != "NULL"))
                    {
                        InsertText($"'{ColumnValue.Text}'"); //NOXLATE
                    }
                    else
                    {
                        if (tag is PropertyValueType && (PropertyValueType)tag == PropertyValueType.String)
                            InsertText($"'{ColumnValue.Text}'"); //NOXLATE
                        else
                            InsertText(ColumnValue.Text);
                    }
                }
                else
                {
                    InsertText(ColumnValue.Text);
                }
            }
        }

        private void insertThemeExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var theme = new ThemeCreator(_edSvc, _cls, this))
            {
                theme.ShowDialog();
            }
        }

        void ITextInserter.InsertText(string text)
        {
            this.InsertText(text);
        }

        private void insertARGBColorExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var picker = new ColorDialog())
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    var c = picker.Color;
                    this.InsertText($"ARGB({c.A}, {c.R}, {c.G}, {c.B})"); //NOXLATE
                }
            }
        }

        private void insertHTMLCOLORExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var picker = new ColorDialog())
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    var c = picker.Color;
                    this.InsertText($"HTMLCOLOR({c.R}, {c.G}, {c.B})"); //NOXLATE
                }
            }
        }

        private void buildAndInsertLOOKUPExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> propNames = new List<string>();
            foreach (var prop in _cls.Properties)
            {
                propNames.Add(prop.Name);
            }
            using (var picker = new LookupExpressionBuilder(propNames.ToArray()))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    this.InsertText(picker.GetExpression());
                }
            }
        }

        private void buildAndInsertRANGEExpressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> propNames = new List<string>();
            foreach (var prop in _cls.Properties)
            {
                propNames.Add(prop.Name);
            }
            using (var picker = new RangeExpressionBuilder(propNames.ToArray()))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    this.InsertText(picker.GetExpression());
                }
            }
        }

        void IExpressionErrorSource.SetCursor(int line, int col)
        {
            string text = ExpressionText.Text;
            int start = -1;
            for (int i = 0; i <= line; i++)
            {
                start++;
                start = text.IndexOf(Environment.NewLine, start);
            }

            ExpressionText.SelectionStart = start + col + 2; //1 to offset col, 1 to offset start
            ExpressionText.SelectionLength = 0;
        }

        void IExpressionErrorSource.HighlightToken(string token)
        {
            int idx = ExpressionText.Text.IndexOf(token, StringComparison.InvariantCultureIgnoreCase);
            ExpressionText.SelectionStart = idx;
            ExpressionText.SelectionLength = token.Length;
        }

        private FdoExpressionValidator _validator = new FdoExpressionValidator();

        private void btnValidate_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mode == ExpressionEditorMode.Filter)
                {
                    FdoFilter filter = FdoFilter.Parse(ExpressionText.Text);
                    _validator.ValidateFilter(filter, _cls, _caps);
                    MessageBox.Show(Strings.FilterIsValid);
                }
                else //Expression
                {
                    FdoExpression expr = FdoExpression.Parse(ExpressionText.Text);
                    _validator.ValidateExpression(expr, _cls, _caps);
                    MessageBox.Show(Strings.ExprIsValid);
                }
            }
            catch (FdoExpressionValidationException ex)
            {
                new ExpressionParseErrorDialog(ex, this).ShowDialog();
            }
            catch (FdoMalformedExpressionException ex)
            {
                new MalformedExpressionDialog(ex, this).ShowDialog();
            }
        }

        private void viewParsedExpressionFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mode == ExpressionEditorMode.Filter)
                {
                    FdoFilter filter = FdoFilter.Parse(ExpressionText.Text);
                    new ExpressionDisplayDialog(filter).ShowDialog();
                }
                else //Expression
                {
                    FdoExpression expr = FdoExpression.Parse(ExpressionText.Text);
                    new ExpressionDisplayDialog(expr).ShowDialog();
                }
            }
            catch (FdoParseException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    // ImageListBoxItem class
    internal class ImageListBoxItem
    {
        private string _myText;
        private int _myImageIndex;

        // properties
        public string Text
        {
            get { return _myText; }
            set { _myText = value; }
        }

        public int ImageIndex
        {
            get { return _myImageIndex; }
            set { _myImageIndex = value; }
        }

        //constructor
        public ImageListBoxItem(string text, int index)
        {
            _myText = text;
            _myImageIndex = index;
        }

        public ImageListBoxItem(string text)
            : this(text, -1)
        {
        }

        public ImageListBoxItem()
            : this("")
        {
        }

        private object _tag;

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public override string ToString()
        {
            return _myText;
        }
    }//End of ImageListBoxItem class

    // ImageListBox class
    //
    // Based on GListBox
    //
    // http://www.codeproject.com/KB/combobox/glistbox.aspx

    internal class ImageListBox : ListBox
    {
        private ImageList _myImageList;

        public ImageList ImageList
        {
            get { return _myImageList; }
            set { _myImageList = value; }
        }

        public ImageListBox()
        {
            // Set owner draw mode
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            ImageListBoxItem item;
            Rectangle bounds = e.Bounds;
            Size imageSize = _myImageList.ImageSize;
            try
            {
                item = (ImageListBoxItem)Items[e.Index];
                if (item.ImageIndex != -1)
                {
                    _myImageList.Draw(e.Graphics, bounds.Left, bounds.Top, item.ImageIndex);
                    e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
                        bounds.Left + imageSize.Width, bounds.Top);
                }
                else
                {
                    e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
                        bounds.Left, bounds.Top);
                }
            }
            catch
            {
                if (e.Index != -1)
                {
                    e.Graphics.DrawString(Items[e.Index].ToString(), e.Font,
                        new SolidBrush(e.ForeColor), bounds.Left, bounds.Top);
                }
                else
                {
                    e.Graphics.DrawString(Text, e.Font, new SolidBrush(e.ForeColor),
                        bounds.Left, bounds.Top);
                }
            }
            base.OnDrawItem(e);
        }
    }//End of ImageListBox class
}