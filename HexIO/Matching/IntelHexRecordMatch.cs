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

using System.Collections.Generic;

namespace HexIO.Matching
{
    /// <summary>
    /// A match specifier
    /// </summary>
    public class IntelHexRecordMatch
    {
        private int? _recordLength;
        private IntelHexRecordType? _recordType;
        private IList<byte>? _data;
        private ushort? _offset;

        /// <summary>
        /// Create an instance of a <see cref="IntelHexRecordMatch"/>
        /// </summary>
        public IntelHexRecordMatch()
        {
            ExpectedMatchResult = IntelHexRecordMatchResult.None;
        }

        /// <summary>
        /// The data to match
        /// </summary>
        public IList<byte>? Data
        {
            get => _data;
            set
            {
                ExpectedMatchResult |= IntelHexRecordMatchResult.DataMatch;

                _data = value;
            }
        }

        /// <summary>
        /// The load offset to match
        /// </summary>
        public ushort? Offset
        {
            get => _offset;
            set
            {
                ExpectedMatchResult |= IntelHexRecordMatchResult.OffsetMatch;

                _offset = value;
            }
        }

        /// <summary>
        /// The number of bytes to match
        /// </summary>
        public int? RecordLength
        {
            get => _recordLength;
            set
            {
                ExpectedMatchResult |= IntelHexRecordMatchResult.RecordLengthMatch;

                _recordLength = value;
            }
        }

        /// <summary>
        /// The record type to match
        /// </summary>
        public IntelHexRecordType? RecordType
        {
            get => _recordType;
            set
            {
                ExpectedMatchResult |= IntelHexRecordMatchResult.RecordTypeMatch;

                _recordType = value;
            }
        }

        /// <summary>
        /// The number of match fields
        /// </summary>
        internal IntelHexRecordMatchResult ExpectedMatchResult { get; private set; }
    }
}