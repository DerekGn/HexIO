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

using System.IO;
using NUnit.Framework;
using HexIO;

namespace HexIOTests
{
    [TestFixture]
    public class IntelHexRecordExtensionsTests
    {
        [Test]
        public void TestParseHexRecordNull()
        {
            Assert.That(() => IntelHexRecordExtensions.ParseHexRecord(null), 
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Hex record line can not be null"));
        }

        [Test]
        public void TestParseHexRecordInvalidLenght()
        {
            Assert.That(() => IntelHexRecordExtensions.ParseHexRecord("".PadRight(7,'X')),
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Hex record line length [XXXXXXX] is less than 11"));
        }

        [Test]
        public void TestParseHexRecordInvalidStart()
        {
            Assert.That(() => IntelHexRecordExtensions.ParseHexRecord("".PadRight(11, 'X')),
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Illegal line start character [XXXXXXXXXXX]"));
        }

        [Test]
        public void TestParseHexRecordRecordType()
        {
            Assert.That(() => IntelHexRecordExtensions.ParseHexRecord(":01ffff06ffe5"),
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Invalid record type value: [6]"));
        }
        [Test]
        public void TestParseHexRecordInvalidCrc()
        {
            Assert.That(() => IntelHexRecordExtensions.ParseHexRecord(":10010000214601360121470136007EFE09D2190155"),
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Checksum for line [:10010000214601360121470136007EFE09D2190155] is incorrect"));
        }
    }
}
