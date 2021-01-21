/*
* MIT License
*
* Copyright (c) 2017 Derek Goslin http://corememorydump.blogspot.ie/
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

namespace HexIO
{
    public enum IntelHexRecordType
    {
        /// <summary>
        /// Indicates the record contains data and a 16-bit starting address for the data
        /// </summary>
        Data,
        /// <summary>
        /// Indicates the record contains no data
        /// </summary>
        EndOfFile,
        /// <summary>
        /// Indicates the record data field contains a 16-bit segment base address
        /// </summary>
        ExtendedSegmentAddress,
        /// <summary>
        /// Indicates the record specifies the initial content of the CS:IP registers
        /// </summary>
        StartSegmentAddress,
        /// <summary>
        /// Indicates the record contains a the upper 16 bit address
        /// </summary>
        ExtendedLinearAddress,
        /// <summary>
        /// Indicates the record contains a 32 bit address
        /// </summary>
        StartLinearAddress
    }
}
