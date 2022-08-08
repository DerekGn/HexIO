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
using System.IO;
using System.Text;

namespace HexIO
{
    public class IntelHexStreamReader : StreamReader, IIntelHexStreamReader
    {
        private readonly IntelHexStreamState state = new IntelHexStreamState();

        public IntelHexStreamReader(Stream stream)
            : base(stream)
        {
        }

        public IntelHexStreamReader(string path)
            : base(path)
        {
        }

        public IntelHexStreamReader(Stream stream, bool detectEncodingFromByteOrderMarks)
            : base(stream, detectEncodingFromByteOrderMarks)
        {
        }

        public IntelHexStreamReader(Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
        }

        public IntelHexStreamReader(string path, bool detectEncodingFromByteOrderMarks)
            : base(path, detectEncodingFromByteOrderMarks)
        {
        }

        public IntelHexStreamReader(string path, Encoding encoding) : base(path, encoding)
        {
        }

        public IntelHexStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
            : base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
        }

        public IntelHexStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
            : base(path, encoding, detectEncodingFromByteOrderMarks)
        {
        }

        public IntelHexStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
            : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
        }

        public IntelHexStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
            : base(path, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
        }

        public IntelHexStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen)
            : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen)
        {
        }

        public IntelHexStreamState State { get => state; }

        public IntelHexRecord ReadHexRecord()
        {
            if (EndOfStream)
            {
                throw new IntelHexStreamException("The stream is empty");
            }

            if (State.Eof)
            {
                throw new IntelHexStreamException("The EOF record has been reached");
            }

            var hexLine = this.ReadLine();

            AssertValidHexLine(hexLine);

            var hexRecord = new IntelHexRecord(TryParseData(hexLine));

            switch (hexRecord.RecordType)
            {
                case IntelHexRecordType.Data:
                    break;

                case IntelHexRecordType.EndOfFile:
                    State.Eof = true;
                    break;

                case IntelHexRecordType.ExtendedSegmentAddress:
                    State.UpperSegmentBaseAddress = (ushort)((hexRecord.Data[0] << 8) + hexRecord.Data[1]);
                    break;

                case IntelHexRecordType.StartSegmentAddress:
                    ushort cs = (ushort)((hexRecord.Data[0] << 8) + hexRecord.Data[1]);
                    ushort ip = (ushort)((hexRecord.Data[2] << 8) + hexRecord.Data[3]);
                    State.SegmentAddress = new SegmentAddress(cs, ip);
                    break;

                case IntelHexRecordType.ExtendedLinearAddress:
                    State.UpperLinearBaseAddress = (ushort)((hexRecord.Data[0] << 8) + hexRecord.Data[1]);
                    break;

                case IntelHexRecordType.StartLinearAddress:
                    State.ExtendedInstructionPointer = (uint)((hexRecord.Data[0] << 24) + (hexRecord.Data[1] << 16) + (hexRecord.Data[2] << 8) + hexRecord.Data[3]);
                    break;

                default:
                    break;
            }

            return hexRecord;
        }

        private static void AssertValidHexLine(string hexLine)
        {
            if (String.IsNullOrWhiteSpace(hexLine))
            {
                throw new IntelHexStreamException($"An invalid Hex record has been read from the stream. Value: [{hexLine}]");
            }

            if (hexLine.Length < 11)
            {
                throw new IntelHexStreamException($"Hex record line length [{hexLine}] is less than 11");
            }

            if (!hexLine.StartsWith(":"))
            {
                throw new IntelHexStreamException($"Illegal line start character [{hexLine}]");
            }
        }

        private static List<byte> TryParseData(string hexData)
        {
            try
            {
                var data = new List<byte>
                {
                    (byte)hexData[0]
                };

                for (var i = 1; i < hexData.Length; i++)
                {
                    data.Add(Convert.ToByte(hexData.Substring(i, 2), 16));
                    i++;
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new IntelHexStreamException($"Unable to extract bytes for [{hexData}]", ex);
            }
        }
    }
}