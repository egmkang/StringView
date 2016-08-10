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

    public int IndexOfAny(char[] anyOf)
    {
        return this.IndexOfAny(anyOf, 0, this.length);
    }

    public int IndexOfAny(char[] anyOf, int offset)
    {
        return this.IndexOfAny(anyOf, offset, this.length - offset);
    }

    public int IndexOfAny(char[] anyOf, int offset, int count)
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
                    InternalEquals(this.str, this.offset + i, s, 0, s.Length))
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

    public int LastIndexOfAny(params char[] anyOf)
    {
        return this.LastIndexOfAny(anyOf, this.length - 1, this.length);
    }
    public int LastIndexOfAny(char[] anyOf, int offset)
    {
        return this.LastIndexOfAny(anyOf, offset, this.length - offset);
    }
    public int LastIndexOfAny(char[] anyOf, int offset, int count)
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
                    InternalEquals(this.str, this.offset + i, s, 0, s.Length))
                {
                    return i;
                }
            }
            return -1;
        }
    }

    public bool StartsWith(string s)
    {
        if (s == null) throw new ArgumentNullException("s");
        if (s.Length == 0) return true;
        if (this.length < s.Length) return false;

        return this.Substring(0, s.Length).Equals(new StringView(s));
    }

    public bool EndsWith(char c)
    {
        if (this.length != 0 && this[this.length - 1] == c)
            return true;
        return false;
    }

    public bool EndsWith(string s)
    {
        if (this.length >= s.Length &&
            this.Substring(this.length - s.Length, s.Length).Equals(new StringView(s)))
            return true;
        return false;
    }

    public bool Contains(string s)
    {
        return this.IndexOf(s) >= 0;
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

    public StringView Substring(int begin, int count)
    {
        if (this.length == 0 && begin == 0 && count == 0) return Empty;

        if (begin < 0 || begin >= this.length) throw new ArgumentOutOfRangeException("begin");
        if (count < 0 || count + begin > this.length) throw new ArgumentOutOfRangeException("count");

        return new StringView(this.str, this.offset + begin, count);
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
                        if (InternalEquals(this.str, this.offset + i, seperator, 0, seperator.Length))
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

    private static bool InternalEquals(String strA, int indexA, String strB, int indexB, int count)
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
            return EqualsHelper(p1 + this.offset, p2 + offset, length);
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
    private static bool EqualsHelper(char* p1, char* p2, int length)
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

    //none System module cannot call .NET VM's FastAllocateString
    //so i must init it with zero
    private static string FastAllocateString(int length)
    {
        string ret = new string('\0', length);
        return ret;
    }

    //check all char is < 0x80
    public bool IsAscii()
    {
        fixed (char* p = this.str)
        {
            char* p1 = p + this.offset;
            int left = this.length;

            if (sizeof(System.IntPtr) == 8)
            {
                while (left >= 32)
                {
                    if ((*(ulong*)(p1 + 0) & 0xFF80FF80FF80FF80ul) != 0) goto RetFalse;
                    if ((*(ulong*)(p1 + 2) & 0xFF80FF80FF80FF80ul) != 0) goto RetFalse;
                    if ((*(ulong*)(p1 + 4) & 0xFF80FF80FF80FF80ul) != 0) goto RetFalse;
                    if ((*(ulong*)(p1 + 8) & 0xFF80FF80FF80FF80ul) != 0) goto RetFalse;
                    if ((*(ulong*)(p1 + 10) & 0xFF80FF80FF80FF80ul) != 0) goto RetFalse;
                    if ((*(ulong*)(p1 + 12) & 0xFF80FF80FF80FF80ul) != 0) goto RetFalse;
                    if ((*(ulong*)(p1 + 14) & 0xFF80FF80FF80FF80ul) != 0) goto RetFalse;
                    if ((*(ulong*)(p1 + 16) & 0xFF80FF80FF80FF80ul) != 0) goto RetFalse;
                    p1 += 32; left -= 32;
                }
            }
            else
            {
                while (left >= 16)
                {
                    if ((*(int*)(p1 + 0) & 0xFF80FF80) != 0) goto RetFalse;
                    if ((*(int*)(p1 + 2) & 0xFF80FF80) != 0) goto RetFalse;
                    if ((*(int*)(p1 + 4) & 0xFF80FF80) != 0) goto RetFalse;
                    if ((*(int*)(p1 + 8) & 0xFF80FF80) != 0) goto RetFalse;
                    if ((*(int*)(p1 + 10) & 0xFF80FF80) != 0) goto RetFalse;
                    if ((*(int*)(p1 + 12) & 0xFF80FF80) != 0) goto RetFalse;
                    if ((*(int*)(p1 + 14) & 0xFF80FF80) != 0) goto RetFalse;
                    if ((*(int*)(p1 + 16) & 0xFF80FF80) != 0) goto RetFalse;
                    p1 += 16; left -= 16;
                }
            }
            while (left >= 8)
            {
                if ((*(int*)(p1 + 0) & 0xFF80FF80) != 0) goto RetFalse;
                if ((*(int*)(p1 + 2) & 0xFF80FF80) != 0) goto RetFalse;
                if ((*(int*)(p1 + 4) & 0xFF80FF80) != 0) goto RetFalse;
                if ((*(int*)(p1 + 8) & 0xFF80FF80) != 0) goto RetFalse;
                p1 += 8; left -= 8;
            }
            while (left > 0)
            {
                if ((*p1 & 0xFF80) != 0) goto RetFalse;
                p1 += 1; left -= 1;
            }
            return true;
            RetFalse:
            return false;
        }
    }

    public char[] ToCharArray()
    {
        return this.ToCharArray(0, this.length);
    }

    public char[] ToCharArray(int offset, int count)
    {
        if (this.length == 0 && count == this.length) return new char[0];

        if (offset < 0 || offset >= this.length) throw new ArgumentOutOfRangeException("offset");
        if (count < 0 || count - 1 < offset) throw new ArgumentOutOfRangeException("count");
        char[] array = new char[count];
        fixed (char* p2 = this.str, p1 = array)
        {
            memcpy((byte*)p1, (byte*)&p2[this.offset + offset], count * sizeof(char));
        }

        return array;
    }

    // A simple memcpy impl
    // copy one CacheLine as it can
    public static void memcpy(byte* dest, byte* source, int length)
    {
        byte* p1 = dest;
        byte* p2 = source;
        if (sizeof(System.IntPtr) == 8)
        {
            while (length >= 64)
            {
                *((long*)p1 + 0) = *((long*)p2 + 0);
                *((long*)p1 + 1) = *((long*)p2 + 1);
                *((long*)p1 + 2) = *((long*)p2 + 2);
                *((long*)p1 + 3) = *((long*)p2 + 3);
                *((long*)p1 + 4) = *((long*)p2 + 4);
                *((long*)p1 + 5) = *((long*)p2 + 5);
                *((long*)p1 + 6) = *((long*)p2 + 6);
                *((long*)p1 + 7) = *((long*)p2 + 7);
                length -= 64; p1 += 64; p2 += 64;
            }
        }
        else
        {
            while (length >= 32)
            {
                *((int*)p1 + 0) = *((int*)p2 + 0);
                *((int*)p1 + 1) = *((int*)p2 + 1);
                *((int*)p1 + 2) = *((int*)p2 + 2);
                *((int*)p1 + 3) = *((int*)p2 + 3);
                length -= 32; p1 += 32; p2 += 32;
            }
        }
        while (length >= 8)
        {
            *((int*)p1 + 0) = *((int*)p2 + 0);
            *((int*)p1 + 1) = *((int*)p2 + 1);
            length -= 8; p1 += 8; p2 += 8;
        }
        switch (length)
        {
            case 7:
                *(int*)p1 = *(int*)p2;
                *(short*)(p1 + 4) = *(short*)(p2 + 4);
                *(p1 + 6) = *(p2 + 6);
                break;
            case 6:
                *(int*)p1 = *(int*)p2;
                *(short*)(p1 + 4) = *(short*)(p2 + 4);
                break;
            case 5:
                *(int*)p1 = *(int*)p2;
                *(p1 + 4) = *(p2 + 4);
                break;
            case 4:
                *(int*)p1 = *(int*)p2;
                break;
            case 3:
                *(short*)p1 = *(short*)p2;
                *(p1 + 2) = *(p2 + 2);
                break;
            case 2:
                *(short*)p1 = *(short*)p2;
                break;
            case 1:
                *p1 = *p2;
                break;
            case 0:break;
        }
    }

    /// <summary>
    /// generate a string instance, which will copy the chars of the StringView
    /// </summary>
    /// <returns>a string</returns>
    public override string ToString()
    {
        return this.str.Substring(offset, length);
    }

    public string ToLower()
    {
        //TODO
        return string.Empty;
    }

    public string ToUpper()
    {
        //TODO
        return string.Empty;
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
