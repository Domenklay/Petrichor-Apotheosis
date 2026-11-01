using System;
using System.Linq;
using PetrichorLegacy.Models;
using PetrichorLegacy.Persistence;
using RoR2;
using UnityEngine;

namespace PetrichorLegacy.Progression
{
    public static class ProgressionService
    {
        public static int LastAwardAmount { get; private set; }
        public static string LastAwardReason { get; private set; } = string.Empty;
        public static float LastAwardTime { get; private set; } = -100f;

        public static int XpRequiredForNextLevel(int level)
        {
            level = Math.Max(1, level);
            return Math.Max(1000, (int)Math.Round(900d + level * 110d + Math.Pow(level, 1.35d) * 20d));
        }

        public static int PrestigeLevel(int level) => Math.Max(0, level / 100);

        public static void AwardXp(ProfileProgress profile, int amount, string reason = null)
        {
            if (profile == null || amount <= 0) return;
            profile.currentXp += amount;
            profile.lifetimeXp += amount;
            LastAwardAmount = amount;
            LastAwardReason = string.IsNullOrWhiteSpace(reason) ? "Опыт" : reason;
            LastAwardTime = Time.unscaledTime;

            while (profile.currentXp >= XpRequiredForNextLevel(profile.level))
            {
                profile.currentXp -= XpRequiredForNextLevel(profile.level);
                profile.level++;
                GrantLevelRewards(profile, profile.level);
            }
            SaveService.SaveCurrent();
        }

        public static void GrantLevelRewards(ProfileProgress profile, int newLevel)
        {
            if (profile == null || newLevel < 2) return;
            if (newLevel <= 40) profile.unspentTalentPoints++;
            else
            {
                profile.unspentMasteryPoints++;
                if ((newLevel & 1) == 0) profile.unspentTalentPoints++;
            }
            if (newLevel % 25 == 0) profile.unspentPerfectionPoints++;
        }

        public static int EarnedTalentPointsForLevel(int level)
        {
            level = Math.Max(1, level);
            if (level <= 40) return Math.Max(0, level - 1);
            return 39 + (level - 40) / 2;
        }

        public static int EarnedMasteryPointsForLevel(int level) => Math.Max(0, level - 40);
        public static int EarnedPerfectionPointsForLevel(int level) => Math.Max(0, level / 25);

        public static bool TryBuyTalent(ProfileProgress profile, string talentId, out string error)
        {
            error = null;
            var def = TalentCatalog.Find(talentId);
            if (profile == null || def == null) { error = "Неизвестный талант."; return false; }
            if (profile.level < def.UnlockLevel) { error = $"Требуется мета-уровень {def.UnlockLevel}."; return false; }
            var current = TalentCatalog.GetRank(profile, talentId);
            if (current >= def.MaxRank) { error = "Талант уже изучен полностью."; return false; }
            if (!TalentCatalog.PrerequisiteMet(profile, def)) { error = "Сначала изучи предыдущий узел ветки."; return false; }

            var path = LegacyPathForTalent(talentId);
            var chosenPath = LegacyPathChoice(profile);
            if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(chosenPath) && !string.Equals(path, chosenPath, StringComparison.OrdinalIgnoreCase))
            {
                error = "Это направление уже недоступно для текущего Наследия.";
                return false;
            }
            if (talentId == "legacy_pack2" && (string.IsNullOrEmpty(chosenPath) || !LegacyPathComplete(profile, chosenPath)))
            {
                error = "Сначала заверши выбранное направление Наследия.";
                return false;
            }

            if (profile.unspentTalentPoints < def.PointCost) { error = "Недостаточно очков талантов."; return false; }

            var record = profile.talents.FirstOrDefault(t => t.id == talentId);
            if (record == null)
            {
                record = new TalentRankRecord { id = talentId, rank = 0 };
                profile.talents.Add(record);
            }
            record.rank++;
            profile.unspentTalentPoints -= def.PointCost;
            SaveService.SaveCurrent();
            return true;
        }

        public static string LegacyPathForTalent(string talentId)
        {
            switch (talentId)
            {
                case "legacy_damage": case "legacy_attack": case "legacy_hunter": case "legacy_precision": case "legacy_finisher": return "combat";
                case "legacy_hp": case "legacy_regen": case "legacy_armor": case "legacy_resolve": case "legacy_secondwind": return "survival";
                case "legacy_move": case "legacy_sprint": case "legacy_jump": case "legacy_stride": case "legacy_aircontrol": return "mobility";
                default: return string.Empty;
            }
        }

        public static string LegacyPathChoice(ProfileProgress profile)
        {
            if (profile == null) return string.Empty;
            var ids = new[]
            {
                "legacy_damage", "legacy_attack", "legacy_hunter", "legacy_precision", "legacy_finisher",
                "legacy_hp", "legacy_regen", "legacy_armor", "legacy_resolve", "legacy_secondwind",
                "legacy_move", "legacy_sprint", "legacy_jump", "legacy_stride", "legacy_aircontrol"
            };
            foreach (var id in ids)
                if (TalentCatalog.GetRank(profile, id) > 0) return LegacyPathForTalent(id);
            return string.Empty;
        }

        public static bool LegacyPathComplete(ProfileProgress profile, string path)
        {
            if (profile == null || string.IsNullOrEmpty(path)) return false;
            if (string.Equals(path, "combat", StringComparison.OrdinalIgnoreCase)) return TalentCatalog.GetRank(profile, "legacy_finisher") > 0;
            if (string.Equals(path, "survival", StringComparison.OrdinalIgnoreCase)) return TalentCatalog.GetRank(profile, "legacy_secondwind") > 0;
            if (string.Equals(path, "mobility", StringComparison.OrdinalIgnoreCase)) return TalentCatalog.GetRank(profile, "legacy_aircontrol") > 0;
            return false;
        }

        public static bool CanBuyLegacyPathTalent(ProfileProgress profile, string talentId)
        {
            if (profile == null) return false;
            var chosen = LegacyPathChoice(profile);
            if (talentId == "legacy_pack2") return !string.IsNullOrEmpty(chosen) && LegacyPathComplete(profile, chosen);
            var path = LegacyPathForTalent(talentId);
            return string.IsNullOrEmpty(path) || string.IsNullOrEmpty(chosen) || string.Equals(path, chosen, StringComparison.OrdinalIgnoreCase);
        }

        public static bool TryBuyMastery(ProfileProgress profile, string masteryId, out string error) =>
            TryBuyMasteryRanks(profile, masteryId, 1, out error);

        public static bool TryBuyMasteryRanks(ProfileProgress profile, string masteryId, int requestedRanks, out string error)
        {
            error = null;
            var def = MasteryCatalog.Find(masteryId);
            if (profile == null || def == null) { error = "Неизвестное мастерство."; return false; }
            if (profile.level < 41) { error = "Мастерство открывается с 41 уровня."; return false; }
            if (profile.activeClass != def.ClassId) { error = "Сначала сделай этот класс активным."; return false; }
            if (profile.unspentMasteryPoints <= 0) { error = "Недостаточно очков мастерства."; return false; }
            var count = Math.Max(1, Math.Min(requestedRanks, profile.unspentMasteryPoints));

            var record = profile.masteries.FirstOrDefault(x => x.id == masteryId);
            if (record == null)
            {
                record = new MasteryRankRecord { id = masteryId, rank = 0 };
                profile.masteries.Add(record);
            }
            record.rank += count;
            profile.unspentMasteryPoints -= count;
            SaveService.SaveCurrent();
            return true;
        }

        public static bool TryBuyPerfection(ProfileProgress profile, string perfectionId, out string error)
        {
            error = null;
            var def = PerfectionCatalog.Find(perfectionId);
            if (profile == null || def == null) { error = "Неизвестный узел Совершенства."; return false; }
            if (profile.level < def.UnlockLevel) { error = $"Требуется уровень {def.UnlockLevel}."; return false; }
            if (!PerfectionCatalog.PrerequisiteMet(profile, def)) { error = "Сначала открой предыдущий узел этого направления."; return false; }
            var rank = PerfectionCatalog.GetRank(profile, perfectionId);
            if (rank >= def.MaxRank) { error = "Узел уже изучен."; return false; }
            if (profile.unspentPerfectionPoints <= 0) { error = "Недостаточно очков Совершенства."; return false; }

            var record = profile.perfections.FirstOrDefault(x => x.id == perfectionId);
            if (record == null)
            {
                record = new PerfectionRankRecord { id = perfectionId, rank = 0 };
                profile.perfections.Add(record);
            }
            record.rank++;
            profile.unspentPerfectionPoints--;
            SaveService.SaveCurrent();
            return true;
        }

        public static bool TrySetActiveClass(ProfileProgress profile, string classId, out string error)
        {
            error = null;
            if (profile == null) return false;
            if (Run.instance != null) { error = "Класс можно менять только вне забега."; return false; }
            if (profile.level < 10) { error = "Классы открываются на 10 уровне."; return false; }
            if (classId != "Guardian" && classId != "Slayer" && classId != "Pathfinder" && classId != "Seeker" && classId != "Arcanist" && classId != "Technomancer" && classId != "Commander")
            { error = "Неизвестный класс."; return false; }

            profile.activeClass = classId;
            if (!SpecBelongsToClass(profile.activeSpecialization, classId)) profile.activeSpecialization = string.Empty;
            SaveService.SaveCurrent();
            return true;
        }

        public static bool TrySetActiveSpecialization(ProfileProgress profile, string specId, out string error)
        {
            error = null;
            if (profile == null) return false;
            if (Run.instance != null) { error = "Специализацию можно менять только вне забега."; return false; }
            if (profile.level < 20) { error = "Специализации открываются на 20 уровне."; return false; }
            if (!SpecBelongsToClass(specId, profile.activeClass)) { error = "Эта специализация не принадлежит активному классу."; return false; }

            profile.activeSpecialization = specId;
            SaveService.SaveCurrent();
            return true;
        }

        public static bool SpecBelongsToClass(string specId, string classId)
        {
            if (string.IsNullOrEmpty(specId)) return true;
            switch (classId)
            {
                case "Guardian": return specId == "Bastion" || specId == "Unbroken" || specId == "Vanguard";
                case "Slayer": return specId == "Executioner" || specId == "Hunter" || specId == "Berserker";
                case "Pathfinder": return specId == "Scout" || specId == "Acrobat" || specId == "Saboteur";
                case "Seeker": return specId == "Merchant" || specId == "Prospector" || specId == "RelicKeeper";
                case "Arcanist": return specId == "Chronomancer" || specId == "Hexer" || specId == "Stormcaller";
                case "Technomancer": return specId == "DroneMaster" || specId == "Interceptor" || specId == "Constructor";
                case "Commander": return specId == "Tactician" || specId == "StandardBearer" || specId == "Warlord";
                default: return false;
            }
        }

        public static void Respec(ProfileProgress profile)
        {
            if (profile == null) return;
            var refund = TalentCatalog.GetSpentPoints(profile);
            profile.talents.Clear();
            profile.unspentTalentPoints += refund;
            profile.activeClass = string.Empty;
            profile.activeSpecialization = string.Empty;
            SaveService.SaveCurrent();
        }

        public static void RespecPerfection(ProfileProgress profile)
        {
            if (profile == null || profile.perfections == null) return;
            var refund = 0;
            foreach (var r in profile.perfections) if (r != null) refund += Math.Max(0, r.rank);
            profile.perfections.Clear();
            profile.unspentPerfectionPoints += refund;
            SaveService.SaveCurrent();
        }

        public static int StartingCapacity(ProfileProgress profile)
        {
            if (profile == null) return 0;
            var level = profile.level;

            // Levels 1-19 rely on the Legacy tree: 0 base slots, then +1/+1 from the two backpack nodes.
            // From level 20 onward the old long-term capacity curve is preserved after those two slots are added.
            var capacity = 0;
            if (level >= 20) capacity = 3;
            if (level >= 40) capacity = 5;
            if (level >= 50) capacity = 8;
            if (level >= 70) capacity = 9;
            if (level >= 100) capacity = 10;
            if (level >= 150) capacity = 11;
            if (level >= 250) capacity = 12;
            if (level >= 400) capacity = 13;

            capacity += TalentCatalog.GetRank(profile, "legacy_pack");
            capacity += TalentCatalog.GetRank(profile, "legacy_pack2");
            capacity += PerfectionCatalog.GetRank(profile, "perfection_pack");
            capacity += PerfectionCatalog.GetRank(profile, "perfection_prestige_treasurer");
            capacity += PrestigeCatalog.ArsenalCapacityBonus(profile.level);
            return capacity;
        }
    }
}
