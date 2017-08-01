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
        private Address _currentAddress;

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
        /// <param name="address"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Read(out Address address, out IList<byte> data)
        {
            bool result = false;

            var hexLine = _hexFileReader.ReadLine();
            
            if(!string.IsNullOrWhiteSpace(hexLine))
            {
                IntelHexRecord hexRecord = hexLine.ParseHexRecord();

                HandleAddress(hexRecord);

                if (hexRecord.RecordType != IntelHexRecordType.EndOfFile)
                    result = true;
            }

            address = _currentAddress;
            data = null;

            return result;
        }

        private void HandleAddress(IntelHexRecord hexRecord)
        {
            switch (hexRecord.RecordType)
            {
                case IntelHexRecordType.Data:
                    _currentAddress = new LinearAddress { Value = hexRecord.Address };
                    break;
                case IntelHexRecordType.EndOfFile:
                    break;
                case IntelHexRecordType.ExtendedSegmentAddress:
                    break;
                case IntelHexRecordType.StartSegmentAddress:
                    break;
                case IntelHexRecordType.ExtendedLinearAddress:
                    break;
                case IntelHexRecordType.StartLinearAddress:
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown value read for [{nameof(hexRecord.RecordType)}]");
            }
        }
    }
}
