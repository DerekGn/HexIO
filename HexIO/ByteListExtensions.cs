using System;
using System.Collections.Generic;
using System.Linq;

namespace HexIO
{
    /// <summary>
    /// Extension methods for a <see cref="IList{T}"/> of bytes
    /// </summary>
    public static class ByteListExtensions
    {
        /// <summary>
        /// Convert a list of hex bytes to a hex string
        /// </summary>
        /// <param name="data">The list of bytes to convert</param>
        /// <returns>The hex string</returns>
        public static string ToHexString(this IList<byte> data)
        {
            return BitConverter.ToString(data.ToArray()).Replace("-", "");
        }
    }
}
