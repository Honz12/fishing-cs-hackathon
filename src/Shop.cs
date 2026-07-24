using System;

class Shop
{
    public static int selected = 0;
    private const int TOTAL_OPTIONS = 3;

    // Helper methods for price calculation.
    public static uint GetRodUpgradeCost(ushort currentLevel) => (uint)((currentLevel + 1) * 100);
    public static uint GetInventoryUpgradeCost(byte currentSize) => (uint)((currentSize + 1) * 150);

    public static void DisplayShop(PlayerData playerData, Image character)
    {
        string title = (
            "\n\n" +
            Program.TITLE_COLOR + @"   ___      _       __    _     __       _        _            _ " + "\x1b[0m\n" +
            Program.TITLE_COLOR + @"  | _ \_  _| |__  _/_/ __| |___/_/   ___| |__  __| |_  ___  __| |" + "\x1b[0m\n" +
            Program.TITLE_COLOR + @"  |   / || | '_ \/ _` (_-< / / || | / _ \ '_ \/ _| ' \/ _ \/ _` |" + "\x1b[0m\n" +
            Program.TITLE_COLOR + @"  |_|_\\_, |_.__/\__,_/__/_\_\\_, | \___/_.__/\__|_||_\___/\__,_|" + "\x1b[0m\n" +
            Program.TITLE_COLOR + @"       |__/                   |__/                               " + "\x1b[0m\n");

        Console.WriteLine(title);
        Console.WriteLine($"Tvoje peníze: {Program.data.Money} mincí");
        Console.WriteLine();

        string str = "\n\n\n";

        uint rodCost = GetRodUpgradeCost(Program.data.RodLevel);
        uint invCost = GetInventoryUpgradeCost(Program.data.InventorySize);

        // Option 0: Fishing rod upgrade.
        string rodOption;
        if (Program.data.RodLevel >= 10) // If can't buy more upgrades.
        {
            rodOption = "Vylepšit Prut - MAX ÚROVEŇ";
        }
        else
        {
            rodOption = $"Vylepšit Prut ({Program.data.RodLevel + 1} / 11) - Cena: {rodCost} mincí";
        }
        str += (selected == 0 ? "> " : "  ") + rodOption + "\n";

        // Option 1: Boat upgrade.
        string invOption;
        if (Program.data.InventorySize >= 4) // If can't buy more upgrades.
        {
            invOption = "Vylepšit Loď - MAX ÚROVEŇ";
        }
        else
        {
            invOption = $"Vylepšit Loď ({Program.data.InventorySize + 1} / 5) - Cena: {invCost} mincí";
        }
        str += (selected == 1 ? "> " : "  ") + invOption + "\n";

        // Option 2: Zpět do menu
        str += (selected == 2 ? "> " : "  ") + "Zpět do hlavního menu" + "\n";

        Program.DisplayImage(character, str);

        Program.DisplayMultipleImages( // Display the fishing rod and the boat images.
            new Image[]
            {
                new("rod", $"prut{playerData.RodLevel}.txt"),
                new("ship", $"lod{playerData.InventorySize}.txt")
            }
        );
    }

    /// <summary>
    /// Called when the user presses <code>ConsoleKey.DownArrow</code>
    /// </summary>
    public static void ShopButtonMenuDown()
    {
        selected = (selected + 1) % TOTAL_OPTIONS;
    }

    /// <summary>
    /// Called when the user presses <code>ConsoleKey.UpArrow</code>
    /// </summary>
    public static void ShopButtonMenuUp()
    {
        selected = (selected - 1 + TOTAL_OPTIONS) % TOTAL_OPTIONS;
    }

    /// <summary>
    /// Called when the user presses <code>ConsoleKey.Enter</code>
    /// </summary>
    /// <param name="playerData">Player's data</param>
    public static void EnterOption(PlayerData playerData)
    {
        switch (selected)
        {
            case 0: // Rod upgrade.
                if (playerData.RodLevel < 10)
                {
                    uint rodCost = GetRodUpgradeCost(playerData.RodLevel);
                    if (playerData.Money >= rodCost)
                    {
                        playerData.Money -= rodCost;
                        playerData.RodLevel++;
                    }
                }
                break;

            case 1: // Inventory upgrade.
                if (playerData.InventorySize < 4)
                {
                    uint invCost = GetInventoryUpgradeCost(playerData.InventorySize);
                    if (playerData.Money >= invCost)
                    {
                        playerData.Money -= invCost;
                        playerData.InventorySize++;
                    }
                }
                break;

            case 2: // Back To Menu
                playerData.GameState = GameState.MainMenu;
                break;
        }
    }
}