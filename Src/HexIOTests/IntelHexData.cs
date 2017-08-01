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

using System.IO;

namespace HexIOTests
{
    internal static class IntelHexData
    {
        public static MemoryStream I8HexData
        {
            get
            {
                var ms = new MemoryStream();
                var sw = new StreamWriter(ms);

                sw.WriteLine(":10010000214601360121470136007EFE09D2190140");
                sw.WriteLine(":100110002146017E17C20001FF5F16002148011928");
                sw.WriteLine(":10012000194E79234623965778239EDA3F01B2CAA7");
                sw.WriteLine(":100130003F0156702B5E712B722B732146013421C7");
                sw.WriteLine(":00000001FF");

                sw.Flush();
                ms.Position = 0;

                return ms;
            }
        }

        public static MemoryStream I16HexData
        {
            get
            {
                var ms = new MemoryStream();
                var sw = new StreamWriter(ms);

                sw.WriteLine(":020000020000FC");
                sw.WriteLine(":020000021000EC");
                sw.WriteLine(":020000022000DC");
                sw.WriteLine(":020000023000CC");
                sw.WriteLine(":00000001FF");

                sw.Flush();
                ms.Position = 0;

                return ms;
            }
        }

        public static MemoryStream I32HexData
        {
            get
            {
                var ms = new MemoryStream();
                var sw = new StreamWriter(ms);
               
                sw.WriteLine(":020000040000FA");
                sw.WriteLine(":020000040001F9");
                sw.WriteLine(":020000040002F8");
                sw.WriteLine(":020000040003F7");
                sw.WriteLine(":020000040004F6");
                sw.WriteLine(":00000001FF");
                
                sw.Flush();
                ms.Position = 0;

                return ms;
            }
        }
    }
}
