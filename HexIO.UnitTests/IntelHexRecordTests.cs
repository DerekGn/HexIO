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
using HexIO.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HexIO.UnitTests
{
    public class IntelHexRecordTests
    {
        [Fact]
        public void TestParseHexRecordInvalidByteCount()
        {
            // Arrange

            // Act
            Action act = () => new IntelHexRecord(new List<byte>()
            {
                (byte)':',0xFF,0,0,0,0
            });

            // Assert
            act.Should().Throw<IntelHexStreamException>().WithMessage("Hex record bytes does not have required length of [0x0105]");
        }

        [Fact]
        public void TestParseHexRecordInvalidByteMaxCount()
        {
            // Arrange

            // Act
            Action act = () => new IntelHexRecord(new byte[0x200].ToList());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("hexRecordBytes");
        }

        [Fact]
        public void TestParseHexRecordInvalidCrc()
        {
            // Arrange

            // Act
            Action act = () => new IntelHexRecord(new List<byte>()
            {
                (byte)':',0x10,0x01,0x00,0x00,0x21,0x46,0x01,0x36,0x01,0x21,0x47,0x01,0x36,0x00,0x7E,0xFE,0x09,0xD2,0x19,0x01,0x55
            });

            // Assert
            act.Should().Throw<IntelHexStreamException>().WithMessage("Checksum incorrect. Expected [0x55] Actual: [0x40]");
        }

        [Fact]
        public void TestParseHexRecordInvalidRecordType()
        {
            // Arrange

            // Act
            Action act = () => new IntelHexRecord(new List<byte>()
            {
                (byte)':', 0x01, 0xFF, 0xFF, 0x06, 0xFF, 0xEF
            });

            // Assert
            act.Should().Throw<IntelHexStreamException>().WithMessage("Invalid record type value: [0x06]");
        }

        [Fact]
        public void TestParseHexRecordInvalidStartCharacter()
        {
            // Arrange

            // Act
            Action act = () => new IntelHexRecord(new List<byte>()
            {
                0,0,0,0,0,0
            });

            // Assert
            act.Should().Throw<IntelHexStreamException>().WithMessage("Illegal line start character");
        }

        [Fact]
        public void TestParseHexRecordNull()
        {
            // Arrange

            // Act
            Action action = () => new IntelHexRecord(null);

            // Assert
            action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("hexRecordBytes");
        }

        [Fact]
        public void TestParseHexRecordNullData()
        {
            // Arrange

            // Act
            Action act = () => new IntelHexRecord(0, IntelHexRecordType.Data, null);

            // Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("data");
        }

        [Fact]
        public void TestParseHexRecordMaxData()
        {
            // Arrange

            // Act
            Action act = () => new IntelHexRecord(0, IntelHexRecordType.Data, new byte[0x200].ToList());

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("data");
        }

        [Fact]
        public void TestParseHexRecordTooShort()
        {
            // Arrange

            // Act
            Action act = () => new IntelHexRecord(new List<byte>()
            {
                0
            });

            // Assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void TestParseHexRecordValidRecord()
        {
            // Arrange
            var payload = new List<byte>()
            {
                (byte)':',0x10,0x01,0x00,0x00,0x21,0x46,0x01,0x36,0x01,0x21,0x47,0x01,0x36,0x00,0x7E,0xFE,0x09,0xD2,0x19,0x01,0x40
            };

            // Act
            var hexRecord = new IntelHexRecord(payload);

            // Assert
            hexRecord.Bytes.Should().Equal(payload);
            hexRecord.CheckSum.Should().Be(0x40);
            hexRecord.Data.Should().Equal(new List<byte>() { 0x21, 0x46, 0x01, 0x36, 0x01, 0x21, 0x47, 0x01, 0x36, 0x00, 0x7E, 0xFE, 0x09, 0xD2, 0x19, 0x01 });
            hexRecord.Offset.Should().Be(0x0100);
            hexRecord.RecordLength.Should().Be(0x10);
            hexRecord.RecordType.Should().Be(IntelHexRecordType.Data);
        }
    }
}