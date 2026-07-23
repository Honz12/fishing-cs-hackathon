/*
    RodLevel:
    - Sladkovodní: 0 až 6
    - Mořské: 2 až 10
    
    Výpočet váhy: Weight + WeightVar * rng (rng = -1.0 až 1.0)
    WeightVar < Weight
*/

class FishData
{
    public static TFish[] fishes = {
        // SLADKOVODNÍ RYBY
        new TFish() {
            Name = "Kapr obecný",
            Weight = 8.25,
            WeightVar = 6.75, // 1.5 kg – 15.0 kg
            RodLevel = 3,
            IsSea = false,
            Image = "kaprObecny.txt"
        },
        new TFish() {
            Name = "Pstruh duhový",
            Weight = 1.125,
            WeightVar = 0.875, // 0.25 kg – 2.0 kg
            RodLevel = 1,
            IsSea = false,
            Image = "pstruhDuhovy.txt"
        },
        new TFish() {
            Name = "Štika obecná",
            Weight = 4.5,
            WeightVar = 3.5, // 1.0 kg – 8.0 kg
            RodLevel = 2,
            IsSea = false,
            Image = "stikaObecna.txt"
        },
        new TFish() {
            Name = "Candát obecný",
            Weight = 3.0,
            WeightVar = 2.0, // 1.0 kg – 5.0 kg
            RodLevel = 2,
            IsSea = false,
            Image = "candatObecny.txt"
        },
        new TFish() {
            Name = "Sumec velký",
            Weight = 16.5,
            WeightVar = 13.5, // 3.0 kg – 30.0 kg
            RodLevel = 6,
            IsSea = false,
            Image = "sumecVelky.txt"
        },
        new TFish() {
            Name = "Okoun říční",
            Weight = 0.45,
            WeightVar = 0.35, // 0.1 kg – 0.8 kg
            RodLevel = 0,
            IsSea = false,
            Image = "okounRicni.txt"
        },
        new TFish() {
            Name = "Lín obecný",
            Weight = 0.9,
            WeightVar = 0.6, // 0.3 kg – 1.5 kg
            RodLevel = 0,
            IsSea = false,
            Image = "linObecny.txt"
        },
        new TFish() {
            Name = "Jeseter velký",
            Weight = 8.5,
            WeightVar = 6.5, // 2.0 kg – 15.0 kg
            RodLevel = 4,
            IsSea = false,
            Image = "jeseterVelky.txt"
        },
        new TFish() {
            Name = "Úhoř říční",
            Weight = 0.9,
            WeightVar = 0.6, // 0.3 kg – 1.5 kg
            RodLevel = 1,
            IsSea = false,
            Image = "uhorRicni.txt"
        },
        new TFish() {
            Name = "Amur bílý",
            Weight = 6.0,
            WeightVar = 4.0, // 2.0 kg – 10.0 kg
            RodLevel = 3,
            IsSea = false,
            Image = "amurBily.txt"
        },

        // MOŘSKÉ RYBY
        new TFish() {
            Name = "Losos obecný",
            Weight = 5.0,
            WeightVar = 3.0, // 2.0 kg – 8.0 kg
            RodLevel = 4,
            IsSea = true,
            Image = "lososObecny.txt"
        },
        new TFish() {
            Name = "Treska tmavá",
            Weight = 3.5,
            WeightVar = 2.5, // 1.0 kg – 6.0 kg
            RodLevel = 3,
            IsSea = true,
            Image = "treskaTmava.txt"
        },
        new TFish() {
            Name = "Tuňák obecný",
            Weight = 135.0,
            WeightVar = 115.0, // 20.0 kg – 250.0 kg
            RodLevel = 8,
            IsSea = true,
            Image = "tunakObecny.txt"
        },
        new TFish() {
            Name = "Sardinka obecná",
            Weight = 0.06,
            WeightVar = 0.04, // 0.02 kg – 0.1 kg
            RodLevel = 2,
            IsSea = true,
            Image = "sardinkaObecna.txt"
        },
        new TFish() {
            Name = "Sleď obecný",
            Weight = 0.25,
            WeightVar = 0.15, // 0.1 kg – 0.4 kg
            RodLevel = 2,
            IsSea = true,
            Image = "sledObecny.txt"
        },
        new TFish() {
            Name = "Makrela obecná",
            Weight = 0.65,
            WeightVar = 0.35, // 0.3 kg – 1.0 kg
            RodLevel = 2,
            IsSea = true,
            Image = "makrelaObecna.txt"
        },
        new TFish() {
            Name = "Platýs bradavičnatý",
            Weight = 1.15,
            WeightVar = 0.85, // 0.3 kg – 2.0 kg
            RodLevel = 3,
            IsSea = true,
            Image = "platysBradavicnaty.txt"
        },
        new TFish() {
            Name = "Pražman zlatý",
            Weight = 0.9,
            WeightVar = 0.6, // 0.3 kg – 1.5 kg
            RodLevel = 2,
            IsSea = true,
            Image = "prazmanZlaty.txt"
        },
        new TFish() {
            Name = "Mořský ďas",
            Weight = 8.5,
            WeightVar = 6.5, // 2.0 kg – 15.0 kg
            RodLevel = 5,
            IsSea = true,
            Image = "morskyDas.txt"
        },
        new TFish() {
            Name = "Mečoun obecný",
            Weight = 115.0,
            WeightVar = 85.0, // 30.0 kg – 200.0 kg
            RodLevel = 7,
            IsSea = true,
            Image = "mecounObecny.txt"
        }
    };
}