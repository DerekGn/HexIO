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

using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HexIO
{
    /// <summary>
    /// The <see cref="IntelHexStreamReader"/> state
    /// </summary>
    public sealed class IntelHexStreamState
    {
        /// <summary>
        /// Create an instance of a <see cref="IntelHexStreamState"/>
        /// </summary>
        public IntelHexStreamState()
        {
            SegmentAddress = new SegmentAddress(0, 0);
        }

        /// <summary>
        /// The last Extended Instruction Pointer (EIP) value read from the stream
        /// </summary>
        public uint ExtendedInstructionPointer { get; internal set; }

        /// <summary>
        /// The last Upper Linear Base Address (ULBA) value read from the stream
        /// </summary>
        public ushort UpperLinearBaseAddress { get; internal set; }

        /// <summary>
        /// The last Upper Segment Base Address (USBA) read from the stream
        /// </summary>
        public ushort UpperSegmentBaseAddress { get; internal set; }

        /// <summary>
        /// The last <see cref="SegmentAddress"/> read from the stream
        /// </summary>
        public SegmentAddress SegmentAddress { get; internal set; }

        /// <summary>
        /// Indicates if the End Of File record has been read
        /// </summary>
        public bool Eof { get; internal set; }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{nameof(ExtendedInstructionPointer)}: 0x{ExtendedInstructionPointer:X8} ");
            stringBuilder.Append($"{nameof(UpperLinearBaseAddress)}: 0x{UpperLinearBaseAddress:X4} ");
            stringBuilder.Append($"{nameof(UpperSegmentBaseAddress)}: 0x{UpperSegmentBaseAddress:X4} ");
            stringBuilder.Append($"{nameof(SegmentAddress)}: {SegmentAddress} ");

            return stringBuilder.ToString();
        }
    }
}