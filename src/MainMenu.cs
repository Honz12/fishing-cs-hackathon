class MainMenu
{
    static int selected = 0;

    public static void DisplayMenu()
    {
        Console.WriteLine("=== === Town Shop === ===");

        if (selected == 0)
            Console.WriteLine("> Catch Fish");
        else
            Console.WriteLine("  Catch Fish");

        if (selected == 1)
            Console.WriteLine("> Go to Shop");
        else
            Console.WriteLine("  Go to Shop");
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
        
    }
}