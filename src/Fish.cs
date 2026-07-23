class Fish
{
    public string Name;
    public double Weight;
    public int RodLevel;
    public bool IsSea;
    public Image Image;

    public Fish(TFish template)
    {
        Name = template.Name;
        Weight = Math.Round((template.Weight + template.WeightVar * (Program.Rng.NextDouble() * 2.0 - 1.0)) * 100.0) / 100.0;
        RodLevel = template.RodLevel;
        IsSea = template.IsSea;
        Image = new Image(template.Image);
    }

    public string GetFormatedData()
    {
        string isFromSea = IsSea ? "From the sea." : "Not from the sea.";

        return $"{Name}\n- Weight: {Weight} Kg\n- {isFromSea}\n- Rod Level: {RodLevel}";
    }
}
