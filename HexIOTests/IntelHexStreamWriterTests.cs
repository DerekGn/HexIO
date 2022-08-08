/**
* MIT License
*
* Copyright (c) 2022 Derek Goslin https://github.com/DerekGn
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

using FluentAssertions;
using HexIO;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace HexIOTests
{
    public class IntelHexStreamWriterTests
    {
        private const string EofRecord = ":00000001FF";

        [Fact]
        public void TestWriteDataTooLong()
        {
            // Arrange
            var intelHexStreamWriter = new IntelHexStreamWriter(new MemoryStream());

            // Act
            Action action = () => intelHexStreamWriter.WriteDataRecord(0, new byte[258].ToList());

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TestWriteDataNull()
        {
            // Arrange
            var intelHexStreamWriter = new IntelHexStreamWriter(new MemoryStream());

            // Act
            Action action = () => intelHexStreamWriter.WriteDataRecord(0, new byte[258].ToList());

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("data");
        }

        [Fact]
        public void TestWriteDataOk()
        {
            // Arrange
            var memoryStream = new MemoryStream();
            var intelHexStreamWriter = new IntelHexStreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            // Act
            intelHexStreamWriter.WriteDataRecord(0x55AA, new byte[16].ToList());
            intelHexStreamWriter.Close();

            // Assert
            memoryStream.Position = 0;

            using (var sr = new StreamReader(memoryStream))
            {
                sr.ReadLine().Should().Be(":1055AA0000000000000000000000000000000000F1");
                sr.ReadLine().Should().Be(EofRecord);
            }
        }

        [Fact]
        public void TestWriteExtendedSegmentAddressRecord()
        {
            // Arrange
            var memoryStream = new MemoryStream();

            var intelHexStreamWriter = new IntelHexStreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            // Act
            intelHexStreamWriter.WriteExtendedSegmentAddressRecord(0x1000);

            intelHexStreamWriter.Close();

            // Assert
            memoryStream.Position = 0;

            using (var sr = new StreamReader(memoryStream))
            {
                sr.ReadLine().Should().Be(":020000021000EC");
                sr.ReadLine().Should().Be(EofRecord);
            }
        }

        [Fact]
        public void TestWriteStartSegmentAddressRecord()
        {
            // Arrange
            var memoryStream = new MemoryStream();

            var intelHexStreamWriter = new IntelHexStreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            // Act
            intelHexStreamWriter.WriteStartSegmentAddressRecord(0x1000, 0x2000);

            intelHexStreamWriter.Close();

            // Assert
            memoryStream.Position = 0;

            using (var sr = new StreamReader(memoryStream))
            {
                sr.ReadLine().Should().Be(":0400000310002000C9");
                sr.ReadLine().Should().Be(EofRecord);
            }
        }

        [Fact]
        public void TestWriteExtendedLinearAddressRecord()
        {
            // Arrange
            var memoryStream = new MemoryStream();

            var intelHexStreamWriter = new IntelHexStreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            // Act
            intelHexStreamWriter.WriteExtendedLinearAddressRecord(0x5000);
            intelHexStreamWriter.Close();

            // Assert
            memoryStream.Position = 0;

            using (var sr = new StreamReader(memoryStream))
            {
                sr.ReadLine().Should().Be(":020000045000AA");
                sr.ReadLine().Should().Be(EofRecord);
            }
        }

        [Fact]
        public void TestWriteStartLinearAddressRecord()
        {
            // Arrange
            var memoryStream = new MemoryStream();

            var intelHexStreamWriter = new IntelHexStreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            // Act
            intelHexStreamWriter.WriteStartLinearAddressRecord(0x1000FFFF);

            intelHexStreamWriter.Close();

            // Assert
            memoryStream.Position = 0;

            using (var sr = new StreamReader(memoryStream))
            {
                sr.ReadLine().Should().Be(":040000051000FFFFE9");
                sr.ReadLine().Should().Be(EofRecord);
            }
        }
    }
}