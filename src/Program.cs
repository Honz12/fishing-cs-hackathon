class Program {
    const char HALF_CHAR = '▄';

    // Lookup table mapping indices 0–15 directly to their RGB values
    private static readonly (byte R, byte G, byte B)[] ColorPalette = new (byte, byte, byte)[]
    {
        (0x00, 0x00, 0x00), // 0: Black
        (0x80, 0x00, 0x00), // 1: Red
        (0x00, 0x80, 0x00), // 2: Green
        (0x80, 0x80, 0x00), // 3: Yellow
        (0x00, 0x00, 0x80), // 4: Blue
        (0x80, 0x00, 0x80), // 5: Purple
        (0x00, 0x80, 0x80), // 6: Cyan
        (0xC0, 0xC0, 0xC0), // 7: Dim White
        (0x80, 0x80, 0x80), // 8: Gray
        (0xFF, 0x00, 0x00), // 9: Light Red
        (0x00, 0xFF, 0x00), // A: Light Green
        (0xFF, 0xFF, 0x00), // B: Light Yellow
        (0x00, 0x00, 0xFF), // C: Light Blue
        (0xFF, 0x00, 0xFF), // D: Light Purple
        (0x00, 0xFF, 0xFF), // E: Light Cyan
        (0xFF, 0xFF, 0xFF)  // F: White
    };

    public static string GetAnsiChar(byte upper, byte lower)
    {
        // Retrieve RGB components for foreground (lower) and background (upper)
        var (fgR, fgG, fgB) = ColorPalette[lower];
        var (bgR, bgG, bgB) = ColorPalette[upper];

        // Combine 24-bit foreground (38;2) and background (48;2) sequences
        return $"\x1b[38;2;{fgR};{fgG};{fgB};48;2;{bgR};{bgG};{bgB}m{HALF_CHAR}";
    }

    public static void DisplayImage(Image image, string text)
    {
        int textPointer = 0;

        for (int y = 0; y < 16; y += 2)
        {
            string line = "";

            for (int x = 0; x < 16; x++)
            {
                byte upper = image.colors[x, y];
                byte lower = image.colors[x, y + 1];

                line += GetAnsiChar(upper, lower);
            }

            string additionalData = "";

            while (textPointer < text.Length)
            {
                if (text[textPointer] == '\r')
                {
                    continue;
                }
                if (text[textPointer] == '\n')
                {
                    textPointer++;
                    break;
                }
                additionalData += text[textPointer];
                textPointer++;
            }

            Console.WriteLine(line + "\x1b[0m " + additionalData);
        }
    }

    public static void Main()
    {
        Image kapr = new Image("kaprObecny");

        DisplayImage(kapr, "Hello\nWorld!");
    }
}
