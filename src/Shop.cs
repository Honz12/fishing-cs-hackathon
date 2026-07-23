class Shop
{
    static int selected = 0;

    public static void DisplayShop()
    {

    }

    public static void ShopButtonMenuDown()
    {
        selected++;
        selected += 2;
        selected %= 2;
    }

    public static void ShopButtonMenuUp()
    {
        selected--;
        selected += 2;
        selected %= 2;
    }

    public static void EnterOption(PlayerData playerData)
    {
        // User wants to buy selected option

        // playerData.Money
        // playerData.RodLevel++;
        // playerData.InventorySize [0 - 4]
    }
}
