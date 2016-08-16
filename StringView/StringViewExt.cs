// Copyright (c) egmkang wang. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

public static partial class Ext
{
    public unsafe static long ToInt64(this StringView view)
    {
        bool negative = false;
        long num = 0;
        if (view.Length <= 0) return num;
        fixed (char* stringPointer = view.Original)
        {
            int left = view.Length;
            char* p = stringPointer + view.Offset;
            int i = 0;
            if (p[0] == '+') { ++i; --left; }
            if (p[0] == '-') { ++i; --left; negative = true; }
            while(left >= 4)
            {
                if (p[i + 0] < '0' || p[i + 0] > '9' ||
                    p[i + 1] < '0' || p[i + 1] > '9' ||
                    p[i + 2] < '0' || p[i + 2] > '9' ||
                    p[i + 3] < '0' || p[i + 3] > '9')
                {
                    throw new ArgumentException(String.Format("Wrong Number Char:{0}{1}{2}{3}"
                        , p[i + 0], p[i + 1], p[i + 2], p[i + 3]));
                }
                num = num * 10000 +
                    (p[i + 0] - '0') * 1000 +
                    (p[i + 1] - '0') * 100 +
                    (p[i + 2] - '0') * 10 +
                    (p[i + 3] - '0');
                i += 4;
                left -= 4;
            }
            for (; i < view.Length; ++i)
            {
                if (p[i] < '0' || p[i] > '9')
                {
                    throw new ArgumentException(String.Format("Wrong Number Char:{0}", p[i]));
                }
                num = num * 10 + (p[i] - '0');
            }
        }

        return negative ? -num : num;
    }
    public unsafe static int ToInt32(this StringView view)
    {
        return (int)view.ToInt64();
    }

    public static void Append(this System.Text.StringBuilder builder, StringView view)
    {
        builder.Append(view.Original, view.Offset, view.Length);
    }
}
