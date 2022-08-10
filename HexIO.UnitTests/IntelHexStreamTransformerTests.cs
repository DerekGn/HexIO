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
using HexIO.Factories;
using HexIO.IO;
using HexIO.Matching;
using HexIO.Transforms;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace HexIO.UnitTests
{
    public class IntelHexStreamTransformerTests
    {
        private const string EofRecord = ":00000001FF";
        private readonly Mock<IFileSystem> _mockFileStream;
        private readonly Mock<IIntelHexRecordMatcher> _mockRecordMatcher;
        private readonly Mock<IIntelHexStreamReader> _mockStreamReader;
        private readonly Mock<IIntelHexStreamReaderFactory> _mockStreamReaderFactory;
        private readonly IntelHexStreamTransformer _transformer;

        public IntelHexStreamTransformerTests()
        {
            _mockStreamReaderFactory = new Mock<IIntelHexStreamReaderFactory>();
            _mockRecordMatcher = new Mock<IIntelHexRecordMatcher>();
            _mockStreamReader = new Mock<IIntelHexStreamReader>();
            _mockFileStream = new Mock<IFileSystem>();

            _transformer = new IntelHexStreamTransformer(
                _mockFileStream.Object,
                _mockRecordMatcher.Object,
                _mockStreamReaderFactory.Object);
        }

        [Fact]
        public void TestApplyDeleteTransform()
        {
            // Arrange
            _mockFileStream
                .Setup(_ => _.Exists(It.IsAny<string>()))
                .Returns(true);

            var memoryStream = new MemoryStream();

            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            _mockFileStream.Setup(_ => _.CreateText(It.IsAny<string>()))
                .Returns(streamWriter);

            _mockStreamReaderFactory.Setup(_ => _.Create(It.IsAny<string>()))
                .Returns(_mockStreamReader.Object);

            _mockStreamReader
                .SetupSequence(_ => _.ReadHexRecord())
                .Returns(new IntelHexRecord(0, IntelHexRecordType.ExtendedLinearAddress, new List<byte>()))
                .Returns(new IntelHexRecord(0, IntelHexRecordType.EndOfFile, new List<byte>()));

            _mockRecordMatcher
                .SetupSequence(_ => _.IsMatch(It.IsAny<IntelHexRecordMatch>(), It.IsAny<IntelHexRecord>()))
                .Returns(true)
                .Returns(false);

            _mockStreamReader
                .SetupSequence(_ => _.EndOfStream)
                .Returns(false)
                .Returns(true);

            // Act
            _transformer
                .ApplyTransforms("c:\\temp\\filename.hex", new List<Transform>()
                {
                    new DeleteTransform(
                        new IntelHexRecordMatch()
                        {
                            RecordType = IntelHexRecordType.ExtendedLinearAddress
                        })
                });

            // Assert
            memoryStream.Position = 0;

            using (var sr = new StreamReader(memoryStream))
            {
                sr.ReadLine().Should().Be(EofRecord);
            }
        }

        [Fact]
        public void TestApplyInsertTransformAfter()
        {
            // Arrange
            _mockFileStream
                .Setup(_ => _.Exists(It.IsAny<string>()))
                .Returns(true);

            var memoryStream = new MemoryStream();

            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            _mockFileStream.Setup(_ => _.CreateText(It.IsAny<string>()))
                .Returns(streamWriter);

            _mockStreamReaderFactory.Setup(_ => _.Create(It.IsAny<string>()))
                .Returns(_mockStreamReader.Object);

            _mockStreamReader
                .SetupSequence(_ => _.ReadHexRecord())
                .Returns(new IntelHexRecord(0, IntelHexRecordType.ExtendedLinearAddress, new List<byte>()))
                .Returns(new IntelHexRecord(0, IntelHexRecordType.EndOfFile, new List<byte>()));

            _mockRecordMatcher
                .SetupSequence(_ => _.IsMatch(It.IsAny<IntelHexRecordMatch>(), It.IsAny<IntelHexRecord>()))
                .Returns(true);

            _mockStreamReader
                .SetupSequence(_ => _.EndOfStream)
                .Returns(false)
                .Returns(true);

            // Act
            _transformer
                .ApplyTransforms("c:\\temp\\filename.hex", new List<Transform>()
                {
                    new InsertTransform(
                        new IntelHexRecordMatch(),
                        InsertPosition.After,
                        new IntelHexRecord(0x1000, IntelHexRecordType.StartLinearAddress, new List<byte>() { 0xDE, 0xAD}))
                });

            // Assert
            memoryStream.Position = 0;

            using (var sr = new StreamReader(memoryStream))
            {
                sr.ReadLine().Should().Be(":00000004FC");
                sr.ReadLine().Should().Be(":02100005DEAD5E");
                sr.ReadLine().Should().Be(EofRecord);
            }
        }

        [Fact]
        public void TestApplyInsertTransformBefore()
        {
            // Arrange
            _mockFileStream
                .Setup(_ => _.Exists(It.IsAny<string>()))
                .Returns(true);

            var memoryStream = new MemoryStream();

            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            _mockFileStream.Setup(_ => _.CreateText(It.IsAny<string>()))
                .Returns(streamWriter);

            _mockStreamReaderFactory.Setup(_ => _.Create(It.IsAny<string>()))
                .Returns(_mockStreamReader.Object);

            _mockStreamReader
                .SetupSequence(_ => _.ReadHexRecord())
                .Returns(new IntelHexRecord(0, IntelHexRecordType.EndOfFile, new List<byte>()));

            _mockRecordMatcher
                .SetupSequence(_ => _.IsMatch(It.IsAny<IntelHexRecordMatch>(), It.IsAny<IntelHexRecord>()))
                .Returns(true);

            _mockStreamReader
                .SetupSequence(_ => _.EndOfStream)
                .Returns(true);

            // Act
            _transformer
                .ApplyTransforms("c:\\temp\\filename.hex", new List<Transform>()
                {
                    new InsertTransform(
                        new IntelHexRecordMatch(),
                        InsertPosition.Before,
                        new IntelHexRecord(0x1000, IntelHexRecordType.StartLinearAddress, new List<byte>()))
                });

            // Assert
            memoryStream.Position = 0;

            using (var sr = new StreamReader(memoryStream))
            {
                sr.ReadLine().Should().Be(":00100005EB");
                sr.ReadLine().Should().Be(EofRecord);
            }
        }

        [Fact]
        public void TestApplyInsertTransformBeforeAndAfter()
        {
            // Arrange
            _mockFileStream
                .Setup(_ => _.Exists(It.IsAny<string>()))
                .Returns(true);

            var memoryStream = new MemoryStream();

            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            _mockFileStream.Setup(_ => _.CreateText(It.IsAny<string>()))
                .Returns(streamWriter);

            _mockStreamReaderFactory.Setup(_ => _.Create(It.IsAny<string>()))
                .Returns(_mockStreamReader.Object);

            _mockStreamReader
                .SetupSequence(_ => _.ReadHexRecord())
                .Returns(new IntelHexRecord(0, IntelHexRecordType.ExtendedLinearAddress, new List<byte>() { 0xBE, 0xEF}))
                .Returns(new IntelHexRecord(0, IntelHexRecordType.EndOfFile, new List<byte>()));

            _mockRecordMatcher
                .SetupSequence(_ => _.IsMatch(It.IsAny<IntelHexRecordMatch>(), It.IsAny<IntelHexRecord>()))
                .Returns(true);

            _mockStreamReader
                .SetupSequence(_ => _.EndOfStream)
                .Returns(false)
                .Returns(true);

            // Act
            _transformer
                .ApplyTransforms("c:\\temp\\filename.hex", new List<Transform>()
                {
                    new InsertTransform(
                        new IntelHexRecordMatch(),
                        InsertPosition.BeforeAndAfter,
                        new IntelHexRecord(0x1000, IntelHexRecordType.StartLinearAddress, new List<byte>() { 0xDE, 0xAD}))
                });

            // Assert
            memoryStream.Position = 0;

            using (var sr = new StreamReader(memoryStream))
            {
                sr.ReadLine().Should().Be(":02100005DEAD5E");
                sr.ReadLine().Should().Be(":02000004BEEF4D");
                sr.ReadLine().Should().Be(":02100005DEAD5E");
                sr.ReadLine().Should().Be(EofRecord);
            }
        }

        [Fact]
        public void TestApplyModifyTransform()
        {
            // Arrange
            _mockFileStream
                .Setup(_ => _.Exists(It.IsAny<string>()))
                .Returns(true);

            var memoryStream = new MemoryStream();

            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true);

            _mockFileStream.Setup(_ => _.CreateText(It.IsAny<string>()))
                .Returns(streamWriter);

            _mockStreamReaderFactory.Setup(_ => _.Create(It.IsAny<string>()))
                .Returns(_mockStreamReader.Object);

            _mockStreamReader
                .SetupSequence(_ => _.ReadHexRecord())
                .Returns(new IntelHexRecord(0, IntelHexRecordType.ExtendedLinearAddress, new List<byte>()))
                .Returns(new IntelHexRecord(0, IntelHexRecordType.EndOfFile, new List<byte>()));

            _mockRecordMatcher
                .SetupSequence(_ => _.IsMatch(It.IsAny<IntelHexRecordMatch>(), It.IsAny<IntelHexRecord>()))
                .Returns(true)
                .Returns(false);

            _mockStreamReader
                .SetupSequence(_ => _.EndOfStream)
                .Returns(false)
                .Returns(true);

            // Act
            _transformer
                .ApplyTransforms("c:\\temp\\filename.hex", new List<Transform>()
                {
                    new ModificationTransform(
                        new IntelHexRecordMatch()
                        {
                            RecordType = IntelHexRecordType.ExtendedLinearAddress
                        },
                        0xAA55,
                        IntelHexRecordType.StartLinearAddress,
                        new List<byte>() { 0xFE, 0xED })
                });

            // Assert
            memoryStream.Position = 0;

            using (var sr = new StreamReader(memoryStream))
            {
                sr.ReadLine().Should().Be(":02AA5505FEED0F");
                sr.ReadLine().Should().Be(EofRecord);
            }
        }

        [Fact]
        public void TestFileNotFound()
        {
            // Arrange
            _mockFileStream
                .Setup(_ => _.Exists(It.IsAny<string>()))
                .Returns(false);

            // Act
            Action action = () => _transformer.ApplyTransforms("filename", new List<Transform>());

            // Assert
            action.Should().Throw<FileNotFoundException>();
        }

        [Fact]
        public void TestInvalidInputFileName()
        {
            // Arrange

            // Act
            Action action = () => _transformer.ApplyTransforms(null, null);

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>().And.ParamName.Should().Be("inputFile");
        }

        [Fact]
        public void TestInvalidTransforms()
        {
            // Arrange

            // Act
            Action action = () => _transformer.ApplyTransforms("filename", null);

            // Assert
            action.Should().Throw<ArgumentNullException>().And.ParamName.Should().Be("transforms");
        }
    }
}