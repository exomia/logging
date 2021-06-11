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
    sealed class ConsoleAppender : IAppender
    {
        private readonly string _className;
        
        /// <summary> Initializes a new instance of the <see cref="ConsoleAppender"/> class. </summary>
        /// <param name="className"> Name of the class. </param>
        public ConsoleAppender(string className)
        {
            _className = className;
        }

        /// <inheritdoc/>
        public void Enqueue(LogType logType, string message, string memberName, string sourceFilePath,
                            int     sourceLineNumber)
        {
            ConsoleColor current = Console.ForegroundColor;
            Console.ForegroundColor = logType switch
            {
                LogType.Trace   => ConsoleColor.Magenta,
                LogType.Debug   => ConsoleColor.Cyan,
                LogType.Info    => ConsoleColor.White,
                LogType.Warning => ConsoleColor.Yellow,
                LogType.Error   => ConsoleColor.Red,
                _               => Console.ForegroundColor
            };
            Console.Out.WriteLine(
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|{_className}|{logType} [{memberName}|{sourceFilePath}:{sourceLineNumber}] {message}");
            Console.ForegroundColor = current;
        }

        /// <inheritdoc/>
        public void Flush(bool force)
        {
            if (force)
            {
                Console.Out.Flush();
            }
        }

        /// <inheritdoc/>
        public void PrepareLogging(DateTime dateTime)
        {
            Console.Out.WriteLine($"== log started {_className} {dateTime:yyyy-MM-dd HH:mm:ss} ==");
        }

        #region IDisposable Support

        private bool _disposedValue;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ConsoleAppender()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;
                if (disposing)
                {
                    Flush(true);
                }
            }
        }

        #endregion
    }
}