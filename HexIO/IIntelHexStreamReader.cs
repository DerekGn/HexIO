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
using System.IO;

namespace HexIO
{
    /// <summary>
    /// Provides a mechanism to read Intel hex records from an underlying <see cref="Stream"/>
    /// </summary>
    public interface IIntelHexStreamReader : IDisposable
    {
        /// <summary>
        /// Indicates end of stream has been reached
        /// </summary>
        bool EndOfStream { get; }

        /// <summary>
        /// Get the current <see cref="IntelHexStreamState"/> for the underlying <see cref="Stream"/>
        /// </summary>
        IntelHexStreamState State { get; }

        /// <summary>
        /// Read the next <see cref="IntelHexRecord"/> from the underlying <see cref="Stream"/>
        /// </summary>
        /// <returns>An instance of a <see cref="IntelHexRecord"/></returns>
        /// <exception cref="IntelHexStreamException">
        /// Thrown when an error occurs reading from the stream or if the stream is empty or the
        /// <see cref="State"/> EOF is set
        /// </exception>
        IntelHexRecord ReadHexRecord();
    }
}