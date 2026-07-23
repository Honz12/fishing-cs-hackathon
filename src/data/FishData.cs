/*
    RodLevel = 0 pri startu hry
*/

class FishData
{
    public static TFish[] fishes = {
        new TFish() {
            Name = "Kapr obecný",
            Weight = 8.25,
            WeightVar = 6.75,
            RodLevel = 1,
            IsSea = false,
            Image = "kaprObecny.txt"
        },
        new TFish() {
            Name = "Pstruh duhový",
            Weight = 1.125,
            WeightVar = 0.875,
            RodLevel = 0,
            IsSea = false,
            Image = "pstruhDuhovy.txt"
        },
        new TFish() {
            Name = "Štika obecná",
            Weight = 4.5,
            WeightVar = 3.5,
            RodLevel = 1,
            IsSea = false,
            Image = "stikaObecna.txt"
        }
    };
}