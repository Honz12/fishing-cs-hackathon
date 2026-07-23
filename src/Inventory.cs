class InventoryUi
{
    static int selected = 0;

    public static void DisplayMenu(PlayerData playerData)
    {
        string title = (
Program.TITLE_COLOR + @"   ___ _    _         _ __   __  ___          " + "\x1b[0m\n" +
Program.TITLE_COLOR + @"  / __| |_ | |__ _ __| /_/__/_/ | _ ) _____ __" + "\x1b[0m\n" +
Program.TITLE_COLOR + @" | (__| ' \| / _` / _` | / _| | | _ \/ _ \ \ /" + "\x1b[0m\n" +
Program.TITLE_COLOR + @"  \___|_||_|_\__,_\__,_|_\__|_| |___/\___/_\_\" + "\x1b[0m\n" 
        );
        Console.WriteLine(title);

        Console.WriteLine("ESC pro zpátky do Hlavního Menu\n");
        Console.WriteLine("S pro Prodání ryby\n");

        if (playerData.Inventory.Count == 0)
        {
            Console.WriteLine("Nemáš nic v Chladícím Boxu.");
            return;
        }

        Console.WriteLine($"Kapacita ({playerData.Inventory.Count} / {Program.GetMaxFishInInventory()})");

        for (int i = 0; i < playerData.Inventory.Count; i++)
        {
            if (selected == i)
            {
                Program.DisplayImage(playerData.Inventory[i].Image, playerData.Inventory[i].GetFormatedData(), "\x1b[1;96m");
            }
            else
            {
                Console.WriteLine(playerData.Inventory[i].GetInfoCompact());
            }
        }
    }

    public static void UiButtonMenuDown(PlayerData playerData)
    {
        if (playerData.Inventory.Count == 0) return;
        selected++;
        selected += playerData.Inventory.Count;
        selected %= playerData.Inventory.Count;
    }

    public static void UiButtonMenuUp(PlayerData playerData)
    {
        if (playerData.Inventory.Count == 0) return;
        selected--;
        selected += playerData.Inventory.Count;
        selected %= playerData.Inventory.Count;
    }

    public static void SellOption(PlayerData playerData)
    {
        if (playerData.Inventory.Count == 0) return;

        Console.Clear();

        selected %= playerData.Inventory.Count;

        playerData.Money += (uint) (playerData.Inventory[selected].PricePerKg * playerData.Inventory[selected].Weight);

        playerData.Inventory.RemoveAt(selected);
        selected = Math.Max(0, selected - 1);
    }
}