using System;

class Shop
{
    // 0: Rod Level Up, 1: Inventory Size Up, 2: Zpět
    static int selected = 0;
    private const int TOTAL_OPTIONS = 3;

    // Pomocné metody pro výpočet ceny
    public static uint GetRodUpgradeCost(ushort currentLevel) => (uint)((currentLevel + 1) * 100);
    public static uint GetInventoryUpgradeCost(byte currentSize) => (uint)((currentSize + 1) * 150);

    public static void DisplayShop()
    {
        Console.WriteLine("=== RYBÁŘSKÝ OBCHOD ===");
        Console.WriteLine($"Tvoje peníze: {Program.data.Money} mincí");
        Console.WriteLine("-----------------------");

        uint rodCost = GetRodUpgradeCost(Program.data.RodLevel);
        uint invCost = GetInventoryUpgradeCost(Program.data.InventorySize);

        // Option 0: Vylepšení prutu
        string rodOption;
        if (Program.data.RodLevel >= 10)
        {
            rodOption = "Vylepšit Prut - MAX ÚROVEŇ";
        }
        else
        {
            rodOption = $"Vylepšit Prut ({Program.data.RodLevel + 1} / 11) - Cena: {rodCost} mincí";
        }
        Console.WriteLine((selected == 0 ? "> " : "  ") + rodOption);

        // Option 1: Zvětšení inventáře (max level 4)
        string invOption;
        if (Program.data.InventorySize >= 4)
        {
            invOption = "Vylepšit Loď - MAX ÚROVEŇ";
        }
        else
        {
            invOption = $"Vylepšit Loď ({Program.data.InventorySize + 1} / 5) - Cena: {invCost} mincí";
        }
        Console.WriteLine((selected == 1 ? "> " : "  ") + invOption);

        // Option 2: Zpět do menu
        Console.WriteLine((selected == 2 ? "> " : "  ") + "Zpět do hlavního menu");
        Console.WriteLine("-----------------------");
    }

    public static void ShopButtonMenuDown()
    {
        selected = (selected + 1) % TOTAL_OPTIONS;
    }

    public static void ShopButtonMenuUp()
    {
        selected = (selected - 1 + TOTAL_OPTIONS) % TOTAL_OPTIONS;
    }

    public static void EnterOption(PlayerData playerData)
    {
        switch (selected)
        {
            case 0: // Vylepšení prutu
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

            case 1: // Vylepšení inventáře
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

            case 2: // Návrat do menu
                playerData.gameState = GameState.MainMenu;
                break;
        }
    }
}