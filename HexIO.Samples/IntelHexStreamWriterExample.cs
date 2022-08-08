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

using System;
using System.Collections.Generic;
using System.IO;

namespace HexIO.Samples
{
    internal class IntelHexStreamWriterExample
    {
        public void Execute(IIntelHexStreamWriter hexStreamWriter, MemoryStream memoryStream)
        {
            hexStreamWriter.WriteDataRecord(0, new List<byte>() { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A });
            hexStreamWriter.WriteDataRecord(0x0B, new List<byte>() { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A });

            hexStreamWriter.WriteExtendedLinearAddressRecord(0x1000);
            hexStreamWriter.WriteExtendedSegmentAddressRecord(0x2000);
            hexStreamWriter.WriteStartLinearAddressRecord(0x3000);
            hexStreamWriter.WriteStartSegmentAddressRecord(0x4000, 0x5000);

            hexStreamWriter.Close();

            memoryStream.Flush();
            memoryStream.Position = 0;

            var readStream = new MemoryStream(memoryStream.ToArray());

            using StreamReader reader = new StreamReader(readStream);
            Console.WriteLine(reader.ReadToEnd());
        }
    }
}