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
using System.Collections.Generic;
using NUnit.Framework;
using HexIO;

namespace HexIOTests
{
    [TestFixture]
    public class IntelHexReaderTests
    {        
        [Test]
        public void TestThrowExceptionIfStreamNull()
        {
            Assert.That(() => new IntelHexReader(null), Throws.Exception.TypeOf<ArgumentNullException>().With.Property("ParamName").EqualTo("stream"));
        }

        [Test]
        public void TestNoExceptionIfStreamNotNull()
        {
            Assert.That(() => new IntelHexReader(new MemoryStream()), Throws.Nothing);
        }

        [Test]
        public void TestReadOnEmptyStream()
        {
            IList<byte> data;
            uint address;

            var intelHexReder = new IntelHexReader(new MemoryStream());
            
            Assert.That(() => intelHexReder.Read(out address, out data), Is.False);
        }

        [Test]
        public void TestReadI8HexData()
        {
            int readCount = 0;
            IList<byte> data;
            uint address;
            
            var intelHexReder = new IntelHexReader(IntelHexData.I8HexData);

            while(intelHexReder.Read(out address, out data))
            {
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.Not.Null);
                    Assert.That(data.Count, Is.EqualTo(16));
                    Assert.That(address, Is.EqualTo(0x100 + (0x10 * readCount)));
                });

                readCount++;
            }

            Assert.AreEqual(4, readCount);
        }

        [Test]
        public void TestReadI16Hex()
        {
            int readCount = 0;
            IList<byte> data;
            uint address;

            var intelHexReder = new IntelHexReader(IntelHexData.I16HexData);

            while (intelHexReder.Read(out address, out data))
            {
                Assert.That(address, Is.EqualTo(0x10000 * readCount));
                
                readCount++;
            }

            Assert.AreEqual(4, readCount);
        }

        [Test]
        public void TestReadI32Hex()
        {
            int readCount = 0;
            IList<byte> data;
            uint address;

            var intelHexReder = new IntelHexReader(IntelHexData.I32HexData);

            while (intelHexReder.Read(out address, out data))
            {
                Assert.That(address, Is.EqualTo(0x10000 * readCount));
                readCount++;
            }

            Assert.AreEqual(5, readCount);
        }
    }
}
