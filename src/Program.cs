public enum GameState
{
    BootScreen, MainMenu, Shop, Catching
}

class PlayerData
{
    public uint Money = 0;
    public ushort RodLevel = 0;
    public byte InventorySize = 0;

    public GameState gameState = GameState.BootScreen;
}

class Program {

    const char LOWER_HALF_CHAR = '▄';
    const char UPPER_HALF_CHAR = '▀';

    public static Random Rng = new Random();
    
    public static PlayerData data = new PlayerData();

    public static string RepeatString(string s, int count) => string.Concat(Enumerable.Repeat(s, count));

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
        var (fgR, fgG, fgB) = ColorPalette[lower];
        var (bgR, bgG, bgB) = ColorPalette[upper];

        string fg = $"{fgR};{fgG};{fgB}m";
        string bg = $"{bgR};{bgG};{bgB}m";

        if (upper == 0 && lower == 0)
        {
            return "\x1b[0m ";
        }
        if (upper == 0)
        {
            return $"\x1b[0m\x1b[38;2;{fg}{LOWER_HALF_CHAR}";
        }
        if (lower == 0)
        {
            return $"\x1b[0m\x1b[38;2;{bg}{UPPER_HALF_CHAR}";
        }
        return $"\x1b[0m\x1b[38;2;{fg}\x1b[48;2;{bg}{LOWER_HALF_CHAR}";
    }

    public static void DisplayImage(Image image, string text = "")
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

    public static void Catch()
    {
        Fish f = new(TFishFinder.FindRandomFish(false, 0));
        Console.WriteLine("Chytil jsi:");
        DisplayImage(f.Image, f.GetFormatedData());
    }

    public static void Main()
    { 
        Console.CursorVisible = false;

        while (true)
        {
            switch (data.gameState)
            {
                case GameState.BootScreen:
                    {
                        Console.Clear();

                        string title = (
"\n\n" +
@"    __ __    __            __                    ____        __             ___  __" + '\n' +
@"   / //_/___/ /__         / /________  __  __   / __ \__  __/ /_  __  __   /__ \/ /" + '\n' +
@"  / ,< / __  / _ \   __  / / ___/ __ \/ / / /  / /_/ / / / / __ \/ / / /    / _/ / " + '\n' +
@" / /| / /_/ /  __/  / /_/ (__  ) /_/ / /_/ /  / _, _/ /_/ / /_/ / /_/ /    /_//_/  " + '\n' +
@"/_/ |_\__,_/\___/   \____/____/\____/\__,_/  /_/ |_|\__, /_.___/\__, /    (_)(_)   " + '\n' +
@"                                                   /____/      /____/              " + '\n'
                        );
                        DisplayImage(new Image("lod3.txt"), title);
                        
                        Console.ReadKey(true);
                        data.gameState = GameState.MainMenu;
                    }
                    break;
                case GameState.MainMenu:
                    {
                        Console.Clear();
                        MainMenu.DisplayMenu();
                        ConsoleKey key = Console.ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.UpArrow:
                                MainMenu.UiButtonMenuUp();
                                break;
                            case ConsoleKey.DownArrow:
                                MainMenu.UiButtonMenuDown();
                                break;
                            case ConsoleKey.Enter:
                                MainMenu.EnterOption(data);
                                break;
                        }
                    }
                    break;
                case GameState.Shop:
                    {
                        Console.Clear();
                        Shop.DisplayShop();
                        ConsoleKey key = Console.ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.UpArrow:
                                Shop.ShopButtonMenuUp();
                                break;
                            case ConsoleKey.DownArrow:
                                Shop.ShopButtonMenuDown();
                                break;
                            case ConsoleKey.Enter:
                                Shop.EnterOption(data);
                                break;
                        }
                    }
                    break;
                case GameState.Catching:
                    {
                        int width = 100;
                        int centerSize = 30;
                        int sideBarWidth = width - centerSize;
                        int leftWidth = sideBarWidth / 2;
                        int rightWidth = sideBarWidth - leftWidth;

                        Console.Write("\x1b[H");
                        Console.WriteLine("Tahej!");
                        Console.WriteLine();
                        
                        string line = "";
                        byte color = 0;

                        for (int i = 0; i < width; i++)
                        {
                            byte desiredColor;

                            if (leftWidth <= i && i < leftWidth + centerSize)
                                desiredColor = 102;
                            else
                                desiredColor = 101;
                            
                            if (desiredColor != color)
                            {
                                line += $"\x1b[{desiredColor}m";
                                color = desiredColor;
                            }
                            line += ' ';
                        }

                        Console.WriteLine(line + "\x1b[0m");

                        ConsoleKey? input = null;

                        if (Console.KeyAvailable)
                        {
                            input = Console.ReadKey().Key;
                        }
                    }
                    break;
            }
        }
    }
}
