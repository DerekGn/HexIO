/*
* MIT License
*
* Copyright (c) 2017 Derek Goslin http://corememorydump.blogspot.ie/
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
    ///     A reader capable of reading intel hex file formatted stream
    /// </summary>
    public class IntelHexReader : IDisposable
    {
        private readonly StreamReader _streamReader;
        private uint _addressBase;

        /// <summary>
        ///     Construct instance of an <see cref="IntelHexReader" />
        /// </summary>
        /// <param name="stream">The source stream of the hex file</param>
        /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="stream" /> is null</exception>
        public IntelHexReader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            _streamReader = new StreamReader(stream);
        }

        /// <summary>
        ///     Read data with address information from the stream
        /// </summary>
        /// <param name="address">The start address for the block of data</param>
        /// <param name="data">The data byte block</param>
        /// <returns>true if there are more records available or false of end of file</returns>
        /// <remarks>
        ///     An address value may be read without a corresponding list data bytes.
        ///     This occurs for record types 02, 04, 05
        /// </remarks>
        public bool Read(out uint address, out IList<byte> data)
        {
            var result = false;
            data = new List<byte>();
            address = 0;

            var hexLine = _streamReader.ReadLine();

            if (!string.IsNullOrWhiteSpace(hexLine))
            {
                var hexRecord = hexLine.ParseHexRecord();

                if (hexRecord.RecordType != IntelHexRecordType.EndOfFile)
                {
                    address = HandleAddress(hexRecord);

                    if (hexRecord.RecordType == IntelHexRecordType.Data)
                        data = hexRecord.Data;

                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        ///     Read a <see cref="IntelHexRecord"/> from the stream
        /// </summary>
        /// <returns>true if there are more records available or false of end of file</returns>
        public bool Read(out IntelHexRecord hexRecord)
        {
            return Read(out hexRecord, out _);
        }

        /// <summary>
        ///     Read a <see cref="IntelHexRecord"/> from the stream
        /// </summary>
        /// <param name="hexRecord">The hex record.</param>
        /// <param name="address">The start address for the record.</param>
        /// <returns>true if there are more records available or false of end of file</returns>
        public bool Read(out IntelHexRecord hexRecord, out uint address)
        {
            var result = false;
            hexRecord = null;
            address = 0;

            var hexLine = _streamReader.ReadLine();

            if (!string.IsNullOrWhiteSpace(hexLine))
            {
                hexRecord = hexLine.ParseHexRecord();

                if(hexRecord.RecordType != IntelHexRecordType.EndOfFile) 
                { 
                    address = HandleAddress(hexRecord); 
                }
                
                result = true;
            }

            return result;
        }

        private uint HandleAddress(IntelHexRecord hexRecord)
        {
            uint result;
            switch (hexRecord.RecordType)
            {
                case IntelHexRecordType.Data:
                    result = _addressBase + hexRecord.Address;
                    break;
                case IntelHexRecordType.ExtendedSegmentAddress:
                    _addressBase = (uint) ((hexRecord.Data[0] << 8) | hexRecord.Data[1]) << 4;
                    result = _addressBase;
                    break;
                case IntelHexRecordType.StartSegmentAddress:
                    result = _addressBase;
                    break;
                case IntelHexRecordType.ExtendedLinearAddress:
                    _addressBase = (uint) ((hexRecord.Data[0] << 8) | hexRecord.Data[1]) << 16;
                    result = _addressBase;
                    break;
                case IntelHexRecordType.StartLinearAddress:
                    _addressBase = (uint) ((hexRecord.Data[0] << 24) | (hexRecord.Data[1] << 16) |
                                           (hexRecord.Data[2] << 8) | hexRecord.Data[3]);
                    result = _addressBase;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown value read for [{nameof(hexRecord.RecordType)}]");
            }

            return result;
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        /// <summary>
        /// Dispose the <see cref="IntelHexReader"/>
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _streamReader?.Dispose();
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Dispose the <see cref="IntelHexReader"/>
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}