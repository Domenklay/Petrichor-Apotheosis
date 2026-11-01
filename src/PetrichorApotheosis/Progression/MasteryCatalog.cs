using System;
using System.Collections.Generic;
using System.Linq;
using PetrichorLegacy.Models;

namespace PetrichorLegacy.Progression
{
    public sealed class MasteryDef
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Description;
        public readonly string ClassId;
        public readonly float PerRank;
        public readonly string IconItem;

        public MasteryDef(string id, string name, string description, string classId, float perRank, string iconItem)
        {
            Id = id;
            Name = name;
            Description = description;
            ClassId = classId;
            PerRank = perRank;
            IconItem = iconItem;
        }
    }

    public sealed class MasteryMilestoneDef
    {
        public readonly string ClassId;
        public readonly int RequiredRanks;
        public readonly string Name;
        public readonly string Description;

        public MasteryMilestoneDef(string classId, int requiredRanks, string name, string description)
        {
            ClassId = classId;
            RequiredRanks = requiredRanks;
            Name = name;
            Description = description;
        }
    }

    public static class MasteryCatalog
    {
        public static readonly IReadOnlyList<MasteryDef> All = new List<MasteryDef>
        {
            new MasteryDef("mastery_guardian_hp", "Основание крепости", "+0.14% максимального здоровья за эффективный ранг.", "Guardian", 0.0014f, "Infusion"),
            new MasteryDef("mastery_guardian_armor", "Стальная доктрина", "+0.18 брони за эффективный ранг.", "Guardian", 0.18f, "ArmorPlate"),
            new MasteryDef("mastery_guardian_regen", "Живучесть", "+0.22% регенерации за эффективный ранг.", "Guardian", 0.0022f, "Medkit"),
            new MasteryDef("mastery_guardian_shield", "Барьерный контур", "+0.10% базового здоровья в виде щита за эффективный ранг.", "Guardian", 0.0010f, "PersonalShield"),

            new MasteryDef("mastery_slayer_damage", "Острие", "+0.11% урона за эффективный ранг.", "Slayer", 0.0011f, "Crowbar"),
            new MasteryDef("mastery_slayer_attack", "Боевой ритм", "+0.11% скорости атаки за эффективный ранг.", "Slayer", 0.0011f, "Syringe"),
            new MasteryDef("mastery_slayer_cooldown", "Техника убийцы", "-0.045% времени восстановления за эффективный ранг.", "Slayer", 0.00045f, "Bandolier"),
            new MasteryDef("mastery_slayer_crit", "Безошибочность", "+0.045% критического шанса за эффективный ранг.", "Slayer", 0.045f, "CritGlasses"),

            new MasteryDef("mastery_path_move", "Дальний шаг", "+0.12% скорости движения за эффективный ранг.", "Pathfinder", 0.0012f, "Hoof"),
            new MasteryDef("mastery_path_sprint", "Темп экспедиции", "+0.18% скорости спринта за эффективный ранг.", "Pathfinder", 0.0018f, "SprintBonus"),
            new MasteryDef("mastery_path_jump", "Контроль высоты", "+0.22% силы прыжка за эффективный ранг.", "Pathfinder", 0.0022f, "Feather"),
            new MasteryDef("mastery_path_control", "Манёвренность", "+0.08% скорости движения за эффективный ранг.", "Pathfinder", 0.0008f, "JumpBoost"),

            new MasteryDef("mastery_seeker_gold", "Доля добычи", "+0.18% золота с особых монстров за эффективный ранг.", "Seeker", 0.0018f, "GoldOnHurt"),
            new MasteryDef("mastery_seeker_specialxp", "Редкие трофеи", "+0.14% мета-XP за особых монстров за эффективный ранг.", "Seeker", 0.0014f, "TreasureCache"),
            new MasteryDef("mastery_seeker_stagegold", "Снабжение", "+0.20 золота при переходе на следующую стадию за эффективный ранг.", "Seeker", 0.20f, "BonusGoldPackOnKill"),
            new MasteryDef("mastery_seeker_startgold", "Стартовый капитал", "+0.15 стартового золота за эффективный ранг.", "Seeker", 0.15f, "EquipmentMagazine"),

            new MasteryDef("mastery_arcanist_damage", "Арканный резонанс", "+0.10% урона за эффективный ранг.", "Arcanist", 0.0010f, "NearbyDamageBonus"),
            new MasteryDef("mastery_arcanist_attack", "Быстрый каст", "+0.11% скорости атаки за эффективный ранг.", "Arcanist", 0.0011f, "AttackSpeedOnCrit"),
            new MasteryDef("mastery_arcanist_cooldown", "Поток времени", "-0.05% времени восстановления за эффективный ранг.", "Arcanist", 0.0005f, "AlienHead"),
            new MasteryDef("mastery_arcanist_move", "Фазовый шаг", "+0.09% скорости движения за эффективный ранг.", "Arcanist", 0.0009f, "UtilitySkillMagazine"),

            new MasteryDef("mastery_tech_attack", "Сервоприводы", "+0.11% скорости атаки владельца за эффективный ранг.", "Technomancer", 0.0011f, "WarCryOnMultiKill"),
            new MasteryDef("mastery_tech_move", "Лёгкое шасси", "+0.11% скорости движения владельца за эффективный ранг.", "Technomancer", 0.0011f, "SprintOutOfCombat"),
            new MasteryDef("mastery_tech_minion_damage", "Боевые алгоритмы", "+0.28% урона миньонов за эффективный ранг.", "Technomancer", 0.0028f, "DroneWeapons"),
            new MasteryDef("mastery_tech_minion_health", "Усиленные корпуса", "+0.28% здоровья миньонов за эффективный ранг.", "Technomancer", 0.0028f, "CaptainDefenseMatrix"),

            new MasteryDef("mastery_commander_damage", "Приказ на огонь", "+0.09% урона за эффективный ранг.", "Commander", 0.0009f, "BossDamageBonus"),
            new MasteryDef("mastery_commander_move", "Перестроение", "+0.09% скорости движения за эффективный ранг.", "Commander", 0.0009f, "SprintArmor"),
            new MasteryDef("mastery_commander_armor", "Дисциплина", "+0.11 брони за эффективный ранг.", "Commander", 0.11f, "FlatHealth"),
            new MasteryDef("mastery_commander_minion", "Командование", "+0.18% урона миньонов за эффективный ранг.", "Commander", 0.0018f, "GhostOnKill")
        };

        public static readonly IReadOnlyList<MasteryMilestoneDef> Milestones = new List<MasteryMilestoneDef>
        {
            new MasteryMilestoneDef("Guardian", 25, "Оплот", "+2 брони."),
            new MasteryMilestoneDef("Guardian", 60, "Непоколебимость", "+2% максимального здоровья."),
            new MasteryMilestoneDef("Guardian", 120, "Вторая линия", "+5% регенерации и щит размером 3% базового здоровья."),
            new MasteryMilestoneDef("Guardian", 220, "Живая крепость", "+4 брони и ещё +2% максимального здоровья."),

            new MasteryMilestoneDef("Slayer", 25, "Острие", "+1% урона."),
            new MasteryMilestoneDef("Slayer", 60, "Разгон", "+1.5% скорости атаки."),
            new MasteryMilestoneDef("Slayer", 120, "Казнь", "+3% урона боссам и особым монстрам."),
            new MasteryMilestoneDef("Slayer", 220, "Совершенный хищник", "+2% урона и +1% скорости атаки."),

            new MasteryMilestoneDef("Pathfinder", 25, "Лёгкий шаг", "+1.5% скорости движения."),
            new MasteryMilestoneDef("Pathfinder", 60, "Без остановки", "+2% скорости спринта."),
            new MasteryMilestoneDef("Pathfinder", 120, "Воздушный контроль", "+5% силы прыжка и +1 броня."),
            new MasteryMilestoneDef("Pathfinder", 220, "Свобода движения", "+2% скорости движения и +1 дополнительный прыжок."),

            new MasteryMilestoneDef("Seeker", 25, "Знаток трофеев", "+5% мета-XP за особых монстров."),
            new MasteryMilestoneDef("Seeker", 60, "Выгодная добыча", "+5% золота с особых монстров."),
            new MasteryMilestoneDef("Seeker", 120, "Линия снабжения", "+15 золота при переходе на новую стадию."),
            new MasteryMilestoneDef("Seeker", 220, "Большой капитал", "+40 стартового золота."),

            new MasteryMilestoneDef("Arcanist", 25, "Стабильный поток", "Ещё -1% времени восстановления навыков."),
            new MasteryMilestoneDef("Arcanist", 60, "Резонанс", "+1% урона."),
            new MasteryMilestoneDef("Arcanist", 120, "Перегрузка", "+1.5% скорости атаки."),
            new MasteryMilestoneDef("Arcanist", 220, "Вне времени", "Ещё -2% времени восстановления навыков."),

            new MasteryMilestoneDef("Technomancer", 25, "Синхронизация", "+4% урона миньонов."),
            new MasteryMilestoneDef("Technomancer", 60, "Резервные корпуса", "+4% здоровья миньонов."),
            new MasteryMilestoneDef("Technomancer", 120, "Боевой рой", "+5% скорости атаки и движения миньонов."),
            new MasteryMilestoneDef("Technomancer", 220, "Единая сеть", "+2% скорости атаки и движения владельца."),

            new MasteryMilestoneDef("Commander", 25, "Дисциплина", "+1 броня."),
            new MasteryMilestoneDef("Commander", 60, "Строй", "+3% урона и здоровья миньонов."),
            new MasteryMilestoneDef("Commander", 120, "Боевой приказ", "+1.5% урона и скорости движения владельца."),
            new MasteryMilestoneDef("Commander", 220, "Главнокомандующий", "+5% скорости атаки и движения миньонов; +2 брони владельцу.")
        };

        private static readonly Dictionary<string, MasteryDef> ById = All.ToDictionary(x => x.Id, x => x);

        public static MasteryDef Find(string id) => id != null && ById.TryGetValue(id, out var def) ? def : null;

        public static int GetRank(ProfileProgress profile, string id)
        {
            if (profile?.masteries == null || string.IsNullOrEmpty(id)) return 0;
            return profile.masteries.FirstOrDefault(x => x.id == id)?.rank ?? 0;
        }

        public static int TotalRanks(ProfileProgress profile, string classId)
        {
            if (profile?.masteries == null || string.IsNullOrEmpty(classId)) return 0;
            var total = 0;
            foreach (var r in profile.masteries)
            {
                if (r == null || r.rank <= 0) continue;
                var def = Find(r.id);
                if (def != null && def.ClassId == classId) total += r.rank;
            }
            return total;
        }

        public static IEnumerable<MasteryMilestoneDef> MilestonesFor(string classId) =>
            Milestones.Where(x => string.Equals(x.ClassId, classId, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.RequiredRanks);

        public static readonly int[] DisciplineThresholds = { 10, 25, 50, 100 };

        public static float TotalEffectiveRanks(ProfileProgress profile, string classId)
        {
            if (profile?.masteries == null || string.IsNullOrEmpty(classId)) return 0f;
            var total = 0f;
            foreach (var r in profile.masteries)
            {
                if (r == null || r.rank <= 0) continue;
                var def = Find(r.id);
                if (def != null && def.ClassId == classId) total += EffectiveRanks(r.rank);
            }
            return total;
        }

        public static float DisciplineMultiplier(int rank)
        {
            var mult = 1f;
            if (rank >= 10) mult += 0.05f;
            if (rank >= 25) mult += 0.075f;
            if (rank >= 50) mult += 0.10f;
            if (rank >= 100) mult += 0.125f;
            return mult;
        }

        public static string DisciplineMilestoneText(MasteryDef def, int threshold)
        {
            if (def == null) return string.Empty;
            if (threshold == 10) return "Практика: сила дисциплины +5%.";
            if (threshold == 25) return "Специализация: ещё +7.5%.";
            if (threshold == 50) return "Эксперт: ещё +10%.";
            if (threshold == 100) return "Совершенное владение: ещё +12.5%; " + SignatureText(def.Id);
            return string.Empty;
        }

        public static string SignatureText(string id)
        {
            switch (id)
            {
                case "mastery_guardian_hp": return "+2% максимального здоровья.";
                case "mastery_guardian_armor": return "+2 брони.";
                case "mastery_guardian_regen": return "в начале стадии восстановить ещё 3% здоровья.";
                case "mastery_guardian_shield": return "в начале стадии получить барьер 5% здоровья.";

                case "mastery_slayer_damage": return "+3% урона боссам и особым монстрам.";
                case "mastery_slayer_attack": return "+2% скорости атаки.";
                case "mastery_slayer_cooldown": return "ещё -1% восстановления навыков.";
                case "mastery_slayer_crit": return "+1% критического шанса.";

                case "mastery_path_move": return "+2% скорости движения.";
                case "mastery_path_sprint": return "+1 броня во время спринта.";
                case "mastery_path_jump": return "+1 дополнительный прыжок.";
                case "mastery_path_control": return "ещё -1% восстановления навыков.";

                case "mastery_seeker_gold": return "ещё +10% золота с особых монстров.";
                case "mastery_seeker_specialxp": return "ещё +10% мета-XP с особых монстров.";
                case "mastery_seeker_stagegold": return "+20 золота на каждой следующей стадии.";
                case "mastery_seeker_startgold": return "+30 золота в начале забега.";

                case "mastery_arcanist_damage": return "+3% урона боссам и особым монстрам.";
                case "mastery_arcanist_attack": return "+2% скорости атаки.";
                case "mastery_arcanist_cooldown": return "ещё -1.5% восстановления навыков.";
                case "mastery_arcanist_move": return "+2% скорости движения.";

                case "mastery_tech_attack": return "+2% скорости атаки владельца.";
                case "mastery_tech_move": return "+2% скорости движения владельца.";
                case "mastery_tech_minion_damage": return "+8% урона миньонов.";
                case "mastery_tech_minion_health": return "+8% здоровья миньонов.";

                case "mastery_commander_damage": return "+2% урона боссам и особым монстрам.";
                case "mastery_commander_move": return "+1.5% скорости движения и +1 броня.";
                case "mastery_commander_armor": return "+2 брони.";
                case "mastery_commander_minion": return "+5% урона и здоровья миньонов.";
                default: return "открывается финальный бонус дисциплины.";
            }
        }

        public static float EffectiveRanks(int rank)
        {
            rank = Math.Max(0, rank);
            var total = 0f;
            var left = rank;
            var take = Math.Min(left, 25); total += take; left -= take;
            if (left <= 0) return total;
            take = Math.Min(left, 25); total += take * 0.75f; left -= take;
            if (left <= 0) return total;
            take = Math.Min(left, 50); total += take * 0.50f; left -= take;
            if (left <= 0) return total;
            take = Math.Min(left, 100); total += take * 0.30f; left -= take;
            if (left > 0) total += left * 0.20f;
            return total;
        }

        public static float Value(ProfileProgress profile, string id)
        {
            var def = Find(id);
            if (def == null) return 0f;
            var rank = GetRank(profile, id);
            return EffectiveRanks(rank) * def.PerRank * DisciplineMultiplier(rank);
        }

        public static string CurrentValueText(ProfileProgress profile, MasteryDef def)
        {
            if (def == null) return string.Empty;
            var value = Value(profile, def.Id);
            if (def.Id.Contains("armor")) return $"Сейчас: +{value:0.##} брони";
            if (def.Id.Contains("stagegold") || def.Id.Contains("startgold")) return $"Сейчас: +{value:0.##} золота";
            return $"Сейчас: {(def.Id.Contains("cooldown") ? "-" : "+")}{value * 100f:0.##}%";
        }

        public static int Milestone(int rank) => Math.Max(0, rank) / 10;
    }
}
