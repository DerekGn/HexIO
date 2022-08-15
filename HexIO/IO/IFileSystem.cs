/*
* MIT License
*
* Copyright (c) 2022 Derek Goslin https://github.com/DerekGn
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

namespace HexIO.IO
{
    /// <summary>
    /// Provides a mechanism to interact with the file system
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="path">The name of the file to be deleted. Wildcard characters are not supported.</param>
        void Delete(string path);
        /// <summary>
        /// Creates or opens a file for writing UTF-8 encoded text. If the file already exists,
        /// its contents are overwritten.
        /// </summary>
        /// <param name="path">The file to be opened for writing.</param>
        /// <returns>A <see cref="StreamWriter"/> that writes to the specified file using UTF-8 encoding.</returns>
        StreamWriter CreateText(string path);

        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">The file to check.</param>
        /// <returns>true if the caller has the required permissions and path contains the name of
        /// an existing file; otherwise, false. This method also returns false if path is
        /// null, an invalid path, or a zero-length string. If the caller does not have sufficient
        /// permissions to read the specified file, no exception is thrown and the method
        /// returns false regardless of the existence of path.</returns>
        bool Exists(string path);

        /// <summary>
        /// Moves a specified file to a new location, providing the option to specify a new file name.
        /// </summary>
        /// <param name="sourceFileName">The name of the file to move. Can include a relative or absolute path.</param>
        /// <param name="destFileName">The new path and name for the file.</param>
        void Move(string sourceFileName, string destFileName);
    }
}