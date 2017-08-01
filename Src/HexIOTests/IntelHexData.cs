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
        public static MemoryStream SimpleStream
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

        public static MemoryStream CompleteStream
        {
            get
            {
                var ms = new MemoryStream();
                var sw = new StreamWriter(ms);
                
                sw.WriteLine(":10001300AC12AD13AE10AF1112002F8E0E8F0F2244\r");
                sw.WriteLine(":10000300E50B250DF509E50A350CF5081200132259\r");
                sw.WriteLine(":03000000020023D8");
                sw.WriteLine(":0C002300787FE4F6D8FD7581130200031D\r");
                sw.WriteLine(":10002F00EFF88DF0A4FFEDC5F0CEA42EFEEC88F016\r");
                sw.WriteLine(":04003F00A42EFE22CB\r");
                sw.WriteLine(":00000001FF\r");
                
                sw.Flush();
                ms.Position = 0;

                return ms;
            }
        }
    }
}
