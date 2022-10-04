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
using HexIO.Properties;
using HexIO.Transforms;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Cerate an instance of a <see cref="IntelHexStreamTransformer"/>
        /// </summary>
        /// <param name="fileSystem">An <see cref="IFileSystem"/> instance for file IO</param>
        /// <param name="intelHexRecordMatcher"></param>
        /// <param name="intelHexStreamReaderFactory"></param>
        [ExcludeFromCodeCoverage]
        public IntelHexStreamTransformer(
            IFileSystem fileSystem,
            IIntelHexRecordMatcher intelHexRecordMatcher,
            IIntelHexStreamReaderFactory intelHexStreamReaderFactory)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
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
            
            if(transforms.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(transforms), Resources.TransformsExpected);
            }

            if (!_fileSystem.Exists(inputFile))
            {
                throw new FileNotFoundException(Resources.FileNotFound, inputFile);
            }

            string tempFileName = null;
            string sourceFileName = inputFile;
            string path = Path.GetDirectoryName(inputFile);

            transforms.ToList().ForEach(transform =>
            {
                tempFileName = Path.Combine(path, Guid.NewGuid().ToString());

                using (StreamWriter streamWriter = _fileSystem.CreateText(tempFileName))
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

            var transformedFileName = Path.Combine(path,
                $"{Path.GetFileNameWithoutExtension(inputFile)}.transformed{Path.GetExtension(inputFile)}");

            if(_fileSystem.Exists(transformedFileName))
            {
                _fileSystem.Delete(transformedFileName);
            }

            _fileSystem.Move(tempFileName, transformedFileName);

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
                    streamWriter.WriteLine(insert.Record.ToHexRecordString());
                    streamWriter.WriteLine(intelHexRecord.ToHexRecordString());
                    streamWriter.WriteLine(insert.Record.ToHexRecordString());
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