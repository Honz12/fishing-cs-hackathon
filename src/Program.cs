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

    // Constants

    public const string TITLE_COLOR = "\x1b[92m";

    const char LOWER_HALF_CHAR = '▄';
    const char UPPER_HALF_CHAR = '▀';
    const int CATCHING_UI_WIDTH = 100;
    const int JUMP_VEL_MIN = 4;
    const int JUMP_VEL_MAX = 6;

    // Variables

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

    // Helper functions

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

    /// <summary>
    /// Gets the transalated text with ansi formating.
    /// </summary>
    /// <param name="r">The rarity to be converted.</param>
    /// <returns>The transated and colored string.</returns>
    /// <exception cref="NotImplementedException">Fatal error.</exception>
    public static string GetTransRarity(FishRarity r) => r switch
    {
        FishRarity.Common => $"\x1b[37m{GetTransRarityNoColor(r)}\x1b[0m",
        FishRarity.Rare => $"\x1b[30;102m{GetTransRarityNoColor(r)}\x1b[0m",
        FishRarity.Epic => $"\x1b[30;105m{GetTransRarityNoColor(r)}\x1b[0m",
        FishRarity.Mythic => $"\x1b[1;30;101m{GetTransRarityNoColor(r)}\x1b[0m",
        _ => throw new NotImplementedException()
    };

    /// <summary>
    /// Gets the transalated text.
    /// </summary>
    /// <param name="r">The rarity to be converted.</param>
    /// <returns>The transated string.</returns>
    /// <exception cref="NotImplementedException">Fatal error.</exception>
    public static string GetTransRarityNoColor(FishRarity r) => r switch
    {
        FishRarity.Common => "Běžná",
        FishRarity.Rare => "\x1b[30;102m Neobyčejná \x1b[0m",
        FishRarity.Epic => "\x1b[30;105m Epická \x1b[0m",
        FishRarity.Mythic => "\x1b[1;30;101m Mytická \x1b[0m",
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

    /// <summary>
    /// Generates a two-pixel ansi-escaped character,
    /// all 0 values will be transparent.
    /// </summary>
    /// <param name="upper">The upper pixel</param>
    /// <param name="lower">The lower pixel</param>
    /// <returns>The ansi escaped string.</returns>
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

    /// <summary>
    /// Displays a 16x16 image,
    /// with a possible text on the side,
    /// which can be styled.
    /// </summary>
    /// <param name="image">The image to be displayed.</param>
    /// <param name="text">The text to be displayed.</param>
    /// <param name="base_color">The styling of the text, in ansi escape sequences.</param>
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

    /// <summary>
    /// Displays multiple images in a row,
    /// with one character gaps between them.
    /// </summary>
    /// <param name="images">The images to be displayed.</param>
    public static void DisplayMultipleImages(Image[] images)
    {
        for (int y = 0; y < 16; y += 2)
        {
            string line = "";

            for (int i = 0; i < images.Length; i++)
            {
                for (int x = 0; x < 16; x++)
                {
                    byte upper = images[i].colors[x, y];
                    byte lower = images[i].colors[x, y + 1];

                    line += GetAnsiChar(upper, lower);
                }
                line += "\x1b[0m ";
            }

            Console.WriteLine(line);
        }
    }

    /// <summary>
    /// Get the maximum size of the player's inventory,
    /// according to the boat level.
    /// </summary>
    /// <returns>The maximal count of items in player's inventory.</returns>
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

    /// <summary>
    /// The main function,
    /// what would you expect?
    /// </summary>
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
@"     __ __    __            __                    ____        __             ___  __ " + '\n' +
@"    / //_/___/ /__         / /________  __  __   / __ \__  __/ /_  __  __   /__ \/ / " + '\n' +
@"   / ,< / __  / _ \   __  / / ___/ __ \/ / / /  / /_/ / / / / __ \/ / / /    / _/ /  " + '\n' +
@"  / /| / /_/ /  __/  / /_/ (__  ) /_/ / /_/ /  / _, _/ /_/ / /_/ / /_/ /    /_//_/   " + '\n' +
@" /_/ |_\__,_/\___/   \____/____/\____/\__,_/  /_/ |_|\__, /_.___/\__, /    (_)(_)    " + '\n' +
@"                                                    /____/      /____/               " + "\n";
                        DisplayImage(new Image("ship", "lod3.txt"), title, TITLE_COLOR); // Display the images/ship/lod3.txt image (the icon of the game), with the title.

                        Console.WriteLine("Jakákoliv klávesa pro pokračovaní ...");
                        
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
                        if (data.Inventory.Count >= GetMaxFishInInventory()) // The capacity limit has been reached.
                        {
                            Console.WriteLine("Maximální capacita chladícího boxu.");
                            Console.ReadKey(true);
                            data.GameState = GameState.MainMenu;
                            break;
                        }

                        int sideBarWidth = CATCHING_UI_WIDTH - catchingCenterSize + catchingOffset;
                        int leftWidth = sideBarWidth / 2;

                        Console.Write("\x1b[H"); // ANSI HOME

                        Console.WriteLine("Tahej!");
                        Console.WriteLine();

                        Console.WriteLine(
                            "Ryba je " + GetTransRarity((catchingFish ?? new Fish()).Rarity) // Display fish rarity.
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

                        Console.WriteLine(line + "\x1b[0m"); // Write the catching bar.

                        int progress = (int) (((double) successfullyCatchingTicks) / ((double) requiredCatchingTicks) * CATCHING_UI_WIDTH); // Calculate progress

                        Console.WriteLine("\x1b[0;106m" + RepeatString(" ", Math.Max(0, progress)) + "\x1b[0m" + RepeatString(" ", Math.Max(0, CATCHING_UI_WIDTH - progress))); // Write the progress bar.

                        if (!currentlyCatching) // When the game just started, wait untill a key is pressed.
                        {
                            Console.WriteLine("Zmačkni klávesu pro start ....");
                            Console.ReadKey(true);
                            currentlyCatching = true;
                        }
                        else
                            Console.WriteLine("                              "); // Overwrite the previous text

                        ConsoleKey? input = ReadKeyNoBlock(); // Non blocking input, allowing the game to run at 100 FPS.

                        if (input != null)
                        {
                            if (input == ConsoleKey.Escape)
                            {
                                data.GameState = GameState.MainMenu;
                            }
                            else // We "jump" the position.
                            {
                                catchingPos += Rng.Next(JUMP_VEL_MIN, JUMP_VEL_MAX);
                            }
                        }
                        
                        if (gameTicks % 5 == 0) // 1 per 0.05 seconds
                            if (leftWidth <= catchingPos && catchingPos < leftWidth + catchingCenterSize)
                            {
                                if (gameTicks % 10 == 0) // 1 per 0.1 seconds
                                    successfullyCatchingTicks++;
                            }
                            else
                                successfullyCatchingTicks--;
                        
                        if (gameTicks % (catchingFish ?? new Fish()).Rarity switch // Get the moving speed based on the rarity
                        {
                            FishRarity.Common => 50,
                            FishRarity.Rare => 15,
                            FishRarity.Epic => 10,
                            FishRarity.Mythic => 7,
                            _ => 0
                        } == 0)
                            catchingOffset += catchingVel;  // If passed, move the green part
                            if (catchingOffset < -20) // If outside of set bounds, reverse direction
                                catchingVel = 1;
                            if (catchingOffset > 20)
                                catchingVel = -1;
                            
                        if (successfullyCatchingTicks > 0xFFFF) // We can surrely say the position overflowed.
                        {
                            Console.WriteLine("Ryba uplavala!");
                            DisplayImage((catchingFish ?? new Fish()).Image, (catchingFish ?? new Fish()).GetFormatedData());
                            Console.WriteLine("Jakákoliv klávesa pro pokračovaní ...");
                            Console.ReadKey();
                            data.GameState = GameState.MainMenu;
                        }
                        else if (successfullyCatchingTicks >= requiredCatchingTicks) // If player successfully reached the goal, we give the win condition
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

    /// <summary>
    /// The initialization function when entering <code>GameState.Catching</code>
    /// </summary>
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
