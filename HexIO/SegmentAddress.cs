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
    /// A segment address
    /// </summary>
    public struct SegmentAddress
    {
        /// <summary>
        /// Create an instance of a <see cref="SegmentAddress"/>
        /// </summary>
        /// <param name="cs">The code segment value</param>
        /// <param name="ip">The instruction pointer value</param>
        public SegmentAddress(ushort cs, ushort ip)
        {
            CodeSegment = cs;
            InstructionPointer = ip;
        }

        /// <summary>
        /// The code segment value
        /// </summary>
        public ushort CodeSegment { get; }

        /// <summary>
        /// The instruction pointer value
        /// </summary>
        public ushort InstructionPointer { get; }

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"{nameof(CodeSegment)}: 0x{CodeSegment:X4} ");
            stringBuilder.Append($"{nameof(InstructionPointer)}: 0x{InstructionPointer:X4}");

            return stringBuilder.ToString();
        }
    }
}