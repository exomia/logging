#region License

// Copyright (c) 2018-2019, exomia
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

#endregion

using System;

namespace Exomia.Logging
{
    interface IAppender : IDisposable
    {
        /// <summary> Adds an object onto the end of this queue. </summary>
        /// <param name="logType">          Type of the log. </param>
        /// <param name="message">          The message. </param>
        /// <param name="memberName">       Name of the member. </param>
        /// <param name="sourceFilePath">   Full pathname of the source file. </param>
        /// <param name="sourceLineNumber"> Source line number. </param>
        void Enqueue(LogType logType, string message, string memberName, string sourceFilePath,
                     int     sourceLineNumber);

        /// <summary> Flushes the given force. </summary>
        /// <param name="force"> True to force. </param>
        void Flush(bool force);

        /// <summary> Prepare logging. </summary>
        /// <param name="dateTime"> The date time. </param>
        void PrepareLogging(DateTime dateTime);
    }
}