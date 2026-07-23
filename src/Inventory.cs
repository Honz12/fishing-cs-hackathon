class InventoryUi
{
    static int selected = 0;

    public static void DisplayMenu(PlayerData playerData)
    {
        string title = "";
        Console.WriteLine(title);

        for (int i = 0; i < playerData.Inventory.Count; i++)
        {
            if (selected == i)
            {
                Program.DisplayImage(playerData.Inventory[i].Image, "\x1b[0;37m" + playerData.Inventory[i].GetFormatedData().Replace("\n", "\x1b[0m\n\x1b[0;37m"));
            }
        }
    }

    public static void UiButtonMenuDown(PlayerData playerData)
    {
        selected++;
        selected += playerData.Inventory.Count;
        selected %= playerData.Inventory.Count;
    }

    public static void UiButtonMenuUp(PlayerData playerData)
    {
        selected--;
        selected += playerData.Inventory.Count;
        selected %= playerData.Inventory.Count;
    }

    public static void EnterOption(PlayerData playerData)
    {
        Console.Clear();

        selected %= playerData.Inventory.Count;


    }
}