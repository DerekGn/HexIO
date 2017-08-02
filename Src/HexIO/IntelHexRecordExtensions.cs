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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HexIO
{
    internal static class IntelHexRecordExtensions
    {
        public static IntelHexRecord ParseHexRecord(this string hexRecord)
        {
            if (hexRecord == null) throw new IOException("Hex record line can not be null");
            if (hexRecord.Length < 11) throw new IOException($"Hex record line length [{hexRecord}] is less than 11");
            if (!hexRecord.StartsWith(":")) throw new IOException($"Illegal line start character [{hexRecord}]");

            var hexData = TryParseData(hexRecord.Substring(1));

            if (hexData.Count != hexData[0] + 5)
                throw new IOException($"Line [{hexRecord}] does not have required record length of [{hexData[0] + 5}]");

            if (!Enum.IsDefined(typeof(IntelHexRecordType), (int)hexData[3]))
                throw new IOException($"Invalid record type value: [{hexData[3]}]");

            var checkSum = hexData[hexData[0] + 4];
            hexData.RemoveAt(hexData[0] + 4);

            if (!VerifyChecksum(hexData, checkSum))
                throw new IOException($"Checksum for line [{hexRecord}] is incorrect");

            var dataSize = hexData[0];
            
            var newRecord = new IntelHexRecord
            {
                ByteCount = dataSize,
                Address = (uint) (hexData[1] << 8 | hexData[2]),
                RecordType = (IntelHexRecordType)hexData[3],
                Data = hexData,
                CheckSum = checkSum
            };

            hexData.RemoveRange(0, 4);

            return newRecord;
        }

        private static bool VerifyChecksum(List<byte> checkSumData, int checkSum)
        {
            var maskedSumBytes = checkSumData.Sum(x => x) & 0xff;
            var checkSumCalculated = (byte)(256 - maskedSumBytes);
            
            return checkSumCalculated == checkSum;
        }

        private static List<byte> TryParseData(string hexData)
        {
            try
            {
                List<byte> data = new List<byte>();

                for (int i = 0; i < hexData.Length; i++)
                {
                    data.Add(Convert.ToByte(hexData.Substring(i, 2), 16));
                    i++;
                }

                return data;
            }
            catch (Exception ex)
            {
                throw new IOException($"Unable to extract bytes for [{hexData}]", ex);
            }
        }
    }
}
