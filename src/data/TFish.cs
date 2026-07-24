enum FishRarity
{
    Common,
    Rare,
    Epic,
    Mythic,
}

class TFish // The "TemplateFish".
{
    public string Name;
    public double Weight;
    public double WeightVar;
    public int RodLevel;
    public bool IsSea;
    public string Image;
    public FishRarity Rarity;
    public double PricePerKg;
}
