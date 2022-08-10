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
using HexIO.Matching;
using System;
using System.Collections.Generic;
using Xunit;

namespace HexIOTests.Matching
{
    public class IntelHexRecordMatcherTests
    {
        private readonly IntelHexRecordMatcher _matcher;

        public IntelHexRecordMatcherTests()
        {
            _matcher = new IntelHexRecordMatcher();
        }

        [Fact]
        public void TestMatchNull()
        {
            // Arrange

            var hexRecord = new IntelHexRecord(0, IntelHexRecordType.Data, new List<byte>());

            // Act
            Action act = () => _matcher.IsMatch(null, hexRecord);

            // Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("match");
        }

        [Fact]
        public void TestRecordNull()
        {
            // Arrange

            // Act
            Action act = () => _matcher.IsMatch(new IntelHexRecordMatch(), null);

            // Assert
            act.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("record");
        }

        [Fact]
        public void TestNoMatch()
        {
            // Arrange

            var match = new IntelHexRecordMatch()
            {
            };

            var hexRecord = new IntelHexRecord(0, IntelHexRecordType.Data, new List<byte>());

            // Act
            var result = _matcher.IsMatch(match, hexRecord);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void TestDataMatch()
        {
            // Arrange
            var match = new IntelHexRecordMatch()
            {
                Data = new List<byte>()
            };

            var hexRecord = new IntelHexRecord(0, IntelHexRecordType.Data, new List<byte>());

            // Act
            var result = _matcher.IsMatch(match, hexRecord);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void TestDataRecordTypeMatch()
        {
            // Arrange
            var match = new IntelHexRecordMatch()
            {
                RecordType = IntelHexRecordType.ExtendedLinearAddress
            };

            var hexRecord = new IntelHexRecord(0, IntelHexRecordType.ExtendedLinearAddress, new List<byte>());

            // Act
            var result = _matcher.IsMatch(match, hexRecord);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void TestDataRecordOffset()
        {
            // Arrange
            var match = new IntelHexRecordMatch()
            {
                Offset = 0x1000
            };

            var hexRecord = new IntelHexRecord(0x1000, IntelHexRecordType.ExtendedLinearAddress, new List<byte>());

            // Act
            var result = _matcher.IsMatch(match, hexRecord);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void TestDataRecordLength()
        {
            // Arrange
            var match = new IntelHexRecordMatch()
            {
                RecordLength = 2
            };

            var hexRecord = new IntelHexRecord(0x1000, IntelHexRecordType.ExtendedLinearAddress, new List<byte>() { 0x30, 0xFF });

            // Act
            var result = _matcher.IsMatch(match, hexRecord);

            // Assert
            result.Should().BeTrue();
        }
    }
}
