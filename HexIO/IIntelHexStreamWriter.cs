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

namespace HexIO
{
    /// <summary>
    /// Provides a mechanism to write Intel hex records to an underlying <see cref="Stream"/>
    /// </summary>
    public interface IIntelHexStreamWriter : IDisposable
    {
        /// <summary>
        /// Close the underlying <see cref="Stream"/> flushing pending changes to the underlying stream and writing EOF record
        /// </summary>
        void Close();

        /// <summary>
        /// Write a block of <paramref name="data"/> at <paramref name="offset"/>
        /// </summary>
        /// <param name="offset">The offset to write the data</param>
        /// <param name="data">The block data to write</param>
        /// <remarks><paramref name="data"/> can be max of 255 bytes</remarks>
        /// <exception cref="IntelHexStreamException">Thrown when an error occurs writing to the stream</exception>
        void WriteDataRecord(ushort offset, IList<byte> data);

        /// <summary>
        /// Write an Extended Linear Address Record to the underlying <see cref="Stream"/>
        /// </summary>
        /// <param name="upperLinearBaseAddress">The 16 bit Upper Linear Base Address (LBA)</param>
        /// <exception cref="IntelHexStreamException">Thrown when an error occurs writing to the stream</exception>
        void WriteExtendedLinearAddressRecord(ushort address);

        /// <summary>
        /// Write an Extended Segment Address Record to the underlying <see cref="Stream"/>
        /// </summary>
        /// <param name="address">The Upper Segment Base Address(USBA)</param>
        /// <exception cref="IntelHexStreamException">Thrown when an error occurs writing to the stream</exception>
        void WriteExtendedSegmentAddressRecord(ushort address);

        /// <summary>
        /// Write a Start Linear Address Record
        /// </summary>
        /// <param name="address">The 32 bit EIP register value</param>
        /// <exception cref="IntelHexStreamException">Thrown when an error occurs writing to the stream</exception>
        void WriteStartLinearAddressRecord(uint address);

        /// <summary>
        /// Write an Start Segment Address Record to the underlying <see cref="Stream"/>
        /// </summary>
        /// <param name="cs">The CS register value</param>
        /// <param name="ip">The IP register value</param>
        /// <exception cref="IntelHexStreamException">Thrown when an error occurs writing to the stream</exception>
        void WriteStartSegmentAddressRecord(ushort cs, ushort ip);
    }
}