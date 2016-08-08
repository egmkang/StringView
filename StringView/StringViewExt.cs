public static partial class Ext
{
    public unsafe static int ToInt32(this StringView view)
    {
        bool negative = false;
        int num = 0;
        if (view.Length <= 0) return num;
        fixed (char* stringPointer = view.Original)
        {
            char* p = stringPointer + view.Offset;
            int i = 0;
            if (p[0] == '+') { ++i; }
            if (p[0] == '-') { ++i; negative = true; }
            for (; i < view.Length; ++i)
            {
                char c = p[i];
                if (c >= '0' && c <= '9')
                {
                    num = num * 10 + (c - '0');
                }
                else { break; }
            }
        }

        return negative ? -num : num;
    }
}
