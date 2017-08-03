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
using System.Linq;

namespace HexIO
{
    /// <summary>
    ///     A reader capable of writing a intel hex file stream
    /// </summary>
    public class IntelHexWriter : IDisposable
    {
        private readonly StreamWriter _streamWriter;

        /// <summary>
        ///     Construct instance of an <see cref="IntelHexWriter" />
        /// </summary>
        /// <param name="stream">The target stream of the hex file</param>
        /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="stream" /> is null</exception>
        public IntelHexWriter(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            _streamWriter = new StreamWriter(stream);
        }

        /// <summary>
        ///     Write an address record to the underlying stream
        /// </summary>
        /// <param name="addressType">The <see cref="AddressType" /> to write to the stream</param>
        /// <param name="address">The address value to write data</param>
        /// <remarks>The </remarks>
        public void WriteAddress(AddressType addressType, uint address)
        {
            if (!Enum.IsDefined(typeof(AddressType), addressType))
                throw new ArgumentOutOfRangeException(nameof(addressType),
                    $"Value [{addressType}] in not a value of [{nameof(AddressType)}]");

            if (addressType == AddressType.ExtendedSegmentAddress && address > 0x100000)
                throw new ArgumentOutOfRangeException(nameof(address), "Value must be less than 0x10000");

            var addressData = FormatAddress(addressType, address);

            WriteHexRecord((IntelHexRecordType) addressType, 0, addressData);
        }

        public void WriteData(ushort address, IList<byte> data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Count > 0xFF)
                throw new ArgumentOutOfRangeException(nameof(data), "Must be less than 255");

            WriteHexRecord(IntelHexRecordType.Data, address, data);
        }

        public void Close()
        {
            if (!_disposedValue)
            {
                _streamWriter?.WriteLine(":00000001FF");
                _streamWriter?.Flush();
            }
        }

        private static List<byte> FormatAddress(AddressType addressType, uint address)
        {
            var result = new List<byte>();
            var shift = (byte) (addressType == AddressType.ExtendedSegmentAddress ? 4 : 0);
            shift = (byte) (addressType == AddressType.ExtendedLinearAddress ? 16 : shift);

            var addressBytes = BitConverter.GetBytes(address >> shift);

            if (addressType == AddressType.StartLinearAddress)
            {
                result.Add(addressBytes[3]);
                result.Add(addressBytes[2]);
            }

            result.Add(addressBytes[1]);
            result.Add(addressBytes[0]);
            
            return result;
        }

        private static byte CalculateCrc(IList<byte> checkSumData)
        {
            var maskedSumBytes = checkSumData.Sum(x => x) & 0xff;
            var calculatedChecksum = (byte) (256 - maskedSumBytes);

            return calculatedChecksum;
        }

        private void WriteHexRecord(IntelHexRecordType recordType, ushort address, IList<byte> data)
        {
            var hexRecordData = new List<byte>();
            hexRecordData.Add((byte) data.Count);
            hexRecordData.AddRange(BitConverter.GetBytes(address));
            hexRecordData.Add((byte) recordType);
            hexRecordData.AddRange(data);
            var checksum = CalculateCrc(hexRecordData);
            hexRecordData.Add(checksum);

            var hexRecord = $":{hexRecordData.ToHexString()}";

            _streamWriter.WriteLine(hexRecord);
        }

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Close();
                    _streamWriter?.Dispose();
                }

                _disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}