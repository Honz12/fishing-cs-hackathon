class InventoryUi
{
    static int selected = 0;

    public static void DisplayMenu(PlayerData playerData)
    {
        string title = "";
        Console.WriteLine(title);

        Console.WriteLine("ESC pro Zpátky Hlavní Menu\n");
        Console.WriteLine("S pro Prodání ryby\n");

        if (playerData.Inventory.Count == 0)
        {
            Console.WriteLine("Nemáš nic v Inventáři.");
            return;
        }

        for (int i = 0; i < playerData.Inventory.Count; i++)
        {
            if (selected == i)
            {
                Program.DisplayImage(playerData.Inventory[i].Image, playerData.Inventory[i].GetFormatedData(), "\x1b[1;96m");
            }
            else
            {
                Program.DisplayImage(playerData.Inventory[i].Image, playerData.Inventory[i].GetFormatedData(), "\x1b[0m");
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