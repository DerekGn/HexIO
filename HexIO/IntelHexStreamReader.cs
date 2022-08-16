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
using HexIO.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace HexIO
{
    /// <summary>
    /// A reader for Intel Hex stream
    /// </summary>
    public class IntelHexStreamReader : StreamReader, IIntelHexStreamReader
    {
        private readonly IntelHexStreamState state = new IntelHexStreamState();

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class 
        /// for the specified stream.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(Stream stream)
            : base(stream)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class 
        /// for the specified file name.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(string path)
            : base(path)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class 
        /// for the specified stream, 
        /// with the specified byte order mark detection option.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(Stream stream, bool detectEncodingFromByteOrderMarks)
            : base(stream, detectEncodingFromByteOrderMarks)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class for the specified stream, 
        /// with the specified character encoding.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(Stream stream, Encoding encoding)
            : base(stream, encoding)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class for the specified 
        /// file name, with the specified byte order mark detection option.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="detectEncodingFromByteOrderMarks"></param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(string path, bool detectEncodingFromByteOrderMarks)
            : base(path, detectEncodingFromByteOrderMarks)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class for the specified
        /// file name, with the specified character encoding.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(string path, Encoding encoding) : base(path, encoding)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class for the specified
        /// stream, with the specified character encoding and byte order mark detection option.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks)
            : base(stream, encoding, detectEncodingFromByteOrderMarks)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class for the specified
        /// file name, with the specified character encoding and byte order mark detection
        /// option.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks)
            : base(path, encoding, detectEncodingFromByteOrderMarks)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class for the specified
        /// stream, with the specified character encoding, byte order mark detection option,
        /// and buffer size.
        /// </summary>
        /// <param name="stream">The stream to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="bufferSize">The minimum buffer size.</param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
            : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class for the specified
        /// file name, with the specified character encoding, byte order mark detection option,
        /// and buffer size.
        /// </summary>
        /// <param name="path">The complete file path to be read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">Indicates whether to look for byte order marks at the beginning of the file.</param>
        /// <param name="bufferSize">The minimum buffer size, in number of 16-bit characters.</param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(string path, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize)
            : base(path, encoding, detectEncodingFromByteOrderMarks, bufferSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the System.IO.StreamReader class for the specified
        /// stream based on the specified character encoding, byte order mark detection option,
        /// and buffer size, and optionally leaves the stream open.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="detectEncodingFromByteOrderMarks">true to look for byte order marks at the beginning of the file; otherwise, false.</param>
        /// <param name="bufferSize">The minimum buffer size.</param>
        /// <param name="leaveOpen">true to leave the stream open after the System.IO.StreamReader object is disposed;
        /// otherwise, false.</param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamReader(Stream stream, Encoding encoding, bool detectEncodingFromByteOrderMarks, int bufferSize, bool leaveOpen)
            : base(stream, encoding, detectEncodingFromByteOrderMarks, bufferSize, leaveOpen)
        {
        }

        /// <inheritdoc/>
        public IntelHexStreamState State { get => state; }

        /// <inheritdoc/>
        public IntelHexRecord ReadHexRecord()
        {
            if (EndOfStream)
            {
                throw new IntelHexStreamException(Resources.StreamEmpty);
            }

            if (State.Eof)
            {
                throw new IntelHexStreamException(Resources.Eof);
            }

            var hexLine = ReadLine();

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
                throw new IntelHexStreamException(string.Format(Resources.InvalidHexRecord, hexLine));
            }

            if (hexLine.Length < 11)
            {
                throw new IntelHexStreamException(string.Format(Resources.InvalidHexRecordLength, hexLine, 11));
            }

            if (!hexLine.StartsWith(":"))
            {
                throw new IntelHexStreamException(string.Format(Resources.InvalidStartCharacter, hexLine));
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
                throw new IntelHexStreamException(string.Format(Resources.UnableToExtract, hexData), ex);
            }
        }
    }
}