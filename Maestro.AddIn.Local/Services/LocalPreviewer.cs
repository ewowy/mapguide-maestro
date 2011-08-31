﻿#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using Maestro.Base.Services;
using OSGeo.MapGuide.ObjectModels.MapDefinition;
using OSGeo.MapGuide.ObjectModels;
using OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide;
using Maestro.AddIn.Local.UI;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using OSGeo.MapGuide.ObjectModels.LayerDefinition;

namespace Maestro.AddIn.Local.Services
{
    public class LocalPreviewer : IResourcePreviewer
    {
        public bool IsPreviewable(OSGeo.MapGuide.MaestroAPI.Resource.IResource res)
        {
            var rt = res.ResourceType;
            return (rt == ResourceTypes.LayerDefinition ||
                    rt == ResourceTypes.MapDefinition ||
                    rt == ResourceTypes.WatermarkDefinition);
        }

        public void Preview(OSGeo.MapGuide.MaestroAPI.Resource.IResource res, Maestro.Editors.IEditorService edSvc)
        {
            IMapDefinition mapDef = null;
            var conn = res.CurrentConnection;

            if (res.ResourceType == ResourceTypes.LayerDefinition)
            {
                var ldf = (ILayerDefinition)res;
                string wkt;
                var env = ldf.GetSpatialExtent(true, out wkt);
                mapDef = ObjectFactory.CreateMapDefinition(conn, "Preview");
                mapDef.CoordinateSystem = wkt;
                mapDef.SetExtents(env.MinX, env.MinY, env.MaxX, env.MaxY);
                string resId = "Session:" + edSvc.SessionID + "//" + Guid.NewGuid() + "." + res.ResourceType.ToString();
                edSvc.ResourceService.SetResourceXmlData(resId, ResourceTypeRegistry.Serialize(res));
                mapDef.AddLayer(null, "PreviewLayer", resId);
            }
            else if (res.ResourceType == ResourceTypes.MapDefinition)
            {
                mapDef = (IMapDefinition)res;
            }
            else if (res.ResourceType == ResourceTypes.WatermarkDefinition)
            {
                string resId = "Session:" + edSvc.SessionID + "//" + Guid.NewGuid() + "." + res.ResourceType.ToString();
                edSvc.ResourceService.SetResourceXmlData(resId, ResourceTypeRegistry.Serialize(res));

                var wm = ((IWatermarkDefinition)res).CreateInstance();
                wm.ResourceId = resId;
                var csFact = new MgCoordinateSystemFactory();
                var arbXY = csFact.ConvertCoordinateSystemCodeToWkt("XY-M");
                mapDef = ObjectFactory.CreateMapDefinition(conn, new Version(2, 3, 0), "Preview");
                mapDef.CoordinateSystem = arbXY;
                mapDef.SetExtents(-100000, -100000, 100000, 100000);
                ((IMapDefinition2)mapDef).AddWatermark(wm);
            }

            var mapResId = new MgResourceIdentifier("Session:" + edSvc.SessionID + "//" + Guid.NewGuid() + "." + res.ResourceType.ToString());
            edSvc.ResourceService.SetResourceXmlData(mapResId.ToString(), ResourceTypeRegistry.Serialize(mapDef));

            MgdMap map = new MgdMap(mapResId);

            var diag = new MapPreviewWindow(map, conn);
            diag.ShowDialog();
        }
    }
}