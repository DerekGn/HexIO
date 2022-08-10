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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace HexIO
{
    /// <summary>
    /// An Intel Hex stream writer
    /// </summary>
    public class IntelHexStreamWriter : StreamWriter, IIntelHexStreamWriter
    {
        private const int MaxDataLength = 0xFF;

        [ExcludeFromCodeCoverage]
        public IntelHexStreamWriter(Stream stream) : base(stream)
        {
        }

        [ExcludeFromCodeCoverage]
        public IntelHexStreamWriter(string path) : base(path)
        {
        }

        [ExcludeFromCodeCoverage]
        public IntelHexStreamWriter(Stream stream, Encoding encoding) : base(stream, encoding)
        {
        }

        [ExcludeFromCodeCoverage]
        public IntelHexStreamWriter(string path, bool append) : base(path, append)
        {
        }

        [ExcludeFromCodeCoverage]
        public IntelHexStreamWriter(Stream stream, Encoding encoding, int bufferSize) : base(stream, encoding, bufferSize)
        {
        }

        [ExcludeFromCodeCoverage]
        public IntelHexStreamWriter(string path, bool append, Encoding encoding) : base(path, append, encoding)
        {
        }

        [ExcludeFromCodeCoverage]
        public IntelHexStreamWriter(Stream stream, Encoding encoding, int bufferSize, bool leaveOpen) : base(stream, encoding, bufferSize, leaveOpen)
        {
        }

        [ExcludeFromCodeCoverage]
        public IntelHexStreamWriter(string path, bool append, Encoding encoding, int bufferSize) : base(path, append, encoding, bufferSize)
        {
        }

        /// <inheritdoc/>
        public override void Close()
        {
            WriteLine(":00000001FF");

            base.Close();
        }

        /// <inheritdoc/>
        public void WriteDataRecord(ushort offset, IList<byte> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Count > MaxDataLength)
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"Must be less than [{MaxDataLength:X}]");
            }

            WriteHexRecord(IntelHexRecordType.Data, offset, data);
        }

        /// <inheritdoc/>
        public void WriteExtendedLinearAddressRecord(ushort address)
        {
            var addressBytes = BitConverter.GetBytes(address).Reverse().ToList();

            WriteHexRecord(IntelHexRecordType.ExtendedLinearAddress, 0, addressBytes);
        }

        /// <inheritdoc/>
        public void WriteExtendedSegmentAddressRecord(ushort address)
        {
            var addressBytes = BitConverter.GetBytes(address).Reverse().ToList();

            WriteHexRecord(IntelHexRecordType.ExtendedSegmentAddress, 0, addressBytes);
        }

        /// <inheritdoc/>
        public void WriteStartLinearAddressRecord(uint address)
        {
            var addressBytes = BitConverter.GetBytes(address).Reverse().ToList();

            WriteHexRecord(IntelHexRecordType.StartLinearAddress, 0, addressBytes);
        }

        /// <inheritdoc/>
        public void WriteStartSegmentAddressRecord(ushort cs, ushort ip)
        {
            var addressBytes = BitConverter.GetBytes(cs).Reverse().ToList();
            addressBytes.AddRange(BitConverter.GetBytes(ip).Reverse().ToList());

            WriteHexRecord(IntelHexRecordType.StartSegmentAddress, 0, addressBytes);
        }

        private void WriteHexRecord(IntelHexRecordType recordType, ushort offset, IList<byte> data)
        {
            var hexRecord = new IntelHexRecord(offset, recordType, data);

            WriteLine(hexRecord.ToHexRecordString());
        }
    }
}
