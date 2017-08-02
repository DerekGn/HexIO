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
            Assert.That(() => "".PadRight(7,'X').ParseHexRecord(),
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Hex record line length [XXXXXXX] is less than 11"));
        }

        [Test]
        public void TestParseHexRecordInvalidStart()
        {
            Assert.That(() => "".PadRight(11, 'X').ParseHexRecord(),
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Illegal line start character [XXXXXXXXXXX]"));
        }

        [Test]
        public void TestParseHexRecordRecordType()
        {
            Assert.That(() => ":01ffff06ffe5".ParseHexRecord(),
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Invalid record type value: [6]"));
        }

        [Test]
        public void TestParseHexRecordInvalidCrc()
        {
            Assert.That(() => ":10010000214601360121470136007EFE09D2190155".ParseHexRecord(),
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Checksum for line [:10010000214601360121470136007EFE09D2190155] is incorrect"));
        }

        [Test]
        public void TestParseHexRecordInvalidRecordLength()
        {
            Assert.That(() => ":0A010000214601360121470136007EFE09D2190140".ParseHexRecord(),
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Line [:0A010000214601360121470136007EFE09D2190140] does not have required record length of [15]"));
        }

        [Test]
        public void TestParseHexRecordInvalidData()
        {
            Assert.That(() => ":0A010000214601360121470136007EFE09D2QQ0140".ParseHexRecord(),
                Throws.Exception.TypeOf<IOException>().With.Property("Message").EqualTo("Unable to extract bytes for [0A010000214601360121470136007EFE09D2QQ0140]"));
        }
    }
}
