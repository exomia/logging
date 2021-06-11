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
    /// <summary> Bitfield of flags for specifying LogAppender. </summary>
    [Flags]
    public enum LogAppender
    {
        /// <summary> A binary constant representing the file flag. </summary>
        File = 1 << 0,

        /// <summary> A binary constant representing the console flag. </summary>
        Console = 1 << 1
    }
}