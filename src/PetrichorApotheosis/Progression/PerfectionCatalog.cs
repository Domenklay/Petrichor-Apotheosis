using System.Collections.Generic;
using System.Linq;
using PetrichorLegacy.Models;

namespace PetrichorLegacy.Progression
{
    public sealed class PerfectionDef
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Description;
        public readonly int UnlockLevel;
        public readonly int MaxRank;
        public readonly string PrerequisiteId;
        public readonly int Column;
        public readonly int Row;
        public readonly bool Major;
        public readonly string IconItem;

        public PerfectionDef(string id, string name, string description, int unlockLevel, string prerequisiteId,
            int column, int row, string iconItem, bool major = false, int maxRank = 1)
        {
            Id = id;
            Name = name;
            Description = description;
            UnlockLevel = unlockLevel;
            MaxRank = maxRank;
            PrerequisiteId = prerequisiteId;
            Column = column;
            Row = row;
            Major = major;
            IconItem = iconItem;
        }
    }

    public static class PerfectionCatalog
    {
        public static readonly IReadOnlyList<PerfectionDef> All = new List<PerfectionDef>
        {
            // ТРИУМФ
            new PerfectionDef("perfection_giant_slayer", "Убийца великанов", "+3% урона боссам.", 25, null, 0, 0, "BossDamageBonus"),
            new PerfectionDef("perfection_prestige_predator", "Знак хищника", "+2% урона особым монстрам.", 50, "perfection_giant_slayer", 0, 1, "Crowbar"),
            new PerfectionDef("perfection_pursuit", "Неотступность", "После убийства особого монстра на 8 сек. +3% скорости движения и +2% скорости атаки.", 75, "perfection_prestige_predator", 0, 2, "WarCryOnMultiKill"),
            new PerfectionDef("perfection_prestige_veteran", "Ветеран охоты", "+1% скорости атаки и движения.", 100, "perfection_pursuit", 0, 3, "HeadHunter", true),
            new PerfectionDef("perfection_execution", "Право на казнь", "+5% урона целям ниже 25% здоровья.", 150, "perfection_prestige_veteran", 0, 4, "Dagger"),
            new PerfectionDef("perfection_relentless", "Без права на отступление", "Ещё +2% урона боссам и особым монстрам.", 200, "perfection_execution", 0, 5, "ArmorReductionOnHit", true),
            new PerfectionDef("perfection_apex", "Вершина охоты", "+2% всего урона и +1% критического шанса.", 300, "perfection_relentless", 0, 6, "CritGlasses", true),

            // СТОЙКОСТЬ
            new PerfectionDef("perfection_reserve", "Запас сил", "При переходе на новую стадию восстановить 5% здоровья.", 25, null, 1, 0, "Medkit"),
            new PerfectionDef("perfection_instinct", "Инстинкт выжившего", "+3 брони, когда здоровье ниже 30%.", 50, "perfection_reserve", 1, 1, "ArmorPlate"),
            new PerfectionDef("perfection_bulwark", "Полевая защита", "В начале каждой стадии получить барьер в 6% максимального здоровья.", 75, "perfection_instinct", 1, 2, "BarrierOnKill"),
            new PerfectionDef("perfection_prestige_hardened", "Закалённый", "+1% максимального здоровья.", 100, "perfection_bulwark", 1, 3, "Infusion", true),
            new PerfectionDef("perfection_recovery", "Живой резерв", "+5% естественной регенерации.", 150, "perfection_prestige_hardened", 1, 4, "HealWhileSafe"),
            new PerfectionDef("perfection_ironwill", "Железная воля", "Ниже 35% здоровья ещё +6 брони.", 200, "perfection_recovery", 1, 5, "SprintArmor", true),
            new PerfectionDef("perfection_laststand", "Последний рубеж", "В начале стадии дополнительно восстановить 8% здоровья и получить 8% барьера.", 300, "perfection_ironwill", 1, 6, "IncreaseHealing", true),

            // ЭКСПЕДИЦИЯ
            new PerfectionDef("perfection_pack", "Трофейный мешок", "+1 вместимость стартового Арсенала.", 25, null, 2, 0, "EquipmentMagazine"),
            new PerfectionDef("perfection_hunter", "Легендарный охотник", "+10% мета-XP за легендарных и более редких монстров.", 50, "perfection_pack", 2, 1, "TreasureCache"),
            new PerfectionDef("perfection_capital", "Походный капитал", "+25 золота в начале первого этапа забега.", 75, "perfection_hunter", 2, 2, "GoldOnHurt"),
            new PerfectionDef("perfection_supply", "Линия снабжения", "+15 золота при переходе на каждую следующую стадию.", 100, "perfection_capital", 2, 3, "BonusGoldPackOnKill", true),
            new PerfectionDef("perfection_bounty", "Редкая добыча", "+5% мета-XP и золота за всех особых монстров.", 150, "perfection_supply", 2, 4, "TreasureCacheVoid"),
            new PerfectionDef("perfection_spoils", "Право на трофеи", "Ещё +10% золота с особых монстров.", 200, "perfection_bounty", 2, 5, "BarrageOnBoss", true),
            new PerfectionDef("perfection_prestige_treasurer", "Хранитель наследия", "+1 вместимость Арсенала.", 300, "perfection_spoils", 2, 6, "Clover", true)
        };

        private static readonly Dictionary<string, PerfectionDef> ById = All.ToDictionary(x => x.Id, x => x);

        public static PerfectionDef Find(string id) => id != null && ById.TryGetValue(id, out var def) ? def : null;

        public static int GetRank(ProfileProgress profile, string id)
        {
            if (profile?.perfections == null || string.IsNullOrEmpty(id)) return 0;
            return profile.perfections.FirstOrDefault(x => x.id == id)?.rank ?? 0;
        }

        public static bool PrerequisiteMet(ProfileProgress profile, PerfectionDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.PrerequisiteId)) return true;
            return GetRank(profile, def.PrerequisiteId) > 0;
        }
    }
}
