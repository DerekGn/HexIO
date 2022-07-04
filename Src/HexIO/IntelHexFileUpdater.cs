using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HexIO;

namespace MemoryFileManager
{
    public class HexFileManager
    {
        private FileStream fileStream;
        private IntelHexReader hexReader;
        private List<IntelHexRecord> fileByRecord;
        private string pathToWriteNewHexFile;

        public HexFileManager(string pathToFile, string pathToNewFile)
        {
            fileStream = new FileStream(pathToFile, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            hexReader = new IntelHexReader(fileStream);
            fileByRecord = TransformHexFileToHexRecordList(hexReader);
            pathToWriteNewHexFile = pathToNewFile;
        }

        private List<IntelHexRecord> TransformHexFileToHexRecordList(IntelHexReader hexReader)
        {
            List<IntelHexRecord> res = new List<IntelHexRecord>();
            IntelHexRecord currentRecord;
            while (hexReader.Read(out currentRecord))
            {
                res.Add(currentRecord);
            }
            return res;
        }

        /*
        * Given dictionnary { address : byte }
        * Substitutes the byte in the record list 
        * generated in the constructor
        */
        public void WriteByteByAddress(Dictionary<uint, byte> bytesToWrite)
        {
            foreach (KeyValuePair<uint, byte> valuePair in bytesToWrite)
            {
                WriteByteByAddress(valuePair.Key, valuePair.Value);
            }
            WriteNewHexFile();
        }

        private void WriteByteByAddress(uint address, byte byteToWrite)
        {
            //TODO: inefficient cause calculates checksum many times for same line --> one solution: calculate CRC when writing file
            IntelHexRecord record = fileByRecord.Find(record => record.AddressInHexRecord(address));
            record.Write(address, byteToWrite);
            record.CheckSum = CalculateChecksum(record);
        }

        public static byte CalculateChecksum(IntelHexRecord record)
        {
            var addresBytes = BitConverter.GetBytes(record.Address);
            var hexRecordData = new List<byte>
            {
                (byte)record.ByteCount,
                addresBytes[1],
                addresBytes[0],
                (byte)record.RecordType
            };
            hexRecordData.AddRange(record.Data);
            int maskedSumBytes = hexRecordData.Sum(x => x) & 0xFF;
            byte calculatedChecksum = (byte)(256 - maskedSumBytes);
            
            return calculatedChecksum;
        }

        /*
        * Writes the new hexfile produced by WriteByAdress method
        */
        private void WriteNewHexFile()
        {
            int count = 0;
            using (StreamWriter fileStreamToWrite = new StreamWriter(pathToWriteNewHexFile))
            {
                foreach (IntelHexRecord record in fileByRecord)
                {
                    fileStreamToWrite.Write(record.GetStringToWrite());
                    count++;
                    fileStreamToWrite.Write(Environment.NewLine);
                }
            }
        }
    }
}
