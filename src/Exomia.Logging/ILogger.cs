#region License

// Copyright (c) 2018-2019, exomia
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

#endregion

using System;
using System.Runtime.CompilerServices;

namespace Exomia.Logging
{
    /// <summary> Interface for logger. </summary>
    public interface ILogger : IDisposable
    {
        /// <summary> a trace log. </summary>
        /// <param name="message">          Message. </param>
        /// <param name="memberName">       (Optional) member name. </param>
        /// <param name="sourceFilePath">   (Optional) source file path. </param>
        /// <param name="sourceLineNumber"> (Optional) source line number. </param>
        void Trace(string                    message,
                   [CallerMemberName] string memberName       = "",
                   [CallerFilePath]   string sourceFilePath   = "",
                   [CallerLineNumber] int    sourceLineNumber = 0);

        /// <summary> a trace log. </summary>
        /// <param name="ex">               Exception. </param>
        /// <param name="memberName">       (Optional) member name. </param>
        /// <param name="sourceFilePath">   (Optional) source file path. </param>
        /// <param name="sourceLineNumber"> (Optional) source line number. </param>
        void Trace(Exception                 ex,
                   [CallerMemberName] string memberName       = "",
                   [CallerFilePath]   string sourceFilePath   = "",
                   [CallerLineNumber] int    sourceLineNumber = 0);

        /// <summary> a debug log. </summary>
        /// <param name="message">          Message. </param>
        /// <param name="memberName">       (Optional) member name. </param>
        /// <param name="sourceFilePath">   (Optional) source file path. </param>
        /// <param name="sourceLineNumber"> (Optional) source line number. </param>
        void Debug(string                    message,
                   [CallerMemberName] string memberName       = "",
                   [CallerFilePath]   string sourceFilePath   = "",
                   [CallerLineNumber] int    sourceLineNumber = 0);

        /// <summary> a debug log. </summary>
        /// <param name="ex">               Exception. </param>
        /// <param name="memberName">       (Optional) member name. </param>
        /// <param name="sourceFilePath">   (Optional) source file path. </param>
        /// <param name="sourceLineNumber"> (Optional) source line number. </param>
        void Debug(Exception                 ex,
                   [CallerMemberName] string memberName       = "",
                   [CallerFilePath]   string sourceFilePath   = "",
                   [CallerLineNumber] int    sourceLineNumber = 0);

        /// <summary> a info log. </summary>
        /// <param name="message">          Message. </param>
        /// <param name="memberName">       (Optional) member name. </param>
        /// <param name="sourceFilePath">   (Optional) source file path. </param>
        /// <param name="sourceLineNumber"> (Optional) source line number. </param>
        void Info(string                    message,
                  [CallerMemberName] string memberName       = "",
                  [CallerFilePath]   string sourceFilePath   = "",
                  [CallerLineNumber] int    sourceLineNumber = 0);

        /// <summary> a info log. </summary>
        /// <param name="ex">               Exception. </param>
        /// <param name="memberName">       (Optional) member name. </param>
        /// <param name="sourceFilePath">   (Optional) source file path. </param>
        /// <param name="sourceLineNumber"> (Optional) source line number. </param>
        void Info(Exception                 ex,
                  [CallerMemberName] string memberName       = "",
                  [CallerFilePath]   string sourceFilePath   = "",
                  [CallerLineNumber] int    sourceLineNumber = 0);

        /// <summary> a warning log. </summary>
        /// <param name="message">          Message. </param>
        /// <param name="memberName">       (Optional) member name. </param>
        /// <param name="sourceFilePath">   (Optional) source file path. </param>
        /// <param name="sourceLineNumber"> (Optional) source line number. </param>
        void Warning(string                    message,
                     [CallerMemberName] string memberName       = "",
                     [CallerFilePath]   string sourceFilePath   = "",
                     [CallerLineNumber] int    sourceLineNumber = 0);

        /// <summary> a warning log. </summary>
        /// <param name="ex">               Exception. </param>
        /// <param name="memberName">       (Optional) member name. </param>
        /// <param name="sourceFilePath">   (Optional) source file path. </param>
        /// <param name="sourceLineNumber"> (Optional) source line number. </param>
        void Warning(Exception                 ex,
                     [CallerMemberName] string memberName       = "",
                     [CallerFilePath]   string sourceFilePath   = "",
                     [CallerLineNumber] int    sourceLineNumber = 0);

        /// <summary> a error log. </summary>
        /// <param name="message">          Message. </param>
        /// <param name="memberName">       (Optional) member name. </param>
        /// <param name="sourceFilePath">   (Optional) source file path. </param>
        /// <param name="sourceLineNumber"> (Optional) source line number. </param>
        void Error(string                    message,
                   [CallerMemberName] string memberName       = "",
                   [CallerFilePath]   string sourceFilePath   = "",
                   [CallerLineNumber] int    sourceLineNumber = 0);

        /// <summary> a error log. </summary>
        /// <param name="ex">               Exception. </param>
        /// <param name="memberName">       (Optional) member name. </param>
        /// <param name="sourceFilePath">   (Optional) source file path. </param>
        /// <param name="sourceLineNumber"> (Optional) source line number. </param>
        void Error(Exception                 ex,
                   [CallerMemberName] string memberName       = "",
                   [CallerFilePath]   string sourceFilePath   = "",
                   [CallerLineNumber] int    sourceLineNumber = 0);

        /// <summary> Flushes the Queue to the physical log file. </summary>
        /// <param name="force"> <c>true</c> if flush should happen immediately; <c>false</c> otherwise. </param>
        void Flush(bool force);
    }
}