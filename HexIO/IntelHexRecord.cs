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

using HexIO.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace HexIO
{
    /// <summary>
    /// A representation of a hex record line
    /// </summary>
    public class IntelHexRecord
    {
        private const int HeaderSize = 6;
        private const int MaximumDataSize = 0xFF;
        private const int MaximumRecordSize = MaximumDataSize + HeaderSize;
        private const int MinimumRecordSize = HeaderSize;
        private const int OffsetIndex = 2;
        private const int RecordLengthIndex = 1;
        private const int RecordMarkIndex = 0;
        private const int RecordTypeIndex = 4;

        /// <summary>
        /// Create an instance of a <see cref="IntelHexRecord"/> from a <see cref="IList{T}"/> of bytes
        /// </summary>
        /// <param name="hexRecordBytes"></param>
        public IntelHexRecord(IList<byte> hexRecordBytes)
        {
            if (hexRecordBytes == null)
            {
                throw new ArgumentNullException(nameof(hexRecordBytes));
            }

            if (hexRecordBytes.Count > MaximumRecordSize)
            {
                throw new ArgumentOutOfRangeException(nameof(hexRecordBytes), $"Must be maximum of [0x{MaximumRecordSize:X}] bytes");
            }

            if (hexRecordBytes.Count < MinimumRecordSize)
            {
                throw new ArgumentOutOfRangeException(nameof(hexRecordBytes), $"Must be minimum of [0x{MinimumRecordSize:X}] bytes");
            }

            if (hexRecordBytes[RecordMarkIndex] != ':')
            {
                throw new IntelHexStreamException($"Illegal line start character");
            }

            if (hexRecordBytes.Count != hexRecordBytes[RecordLengthIndex] + HeaderSize)
            {
                throw new IntelHexStreamException($"Hex record bytes does not have required length of [0x{hexRecordBytes[RecordLengthIndex] + HeaderSize:X4}]");
            }

            if (!Enum.IsDefined(typeof(IntelHexRecordType), (int)hexRecordBytes[RecordTypeIndex]))
            {
                throw new IntelHexStreamException($"Invalid record type value: [0x{hexRecordBytes[RecordTypeIndex]:X2}]");
            }

            RecordType = (IntelHexRecordType)hexRecordBytes[RecordTypeIndex];

            CheckSum = hexRecordBytes.Last();

            byte calculatedChecksum = CalculateChecksum(hexRecordBytes.Skip(1).Take(hexRecordBytes.Count - 2).ToList());

            if (calculatedChecksum != CheckSum)
            {
                throw new IntelHexStreamException($"Checksum incorrect. Expected [0x{CheckSum:X}] Actual: [0x{calculatedChecksum:X}]");
            }

            RecordLength = hexRecordBytes[RecordLengthIndex];

            Offset = (ushort)((hexRecordBytes[OffsetIndex] << 8) + hexRecordBytes[OffsetIndex + 1]);

            Data = hexRecordBytes.Skip(HeaderSize - 1).Take(RecordLength).ToList();

            Bytes = hexRecordBytes.ToList();
        }

        /// <summary>
        /// Construct an instance of a <see cref="IntelHexRecord"/>
        /// </summary>
        /// <param name="offset">The offset of the record</param>
        /// <param name="recordType">The <see cref="IntelHexRecordType"/></param>
        /// <param name="data">The <see cref="IList{T}"/> of <see cref="Byte"/></param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="data"/> length is greater than <see cref="MaximumDataSize"/></exception>
        public IntelHexRecord(ushort offset, IntelHexRecordType recordType, IList<byte> data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Count > MaximumDataSize)
            {
                throw new ArgumentOutOfRangeException(nameof(data), $"Must be maximum of [0x{MaximumDataSize:X}] bytes");
            }

            Bytes = new List<byte>
            {
                (byte)':',
                (byte)data.Count,
                BitConverter.GetBytes(offset).Last(),
                BitConverter.GetBytes(offset).First(),
                (byte)recordType
            };

            Bytes.AddRange(data);

            Bytes.Add(CalculateChecksum(Bytes.Skip(1).ToList()));

            Offset = offset;
            RecordType = recordType;
            Data = data.ToList();
            RecordLength = Data.Count;
        }

        /// <summary>
        /// The record bytes for the <see cref="IntelHexRecord"/> read from the stream
        /// </summary>
        public List<Byte> Bytes { get; }

        /// <summary>
        /// The record checksum
        /// </summary>
        public int CheckSum { get; }

        /// <summary>
        /// The data from the record
        /// </summary>
        public List<byte> Data { get; }

        /// <summary>
        /// The load offset of the record
        /// </summary>
        public ushort Offset { get; }

        /// <summary>
        /// The number of bytes in the record
        /// </summary>
        public int RecordLength { get; }

        /// <summary>
        /// The record type
        /// </summary>
        public IntelHexRecordType RecordType { get; }

        /// <summary>
        /// Convert the <see cref="IntelHexRecord"/> to a hex record string representation
        /// </summary>
        /// <returns></returns>
        public string ToHexRecordString()
        {
            return $":{BitConverter.ToString(Bytes.Skip(1).ToArray()).Replace("-", "")}";
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{nameof(RecordType)}: {RecordType} ");
            stringBuilder.Append($"{nameof(RecordLength)}: 0x{RecordLength:X2} ");
            stringBuilder.Append($"{nameof(Offset)}: 0x{Offset:X4} ");
            stringBuilder.Append($"{nameof(Data)}: {BitConverter.ToString(Data.ToArray())} ");
            stringBuilder.Append($"{nameof(CheckSum)}: 0x{CheckSum:X2} ");

            return stringBuilder.ToString();
        }

        private static byte CalculateChecksum(IList<byte> checkSumData)
        {
            var maskedSumBytes = checkSumData.Sum(x => x) & 0xff;
            var calculatedChecksum = (byte)(256 - maskedSumBytes);

            return calculatedChecksum;
        }
    }
}