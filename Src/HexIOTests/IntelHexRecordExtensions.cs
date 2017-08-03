/**
* MIT License
*
* Copyright (c) 2017 Derek Goslin < http://corememorydump.blogspot.ie/ >
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
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HexIO;

namespace HexIOTests
{
    [TestClass]
    public class IntelHexRecordExtensionsTests
    {
        [TestMethod]
        public void TestParseHexRecordNull()
        {
            Action act = () => IntelHexRecordExtensions.ParseHexRecord(null);
            act.ShouldThrow<IOException>().WithMessage("Hex record line can not be null");
        }

        [TestMethod]
        public void TestParseHexRecordInvalidLenght()
        {
            Action act = () => "".PadRight(7, 'X').ParseHexRecord();
            act.ShouldThrow<IOException>().WithMessage("Hex record line length [XXXXXXX] is less than 11");
        }

        [TestMethod]
        public void TestParseHexRecordInvalidStart()
        {
            Action act = () => "".PadRight(11, 'X').ParseHexRecord();
            act.ShouldThrow<IOException>().WithMessage("Illegal line start character [XXXXXXXXXXX]");
        }

        [TestMethod]
        public void TestParseHexRecordRecordType()
        {
            Action act = () => ":01ffff06ffe5".ParseHexRecord();
            act.ShouldThrow<IOException>().WithMessage("Invalid record type value: [6]");
        }

        [TestMethod]
        public void TestParseHexRecordInvalidCrc()
        {
            Action act = () => ":10010000214601360121470136007EFE09D2190155".ParseHexRecord();
            act.ShouldThrow<IOException>().WithMessage("Checksum for line [:10010000214601360121470136007EFE09D2190155] is incorrect");
        }

        [TestMethod]
        public void TestParseHexRecordInvalidRecordLength()
        {
            Action act = () => ":0A010000214601360121470136007EFE09D2190140".ParseHexRecord();
            act.ShouldThrow<IOException>().WithMessage("Line [:0A010000214601360121470136007EFE09D2190140] does not have required record length of [15]");
        }

        [TestMethod]
        public void TestParseHexRecordInvalidData()
        {
            Action act = () => ":0A010000214601360121470136007EFE09D2QQ0140".ParseHexRecord();
            act.ShouldThrow<IOException>().WithMessage("Unable to extract bytes for [0A010000214601360121470136007EFE09D2QQ0140]");
        }
    }
}
