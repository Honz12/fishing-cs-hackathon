class MainMenu
{
    static int selected = 0;

    public static void DisplayMenu()
    {
        string title = (
"\n\n" +
@"   __  __      _        __  __            " + '\n' +
@"  |  \/  |__ _(_)_ _   |  \/  |___ _ _ _ _ " + '\n' +
@"  | |\/| / _` | | ' \  | |\/| / -_) ' \ || |" + '\n' +
@"  |_|  |_\__,_|_|_||_| |_|  |_\___|_||_\_,_|" + '\n');
        Console.WriteLine(title);

        if (selected == 0)
            Console.WriteLine("> Jít chytat ryby");
        else
            Console.WriteLine("  Jít chytat ryby");

        if (selected == 1)
            Console.WriteLine("> Jít do Obchodu");
        else
            Console.WriteLine("  Jít do Obchodu");

        if (selected == 2)
            Console.WriteLine("> Inventář");
        else
            Console.WriteLine("  Inventář");

        if (selected == 3)
            Console.WriteLine("> Opustit Hru");
        else
            Console.WriteLine("  Opustit Hru");
    }

    public static void UiButtonMenuDown()
    {
        selected++;
        selected += 4;
        selected %= 4;
    }

    public static void UiButtonMenuUp()
    {
        selected--;
        selected += 4;
        selected %= 4;
    }

    public static void EnterOption(PlayerData playerData)
    {
        Console.Clear();

        switch (selected)
        {
            case 0: playerData.GameState = GameState.Catching; Program.CatchingInit(); break;
            case 1: playerData.GameState = GameState.Shop; break;
            case 2: playerData.GameState = GameState.Inventory; break;
            case 3: Console.CursorVisible = true; Environment.Exit(0); break;
        }
    }
}