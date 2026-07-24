class CommandProc
{
    private static PlayerData data;

    private static bool running = false;

    private static string helpString = @"Kde Jsou Ryby!? Debug Command Interface

Commands:
    money <ammount>                 Sets the money of the player.
    upgrade rod <level>             Sets the upgrade level of the fishing rod.
    upgrade ship <level>            Sets the upgrade level of the ship.
    inventory add <name> <weight>   Adds a fish to the inventory.
";

    public static void Enter(string command, PlayerData playerData)
    {
        playerData = playerData;

        Console.Clear();
        Console.WriteLine("Welcome to Kde Jsou Ryby!? Debug Command Interface");

        running = true;
        Loop();
    }

    private static void Loop()
    {
        while (running)
        {
            string? input = Console.ReadLine();

            if (input != null)
            {
                ProcessCommand(input);
            }
        }
    }

    private static void ProcessCommand(string cmd)
    {
        List<string> parts = new List<string>();
        string part = "";

        foreach (char c in cmd)
        {
            if (c == ' ')
            {
                parts.Add(part);
                part = "";
            }
            else
            {
                part += c;
            }
        }
    }
}