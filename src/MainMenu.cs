class MainMenu
{
    static int selected = 0;

    public static void DisplayMenu()
    {
        Console.WriteLine("====== Hlavní Menu ======");

        if (selected == 0)
            Console.WriteLine("> Chytej Ryby");
        else
            Console.WriteLine("  Chytej Ryby");

        if (selected == 1)
            Console.WriteLine("> Jdi do Obchodu");
        else
            Console.WriteLine("  Jdi do Obchodu");
    }

    public static void UiButtonMenuDown()
    {
        selected++;
        selected += 2;
        selected %= 2;
    }

    public static void UiButtonMenuUp()
    {
        selected--;
        selected += 2;
        selected %= 2;
    }

    public static void EnterOption(PlayerData playerData)
    {
        Console.Clear();
        
        switch (selected)
        {
            case 0: playerData.gameState = GameState.Catching; break;
            case 1: playerData.gameState = GameState.Shop; break;
        }
    }
}