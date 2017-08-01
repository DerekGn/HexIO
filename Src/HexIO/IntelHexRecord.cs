/**
* MIT License
*
* Copyright (c) 2017 Derek Goslin < http://corememorydump.blogspot.ie/ >
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

using System.Collections.Generic;

namespace HexIO
{
    /// <summary>
    /// A representation of a hex record line
    /// </summary>
    internal class IntelHexRecord
    {
        /// <summary>
        /// The record type
        /// </summary>
        public IntelHexRecordType RecordType { get; set; }
        /// <summary>
        /// The number of bytes in the record
        /// </summary>
        public int ByteCount { get; set; }
        /// <summary>
        /// The address of the record
        /// </summary>
        public uint Address { get; set; }
        /// <summary>
        /// The data from the record
        /// </summary>
        public List<byte> Data { get; set; }
        /// <summary>
        /// The record checksum
        /// </summary>
        public int CheckSum { get; set; }
    }
}