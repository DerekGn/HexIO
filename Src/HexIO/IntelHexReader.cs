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

using System;
using System.Collections.Generic;
using System.IO;

namespace HexIO
{
    /// <summary>
    /// A reader capable of reading intel hex file formatted stream
    /// </summary>
    public class IntelHexReader
    {
        private readonly StreamReader _hexFileReader;
        private uint _addressBase = 0;

        /// <summary>
        /// Construct instance of an <see cref="IntelHexReader"/>
        /// </summary>
        /// <param name="stream">The ssource stream of the hex file</param>
        /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="stream"/> is null</exception>
        public IntelHexReader(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            _hexFileReader = new StreamReader(stream);
        }
        
        /// <summary>
        /// Read data with address information from the stream
        /// </summary>
        /// <param name="address">The current address for the block</param>
        /// <param name="data">The data</param>
        /// <returns></returns>
        public bool Read(out uint address, out IList<byte> data)
        {
            bool result = false;
            data = null;
            address = 0;

            var hexLine = _hexFileReader.ReadLine();
            
            if(!string.IsNullOrWhiteSpace(hexLine))
            {
                IntelHexRecord hexRecord = hexLine.ParseHexRecord();

                if (hexRecord.RecordType != IntelHexRecordType.EndOfFile)
                {
                    address = HandleAddress(hexRecord);

                    if(hexRecord.RecordType == IntelHexRecordType.Data)
                        data = hexRecord.Data;

                    result = true;
                }
            }
                        
            return result;
        }

        private uint HandleAddress(IntelHexRecord hexRecord)
        {
            uint result = 0;
            switch (hexRecord.RecordType)
            {
                case IntelHexRecordType.Data:
                    result = _addressBase + hexRecord.Address;
                    break;
                case IntelHexRecordType.ExtendedSegmentAddress:
                    _addressBase = (uint) (hexRecord.Data[0] << 8 | hexRecord.Data[1]) << 4;
                    result = _addressBase;
                    break;
                case IntelHexRecordType.StartSegmentAddress:
                    break;
                case IntelHexRecordType.ExtendedLinearAddress:
                    _addressBase = (uint)(hexRecord.Data[0] << 8 | hexRecord.Data[1]) << 16;
                    result = _addressBase;
                    break;
                case IntelHexRecordType.StartLinearAddress:
                    _addressBase = (uint)(hexRecord.Data[0] << 24 | hexRecord.Data[1] << 16 | hexRecord.Data[2] << 8 | hexRecord.Data[3]);
                    result = _addressBase;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown value read for [{nameof(hexRecord.RecordType)}]");
            }

            return result;
        }
    }
}
