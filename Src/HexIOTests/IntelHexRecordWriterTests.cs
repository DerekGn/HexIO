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
using System.Linq;
using NUnit.Framework;
using HexIO;

// ReSharper disable AccessToDisposedClosure

namespace HexIOTests
{
    [TestFixture]
    public class IntelHexRecordWriterTests
    {
        [Test]
        public void TestThrowExceptionIfStreamNull()
        {
            Assert.That(() => new IntelHexWriter(null),
                Throws.Exception.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("stream"));
        }

        [Test]
        public void TestNoExceptionIfStreamNotNull()
        {
            Assert.That(() => new IntelHexReader(new MemoryStream()), Throws.Nothing);
        }

        [Test]
        public void TestWriteDataTooLong()
        {
            var intelHexWriter = new IntelHexWriter(new MemoryStream());

            Assert.That(() => intelHexWriter.WriteData(0, new byte[258].ToList()),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>().With.Property("Message")
                    .EqualTo("Must be less than 255\r\nParameter name: data"));
        }

        [Test]
        public void TestWriteDataNull()
        {
            var intelHexWriter = new IntelHexWriter(new MemoryStream());

            Assert.That(() => intelHexWriter.WriteData(0, null),
                Throws.Exception.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("data"));
        }

        [Test]
        public void TestWriteDataOk()
        {
            var ms = new MemoryStream();

            var intelHexWriter = new IntelHexWriter(ms);

            intelHexWriter.WriteData(0, new byte[16].ToList());

            intelHexWriter.Close();

            ms.Position = 0;

            using (var sr = new StreamReader(ms))
            {
                Assert.Multiple(() =>
                {
                    Assert.That(sr.ReadLine(), Is.EqualTo(":1000000000000000000000000000000000000000F0"));
                    Assert.That(sr.ReadLine(), Is.EqualTo(":00000001FF"));
                });
            }
        }

        [Test]
        public void TestWriteAddressExtendedSegmentAddressTooLarge()
        {
            var intelHexWriter = new IntelHexWriter(new MemoryStream());
            intelHexWriter.WriteAddress(AddressType.ExtendedSegmentAddress, 0x10000);
        }

        [Test]
        public void TestWriteAddressOkExtendedLinearAddress()
        {
            TestAddressWrite(AddressType.ExtendedLinearAddress, 0xBEEFDEAD, ":02000004BEEF4D");
        }

        [Test]
        public void TestWriteAddressOkExtendedSegmentAddress()
        {
            TestAddressWrite(AddressType.ExtendedSegmentAddress, 0x9000, ":020000020900F3");
        }

        [Test]
        public void TestWriteAddressOkStartLinearAddress()
        {
            TestAddressWrite(AddressType.StartLinearAddress, 0xDEADBEEF, ":04000005DEADBEEFBF");
        }

        private static void TestAddressWrite(AddressType addressType, uint address, string expected)
        {
            var ms = new MemoryStream();

            var intelHexWriter = new IntelHexWriter(ms);
            intelHexWriter.WriteAddress(addressType, address);
            intelHexWriter.Close();
            
            ms.Position = 0;

            using (var sr = new StreamReader(ms))
            {
                Assert.That(sr.ReadLine(), Is.EqualTo(expected));
                Assert.That(sr.ReadLine(), Is.EqualTo(":00000001FF"));
            }
        }
    }
}