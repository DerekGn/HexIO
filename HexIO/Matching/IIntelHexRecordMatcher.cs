﻿/*
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

namespace HexIO.Matching
{
    /// <summary>
    /// Provides a mechanism to match a <see cref="IntelHexRecordMatch"/> against a <see cref="IntelHexRecord"/>
    /// </summary>
    public interface IIntelHexRecordMatcher
    {
        /// <summary>
        /// Determine if a <see cref="IntelHexRecordMatch"/> matches an <see cref="IntelHexRecord"/> instance
        /// </summary>
        /// <param name="match">The <see cref="IntelHexRecordMatch"/> to match</param>
        /// <param name="record">The <see cref="IntelHexRecord"/> to check</param>
        /// <returns>true if match found</returns>
        bool IsMatch(IntelHexRecordMatch match, IntelHexRecord record);
    }
}
