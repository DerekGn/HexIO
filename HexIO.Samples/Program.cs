﻿/*
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
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;

namespace HexIO.Samples
{
    public static class Program
    {
        public static void Main()
        {
            try
            {
                //setup our DI
                var serviceProvider = new ServiceCollection()
                    .AddSingleton<IIntelHexStreamReaderFactory, IntelHexStreamReaderFactory>()
                    .AddSingleton<IIntelHexStreamTransformer, IntelHexStreamTransformer>()
                    .AddSingleton<IIntelHexRecordMatcher, IntelHexRecordMatcher>()
                    .AddSingleton<IFileSystem, FileSystem>()
                    .BuildServiceProvider();

                Console.WriteLine("Executing Reader example\r\n");
                var readExample = new IntelHexStreamReaderExample();
                readExample.Execute(new IntelHexStreamReader(new FileStream("sample.hex", FileMode.Open)));

                Console.WriteLine("\r\n".PadLeft(80, '='));

                Console.WriteLine("Executing Writer example\r\n");
                var writeExample = new IntelHexStreamWriterExample();
                var memoryStream = new MemoryStream();
                writeExample.Execute(new IntelHexStreamWriter(memoryStream, Encoding.UTF8, 1024, true), memoryStream);

                Console.WriteLine("Execute Transformer Example");
                var transformExample = new IntelHexStreamTransformerExample();
                var transformer = serviceProvider.GetService<IIntelHexStreamTransformer>();
                transformExample.Execute(transformer);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An exception occurred {ex}");
            }
        }
    }
}
