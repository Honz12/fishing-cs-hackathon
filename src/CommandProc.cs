class CommandProc
{
    private static PlayerData data;

    private static bool running = false;

    private static string helpString = @"Kde Jsou Ryby!? Debug Command Interface

Commands:
    quit                            Exits the Kde Jsou Ryby!? Debug Command Interface.
    help                            Shows the Kde Jsou Ryby!? Debug Command Interface help text.
    money <ammount>                 Sets the money of the player.
    upgrade rod <level>             Sets the upgrade level of the fishing rod.
    upgrade ship <level>            Sets the upgrade level of the ship.
    upgrade house <level>           Sets the upgrade level of the house.
";

    public static void Enter(PlayerData playerData)
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
            Console.Write(">>> ");

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
        parts.Add(part);
        part = "";

        switch (parts.Count)
        {
            case 1:
                switch (parts[0])
                {
                    case "quit":
                        running = false;
                        break;
                    case "help":
                        Console.Write(helpString);
                        break;
                }
                break;
            case 2:
                if (parts[0] == "money")
                {
                    bool success = int.TryParse(parts[1], out int v);

                    data.Money = (uint) v;
                }
                break;
            case 3:
                if (parts[0] == "upgrade")
                {
                    bool success = int.TryParse(parts[2], out int v);

                    data.Money = (uint) v;
                    
                    if (parts[1] == "rod")
                    {
                        data.RodLevel = (ushort) v;
                    }
                    
                    if (parts[1] == "ship")
                    {
                        data.InventorySize = (byte) v;
                    }
                    
                    if (parts[1] == "rod")
                    {
                        data.HouseLevel = (byte) v;
                    }
                }
                break;
        }
    }
}