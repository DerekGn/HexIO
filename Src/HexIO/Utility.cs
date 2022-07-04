using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HexIO;
using Microsoft.VisualBasic;

namespace MemoryFileManager
{
    static class Utility
    {
        public static bool AddressInHexRecord(this IntelHexRecord record, uint address)
        {
            return (address >= record.Address && address < record.Address + record.ByteCount);
        }

        public static void Write(this IntelHexRecord record, uint address, byte byteToWrite)
        {
            record.Data[(int)(address - record.Address)] = byteToWrite;
        }

        public static string GetStringToWrite(this IntelHexRecord record)
        {
            StringBuilder recordAsString = new StringBuilder();
            recordAsString.Append(":");
            recordAsString.Append(record.ByteCount.ToString("X2"));
            recordAsString.Append(record.Address.ToString("X4"));
            recordAsString.Append(((byte)record.RecordType).ToString("X2"));
            recordAsString.Append(ByteArrayToString(record.Data.ToArray()));
            recordAsString.Append(record.CheckSum.ToString("X2"));
            return recordAsString.ToString();
        }

        public static string ByteArrayToString(byte[] byteArray)
        {
            StringBuilder res = new StringBuilder();
            foreach (byte byteToConvert in byteArray)
            {
                res.Append(byteToConvert.ToString("X2"));
            }
            return res.ToString();
        }
    }
}
