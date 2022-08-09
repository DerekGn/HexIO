/*
* MIT License
*
* Copyright (c) 2022 Derek Goslin http://corememorydump.blogspot.ie/
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

using HexIO.Transforms;
using System.Collections.Generic;

namespace HexIO
{
    /// <summary>
    /// Provides a mechanism to modify Intel hex records
    /// </summary>
    public interface IIntelHexStreamTransformer
    {
        /// <summary>
        /// Apply a set of <see cref="Transform"/> to an Intel hex record file
        /// </summary>
        /// <param name="inputFile">The input Intel Hex record file</param>
        /// <param name="transforms">The set of <see cref="IList{T}"/> of <see cref="Transform"/></param>
        /// <returns></returns>
        string ApplyTransforms(
            string inputFile,
            IList<Transform> transforms);
    }
}
