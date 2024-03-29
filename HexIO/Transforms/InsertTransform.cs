﻿/*
* MIT License
*
* Copyright (c) 2022 Derek Goslin https://github.com/DerekGn
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

using HexIO.Matching;
using System;

namespace HexIO.Transforms
{
    /// <summary>
    /// Perform an insert of an <see cref="IntelHexRecord"/>
    /// </summary>
    public class InsertTransform : Transform 
    {
        /// <summary>
        /// Create an instance of a <see cref="InsertTransform"/>
        /// </summary>
        /// <param name="match">The <see cref="IntelHexRecordMatch"/> to match the transform</param>
        /// <param name="position">The <see cref="InsertPosition"/> to insert the <paramref name="record"/></param>
        /// <param name="record">The <see cref="IntelHexRecord"/> to insert</param>
        public InsertTransform(
            IntelHexRecordMatch match,
            InsertPosition position,
            IntelHexRecord record) : base(match)
        {
            Position = position;
            Record = record ?? throw new ArgumentNullException(nameof(record));
        }

        /// <summary>
        /// The insert position of the <see cref="IntelHexRecord"/>
        /// </summary>
        public InsertPosition Position { get; }

        /// <summary>
        /// The <see cref="IntelHexRecord"/> to insert
        /// </summary>
        public IntelHexRecord Record { get; }
    }
}
