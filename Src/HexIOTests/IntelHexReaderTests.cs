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
using HexIO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable AccessToModifiedClosure

namespace HexIOTests
{
    [TestClass]
    public class IntelHexReaderTests
    {        
        [TestMethod]
        public void TestThrowExceptionIfStreamNull()
        {
            Action act = () => new IntelHexReader(null);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("stream");
        }

        [TestMethod]
        public void TestNoExceptionIfStreamNotNull()
        {
            Action act = () => new IntelHexReader(new MemoryStream());
            act.Should().NotThrow();
        }

        [TestMethod]
        public void TestReadOnEmptyStream()
        {
            var intelHexReader = new IntelHexReader(new MemoryStream());
            var result = intelHexReader.Read(out _, out _);

            result.Should().Be(false);
        }

        [TestMethod]
        public void TestReadDataRecords()
        {
            int readCount = 0;
            
            using (var intelHexReader = new IntelHexReader(IntelHexData.DataRecords))
            {
                while (intelHexReader.Read(out uint address, out IList<byte> data))
                {
                    data.Should().NotBeNull();
                    data.Count.Should().Be(16);

                    address.Should().Be((uint) (0x100 + (0x10 * readCount)));
                    
                    readCount++;
                }
            }

            readCount.Should().Be(4);
        }

        [TestMethod]
        public void TestReadExtendedSegmentAddressRecords()
        {
            int readCount = 0;

            using (var intelHexReader = new IntelHexReader(IntelHexData.ExtendedSegmentAddressRecords))
            {
                while (intelHexReader.Read(out uint address, out IList<byte> data))
                {
                    address.Should().Be((uint) (0x10000 * readCount));
                    
                    readCount++;
                }
            }

            Assert.AreEqual(4, readCount);
        }

        [TestMethod]
        public void TestReadExtendedLinearAddressRecords()
        {
            int readCount = 0;

            using (var intelHexReader = new IntelHexReader(IntelHexData.ExtendedLinearAddressRecords))
            {
                while (intelHexReader.Read(out uint address, out IList<byte> data))
                {
                    address.Should().Be((uint)(0x10000 * readCount));

                    readCount++;
                }
            }
            Assert.AreEqual(5, readCount);
        }
    }
}
