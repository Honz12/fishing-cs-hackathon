public enum GameState
{
    BootScreen, MainMenu, Shop, Catching, Inventory
}

class PlayerData
{
    public uint Money = 0;
    public ushort RodLevel = 0;
    public byte InventorySize = 0;

    public List<Fish> Inventory = new();

    public GameState GameState = GameState.BootScreen;
}

class Program {

    public const string TITLE_COLOR = "\x1b[92m";

    const char LOWER_HALF_CHAR = '▄';
    const char UPPER_HALF_CHAR = '▀';
    const int CATCHING_UI_WIDTH = 100;
    const int JUMP_VEL_MIN = 4;
    const int JUMP_VEL_MAX = 6;

    public static Random Rng = new();
    
    public static PlayerData data = new();
    private static int catchingPos = 0;
    private static uint gameTicks = 0;
    private static uint successfullyCatchingTicks = 0;
    private static int catchingCenterSize = 0;
    private static Fish? catchingFish = null;
    private static uint requiredCatchingTicks = 0;
    private static bool catchingFlipped = false;
    private static bool currentlyCatching = false;
    private static int catchingOffset = 0;
    private static int catchingVel = 0;

    public static string RepeatString(string s, int count) => string.Concat(Enumerable.Repeat(s, count));

    public static ConsoleKey? ReadKeyNoBlock()
    {
        ConsoleKey? input = null;

        if (Console.KeyAvailable)
        {
            input = Console.ReadKey(true).Key;
        }

        return input;
    }

    public static string GetTransRarity(FishRarity r) => r switch
    {
        FishRarity.Common => "Běžná",
        FishRarity.Rare => "\x1b[4;102m Neobyčejná \x1b[0m",
        FishRarity.Epic => "\x1b[4;105m Epická \x1b[0m",
        FishRarity.Mythic => "\x1b[4;101m Mytykální \x1b[0m",
        _ => throw new NotImplementedException()
    };

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

    public static void DisplayImage(Image image, string text = "", string base_color = "")
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

            Console.WriteLine(line + "\x1b[0m " + base_color + additionalData + "\x1b[0m");
        }
    }

    public static void DisplayMultipleImages(Image[] image)
    {
        for (int y = 0; y < 16; y += 2)
        {
            string line = "";

            for (int i = 0; i < image.Length; i++)
            {
                for (int x = 0; x < 16; x++)
                {
                    byte upper = image[i].colors[x, y];
                    byte lower = image[i].colors[x, y + 1];

                    line += GetAnsiChar(upper, lower);
                }
                line += "\x1b[0m ";
            }

            Console.WriteLine(line);
        }
    }

    public static int GetMaxFishInInventory()
    {
        switch (data.InventorySize)
        {
            case 0: return 3;
            case 1: return 5;
            case 2: return 7;
            case 3: return 10;
            case 4: return 15;
        }
        return 3;
    }

    public static void Main()
    {
        Console.CursorVisible = false;

        while (true)
        {
            switch (data.GameState)
            {
                case GameState.BootScreen:
                    {
                        Console.Clear();

                        string title = 
"\n" +
"\x1b[92m" + @"     __ __    __            __                    ____        __             ___  __ " + '\n' +
"\x1b[92m" + @"    / //_/___/ /__         / /________  __  __   / __ \__  __/ /_  __  __   /__ \/ / " + '\n' +
"\x1b[92m" + @"   / ,< / __  / _ \   __  / / ___/ __ \/ / / /  / /_/ / / / / __ \/ / / /    / _/ /  " + '\n' +
"\x1b[92m" + @"  / /| / /_/ /  __/  / /_/ (__  ) /_/ / /_/ /  / _, _/ /_/ / /_/ / /_/ /    /_//_/   " + '\n' +
"\x1b[92m" + @" /_/ |_\__,_/\___/   \____/____/\____/\__,_/  /_/ |_|\__, /_.___/\__, /    (_)(_)    " + '\n' +
"\x1b[92m" + @"                                                    /____/      /____/               " + "\x1b[0m\n";
                        DisplayImage(new Image("ship", "lod3.txt"), title);

                        Console.WriteLine("Cokoliv pro pokračovaní ...");
                        
                        Console.ReadKey(true);
                        data.GameState = GameState.MainMenu;
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
                            case ConsoleKey.Spacebar:
                            case ConsoleKey.Enter:
                                MainMenu.EnterOption(data);
                                break;
                        }
                    }
                    break;
                case GameState.Shop:
                    {
                        Console.Clear();
                        Shop.DisplayShop(data);
                        ConsoleKey key = Console.ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.UpArrow:
                                Shop.ShopButtonMenuUp();
                                break;
                            case ConsoleKey.DownArrow:
                                Shop.ShopButtonMenuDown();
                                break;
                            case ConsoleKey.Spacebar:
                            case ConsoleKey.Enter:
                                Shop.EnterOption(data);
                                break;
                            case ConsoleKey.Escape:
                                data.GameState = GameState.MainMenu;
                                break;
                        }
                    }
                    break;
                case GameState.Catching:
                    {
                        if (data.Inventory.Count >= GetMaxFishInInventory())
                        {
                            Console.WriteLine("Maximální capacita chladícího boxu.");
                            Console.ReadKey(true);
                            data.GameState = GameState.MainMenu;
                            break;
                        }

                        int sideBarWidth = CATCHING_UI_WIDTH - catchingCenterSize + catchingOffset;
                        int leftWidth = sideBarWidth / 2;

                        Console.Write("\x1b[H");

                        //DisplayImage((catchingFish ?? new Fish()).Image, (catchingFish ?? new Fish()).GetFormatedData());

                        Console.WriteLine("Tahej!");
                        Console.WriteLine();

                        Console.WriteLine(
                            "Ryba je " + GetTransRarity((catchingFish ?? new Fish()).Rarity)
                        );
                        
                        string line = "";
                        byte color = 0;

                        for (int i = 0; i < CATCHING_UI_WIDTH; i++)
                        {
                            byte desiredColor;

                            if (leftWidth <= i && i < leftWidth + catchingCenterSize)
                                desiredColor = 102;
                            else
                                desiredColor = 101;
                            if (i == catchingPos)
                                desiredColor = 0;
                            
                            if (desiredColor != color)
                            {
                                line += $"\x1b[{desiredColor}m";
                                color = desiredColor;
                            }
                            line += ' ';
                        }

                        Console.WriteLine(line + "\x1b[0m");

                        int progress = (int) (((double) successfullyCatchingTicks) / ((double) requiredCatchingTicks) * CATCHING_UI_WIDTH);

                        Console.WriteLine("\x1b[0;106m" + RepeatString(" ", Math.Max(0, progress)) + "\x1b[0m" + RepeatString(" ", Math.Max(0, CATCHING_UI_WIDTH - progress)));

                        if (!currentlyCatching)
                        {
                            Console.WriteLine("Zmačkni klávesu pro start ....");
                            Console.ReadKey(true);
                            currentlyCatching = true;
                        }
                        else
                            Console.WriteLine("                              ");

                        ConsoleKey? input = ReadKeyNoBlock();

                        if (input != null)
                        {
                            if (input == ConsoleKey.Escape)
                            {
                                data.GameState = GameState.MainMenu;
                            }
                            else
                            {
                                catchingPos += Rng.Next(JUMP_VEL_MIN, JUMP_VEL_MAX);
                            }
                        }
                        
                        if (gameTicks % 5 == 0)
                            if (leftWidth <= catchingPos && catchingPos < leftWidth + catchingCenterSize)
                            {
                                if (gameTicks % 10 == 0)
                                    successfullyCatchingTicks++;
                            }
                            else
                                successfullyCatchingTicks--;
                        
                        if (gameTicks % (catchingFish ?? new Fish()).Rarity switch
                        {
                            FishRarity.Common => 50,
                            FishRarity.Rare => 15,
                            FishRarity.Epic => 10,
                            FishRarity.Mythic => 7,
                            _ => 0
                        } == 0)
                            catchingOffset += catchingVel;
                            if (catchingOffset < -10)
                                catchingVel = 1;
                            if (catchingOffset > 10)
                                catchingVel = -1;
                            
                        if (successfullyCatchingTicks > 0xFFFF)
                        {
                            Console.WriteLine("Ryba uplavala!");
                            DisplayImage((catchingFish ?? new Fish()).Image, (catchingFish ?? new Fish()).GetFormatedData());
                            Console.ReadKey();
                            data.GameState = GameState.MainMenu;
                        }
                        else if (successfullyCatchingTicks >= requiredCatchingTicks)
                        {
                            Console.Clear();
                            Console.WriteLine("Chytil jsi:");
                            DisplayImage((catchingFish ?? new Fish()).Image, (catchingFish ?? new Fish()).GetFormatedData());
                            Console.Write("Ponechat? (A/n)");
                            ConsoleKey consoleKey = Console.ReadKey(true).Key;
                            while (!(consoleKey == ConsoleKey.A || consoleKey == ConsoleKey.N || consoleKey == ConsoleKey.Y))
                            {
                                consoleKey = Console.ReadKey(true).Key;
                            }
                            if (consoleKey == ConsoleKey.A || consoleKey == ConsoleKey.Y)
                            {
                                data.Inventory.Add(catchingFish ?? new Fish());
                            }
                            data.GameState = GameState.MainMenu;
                        }
                        
                        gameTicks++;

                        if (gameTicks % 5 == 0)
                            catchingPos--;

                        Thread.Sleep(10);
                    }
                    break;
                case GameState.Inventory:
                    {
                        Console.Clear();
                        InventoryUi.DisplayMenu(data);
                        ConsoleKey key = Console.ReadKey(true).Key;
                        Console.Clear();
                        switch (key)
                        {
                            case ConsoleKey.UpArrow:
                                InventoryUi.UiButtonMenuUp(data);
                                break;
                            case ConsoleKey.DownArrow:
                                InventoryUi.UiButtonMenuDown(data);
                                break;
                            case ConsoleKey.Spacebar:
                            case ConsoleKey.S:
                                InventoryUi.SellOption(data);
                                break;
                            case ConsoleKey.Escape:
                                data.GameState = GameState.MainMenu;
                                break;
                        }
                    }
                    break;
            }
        }
    }

    public static void CatchingInit()
    {
        catchingPos = CATCHING_UI_WIDTH / 2;
        gameTicks = 0;
        successfullyCatchingTicks = 0;
        catchingCenterSize = Rng.Next(10, 20);
        requiredCatchingTicks = (uint) Rng.Next(20, 50);
        catchingFish = new Fish(TFishFinder.FindRandomFish(data.RodLevel));
        catchingFlipped = false;
        catchingOffset = Rng.Next(-catchingCenterSize + 5, catchingCenterSize - 5);
        if ((int) catchingFish.Rarity >= (int) FishRarity.Rare)
        {
            catchingVel = Rng.Next(0, 1) * 2 - 1;
        }
    }
}
