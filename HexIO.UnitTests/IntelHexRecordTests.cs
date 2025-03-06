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

            // action
            Action action = () => new IntelHexRecord(new List<byte>()
            {
                (byte)':',0xFF,0,0,0,0
            });

            // Assert
            IntelHexStreamException exception = Assert.Throws<IntelHexStreamException>(action);
            Assert.Equal("Hex record bytes does not have required length of [0x0105]", exception.Message);
        }

        [Fact]
        public void TestParseHexRecordInvalidByteMaxCount()
        {
            // Arrange

            // action
            Action action = () => new IntelHexRecord(new byte[0x200].ToList());

            // Assert
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(action);
            Assert.Equal("hexRecordBytes", exception.ParamName);
        }

        [Fact]
        public void TestParseHexRecordInvalidCrc()
        {
            // Arrange

            // action
            Action action = () => new IntelHexRecord(new List<byte>()
            {
                (byte)':',0x10,0x01,0x00,0x00,0x21,0x46,0x01,0x36,0x01,0x21,0x47,0x01,0x36,0x00,0x7E,0xFE,0x09,0xD2,0x19,0x01,0x55
            });

            // Assert
            IntelHexStreamException exception = Assert.Throws<IntelHexStreamException>(action);
            Assert.Equal("Checksum incorrect. Expected [0x55] Actual: [0x40]", exception.Message);
        }

        [Fact]
        public void TestParseHexRecordInvalidRecordType()
        {
            // Arrange

            // action
            Action action = () => new IntelHexRecord(new List<byte>()
            {
                (byte)':', 0x01, 0xFF, 0xFF, 0x06, 0xFF, 0xEF
            });

            // Assert
            IntelHexStreamException exception = Assert.Throws<IntelHexStreamException>(action);
            Assert.Equal("Invalid record type value: [0x06]", exception.Message);
        }

        [Fact]
        public void TestParseHexRecordInvalidStartCharacter()
        {
            // Arrange

            // action
            Action action = () => new IntelHexRecord(new List<byte>()
            {
                0,0,0,0,0,0
            });

            // Assert
            IntelHexStreamException exception = Assert.Throws<IntelHexStreamException>(action);
            Assert.Equal("Illegal line start character", exception.Message);
        }

        [Fact]
        public void TestParseHexRecordNull()
        {
            // Arrange

            // action
            Action action = () => new IntelHexRecord(null);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(action);
            Assert.Equal("hexRecordBytes", exception.ParamName);
        }

        [Fact]
        public void TestParseHexRecordNullData()
        {
            // Arrange

            // action
            Action action = () => new IntelHexRecord(0, IntelHexRecordType.Data, null);

            // Assert
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(action);
            Assert.Equal("data", exception.ParamName);
        }

        [Fact]
        public void TestParseHexRecordMaxData()
        {
            // Arrange

            // action
            Action action = () => new IntelHexRecord(0, IntelHexRecordType.Data, new byte[0x200].ToList());

            // Assert
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(action);
            Assert.Equal("data", exception.ParamName);
        }

        [Fact]
        public void TestParseHexRecordTooShort()
        {
            // Arrange

            // action
            Action action = () => new IntelHexRecord(new List<byte>()
            {
                0
            });

            // Assert
            Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        public void TestParseHexRecordValidRecord()
        {
            // Arrange
            var payload = new List<byte>()
            {
                (byte)':',0x10,0x01,0x00,0x00,0x21,0x46,0x01,0x36,0x01,0x21,0x47,0x01,0x36,0x00,0x7E,0xFE,0x09,0xD2,0x19,0x01,0x40
            };

            // action
            var hexRecord = new IntelHexRecord(payload);

            // Assert
            Assert.Equal(payload, hexRecord.Bytes);
            Assert.Equal(0x40, hexRecord.CheckSum);
            Assert.Equal(new List<byte>() { 0x21, 0x46, 0x01, 0x36, 0x01, 0x21, 0x47, 0x01, 0x36, 0x00, 0x7E, 0xFE, 0x09, 0xD2, 0x19, 0x01 }, hexRecord.Data);
            Assert.Equal(0x100, hexRecord.Offset);
            Assert.Equal(0x10, hexRecord.RecordLength);
            Assert.Equal(IntelHexRecordType.Data, hexRecord.RecordType);
        }
    }
}