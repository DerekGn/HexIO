/*
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
using System.Collections.Generic;

namespace HexIO.Transforms
{
    /// <summary>
    /// Perform a modification of an <see cref="IntelHexRecord"/>
    /// </summary>
    public class ModificationTransform : Transform
    {
        /// <summary>
        /// Create an instance of a <see cref="ModificationTransform"/>
        /// </summary>
        /// <param name="match">The <see cref="IntelHexRecordMatch"/> instance to match <see cref="IntelHexRecord"/> instances against</param>
        /// <param name="offset">The offset value to apply, set to null to leave unmodified</param>
        /// <param name="recordType"></param>
        /// <param name="data"></param>
        public ModificationTransform(
            IntelHexRecordMatch match,
            ushort? offset,
            IntelHexRecordType? recordType,
            List<byte> data) : base(match)
        {
            Offset = offset;
            RecordType = recordType;
            Data = data;
        }

        /// <summary>
        /// The data from the record
        /// </summary>
        public List<byte> Data { get; set; }

        /// <summary>
        /// The load offset of the record
        /// </summary>
        public ushort? Offset { get; set; }

        /// <summary>
        /// The record type
        /// </summary>
        public IntelHexRecordType? RecordType { get; set; }

        internal IntelHexRecord Apply(IntelHexRecord intelHexRecord)
        {
            var modifiedRecord = new IntelHexRecord
                (
                    Offset ?? intelHexRecord.Offset,
                    RecordType ?? intelHexRecord.RecordType,
                    Data ?? intelHexRecord.Data
                );

            return modifiedRecord;
        }
    }
}