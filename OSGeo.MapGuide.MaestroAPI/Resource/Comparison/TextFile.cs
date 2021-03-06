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

// Original code by Michael Potter, made available under Public Domain
//
// http://www.codeproject.com/Articles/6943/A-Generic-Reusable-Diff-Algorithm-in-C-II/

#endregion Disclaimer / License

using System;
using System.Collections.Generic;
using System.IO;

namespace OSGeo.MapGuide.MaestroAPI.Resource.Comparison
{
    /// <summary>
    /// Represents a line of text in a diff
    /// </summary>
    public class TextLine : IComparable
    {
        /// <summary>
        /// The line content
        /// </summary>
        public string Line;

        internal int _hash;

        internal TextLine(string str)
        {
            Line = str.Replace("\t", "    "); //NOXLATE
            _hash = str.GetHashCode();
        }

        #region IComparable Members

        /// <summary>
        /// Compares this instance against the given instance
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return _hash.CompareTo(((TextLine)obj)._hash);
        }

        #endregion IComparable Members
    }

    /// <summary>
    /// Represents a list of differences
    /// </summary>
    public class TextFileDiffList : IDiffList
    {
        private readonly List<TextLine> _lines;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="fileName"></param>
        public TextFileDiffList(string fileName)
            : this(fileName, false)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="deleteFile"></param>
        public TextFileDiffList(string fileName, bool deleteFile)
        {
            _lines = new List<TextLine>();
            using (StreamReader sr = new StreamReader(fileName))
            {
                String line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    _lines.Add(new TextLine(line));
                }
            }
            if (deleteFile)
            {
                try
                {
                    File.Delete(fileName);
                }
                catch { }
            }
        }

        #region IDiffList Members

        /// <summary>
        /// Gets the number of lines
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _lines.Count;
        }

        /// <summary>
        /// Gets the line at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IComparable GetByIndex(int index)
        {
            return _lines[index];
        }

        #endregion IDiffList Members
    }
}