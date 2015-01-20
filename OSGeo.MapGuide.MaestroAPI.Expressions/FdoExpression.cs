﻿#region Disclaimer / License

// Copyright (C) 2015, Jackie Ng
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
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSGeo.MapGuide.MaestroAPI.Expressions
{
    public enum ExpressionType
    {
        UnaryExpression,
        BinaryExpression,
        Function,
        Identifier,
        Parameter,
        GeometryValue,
        BooleanValue,
        StringValue,
        Int32Value,
        DoubleValue,
        DateTimeValue
    }

    public abstract class FdoExpression : FdoParseable
    {
        public abstract ExpressionType ExpressionType { get; }

        public static FdoExpression Parse(string str)
        {
            Parser p = new Parser(new FdoExpressionGrammar());
            var tree = p.Parse(str);
            CheckParserErrors(tree);
            if (tree.Root.Term.Name == FdoTerminalNames.Expression)
            {
                var child = tree.Root.ChildNodes[0];
                return ParseNode(child);
            }
            else
            {
                throw new FdoParseException();
            }
        }

        private static void CheckParserErrors(ParseTree tree)
        {
            if (tree.HasErrors())
            {
                List<FdoParseErrorMessage> errors = new List<FdoParseErrorMessage>();
                foreach (var msg in tree.ParserMessages)
                {
                    if (msg.Level == Irony.ErrorLevel.Error)
                    {
                        errors.Add(new FdoParseErrorMessage(msg.Message, msg.Location.Line, msg.Location.Column));
                    }
                }
                throw new FdoMalformedExpressionException(Strings.ParserErrorMessage, errors);
            }
        }

        internal static FdoExpression ParseNode(ParseTreeNode child)
        {
            if (child.Term.Name == FdoTerminalNames.Expression)
            {
                return ParseNode(child.ChildNodes[0]);
            }
            else
            {
                switch (child.Term.Name)
                {
                    case FdoTerminalNames.UnaryExpression:
                        return new FdoUnaryExpression(child);
                    case FdoTerminalNames.BinaryExpression:
                        return new FdoBinaryExpression(child);
                    case FdoTerminalNames.Function:
                        return new FdoFunction(child);
                    case FdoTerminalNames.Identifier:
                        return new FdoIdentifier(child);
                    case FdoTerminalNames.ValueExpression:
                        return FdoValueExpression.ParseValueNode(child);
                    default:
                        throw new FdoParseException("Unknown terminal: " + child.Term.Name);
                }
            }
        }
    }
}