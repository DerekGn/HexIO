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
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace HexIO
{
    /// <summary>
    /// A reader capable of writing a intel hex file stream
    /// </summary>
    public class IntelHexWriter : IDisposable
    {
        private StreamWriter _streamWriter;

        /// <summary>
        /// Construct instance of an <see cref="IntelHexWriter"/>
        /// </summary>
        /// <param name="stream">The target stream of the hex file</param>
        /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="stream"/> is null</exception>
        public IntelHexWriter(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            _streamWriter = new StreamWriter(stream);
        }

        /// <summary>
        /// Write an address record to the underlying stream
        /// </summary>
        /// <param name="addressType">The <see cref="AddressType"/> to write to the stream</param>
        /// <param name="address">The address value to write data</param>
        /// <remarks>The </remarks>
        public void WriteAddress(AddressType addressType, uint address)
        {
            if (!Enum.IsDefined(typeof(AddressType), addressType))
                throw new ArgumentOutOfRangeException(nameof(addressType), $"Value [{addressType}] in not a value of [{nameof(AddressType)}]");

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
                throw new ArgumentOutOfRangeException(nameof(data), "must be less than 255");

            WriteHexRecord(IntelHexRecordType.Data, address, data);
        }
        
        public void Close()
        {
            _streamWriter?.WriteLine(":00000001FF");
            _streamWriter?.Flush();
        }

        private List<byte> FormatAddress(AddressType addressType, uint address)
        {
            List<byte> result = new List<byte>();
            byte shift = (byte) (addressType == AddressType.ExtendedSegmentAddress ? 4 : 0);
            shift = (byte)(addressType == AddressType.ExtendedLinearAddress ? 16 : 0);
            
            byte[] addressBytes = BitConverter.GetBytes(address >> shift);

            result.Add(addressBytes[0]);
            result.Add(addressBytes[1]);
            if (addressType == AddressType.StartLinearAddress)
            {
                result.Add(addressBytes[2]);
                result.Add(addressBytes[3]);
            }
            
            return result;
        }

        private byte CalculateCrc(IList<byte> checkSumData)
        {
            var maskedSumBytes = checkSumData.Sum(x => x) & 0xff;
            var calculatedChecksum = (byte)(256 - maskedSumBytes);

            return calculatedChecksum;
        }

        private void WriteHexRecord(IntelHexRecordType recordType, ushort address, IList<byte> data)
        {
            List<byte> hexRecordData = new List<byte>();
            hexRecordData.Add((byte)data.Count);
            hexRecordData.AddRange(BitConverter.GetBytes(address));
            hexRecordData.Add((byte)recordType);
            hexRecordData.AddRange(data);
            var checksum = CalculateCrc(hexRecordData);
            hexRecordData.Add(checksum);

            var hexRecord = $":{hexRecordData.ToHexString()}";

            _streamWriter.Write(hexRecord);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Close();
                    _streamWriter?.Close();
                    _streamWriter?.Dispose();
                }
                
                disposedValue = true;
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
