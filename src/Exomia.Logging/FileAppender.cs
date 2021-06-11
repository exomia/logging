#region License

// Copyright (c) 2018-2019, exomia
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

#endregion

using System;
using System.IO;
using System.Text;

namespace Exomia.Logging
{
    sealed class FileAppender : IAppender
    {
        private readonly string        _className;
        private readonly string        _currentLogDirectory;
        private readonly int           _maxQueueSize;
        private readonly Queue<string> _queue;
        private readonly Queue<string> _tempQueue;
        private          string        _currentLogFile = string.Empty;
        private          FileStream?   _fileStream;

        /// <summary> Gets the current log file. </summary>
        /// <value> The current log file. </value>
        public string CurrentLogFile
        {
            get { return _currentLogFile; }
        }

        /// <summary> Initializes a new instance of the <see cref="FileAppender"/> class. </summary>
        /// <param name="className">    Name of the class. </param>
        /// <param name="logDirectory"> Pathname of the log directory. </param>
        /// <param name="maxQueueSize"> Size of the maximum queue. </param>
        public FileAppender(string className, string logDirectory, int maxQueueSize)
        {
            _className           = className;
            _currentLogDirectory = logDirectory;
            _maxQueueSize        = maxQueueSize;

            _queue     = new Queue<string>(32);
            _tempQueue = new Queue<string>(32);
        }

        public void Enqueue(LogType logType,
                            string  message,
                            string  memberName,
                            string  sourceFilePath,
                            int     sourceLineNumber)
        {
            lock (_queue)
            {
                _queue.Enqueue(
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}|{_className}|{logType} [{memberName}|{sourceFilePath}:{sourceLineNumber}] {message}");
            }
        }

        /// <inheritdoc/>
        public void Flush(bool force)
        {
            lock (_queue)
            {
                if (!force && _queue.Count < _maxQueueSize) { return; }
                _tempQueue.Clear(_queue);
                _queue.Clear();
            }
            while (_tempQueue.Count > 0)
            {
                byte[] b = Encoding.Default.GetBytes(_tempQueue.Dequeue() + Environment.NewLine);
                _fileStream!.Write(b, 0, b.Length);
            }
            _fileStream!.Flush();
        }

        /// <inheritdoc/>
        public void PrepareLogging(DateTime dateTime)
        {
            if (_fileStream != null)
            {
                _fileStream.Close();
                _fileStream.Dispose();
                _fileStream = null;
            }
            _currentLogFile = _className + "_" + dateTime.ToString("yyyy-MM-dd") + ".log";
            if (!Directory.Exists(_currentLogDirectory))
            {
                Directory.CreateDirectory(_currentLogDirectory);
            }
            _fileStream = new FileStream(
                Path.Combine(_currentLogDirectory, _currentLogFile), FileMode.Append, FileAccess.Write);
        }

        #region IDisposable Support

        private bool _disposedValue;

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FileAppender()
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
                    if (_fileStream != null)
                    {
                        Flush(true);
                        _fileStream.Close();
                        _fileStream.Dispose();
                        _fileStream = null;
                    }
                }
            }
        }

        #endregion
    }
}