using System.Collections.Generic;

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

        if (this.offset < 0 ||
            this.offset >= this.str.Length ||
            this.offset + this.length > this.str.Length)
        {
            throw new System.Exception("StringView's Constructor OutOfBound");
        }
    }

    public int IndexOf(char c, int start = 0)
    {
        fixed (char* p = this.str)
        {
            for (int i = start; i < length; ++i)
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

    public int IndexOf(char[] array, int start = 0)
    {
        if (array.Length == 1) return this.IndexOf(array[0], start);

        fixed (char* p = this.str)
        {
            for (int i = start; i < length; ++i)
            {
                if (ArrayContains(array, p[this.offset + i])) return i;
            }
        }

        return -1;
    }

    public int IndexOf(string s, int start = 0)
    {
        int s1_length = this.str.Length;
        int s2_length = s.Length;
        fixed (char* p1 = this.str)
        {
            fixed (char* p2 = s)
            {
                int index = this.IndexOf(p2[0], start);
                while (index >= 0)
                {
                    if (s2_length > s1_length - this.offset - index)
                        return -1;
                    bool match = true;
                    for (int i = 0; i < s2_length; ++i)
                    {
                        if (p1[this.offset + index + i] != p2[i]) { match = false; break; }
                    }
                    if (match) return index;

                    index = this.IndexOf(p2[0], index + 1);
                }
                return -1;
            }
        }
    }

    public unsafe char this[int index]
    {
        get
        {
            if (index < 0 || index >= this.length)
            {
                throw new System.Exception("StringView's Index OutOfBound");
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
        fixed (char* p = this.str)
        {
            int length = this.length;
            int splitCount = 0;
            int[] posArray = new int[length];
            for (int i = 0; i < length; ++i)
            {
                if (p[this.offset + i] == split) posArray[splitCount++] = i;
            }

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
    }

    public override bool Equals(object obj)
    {
        if (obj is StringView)
        {
            StringView v = (StringView)obj;
            return this.Equals(v);
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
        for (int i = 0; i < length; ++i)
        {
            if (this[i] != s[offset + i]) return false;
        }
        return true;
    }

    internal static int CombineHashCodes(int h1, int h2)
    {
        return (((h1 << 5) + h1) ^ h2);
    }

    public override int GetHashCode()
    {
        int hash_code = 0;
        for (int i = 0; i < this.length; ++i)
        {
            hash_code = CombineHashCodes(hash_code, this[i].GetHashCode());
        }
        return hash_code;
    }

    public int Length { get { return this.length; } }

    public override string ToString()
    {
        return this.str.Substring(offset, length);
    }

    public string Original { get { return this.str; } }
    public int Offset { get { return this.offset; } }

    private string str;
    private int offset;
    private int length;
}
