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
using System.IO;
using System.Linq;
using Xunit;

namespace HexIO.UnitTests
{
    public class IntelHexStreamReaderTests
    {
        [Fact]
        public void TestReadOnEmptyStream()
        {
            // Arrange
            var intelHexStreamReader = new IntelHexStreamReader(new MemoryStream());

            // Act
            Action action = () => { intelHexStreamReader.ReadHexRecord(); };

            // Assert
            action.Should().Throw<IntelHexStreamException>().And.Message.Should().Be("The stream is empty");
        }

        [Fact]
        public void TestInvalidRecords()
        {
            // Arrange
            var intelHexStreamReader = new IntelHexStreamReader(
                IntelHexTestData.InvalidRecords);

            // Act
            Action action = () => { intelHexStreamReader.ReadHexRecord(); };

            // Assert
            action.Should().Throw<IntelHexStreamException>().And.Message.Should().Be("An invalid Hex record has been read from the stream. Value: [ 	]");
        }

        [Fact]
        public void TestInvalidHexRecords()
        {
            // Arrange
            var intelHexStreamReader = new IntelHexStreamReader(
                IntelHexTestData.InvalidLengthRecords);

            // Act
            Action action = () => { intelHexStreamReader.ReadHexRecord(); };

            // Assert
            action.Should().Throw<IntelHexStreamException>().And.Message.Should().Be("Hex record line length [AAA] is less than 11");
        }

        [Fact]
        public void TestReadHexDataRecords()
        {
            // Arrange
            List<IntelHexRecord> hexRecords = new List<IntelHexRecord>();

            var intelHexStreamReader = new IntelHexStreamReader(
                IntelHexTestData.DataRecords);

            // Act
            using (intelHexStreamReader)
            {
                do
                {
                    IntelHexRecord dataRecord = intelHexStreamReader.ReadHexRecord();

                    hexRecords.Add(dataRecord);

                } while (!intelHexStreamReader.State.Eof);
            }

            // Assert
            hexRecords.Count.Should().Be(5);
            hexRecords.First().RecordType.Should().Be(IntelHexRecordType.Data);
            hexRecords.Last().RecordType.Should().Be(IntelHexRecordType.EndOfFile);
        }

        [Fact]
        public void TestReadExtendedSegmentAddressRecord()
        {
            // Arrange
            List<IntelHexRecord> hexRecords = new List<IntelHexRecord>();

            var intelHexStreamReader = new IntelHexStreamReader(
                IntelHexTestData.ExtendedSegmentAddressRecords);

            // Act
            do
            {
                IntelHexRecord dataRecord = intelHexStreamReader.ReadHexRecord();

                hexRecords.Add(dataRecord);

            } while (!intelHexStreamReader.State.Eof);

            // Assert
            hexRecords.Count.Should().Be(2);
            intelHexStreamReader.State.UpperSegmentBaseAddress.Should().Be(0x3000);
        }

        [Fact]
        public void TestReadStartSegmentAddressRecord()
        {
            // Arrange
            List<IntelHexRecord> hexRecords = new List<IntelHexRecord>();

            var intelHexStreamReader = new IntelHexStreamReader(
                IntelHexTestData.StartSegmentAddressRecords);

            // Act
            do
            {
                IntelHexRecord dataRecord = intelHexStreamReader.ReadHexRecord();

                hexRecords.Add(dataRecord);

            } while (!intelHexStreamReader.State.Eof);

            // Assert
            hexRecords.Count.Should().Be(2);
            intelHexStreamReader.State.SegmentAddress.CodeSegment.Should().Be(0xBEEF);
            intelHexStreamReader.State.SegmentAddress.InstructionPointer.Should().Be(0xFEED);
        }

        [Fact]
        public void TestReadExtendedLinearAddressRecord()
        {
            // Arrange
            List<IntelHexRecord> hexRecords = new List<IntelHexRecord>();

            var intelHexStreamReader = new IntelHexStreamReader(
                IntelHexTestData.ExtendedLinearAddressRecords);

            // Act
            do
            {
                IntelHexRecord dataRecord = intelHexStreamReader.ReadHexRecord();

                hexRecords.Add(dataRecord);

            } while (!intelHexStreamReader.State.Eof);

            // Assert
            hexRecords.Count.Should().Be(2);
            intelHexStreamReader.State.UpperLinearBaseAddress.Should().Be(0x5000);
        }

        [Fact]
        public void TestReadStartLinearAddressRecord()
        {
            // Arrange
            List<IntelHexRecord> hexRecords = new List<IntelHexRecord>();

            var intelHexStreamReader = new IntelHexStreamReader(
                IntelHexTestData.StartLinearAddressRecords);

            // Act
            do
            {
                IntelHexRecord dataRecord = intelHexStreamReader.ReadHexRecord();

                hexRecords.Add(dataRecord);

            } while (!intelHexStreamReader.State.Eof);

            // Assert
            hexRecords.Count.Should().Be(2);
            intelHexStreamReader.State.ExtendedInstructionPointer.Should().Be(0xBEEFFEED);
        }
    }
}
