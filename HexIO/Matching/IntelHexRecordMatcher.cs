﻿/*
* MIT License
*
* Copyright (c) 2022 Derek Goslin http://corememorydump.blogspot.ie/
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

using System;
using System.Linq;

namespace HexIO.Matching
{
    /// <summary>
    /// An Intel hex record matcher 
    /// </summary>
    public class IntelHexRecordMatcher : IIntelHexRecordMatcher
    {
        /// <inheritdoc/>
        public bool IsMatch(IntelHexRecordMatch match, IntelHexRecord record)
        {
            ArgumentNullException.ThrowIfNull(match);

            ArgumentNullException.ThrowIfNull(record);

            IntelHexRecordMatchResult result = default;

            if (match.ExpectedMatchResult != IntelHexRecordMatchResult.None)
            {
                if (match.Data != null && match.Data.SequenceEqual(record.Data))
                {
                    result |= IntelHexRecordMatchResult.DataMatch;
                }

                if (match.RecordType is IntelHexRecordType recordType && recordType == record.RecordType)
                {
                    result |= IntelHexRecordMatchResult.RecordTypeMatch;
                }

                if (match.Offset is ushort offset && offset == record.Offset)
                {
                    result |= IntelHexRecordMatchResult.OffsetMatch;
                }

                if (match.RecordLength is int recordLength && recordLength == record.RecordLength)
                {
                    result |= IntelHexRecordMatchResult.RecordLengthMatch;
                }

                return match.ExpectedMatchResult == result;
            }
            else
            {
                return false;
            }
        }
    }
}
