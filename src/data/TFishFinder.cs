static class TFishFinder
{
    public static TFish FindRandomFish(bool isSea, int rodLevel)
    {
        List<int> possible = new List<int>();

        for (int i = 0; i < FishData.fishes.Length; i++)
        {
            TFish fish = FishData.fishes[i];

            if (fish.IsSea == isSea && rodLevel >= fish.RodLevel)
                possible.Add(i);
        }

        return FishData.fishes[possible[Program.Rng.Next(0, possible.Count)]];
    }
}
