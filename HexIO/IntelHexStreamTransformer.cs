/*
* MIT License
*
* Copyright (c) 2022 Derek Goslin http://corememorydump.blogspot.ie/
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

using HexIO.Factories;
using HexIO.IO;
using HexIO.Matching;
using HexIO.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HexIO
{
    /// <summary>
    /// An Intel Hex stream transformer
    /// </summary>
    public class IntelHexStreamTransformer : IIntelHexStreamTransformer
    {
        private readonly IIntelHexStreamReaderFactory _intelHexStreamReaderFactory;
        private readonly IIntelHexRecordMatcher _intelHexRecordMatcher;
        private readonly IStreamWriterFactory _streamWriterFactory;
        private readonly IFileSystem _fileSystem;

        public IntelHexStreamTransformer(
            IFileSystem fileSystem,
            IStreamWriterFactory streamWriterFactory,
            IIntelHexRecordMatcher intelHexRecordMatcher,
            IIntelHexStreamReaderFactory intelHexStreamReaderFactory)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _streamWriterFactory = streamWriterFactory ?? throw new ArgumentNullException(nameof(streamWriterFactory));
            _intelHexRecordMatcher = intelHexRecordMatcher ?? throw new ArgumentNullException(nameof(intelHexRecordMatcher));
            _intelHexStreamReaderFactory = intelHexStreamReaderFactory ?? throw new ArgumentNullException(nameof(intelHexStreamReaderFactory));
        }

        /// <inheritdoc/>
        public string ApplyTransforms(string inputFile, IList<Transform> transforms)
        {
            if (string.IsNullOrWhiteSpace(inputFile))
            {
                throw new ArgumentOutOfRangeException(nameof(inputFile));
            }

            if (transforms is null)
            {
                throw new ArgumentNullException(nameof(transforms));
            }

            if (!_fileSystem.Exists(inputFile))
            {
                throw new FileNotFoundException("File not found", inputFile);
            }

            string tempFileName = null;
            string sourceFileName = inputFile;
            string path = Path.GetDirectoryName(inputFile);

            transforms.ToList().ForEach(transform =>
            {
                tempFileName = Path.Combine(path, Guid.NewGuid().ToString());

                using (StreamWriter streamWriter = _streamWriterFactory.Create(tempFileName))
                {
                    using (IIntelHexStreamReader intelHexStreamReader = _intelHexStreamReaderFactory.Create(sourceFileName))
                    {
                        do
                        {
                            var intelHexRecord = intelHexStreamReader.ReadHexRecord();

                            if (_intelHexRecordMatcher.IsMatch(transform.Match, intelHexRecord))
                            {
                                ApplyTransform(transform, streamWriter, intelHexRecord);
                            }
                            else
                            {
                                streamWriter.WriteLine(intelHexRecord.ToHexRecordString());
                            }
                        } while (!intelHexStreamReader.EndOfStream);
                    }
                }

                sourceFileName = tempFileName;
            });

            var transformedFileName = Path.Combine(path, $"{inputFile}.transformed");

            _fileSystem.Rename(tempFileName, transformedFileName);

            return transformedFileName;
        }

        private void ApplyTransform(
            Transform transform,
            StreamWriter streamWriter,
            IntelHexRecord intelHexRecord)
        {
            if (transform is InsertTransform insert)
            {
                if (insert.Position == InsertPosition.After)
                {
                    streamWriter.WriteLine(intelHexRecord.ToHexRecordString());
                    streamWriter.WriteLine(insert.Record.ToHexRecordString());
                }
                else if (insert.Position == InsertPosition.Before)
                {
                    streamWriter.WriteLine(insert.Record.ToHexRecordString());
                    streamWriter.WriteLine(intelHexRecord.ToHexRecordString());
                }
                else if (insert.Position == InsertPosition.BeforeAndAfter)
                {
                    streamWriter.WriteLine(insert.Record);
                    streamWriter.WriteLine(intelHexRecord);
                    streamWriter.WriteLine(insert.Record);
                }
            }
            else if (transform is ModificationTransform modification)
            {
                var modifiedRecord = modification.Apply(intelHexRecord);
                streamWriter.WriteLine(modifiedRecord.ToHexRecordString());
            }
        }
    }
}