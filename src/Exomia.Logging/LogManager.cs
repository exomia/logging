#region License

// Copyright (c) 2018-2019, exomia
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Exomia.Logging
{
    /// <summary> Manager for logs. </summary>
    public static class LogManager
    {
        private static readonly Dictionary<Type, Logger> s_typeLoggers;
        private static          Logger?[]                s_loggers;
        private static          int                      s_loggerCount;
        private static readonly Thread                   s_mainThread;

        /// <summary> Define the log directory for all file appenders. </summary>
        /// <value> The pathname of the log directory. </value>
        public static string LogDirectory { get; set; } = "./";

        /// <summary> MaxLogAge. </summary>
        /// <value> The maximum log age. </value>
        public static int MaxLogAge { get; set; } = 5 * 1000;

        /// <summary> MaxQueueSize. </summary>
        /// <value> The size of the maximum queue. </value>
        public static int MaxQueueSize { get; set; } = 100;

        static LogManager()
        {
            s_mainThread  = Thread.CurrentThread;
            s_typeLoggers = new Dictionary<Type, Logger>(16);
            s_loggers     = new Logger[16];
            new Thread(LoggingThread)
            {
                Name = "Exomia.Logging.LogManager", Priority = ThreadPriority.Lowest, IsBackground = false
            }.Start();
        }

        /// <summary> Gets a logger. </summary>
        /// <param name="type">        The type. </param>
        /// <param name="logAppender"> (Optional) The <see cref="LogAppender" /> </param>
        /// <param name="className">   (Optional) The class name. </param>
        /// <returns> The logger. </returns>
        public static ILogger GetLogger(Type    type, LogAppender logAppender = LogAppender.File | LogAppender.Console,
                                        string? className = null)
        {
            if (string.IsNullOrEmpty(className))
            {
                className = !type.IsGenericType
                    ? type.Name
                    : Regex.Replace(
                        type.Name, "`[0-9]+", string.Join(" ", type.GetGenericArguments().Select(v => v.Name)));
            }

            lock (s_typeLoggers)
            {
                if (!s_typeLoggers.TryGetValue(type, out Logger? logger))
                {
                    List<IAppender> appenders = new List<IAppender>();
                    if ((logAppender & LogAppender.File) == LogAppender.File)
                    {
                        appenders.Add(new FileAppender(className, LogDirectory, MaxQueueSize));
                    }
                    if ((logAppender & LogAppender.Console) == LogAppender.Console)
                    {
                        appenders.Add(new ConsoleAppender(className));
                    }
                    logger = new Logger(appenders.ToArray());
                    logger.PrepareLogging(DateTime.Now);
                    s_typeLoggers.Add(type, logger);

                    if (s_loggerCount + 1 >= s_loggers.Length)
                    {
                        Array.Resize(ref s_loggers, s_loggers.Length * 2);
                    }
                    s_loggers[s_loggerCount] = logger;
                    s_loggerCount++;
                }

                return logger;
            }
        }

        /// <summary> Gets a logger. </summary>
        /// <typeparam name="T"> Generic type parameter. </typeparam>
        /// <param name="logAppender"> (Optional) The <see cref="LogAppender" /> </param>
        /// <param name="className">   (Optional) The class name. </param>
        /// <returns> The logger. </returns>
        public static ILogger GetLogger<T>(LogAppender logAppender = LogAppender.File | LogAppender.Console,
                                           string?     className   = null)
            where T : class
        {
            return GetLogger(typeof(T), logAppender, className);
        }

        private static void LoggingThread()
        {
            DateTime  current = DateTime.Now;
            Stopwatch sw      = new Stopwatch();

            while (s_mainThread.IsAlive)
            {
                DateTime now = DateTime.Now;
                if (current.Day != now.Day)
                {
                    current = now;
                    for (int i = s_loggerCount - 1; i >= 0 && s_mainThread.IsAlive; i--)
                    {
                        Logger? logger = s_loggers[i];
                        if (logger != null)
                        {
                            logger.Flush(true);
                            logger.PrepareLogging(now);
                        }
                    }
                }
                sw.Restart();
                while (sw.Elapsed.TotalMilliseconds < MaxLogAge)
                {
                    for (int i = s_loggerCount - 1; i >= 0 && s_mainThread.IsAlive; i--)
                    {
                        s_loggers[i]?.Flush(false);
                    }
                    Thread.Sleep(1);
                }
                for (int i = s_loggerCount - 1; i >= 0 && s_mainThread.IsAlive; i--)
                {
                    s_loggers[i]?.Flush(true);
                }
            }

            for (int i = s_loggerCount - 1; i >= 0; i--)
            {
                s_loggers[i]?.Dispose();
                s_loggers[i] = null;
            }
            s_loggerCount = 0;
        }
    }
}