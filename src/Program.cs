class Program {
    const char HALF_CHAR = '▄';

    public static string GetAnsiChar(byte upper, byte lower)
    {
        int u = upper;
        int l = lower;

        if (u > 7)
        {
            
        }

        return $"\x1b[{lower + 30};{upper + 40}m{HALF_CHAR}";
    }

    public static void Main()
    {

    }
}
