enum FishRarity
{
    Common,
    Rare,
    Epic,
    Mythic,
}

class TFish
{
    public required string Name;
    public required double Weight;
    public required double WeightVar;
    public required int RodLevel;
    public required bool IsSea;
    public required string Image;
    public FishRarity Rarity;
    public double PricePerKg;
}
