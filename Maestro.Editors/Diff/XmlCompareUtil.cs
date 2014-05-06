﻿#region Disclaimer / License
// Copyright (C) 2014, Jackie Ng
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
using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.MaestroAPI.Resource;
using OSGeo.MapGuide.MaestroAPI.Resource.Comparison;
using OSGeo.MapGuide.MaestroAPI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Maestro.Editors.Diff
{
    public class XmlComparisonSet
    {
        internal XmlComparisonSet(TextFileDiffList source, TextFileDiffList target)
        {
            this.Source = source;
            this.Target = target;
        }

        public TextFileDiffList Source { get; private set; }

        public TextFileDiffList Target { get; private set; }
    }

    public class XmlCompareUtil
    {
        /// <summary>
        /// Prepares the source and target resource content for XML comparison
        /// </summary>
        /// <param name="resSvc"></param>
        /// <param name="sourceId"></param>
        /// <param name="targetId"></param>
        public static XmlComparisonSet PrepareForComparison(IResourceService resSvc, string sourceId, string targetId)
        {
            //Route both source and target XML content through
            //XmlDocument objects to ensure issues like whitespacing do
            //not throw us off
            var sourceFile = Path.GetTempFileName();
            var targetFile = Path.GetTempFileName();

            IResource source = resSvc.GetResource(sourceId);
            IResource target = resSvc.GetResource(targetId);

            var sourceDoc = new XmlDocument();
            var targetDoc = new XmlDocument();

            using (var sourceStream = ResourceTypeRegistry.Serialize(source))
            using (var targetStream = ResourceTypeRegistry.Serialize(target))
            {
                sourceDoc.Load(sourceStream);
                targetDoc.Load(targetStream);

                sourceDoc.Normalize();
                targetDoc.Normalize();

                using (var fs = File.OpenWrite(sourceFile))
                using (var ft = File.OpenWrite(targetFile))
                {
                    sourceDoc.Save(fs);
                    targetDoc.Save(ft);
                }

                return new XmlComparisonSet(
                    new TextFileDiffList(sourceFile, true),
                    new TextFileDiffList(targetFile, true));
            }
        }
    }
}
