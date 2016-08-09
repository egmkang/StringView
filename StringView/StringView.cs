// Copyright (c) egmkang wang. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

public unsafe struct StringView
{
    public static readonly StringView Empty = new StringView("");

    public StringView(string str) : this(str, 0, str.Length) { }

    public StringView(string str, int begin, int length)
    {
        this.str = str;
        this.offset = begin;
        this.length = length;
        if (str.Length <= 0) return;

        if (this.offset < 0 || this.offset >= this.str.Length) throw new ArgumentOutOfRangeException("begin");
        if (this.offset + this.length > this.str.Length) throw new ArgumentOutOfRangeException("length");
    }

    public int IndexOf(char c)
    {
        return this.IndexOf(c, 0, this.length);
    }

    public int IndexOf(char c, int offset)
    {
        return this.IndexOf(c, offset, this.length - offset);
    }

    public int IndexOf(char c, int offset, int count)
    {
        if (offset < 0 || offset >= this.length) throw new ArgumentOutOfRangeException("offset");
        if (count < 0 || count - 1 < offset) throw new ArgumentOutOfRangeException("count");

        fixed (char* p = this.str)
        {
            int length = System.Math.Min(this.length, count);
            for (int i = offset; i < length; ++i)
            {
                if (p[this.offset + i] == c) return i;
            }
        }

        return -1;
    }


    private static bool ArrayContains(char[] array, char c)
    {
        int length = array.Length;
        fixed (char* p = array)
        {
            for (int i = 0; i < length; ++i)
                if (p[i] == c) return true;
        }

        return false;
    }

    public int IndexOf(char[] anyOf)
    {
        return this.IndexOf(anyOf, 0, this.length);
    }

    public int IndexOf(char[] anyOf, int offset)
    {
        return this.IndexOf(anyOf, offset, this.length - offset);
    }

    public int IndexOf(char[] anyOf, int offset, int count)
    {
        if (offset < 0 || offset >= this.length) throw new ArgumentOutOfRangeException("offset");
        if (count < 0 || count - 1 < offset) throw new ArgumentOutOfRangeException("count");

        if (anyOf.Length == 1) return this.IndexOf(anyOf[0], offset);

        fixed (char* p = this.str)
        {
            int length = System.Math.Min(this.length, count);
            for (int i = offset; i < length; ++i)
            {
                if (ArrayContains(anyOf, p[this.offset + i])) return i;
            }
        }

        return -1;
    }

    public int IndexOf(string s)
    {
        return this.IndexOf(s, 0);
    }

    public int IndexOf(string s, int offset)
    {
        if (s == null) throw new ArgumentNullException("s");
        if (offset < 0 || offset >= this.length) throw new ArgumentOutOfRangeException("offset");

        int s1_length = this.str.Length;
        int s2_length = s.Length;
        fixed (char* p1 = this.str)
        {
            for (int i = offset; i < this.length; ++i)
            {
                if (p1[this.offset + i] == s[0] &&
                    (this.length - i) >= s.Length &&
                    InternalCompareOrdinal(this.str, this.offset + i, s, 0, s.Length))
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public int LastIndexOf(char split)
    {
        return this.LastIndexOf(split, this.length - 1, this.length);
    }

    public int LastIndexOf(char split, int offst)
    {
        return this.LastIndexOf(split, offset, this.length - offset);
    }

    public int LastIndexOf(char split, int offset, int count)
    {
        if (offset < 0 || offset >= this.length) throw new ArgumentOutOfRangeException("offset");
        if (count < 0 || count - 1 > offset) throw new ArgumentOutOfRangeException("count");

        fixed (char* p = this.str)
        {
            int left = System.Math.Min((offset + 1), count);
            while (left >= 4)
            {
                if (p[this.offset + --left] == split) goto Found;
                if (p[this.offset + --left] == split) goto Found;
                if (p[this.offset + --left] == split) goto Found;
                if (p[this.offset + --left] == split) goto Found;
            }
            while (left > 0)
            {
                if (p[this.offset + --left] == split) goto Found;
            }
            return -1;
            Found:
            return left;
        }
    }

    public int LastIndexOf(params char[] anyOf)
    {
        return this.LastIndexOf(anyOf, this.length - 1, this.length);
    }
    public int LastIndexOf(char[] anyOf, int offset)
    {
        return this.LastIndexOf(anyOf, offset, this.length - offset);
    }
    public int LastIndexOf(char[] anyOf, int offset, int count)
    {
        if (anyOf == null) throw new ArgumentNullException("anyOf");
        if (offset < 0 || offset >= this.length) throw new ArgumentOutOfRangeException("offset");
        if (count < 0 || count - 1 > offset) throw new ArgumentOutOfRangeException("count");

        fixed (char* p = this.str)
        {
            int left = System.Math.Min((offset + 1), count);
            while (left >= 4)
            {
                if (ArrayContains(anyOf, p[this.offset + --left])) goto Found;
                if (ArrayContains(anyOf, p[this.offset + --left])) goto Found;
                if (ArrayContains(anyOf, p[this.offset + --left])) goto Found;
                if (ArrayContains(anyOf, p[this.offset + --left])) goto Found;
            }
            while (left > 0)
            {
                if (ArrayContains(anyOf, p[this.offset + --left])) goto Found;
            }
            return -1;
            Found:
            return left;
        }
    }

    public int LastIndexOf(string s)
    {
        return this.LastIndexOf(s, this.length - 1);

    }
    public int LastIndexOf(string s, int offset)
    {
        if (s == null) throw new ArgumentNullException("s");
        if (offset < 0 || offset >= this.length) throw new ArgumentOutOfRangeException("offset");

        if (s.Length == 0 && this.length > 0) return 0;
        offset = System.Math.Min(offset, this.length - s.Length);

        int s1_length = this.str.Length;
        int s2_length = s.Length;
        fixed (char* p1 = this.str)
        {
            for (int i = offset; i >= 0; --i)
            {
                if (p1[this.offset + i] == s[0] &&
                    (this.length - i) >= s.Length &&
                    InternalCompareOrdinal(this.str, this.offset + i, s, 0, s.Length))
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public char this[int index]
    {
        get
        {
            if (index < 0 || index >= this.length)
            {
                throw new ArgumentOutOfRangeException("index");
            }

            fixed (char* p = this.str)
            {
                return p[this.offset + index];
            }
        }
    }

    public StringView Substring(int begin)
    {
        return this.Substring(begin, this.length - begin);
    }

    public StringView Substring(int begin, int length)
    {
        return new StringView(this.str, this.offset + begin, length);
    }

    public StringView[] Split(char split)
    {
        int length = this.length;
        int[] posArray = new int[length];
        int splitCount = MakeSplitIndexArray(split, posArray);

        StringView[] ret = new StringView[splitCount + 1];
        int count = 0;
        int index = 0;
        for (int i = 0; i < splitCount; ++i)
        {
            ret[count++] = this.Substring(index, posArray[i] - index);
            index = posArray[i]+ 1;
        }

        if (index != length) ret[count++] = this.Substring(index, length - index);
        return ret;
    }

    public StringView[] Split(params char[] split)
    {
        int length = this.length;
        int[] posArray = new int[length];
        int splitCount = MakeSplitIndexArray(split, posArray);

        StringView[] ret = new StringView[splitCount + 1];
        int count = 0;
        int index = 0;
        for (int i = 0; i < splitCount; ++i)
        {
            ret[count++] = this.Substring(index, posArray[i] - index);
            index = posArray[i] + 1;
        }

        if (index != length) ret[count++] = this.Substring(index, length - index);
        return ret;
    }

    public StringView[] Split(params string[] split)
    {
        int length = this.length;
        int[] posArray = new int[length];
        int[] lenArray = new int[length];
        int splitCount = MakeSplitIndexArray(split, posArray, lenArray);

        StringView[] ret = new StringView[splitCount + 1];
        int count = 0;
        int index = 0;
        for (int i = 0; i < splitCount; ++i)
        {
            ret[count++] = this.Substring(index, posArray[i] - index);
            index = posArray[i] + lenArray[i];
        }

        if (index != length) ret[count++] = this.Substring(index, length - index);
        return ret;
    }

    private int MakeSplitIndexArray(char split, int[] posArray)
    {
        fixed (char* p = this.str)
        {
            int splitCount = 0;
            for (int i = 0; i < this.length; ++i)
            {
                if (p[this.offset + i] == split) posArray[splitCount++] = i;
            }
            return splitCount;
        }
    }

    private int MakeSplitIndexArray(char[] split, int[] posArray)
    {
        fixed (char* p = this.str)
        {
            int splitCount = 0;

            if (split == null || split.Length == 0)
            {
                for (int i = 0; i < this.length; ++i)
                {
                    if (char.IsWhiteSpace(p[this.offset + i])) posArray[splitCount++] = i;
                }
                return splitCount;
            }

            for (int i = 0; i < this.length; ++i)
            {
                if (ArrayContains(split, p[this.offset + i])) posArray[splitCount++] = i;
            }
            return splitCount;
        }
    }

    private int MakeSplitIndexArray(string[] split, int[] posArray, int[] lenArray)
    {
        if (split == null || split.Length == 0)
        {
            int count = this.MakeSplitIndexArray((char[])null, posArray);
            for (int i = 0; i < count; ++i)
                lenArray[i] = 1;
            return count;
        }

        fixed (char* p = this.str)
        {
            int splitCount = 0;
            for (int i = 0; i < this.length; ++i)
            {
                foreach (var seperator in split)
                {
                    if (String.IsNullOrEmpty(seperator)) continue;

                    if (p[this.offset + i] == seperator[0] && (this.length - i) >= seperator.Length)
                    {
                        if (InternalCompareOrdinal(this.str, this.offset + i, seperator, 0, seperator.Length))
                        {
                            posArray[splitCount] = i;
                            lenArray[splitCount] = seperator.Length;
                            splitCount++;
                            i += seperator.Length - 1;
                            break;
                        }
                    }
                }
            }

            return splitCount;
        }
    }

    private static bool InternalCompareOrdinal(String strA, int indexA, String strB, int indexB, int count)
    {
        return new StringView(strA, indexA, count) == new StringView(strB, indexB, count);
    }

    public override bool Equals(object obj)
    {
        if (obj is StringView)
        {
            return this.Equals((StringView)obj);
        }
        else if (obj is string)
        {
            return this.Equals((string)obj);
        }
        return false;
    }

    public bool Equals(StringView v)
    {
        return this.Equals(v.str, v.offset, v.length);
    }

    public bool Equals(string s)
    {
        return this.Equals(s, 0, s.Length);
    }

    private bool Equals(string s, int offset, int length)
    {
        if (length != this.length) return false;
        if (length == 0) return true;
        if (object.ReferenceEquals(s, this.str) && offset == this.offset) return true;

        fixed (char* p1 = this.str, p2 = s)
        {
            return EqualHelper(p1 + this.offset, p2 + offset, length);
        }
    }

    public static bool operator ==(StringView a, StringView b)
    {
        return a.Equals(b);
    }
    public static bool operator !=(StringView a, StringView b)
    {
        return !a.Equals(b);
    }

    //Copy from .NET System.String.EqualsHelper
    private static bool EqualHelper(char* p1, char* p2, int length)
    {
        int left = length;
        if (sizeof(System.IntPtr) == 8)
        {
            if (*(int*)p1 != *(int*)2) return false;
            left -= 2; p1 += 2; p2 += 2;

            while (left >= 12)
            {
                if (*(long*)(p1 + 0) != *(long*)(p2 + 0)) goto RetFalse;
                if (*(long*)(p1 + 4) != *(long*)(p2 + 4)) goto RetFalse;
                if (*(long*)(p1 + 8) != *(long*)(p2 + 8)) goto RetFalse;
                left -= 12; p1 += 12; p2 += 12;
            }
        }
        else
        {
            while (left >= 10)
            {
                if (*(int*)(p1 + 0) != *(int*)(p2 + 0)) goto RetFalse;
                if (*(int*)(p1 + 2) != *(int*)(p2 + 2)) goto RetFalse;
                if (*(int*)(p1 + 4) != *(int*)(p2 + 4)) goto RetFalse;
                if (*(int*)(p1 + 6) != *(int*)(p2 + 6)) goto RetFalse;
                if (*(int*)(p1 + 8) != *(int*)(p2 + 8)) goto RetFalse;
                left -= 10; p1 += 10; p2 += 10;
            }
        }

        //StringView's string will not be end with '\0'
        //so must scan by char, not int
        while (left > 0)
        {
            if (p1[0] != p2[0]) goto RetFalse;
            left -= 1; p1 += 1; p2 += 1;
        }

        return true;
        RetFalse:
        return false;
    }


    internal static int CombineHashCodes(int h1, int h2)
    {
        return (((h1 << 5) + h1) ^ h2);
    }

    //TODO: improve the performance
    public override int GetHashCode()
    {
        int hash_code = 0;
        for (int i = 0; i < this.length; ++i)
        {
            hash_code = CombineHashCodes(hash_code, this[i].GetHashCode());
        }
        return hash_code;
    }


    /// <summary>
    /// generate a string instance, which will copy the chars of the StringView
    /// </summary>
    /// <returns>a string</returns>
    public override string ToString()
    {
        return this.str.Substring(offset, length);
    }
    
    /// <summary>
    /// the real string of the StringView
    /// </summary>
    public string Original { get { return this.str; } }
    /// <summary>
    /// the first char index of the Original String
    /// </summary>
    public int Offset { get { return this.offset; } }
    /// <summary>
    /// the length of the StringView
    /// </summary>
    public int Length { get { return this.length; } }

    private string str;
    private int offset;
    private int length;
}
