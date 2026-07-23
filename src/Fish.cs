class Fish
{
    public string Name;
    public double Weight;
    public int RodLevel;
    public bool IsSea;
    public Image Image;
    public FishRarity Rarity;
    public double PricePerKg;
    public double AverageWeight;

    public Fish(TFish template)
    {
        Name = template.Name;
        Weight = Math.Round((template.Weight + template.WeightVar * (Program.Rng.NextDouble() * 2.0 - 1.0)) * 100.0) / 100.0;
        RodLevel = template.RodLevel;
        IsSea = template.IsSea;
        Image = new Image("fish", template.Image);
        Rarity = template.Rarity;
        PricePerKg = template.PricePerKg;
        AverageWeight = template.Weight;
    }

    public Fish()
    {
        Name = "";
        Weight = 0.0;
        RodLevel = 0;
        IsSea = false;
        Image = new Image("fish", "uhorRicniEletricky.txt");
        Rarity = FishRarity.Common;
        PricePerKg = 0.0;
    }

    public string GetFormatedData()
    {
        string isFromSea = IsSea ? "Mořská" : "Sladkovodní";

        return $"{Name}\n- Váha: {Weight} Kg (Průměr {AverageWeight} Kg)\n- {isFromSea}\n- Vzácnost: {Program.GetTransRarity(Rarity)}\n- Požadovaná Úroveň Prut: {RodLevel+1}\n- Prodává se za: {(uint) (PricePerKg * Weight)}";
    }

    public string GetInfoCompact()
    {
        return $"{Name, 20} | Váha: {Weight, 6} Kg | {Program.GetTransRarity(Rarity)} | Cena: {(uint) (PricePerKg * Weight)}";
    }
}
