using System;

namespace PetrichorLegacy.Progression
{
    public static class PrestigeCatalog
    {
        public static int RankForLevel(int level) => Math.Max(0, level / 100);
        public static int NextLevel(int level) => (RankForLevel(level) + 1) * 100;
        public static int Step(int rank) => rank <= 0 ? 0 : ((rank - 1) % 5) + 1;
        public static int Cycle(int rank) => rank <= 0 ? 0 : ((rank - 1) / 5) + 1;

        private static int StepStacks(int rank, int step)
        {
            if (rank < step || step < 1 || step > 5) return 0;
            return 1 + (rank - step) / 5;
        }

        public static int StacksForStep(int level, int step) => StepStacks(RankForLevel(level), step);
        public static float SpecialXpBonus(int level) => StepStacks(RankForLevel(level), 1) * 0.02f;
        public static int StartingGoldBonus(int level) => StepStacks(RankForLevel(level), 2) * 10;
        public static float SpecialGoldBonus(int level) => StepStacks(RankForLevel(level), 3) * 0.05f;
        public static float HunterDamageBonus(int level) => StepStacks(RankForLevel(level), 4) * 0.02f;
        public static int ArsenalCapacityBonus(int level) => StepStacks(RankForLevel(level), 5);

        public static string RewardName(int step)
        {
            switch (step)
            {
                case 1: return "Летопись охоты";
                case 2: return "Ветеранский запас";
                case 3: return "Право трофея";
                case 4: return "Печать охотника";
                case 5: return "Хартия Арсенала";
                default: return "Престиж";
            }
        }

        public static string RewardDescription(int step)
        {
            switch (step)
            {
                case 1: return "+2% мета-XP с особых монстров.";
                case 2: return "+10 стартового золота.";
                case 3: return "+5% золота с особых монстров.";
                case 4: return "+2% урона боссам и особым монстрам.";
                case 5: return "+1 вместимость Арсенала.";
                default: return string.Empty;
            }
        }
    }
}
