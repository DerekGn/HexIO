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
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HexIO;

// ReSharper disable AccessToDisposedClosure

namespace HexIOTests
{
    [TestClass]
    public class IntelHexRecordWriterTests
    {
        [TestMethod]
        public void TestThrowExceptionIfStreamNull()
        {
            Action act = () => new IntelHexWriter(null);
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("stream");
        }

        [TestMethod]
        public void TestNoExceptionIfStreamNotNull()
        {
            Action act = () => new IntelHexReader(new MemoryStream());
            act.Should().NotThrow();
        }

        [TestMethod]
        public void TestWriteDataTooLong()
        {
            var intelHexWriter = new IntelHexWriter(new MemoryStream());

            Action act = () => intelHexWriter.WriteData(0, new byte[258].ToList());
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestMethod]
        public void TestWriteDataNull()
        {
            var intelHexWriter = new IntelHexWriter(new MemoryStream());

            Action act = () => intelHexWriter.WriteData(0, new byte[258].ToList());
            act.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("data");
        }

        [TestMethod]
        public void TestWriteDataOk()
        {
            var ms = new MemoryStream();

            var intelHexWriter = new IntelHexWriter(ms);

            intelHexWriter.WriteData(0x55AA, new byte[16].ToList());

            intelHexWriter.Close();

            ms.Position = 0;

            using (var sr = new StreamReader(ms))
            {
                sr.ReadLine().Should().Be(":1055AA0000000000000000000000000000000000F1");
                sr.ReadLine().Should().Be(":00000001FF");                
            }
        }

        [TestMethod]
        public void TestWriteAddressExtendedSegmentAddressTooLarge()
        {
            var intelHexWriter = new IntelHexWriter(new MemoryStream());
            intelHexWriter.WriteAddress(AddressType.ExtendedSegmentAddress, 0x10000);
        }

        [TestMethod]
        public void TestWriteAddressOkExtendedLinearAddress()
        {
            TestAddressWrite(AddressType.ExtendedLinearAddress, 0xBEEFDEAD, ":02000004BEEF4D");
        }

        [TestMethod]
        public void TestWriteAddressOkExtendedSegmentAddress()
        {
            TestAddressWrite(AddressType.ExtendedSegmentAddress, 0x9000, ":020000020900F3");
        }

        [TestMethod]
        [DataRow(0xDEADBEEF, ":04000005DEADBEEFBF")]
        [DataRow((uint)0x8005000, ":04000005080050009F")]
        public void TestWriteAddressOkStartLinearAddress(uint address, string expected)
        {
            TestAddressWrite(AddressType.StartLinearAddress, address, expected);
        }

        private void MergeHexStreams()
        {
            using (var mergedStream = new FileStream("merged.hex", FileMode.Open))
            {
                using (IntelHexWriter hexWriter = new IntelHexWriter(mergedStream))
                {
                    WriteHexFile(hexWriter, "HexA.hex");
                    WriteHexFile(hexWriter, "HexB.hex");
                }
            }
        }

        private void WriteHexFile(IntelHexWriter intelHexWriter, string inputfile)
        {
            IntelHexRecord intelHexRecord;

            using (var fileStream = new FileStream(inputfile, FileMode.Open))
            {
                using (IntelHexReader hexReader = new IntelHexReader(fileStream))
                {
                    while (hexReader.Read(out intelHexRecord))
                    {
#warning filter out records that you dont want to copy for example EOF

                        if (intelHexRecord.RecordType == IntelHexRecordType.ExtendedLinearAddress)
                        {
                            intelHexWriter.WriteAddress(AddressType.ExtendedLinearAddress, intelHexRecord.Address);
                        }
                        else if(intelHexRecord.RecordType == IntelHexRecordType.Data)
                        {
                            intelHexWriter.WriteData((ushort)intelHexRecord.Address, intelHexRecord.Data);
                        }
                    }
                }
            }
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
                sr.ReadLine().Should().Be(expected);
                sr.ReadLine().Should().Be(":00000001FF");
            }
        }
    }
}