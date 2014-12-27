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

#endregion Disclaimer / License

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSGeo.MapGuide.ObjectModels.Capabilities
{
    public interface IFdoCommandCapabilities
    {
        string[] SupportedCommands { get; }

        bool SupportsParameters { get; }

        bool SupportsTimeout { get; }
    }

    public interface IFdoConnectionCapabilities
    {
        string[] SupportedSpatialContextExtentTypes { get; }

        bool SupportsConfiguration { get; }

        bool SupportsLocking { get; }

        bool SupportsLongTransactions { get; }

        bool SupportsSQL { get; }

        bool SupportsTimeout { get; }

        bool SupportsTransactions { get; }

        string ThreadCapability { get; }
    }

    public interface IFdoExpressionCapabilities
    {
        IFdoFunctionDefintion[] SupportedFunctions { get; }

        string[] ExpressionTypes { get; }
    }

    public interface IFdoFunctionArgumentDefinition
    {
        string Name { get; }

        string Description { get; }
    }

    public interface IFdoFunctionDefintionSignature
    {
        IFdoFunctionArgumentDefinition[] Arguments { get; }

        string ReturnType { get; }
    }

    public interface IFdoFunctionDefintion
    {
        string Name { get; }

        string Description { get; }

        IFdoFunctionDefintionSignature[] Signatures { get; }
    }

    public interface IFdoFilterCapabilities
    {
        string[] ConditionTypes { get; }

        string[] DistanceOperations { get; }

        string[] SpatialOperations { get; }

        bool SupportsGeodesicDistance { get; }

        bool SupportsNonLiteralGeometricOperations { get; }
    }

    public interface IFdoGeometryCapabilities
    {
        string[] SupportedComponentTypes { get; }

        string Dimensionality { get; }

        string[] SupportedGeometryTypes { get; }
    }

    public interface IFdoRasterCapabilities
    {
        bool SupportsRaster { get; }

        bool SupportsStitching { get; }

        bool SupportsSubsampling { get; }
    }

    public interface IFdoSchemaCapabilities
    {
        string[] SupportedClassTypes { get; }

        string[] SupportedDataTypes { get; }

        bool SupportsAssociationProperties { get; }

        bool SupportsInheritance { get; }

        bool SupportsMultipleSchemas { get; }

        bool SupportsNetworkModel { get; }

        bool SupportsObjectProperties { get; }

        bool SupportsSchemaOverrides { get; }
    }

    public interface IFdoProviderCapabilities
    {
        IFdoCommandCapabilities Command { get; }

        IFdoConnectionCapabilities Connection { get; }

        IFdoExpressionCapabilities Expression { get; }

        IFdoFilterCapabilities Filter { get; }

        IFdoGeometryCapabilities Geometry { get; }

        IFdoRasterCapabilities Raster { get; }

        IFdoSchemaCapabilities Schema { get; }
    }
}