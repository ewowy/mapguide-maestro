﻿#region Disclaimer / License

// Copyright (C) 2012, Jackie Ng
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
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace Maestro.MapViewer
{
    /// <summary>
    /// A toolbar that contains a default set of viewer commands
    /// </summary>
    [ToolboxItem(true)]
    public class DefaultToolbar : ToolStrip, IDefaultToolbar
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultToolbar"/> class.
        /// </summary>
        public DefaultToolbar()
            : base()
        {
            this.GripStyle = ToolStripGripStyle.Visible;
            this.ZoomIn = new ToolStripButton(string.Empty, Properties.Resources.zoom_in_fixed, OnZoomIn)
            {
                ToolTipText = Properties.Resources.TextZoomIn,
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };
            this.ZoomOut = new ToolStripButton(string.Empty, Properties.Resources.zoom_out_fixed, OnZoomOut)
            {
                ToolTipText = Properties.Resources.TextZoomOut,
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };
            this.ZoomExtents = new ToolStripButton(string.Empty, Properties.Resources.zoom_full, OnZoomExtents)
            {
                ToolTipText = Properties.Resources.TextZoomExtents,
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };
            this.SelectTool = new ToolStripButton(string.Empty, Properties.Resources.select_features, OnSelect)
            {
                ToolTipText = Properties.Resources.TextSelect,
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };
            this.Pan = new ToolStripButton(string.Empty, Properties.Resources.icon_pan, OnPan)
            {
                ToolTipText = Properties.Resources.TextPan,
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };
            this.ClearSelection = new ToolStripButton(string.Empty, Properties.Resources.select_clear, OnClearSelection)
            {
                ToolTipText = Properties.Resources.TextClearSelection,
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };
            this.RefreshMap = new ToolStripButton(string.Empty, Properties.Resources.view_refresh, OnRefreshMap)
            {
                ToolTipText = Properties.Resources.TextRefreshMap,
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };
            this.ToggleTooltips = new ToolStripButton(Properties.Resources.TextDisableTooltips, Properties.Resources.ui_tooltip_balloon_bottom, OnToggleTooltips)
            {
                ToolTipText = Properties.Resources.TextDisableTooltips,
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            };
            this.Loading = new ToolStripButton(string.Empty, Properties.Resources.icon_loading)
            {
                Alignment = ToolStripItemAlignment.Right,
                ImageScaling = ToolStripItemImageScaling.None,
                Visible = false
            };
            this.SelectPolygon = new ToolStripButton(string.Empty, Properties.Resources.select_polygon, OnSelectPolygon)
            {
                ToolTipText = Properties.Resources.TextSelectPolygon,
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };
            this.SelectRadius = new ToolStripButton(string.Empty, Properties.Resources.select_radius, OnSelectRadius)
            {
                ToolTipText = Properties.Resources.TextSelectRadius,
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };
            this.CopyMap = new ToolStripButton(string.Empty, Properties.Resources.edit_copy, OnCopyMap)
            {
                ToolTipText = Properties.Resources.TextCopyMap,
                DisplayStyle = ToolStripItemDisplayStyle.Image
            };

            this.Items.AddRange(new ToolStripItem[]
            {
                CopyMap,
                new ToolStripSeparator(),
                ZoomIn,
                ZoomOut,
                ZoomExtents,
                new ToolStripSeparator(),
                SelectTool,
                SelectRadius,
                SelectPolygon,
                Pan,
                new ToolStripSeparator(),
                ClearSelection,
                RefreshMap,
                new ToolStripSeparator(),
                ToggleTooltips,
                Loading
            });
        }

        /// <summary>
        /// Gets or sets the zoom out mode.
        /// </summary>
        /// <value>
        /// The zoom out mode.
        /// </value>
        [Category("MapGuide Viewer")]
        [Description("The behaviour of the zoom out command")]
        [DefaultValue(ZoomOutMode.ClickToZoom)]
        public ZoomOutMode ZoomOutMode
        {
            get;
            set;
        }

        private IMapViewer _viewer;

        /// <summary>
        /// Gets or sets the viewer this toolbar is associated with
        /// </summary>
        [Category("MapGuide Viewer")]
        [Description("The map viewer component this toolbar will control")]
        public IMapViewer Viewer
        {
            get { return _viewer; }
            set
            {
                if (_viewer != null)
                {
                    _viewer.PropertyChanged -= OnViewerPropertyChanged;
                    _viewer = null;
                }

                _viewer = value;
                if (_viewer != null)
                    _viewer.PropertyChanged += OnViewerPropertyChanged;

                TooltipStateChanged();
                UpdateButtonCheckedState();
            }
        }

        internal ToolStripButton Loading { get; }

        internal ToolStripButton ZoomIn { get; }

        private void OnZoomIn(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            _viewer.ActiveTool = MapActiveTool.ZoomIn;
        }

        internal ToolStripButton ZoomOut { get; }

        private void OnZoomOut(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            if (this.ZoomOutMode == ZoomOutMode.ClickToZoom)
            {
                _viewer.ActiveTool = MapActiveTool.ZoomOut;
            }
            else
            {
                var map = _viewer.GetMap();
                var centerPt = map.ViewCenter;

                _viewer.ZoomToView(centerPt.X, centerPt.Y, map.ViewScale * _viewer.ZoomOutFactor, true);
            }
        }

        internal ToolStripButton ZoomExtents { get; }

        private void OnZoomExtents(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            _viewer.InitialMapView();
        }

        internal ToolStripButton SelectTool { get; }

        private void OnSelect(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            _viewer.ActiveTool = MapActiveTool.Select;
        }

        internal ToolStripButton Pan { get; }

        private void OnPan(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            _viewer.ActiveTool = MapActiveTool.Pan;
        }

        internal ToolStripButton ClearSelection { get; }

        private void OnClearSelection(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            _viewer.ClearSelection();
        }

        internal ToolStripButton SelectRadius { get; }

        private void OnSelectRadius(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            _viewer.DigitizeCircle((x, y, r) =>
            {
                _viewer.SelectByWkt(Utility.MakeWktCircle(x, y, r), -1);
            });
        }

        internal ToolStripButton SelectPolygon { get; }

        private void OnSelectPolygon(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            _viewer.DigitizePolygon((coordinates) =>
            {
                StringBuilder wkt = new StringBuilder("POLYGON ((");
                for (int i = 0; i < coordinates.GetLength(0); i++)
                {
                    if (i > 0)
                        wkt.Append(", ");
                    wkt.Append(coordinates[i, 0] + " " + coordinates[i, 1]);
                }
                wkt.Append("))");
                _viewer.SelectByWkt(wkt.ToString(), -1);
            });
        }

        internal ToolStripButton RefreshMap { get; }

        private void OnRefreshMap(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            _viewer.RefreshMap();
        }

        internal ToolStripButton ToggleTooltips { get; }

        private void OnToggleTooltips(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            _viewer.FeatureTooltipsEnabled = !_viewer.FeatureTooltipsEnabled;
            TooltipStateChanged();
        }

        internal ToolStripButton CopyMap { get; }

        private void OnCopyMap(object sender, EventArgs e)
        {
            if (_viewer == null)
                return;

            _viewer.CopyMap();
        }

        private void TooltipStateChanged()
        {
            if (_viewer == null)
                return;

            if (_viewer.FeatureTooltipsEnabled)
            {
                this.ToggleTooltips.Text = Properties.Resources.TextDisableTooltips;
                this.ToggleTooltips.ToolTipText = Properties.Resources.TooltipDisableTooltips;
            }
            else
            {
                this.ToggleTooltips.Text = Properties.Resources.TextEnableTooltips;
                this.ToggleTooltips.ToolTipText = Properties.Resources.TooltipEnableTooltips;
            }
        }

        private void UpdateButtonCheckedState()
        {
            var at = (_viewer == null) ? MapActiveTool.None : _viewer.ActiveTool;
            this.Pan.Checked = (at == MapActiveTool.Pan);
            this.SelectTool.Checked = (at == MapActiveTool.Select);
            this.ZoomIn.Checked = (at == MapActiveTool.ZoomIn);
            this.ZoomOut.Checked = (at == MapActiveTool.ZoomOut);
        }

        private void OnViewerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_viewer.IsBusy))
            {
                var busy = _viewer.IsBusy;
                this.ZoomExtents.Enabled = this.ZoomIn.Enabled
                                         = this.ZoomOut.Enabled
                                         = this.ClearSelection.Enabled
                                         = this.Pan.Enabled
                                         = this.SelectTool.Enabled
                                         = this.SelectPolygon.Enabled
                                         = this.SelectRadius.Enabled
                                         = this.ToggleTooltips.Enabled
                                         = this.CopyMap.Enabled
                                         = this.RefreshMap.Enabled = !busy;
                this.Loading.Visible = busy;
            }
            else if (e.PropertyName == nameof(_viewer.ActiveTool) || e.PropertyName == nameof(_viewer.DigitizingType))
            {
                UpdateButtonCheckedState();
            }
        }
    }

    /// <summary>
    /// Determines the behaviour of the zoom out command in the <see cref="DefaultToolbar"/>
    /// </summary>
    public enum ZoomOutMode
    {
        /// <summary>
        /// The user must click on the map to zoom out from that selected point
        /// </summary>
        ClickToZoom,

        /// <summary>
        /// The map automatically zooms out on command invocation
        /// </summary>
        AutoZoom
    }
}