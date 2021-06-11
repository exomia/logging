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
    sealed class Logger : ILogger
    {
        private readonly IAppender[] _appenders;

        public Logger(IAppender[] appenders)
        {
            _appenders = appenders;
        }

        /// <inheritdoc/>
        public void Trace(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Trace, ex.StackTrace!, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc/>
        public void Trace(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Trace, message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc/>
        public void Debug(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Debug, ex.StackTrace!, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc/>
        public void Debug(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Debug, message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc/>
        public void Info(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Info, ex.StackTrace!, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc/>
        public void Info(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Info, message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc/>
        public void Warning(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Warning, ex.StackTrace!, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc/>
        public void Warning(string message, string memberName = "", string sourceFilePath = "",
                            int    sourceLineNumber = 0)
        {
            Internal(LogType.Warning, message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc/>
        public void Error(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Error, ex.StackTrace!, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc/>
        public void Error(string message, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            Internal(LogType.Error, message, memberName, sourceFilePath, sourceLineNumber);
        }

        /// <inheritdoc/>
        public void Flush(bool force)
        {
            for (int i = 0; i < _appenders.Length; i++)
            {
                _appenders[i].Flush(force);
            }
        }

        /// <summary> Prepare logging. </summary>
        /// <param name="dateTime"> The date time. </param>
        public void PrepareLogging(DateTime dateTime)
        {
            for (int i = 0; i < _appenders.Length; i++)
            {
                _appenders[i].PrepareLogging(dateTime);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Internal(LogType logType, string message, string memberName, string sourceFilePath,
                              int     sourceLineNumber)
        {
            for (int i = 0; i < _appenders.Length; i++)
            {
                _appenders[i].Enqueue(logType, message, memberName, sourceFilePath, sourceLineNumber);
            }
        }

        #region IDisposable Support

        /// <summary>
        ///     True to disposed value.
        /// </summary>
        private bool _disposedValue;

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    for (int i = 0; i < _appenders.Length; i++)
                    {
                        _appenders[i].Dispose();
                    }
                }
                _disposedValue = true;
            }
        }

        ~Logger()
        {
            Dispose(false);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}