using System.Collections.Generic;
using System.Linq;
using PetrichorLegacy.Models;

namespace PetrichorLegacy.Progression
{
    public enum TalentBranch
    {
        Legacy,
        Guardian,
        GuardianBastion,
        GuardianUnbroken,
        GuardianVanguard,
        Slayer,
        SlayerExecutioner,
        SlayerHunter,
        SlayerBerserker,
        Pathfinder,
        PathfinderScout,
        PathfinderAcrobat,
        PathfinderSaboteur,
        Seeker,
        SeekerMerchant,
        SeekerProspector,
        SeekerRelicKeeper,
        Arcanist,
        ArcanistChronomancer,
        ArcanistHexer,
        ArcanistStormcaller,
        Technomancer,
        TechnomancerDroneMaster,
        TechnomancerInterceptor,
        TechnomancerConstructor,
        Commander,
        CommanderTactician,
        CommanderStandardBearer,
        CommanderWarlord
    }

    public sealed class TalentDef
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Description;
        public readonly TalentBranch Branch;
        public readonly int MaxRank;
        public readonly int PointCost;
        public readonly int UnlockLevel;
        public readonly string PrerequisiteId;
        public readonly int PrerequisiteRank;
        public readonly int TreeColumn;
        public readonly int TreeRow;

        public TalentDef(string id, string name, string description, TalentBranch branch, int maxRank,
            int pointCost = 1, int unlockLevel = 1, string prerequisiteId = null, int prerequisiteRank = 0,
            int treeColumn = 1, int treeRow = 0)
        {
            Id = id;
            Name = name;
            Description = description;
            Branch = branch;
            MaxRank = maxRank;
            PointCost = pointCost;
            UnlockLevel = unlockLevel;
            PrerequisiteId = prerequisiteId;
            PrerequisiteRank = prerequisiteRank;
            TreeColumn = treeColumn;
            TreeRow = treeRow;
        }
    }

    public static class TalentCatalog
    {
        public static readonly IReadOnlyList<TalentDef> All = new List<TalentDef>
        {
            // НАСЛЕДИЕ — десять покупок к моменту открытия первого класса.
            new TalentDef("legacy_pack", "Первый трофей", "+1 вместимость стартового Арсенала. Базовая вместимость до этого равна 0.", TalentBranch.Legacy, 1, 1, 1, null, 0, 1, 0),

            // НАТИСК
            new TalentDef("legacy_damage", "Заточенное оружие", "+1.5% урона.", TalentBranch.Legacy, 1, 1, 2, "legacy_pack", 1, 0, 1),
            new TalentDef("legacy_attack", "Быстрая рука", "+1.5% скорости атаки.", TalentBranch.Legacy, 1, 1, 3, "legacy_damage", 1, 0, 2),
            new TalentDef("legacy_hunter", "Охотничий инстинкт", "+3% урона боссам и особым монстрам.", TalentBranch.Legacy, 1, 1, 4, "legacy_attack", 1, 0, 3),
            new TalentDef("legacy_precision", "Точный расчёт", "+1% шанса критического удара.", TalentBranch.Legacy, 1, 1, 5, "legacy_hunter", 1, 0, 4),
            new TalentDef("legacy_finisher", "Добивание", "+6% урона целям ниже 25% здоровья.", TalentBranch.Legacy, 1, 1, 6, "legacy_precision", 1, 0, 5),
            new TalentDef("legacy_momentum", "Боевой темп", "+1% урона и +1% скорости движения.", TalentBranch.Legacy, 1, 1, 7, "legacy_finisher", 1, 0, 6),

            // СТОЙКОСТЬ
            new TalentDef("legacy_hp", "Закалка", "+2% максимального здоровья.", TalentBranch.Legacy, 1, 1, 2, "legacy_pack", 1, 1, 1),
            new TalentDef("legacy_regen", "Полевой рацион", "+4% естественной регенерации.", TalentBranch.Legacy, 1, 1, 3, "legacy_hp", 1, 1, 2),
            new TalentDef("legacy_armor", "Крепкая кожа", "+2 брони.", TalentBranch.Legacy, 1, 1, 4, "legacy_regen", 1, 1, 3),
            new TalentDef("legacy_recovery", "Запас сил", "+2% максимального здоровья.", TalentBranch.Legacy, 1, 1, 5, "legacy_armor", 1, 1, 4),
            new TalentDef("legacy_resolve", "Упрямство", "Ниже 35% здоровья получаешь +3 брони.", TalentBranch.Legacy, 1, 1, 6, "legacy_recovery", 1, 1, 5),
            new TalentDef("legacy_secondwind", "Резервный щит", "+3% максимального здоровья в виде щита.", TalentBranch.Legacy, 1, 1, 7, "legacy_resolve", 1, 1, 6),

            // МАНЁВР
            new TalentDef("legacy_move", "Лёгкая поступь", "+2% скорости движения.", TalentBranch.Legacy, 1, 1, 2, "legacy_pack", 1, 2, 1),
            new TalentDef("legacy_sprint", "Марш-бросок", "+3% скорости спринта.", TalentBranch.Legacy, 1, 1, 3, "legacy_move", 1, 2, 2),
            new TalentDef("legacy_jump", "Лёгкое снаряжение", "+8% силы прыжка.", TalentBranch.Legacy, 1, 1, 4, "legacy_sprint", 1, 2, 3),
            new TalentDef("legacy_evasion", "Смена позиции", "+1.5% скорости движения.", TalentBranch.Legacy, 1, 1, 5, "legacy_jump", 1, 2, 4),
            new TalentDef("legacy_stride", "Длинный шаг", "Ещё +2% скорости спринта.", TalentBranch.Legacy, 1, 1, 6, "legacy_evasion", 1, 2, 5),
            new TalentDef("legacy_aircontrol", "Чувство дистанции", "+5% силы прыжка.", TalentBranch.Legacy, 1, 1, 7, "legacy_stride", 1, 2, 6),

            // Общая концовка: уровни 8-10.
            new TalentDef("legacy_pack2", "Экспедиционный ранец", "+1 вместимость стартового Арсенала.", TalentBranch.Legacy, 1, 1, 8, "legacy_pack", 1, 1, 7),
            new TalentDef("legacy_startgold", "Заначка", "+15 золота в начале каждого забега.", TalentBranch.Legacy, 1, 1, 9, "legacy_pack2", 1, 1, 8),
            new TalentDef("legacy_fieldcraft", "Полевой опыт", "+5% мета-опыта за убийство особых монстров.", TalentBranch.Legacy, 1, 1, 10, "legacy_startgold", 1, 1, 9),

            // СТРАЖ — лучшая защита, но почти без прироста урона.
            new TalentDef("guardian_vitality", "Крепкое тело", "+1.5% максимального здоровья за ранг.", TalentBranch.Guardian, 3, unlockLevel: 10, treeColumn: 1, treeRow: 0),
            new TalentDef("guardian_armor", "Закалённая кожа", "+2 брони за ранг.", TalentBranch.Guardian, 3, unlockLevel: 10, prerequisiteId: "guardian_vitality", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("guardian_regen", "Восстановление", "+4% регенерации здоровья за ранг.", TalentBranch.Guardian, 2, unlockLevel: 10, prerequisiteId: "guardian_vitality", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("guardian_stability", "Стабилизация", "+1.5% базового здоровья в виде щита за ранг.", TalentBranch.Guardian, 2, unlockLevel: 12, prerequisiteId: "guardian_armor", prerequisiteRank: 1, treeColumn: 0, treeRow: 2),
            new TalentDef("guardian_stageheal", "Передышка", "На новой стадии восстановить 4% здоровья за ранг.", TalentBranch.Guardian, 2, unlockLevel: 12, prerequisiteId: "guardian_regen", prerequisiteRank: 1, treeColumn: 2, treeRow: 2),
            new TalentDef("guardian_endurance", "Стойкость", "+1% здоровья и +2 брони.", TalentBranch.Guardian, 1, 2, 15, "guardian_stageheal", 2, 1, 3),
            new TalentDef("guardian_void_capstone", "Пустотный бастион", "Даёт Safer Spaces: полностью блокирует следующий полученный удар, затем эффект перезаряжается.", TalentBranch.Guardian, 1, 3, 18, "guardian_endurance", 1, 1, 4),

            // СТРАЖ / БАСТИОН — щиты и анти-снаряды.
            new TalentDef("bastion_shield", "Энергетический слой", "+2% базового здоровья в виде щита за ранг.", TalentBranch.GuardianBastion, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("bastion_armor", "Укрепление", "+2 брони за ранг.", TalentBranch.GuardianBastion, 2, unlockLevel: 20, prerequisiteId: "bastion_shield", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("bastion_health", "Плотный контур", "+1.5% здоровья за ранг.", TalentBranch.GuardianBastion, 2, unlockLevel: 22, prerequisiteId: "bastion_shield", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("bastion_stagebarrier", "Аварийный щит", "На новой стадии получить барьер 4% HP за ранг.", TalentBranch.GuardianBastion, 2, unlockLevel: 25, prerequisiteId: "bastion_armor", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("bastion_lastwall", "Последний рубеж", "+5% щита и +3 брони.", TalentBranch.GuardianBastion, 1, 2, 35, "bastion_stagebarrier", 2, 1, 3),
            new TalentDef("bastion_legendary", "Легенда: Эгида", "Даёт Aegis: лечение сверх полного здоровья превращает часть перелечения во временный барьер.", TalentBranch.GuardianBastion, 1, 3, 45, "bastion_lastwall", 1, 1, 4),

            // СТРАЖ / НЕСОКРУШИМЫЙ — лечение и восстановление.
            new TalentDef("unbroken_regen", "Второе дыхание", "+5% регенерации за ранг.", TalentBranch.GuardianUnbroken, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("unbroken_health", "Живучий", "+1.5% здоровья за ранг.", TalentBranch.GuardianUnbroken, 2, unlockLevel: 20, prerequisiteId: "unbroken_regen", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("unbroken_stageheal", "Спокойствие", "На новой стадии восстановить ещё 3% здоровья за ранг.", TalentBranch.GuardianUnbroken, 2, unlockLevel: 22, prerequisiteId: "unbroken_regen", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("unbroken_bossheal", "Передышка после боя", "Убийство босса восстанавливает 5% здоровья за ранг.", TalentBranch.GuardianUnbroken, 2, unlockLevel: 25, prerequisiteId: "unbroken_health", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("unbroken_ironwill", "Железная воля", "+5 брони и +5% регенерации.", TalentBranch.GuardianUnbroken, 1, 2, 35, "unbroken_bossheal", 2, 1, 3),
            new TalentDef("unbroken_legendary", "Легенда: Омолаживающая стойка", "Даёт Rejuvenation Rack: всё получаемое лечение становится значительно сильнее.", TalentBranch.GuardianUnbroken, 1, 3, 45, "unbroken_ironwill", 1, 1, 4),

            // СТРАЖ / АВАНГАРД — защитник, который давит вперёд.
            new TalentDef("vanguard_damage", "Ответный натиск", "+1% урона за ранг.", TalentBranch.GuardianVanguard, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("vanguard_move", "Линия фронта", "+1.5% скорости движения за ранг.", TalentBranch.GuardianVanguard, 2, unlockLevel: 20, prerequisiteId: "vanguard_damage", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("vanguard_armor", "Плечом вперёд", "+2 брони за ранг.", TalentBranch.GuardianVanguard, 2, unlockLevel: 20, prerequisiteId: "vanguard_damage", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("vanguard_attack", "Боевой темп", "+1.5% скорости атаки за ранг.", TalentBranch.GuardianVanguard, 2, unlockLevel: 25, prerequisiteId: "vanguard_move", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("vanguard_breaker", "Продавить строй", "+2% урона и +2 брони.", TalentBranch.GuardianVanguard, 1, 2, 35, "vanguard_attack", 2, 1, 3),
            new TalentDef("vanguard_legendary", "Легенда: Разрушительное правосудие", "Даёт Shattering Justice: серия попаданий снижает броню цели и усиливает последующий урон по ней.", TalentBranch.GuardianVanguard, 1, 3, 45, "vanguard_breaker", 1, 1, 4),

            // ИСТРЕБИТЕЛЬ — лучший прямой урон, почти без защиты.
            new TalentDef("slayer_damage", "Натиск", "+1.5% урона за ранг.", TalentBranch.Slayer, 3, unlockLevel: 10, treeColumn: 1, treeRow: 0),
            new TalentDef("slayer_attack", "Темп", "+1% скорости атаки за ранг.", TalentBranch.Slayer, 3, unlockLevel: 10, prerequisiteId: "slayer_damage", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("slayer_crit", "Точность", "+1% критического шанса за ранг.", TalentBranch.Slayer, 2, unlockLevel: 10, prerequisiteId: "slayer_damage", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("slayer_cooldown", "Техника", "-1% времени восстановления навыков за ранг.", TalentBranch.Slayer, 2, unlockLevel: 12, prerequisiteId: "slayer_attack", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("slayer_move", "Адреналин", "+1.5% скорости движения за ранг.", TalentBranch.Slayer, 2, unlockLevel: 12, prerequisiteId: "slayer_crit", prerequisiteRank: 1, treeColumn: 2, treeRow: 2),
            new TalentDef("slayer_focus", "Боевая концентрация", "+2% урона и +1% скорости атаки.", TalentBranch.Slayer, 1, 2, 15, "slayer_cooldown", 2, 1, 3),
            new TalentDef("slayer_void_capstone", "Игла Бездны", "Даёт Needletick: попадания могут наложить Collapse и вызвать отложенный взрывной урон.", TalentBranch.Slayer, 1, 3, 18, "slayer_focus", 1, 1, 4),

            // ИСТРЕБИТЕЛЬ / ПАЛАЧ — криты и добивание.
            new TalentDef("executioner_power", "Приоритетная цель", "+1% урона за ранг.", TalentBranch.SlayerExecutioner, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("executioner_critdamage", "Точный удар", "+5% критического урона за ранг.", TalentBranch.SlayerExecutioner, 2, unlockLevel: 20, prerequisiteId: "executioner_power", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("executioner_cooldown", "Хладнокровие", "-1% времени восстановления за ранг.", TalentBranch.SlayerExecutioner, 2, unlockLevel: 22, prerequisiteId: "executioner_power", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("executioner_attack", "Не отпускать", "+1.5% скорости атаки за ранг.", TalentBranch.SlayerExecutioner, 2, unlockLevel: 25, prerequisiteId: "executioner_critdamage", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("executioner_verdict", "Приговор", "+2% урона и +10% критического урона.", TalentBranch.SlayerExecutioner, 1, 2, 35, "executioner_attack", 2, 1, 3),
            new TalentDef("executioner_legendary", "Легенда: Церемониальный кинжал", "Даёт Ceremonial Dagger: убийства выпускают самонаводящиеся кинжалы по ближайшим врагам.", TalentBranch.SlayerExecutioner, 1, 3, 45, "executioner_verdict", 1, 1, 4),

            // ИСТРЕБИТЕЛЬ / ОХОТНИК — особые монстры и элитная добыча.
            new TalentDef("hunter_specialxp", "Знаток добычи", "+5% XP за особых монстров за ранг.", TalentBranch.SlayerHunter, 2, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("hunter_specialgold", "Трофейщик", "+10% золота за особых монстров за ранг.", TalentBranch.SlayerHunter, 2, unlockLevel: 20, prerequisiteId: "hunter_specialxp", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("hunter_haste", "Охотничий азарт", "Убийство эпического+ врага даёт +4% скорости атаки на 8 сек. за ранг.", TalentBranch.SlayerHunter, 2, unlockLevel: 22, prerequisiteId: "hunter_specialxp", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("hunter_legend", "Большая добыча", "Легендарные+ враги дают ещё +20% золота.", TalentBranch.SlayerHunter, 1, unlockLevel: 25, prerequisiteId: "hunter_specialgold", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("hunter_greathunt", "Великая охота", "Первый мифический+ враг каждой стадии даёт +50% своей XP-награды.", TalentBranch.SlayerHunter, 1, 2, 35, "hunter_haste", 2, 1, 3),
            new TalentDef("hunter_legendary", "Легенда: Пробуждение стервятников", "Даёт Wake of Vultures: убийство элитного врага временно даёт его элитную силу.", TalentBranch.SlayerHunter, 1, 3, 45, "hunter_greathunt", 1, 1, 4),

            // ИСТРЕБИТЕЛЬ / БЕРСЕРК — непрерывный темп и ближняя агрессия.
            new TalentDef("berserker_attack", "Ярость", "+2% скорости атаки за ранг.", TalentBranch.SlayerBerserker, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("berserker_damage", "Размашистый удар", "+1.5% урона за ранг.", TalentBranch.SlayerBerserker, 2, unlockLevel: 20, prerequisiteId: "berserker_attack", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("berserker_move", "В погоню", "+1.5% скорости движения за ранг.", TalentBranch.SlayerBerserker, 2, unlockLevel: 20, prerequisiteId: "berserker_attack", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("berserker_cooldown", "Без остановки", "-1% времени восстановления навыков за ранг.", TalentBranch.SlayerBerserker, 2, unlockLevel: 25, prerequisiteId: "berserker_damage", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("berserker_frenzy", "Кровавая ярость", "+2% урона и +2% скорости атаки.", TalentBranch.SlayerBerserker, 1, 2, 35, "berserker_cooldown", 2, 1, 3),
            new TalentDef("berserker_legendary", "Легенда: Катушка Теслы", "Даёт Unstable Tesla Coil: периодически поражает молниями ближайших врагов вокруг тебя.", TalentBranch.SlayerBerserker, 1, 3, 45, "berserker_frenzy", 1, 1, 4),

            // СЛЕДОПЫТ — лучший темп, мобильность и позиционирование.
            new TalentDef("path_move", "Походный шаг", "+1.5% скорости движения за ранг.", TalentBranch.Pathfinder, 3, unlockLevel: 10, treeColumn: 1, treeRow: 0),
            new TalentDef("path_sprint", "Бегун", "+2% скорости спринта за ранг.", TalentBranch.Pathfinder, 2, unlockLevel: 10, prerequisiteId: "path_move", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("path_jump", "Пружина", "+5% силы прыжка за ранг.", TalentBranch.Pathfinder, 2, unlockLevel: 10, prerequisiteId: "path_move", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("path_armor", "Уверенное движение", "+1 брони за ранг.", TalentBranch.Pathfinder, 2, unlockLevel: 12, prerequisiteId: "path_sprint", prerequisiteRank: 1, treeColumn: 0, treeRow: 2),
            new TalentDef("path_control", "Манёвренность", "+1% скорости движения за ранг.", TalentBranch.Pathfinder, 2, unlockLevel: 12, prerequisiteId: "path_jump", prerequisiteRank: 1, treeColumn: 2, treeRow: 2),
            new TalentDef("path_flow", "Поток странника", "+2% скорости движения.", TalentBranch.Pathfinder, 1, 2, 15, "path_control", 2, 1, 3),
            new TalentDef("path_void_capstone", "Полиимпульс", "Даёт Polylute: попадания могут вызвать серию повторных электрических ударов по цели.", TalentBranch.Pathfinder, 1, 3, 18, "path_flow", 1, 1, 4),

            // СЛЕДОПЫТ / РАЗВЕДЧИК — скорость прохождения и частые навыки.
            new TalentDef("scout_move", "Маршрут", "+1.5% скорости движения за ранг.", TalentBranch.PathfinderScout, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("scout_sprint", "Быстрый старт", "+2% скорости спринта за ранг.", TalentBranch.PathfinderScout, 2, unlockLevel: 20, prerequisiteId: "scout_move", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("scout_stagegold", "Полевое снабжение", "+10 золота на новой стадии за ранг.", TalentBranch.PathfinderScout, 2, unlockLevel: 22, prerequisiteId: "scout_move", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("scout_jump", "Срез пути", "+5% силы прыжка за ранг.", TalentBranch.PathfinderScout, 2, unlockLevel: 25, prerequisiteId: "scout_sprint", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("scout_expedition", "Экспедиция", "+4% скорости движения и +20 золота на новой стадии.", TalentBranch.PathfinderScout, 1, 2, 35, "scout_stagegold", 2, 1, 3),
            new TalentDef("scout_legendary", "Легенда: Чужеродная голова", "Даёт Alien Head: сокращает время восстановления всех навыков.", TalentBranch.PathfinderScout, 1, 3, 45, "scout_expedition", 1, 1, 4),

            // СЛЕДОПЫТ / АКРОБАТ — воздух, рывки и вертикальное перемещение.
            new TalentDef("acro_jump", "Лёгкость", "+6% силы прыжка за ранг.", TalentBranch.PathfinderAcrobat, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("acro_move", "Инерция", "+1.5% скорости движения за ранг.", TalentBranch.PathfinderAcrobat, 2, unlockLevel: 20, prerequisiteId: "acro_jump", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("acro_armor", "Мягкое приземление", "+1 брони за ранг.", TalentBranch.PathfinderAcrobat, 2, unlockLevel: 22, prerequisiteId: "acro_jump", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("acro_extra_jump", "Дополнительный импульс", "+1 дополнительный прыжок.", TalentBranch.PathfinderAcrobat, 1, 2, 25, "acro_move", 2, 0, 2),
            new TalentDef("acro_elusive", "Неуловимый", "+3% скорости движения и +5% силы прыжка.", TalentBranch.PathfinderAcrobat, 1, 2, 35, "acro_extra_jump", 1, 1, 3),
            new TalentDef("acro_legendary", "Легенда: Жёсткий форсаж", "Даёт Hardlight Afterburner: добавляет заряды Utility-навыка и ускоряет его восстановление.", TalentBranch.PathfinderAcrobat, 1, 3, 45, "acro_elusive", 1, 1, 4),

            // СЛЕДОПЫТ / ДИВЕРСАНТ — контроль толпы и частые навыки.
            new TalentDef("saboteur_cooldown", "Подготовленный заряд", "-1% времени восстановления навыков за ранг.", TalentBranch.PathfinderSaboteur, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("saboteur_damage", "Точный подрыв", "+1% урона за ранг.", TalentBranch.PathfinderSaboteur, 2, unlockLevel: 20, prerequisiteId: "saboteur_cooldown", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("saboteur_move", "Отход после подрыва", "+1.5% скорости движения за ранг.", TalentBranch.PathfinderSaboteur, 2, unlockLevel: 20, prerequisiteId: "saboteur_cooldown", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("saboteur_attack", "Быстрый запал", "+1.5% скорости атаки за ранг.", TalentBranch.PathfinderSaboteur, 2, unlockLevel: 25, prerequisiteId: "saboteur_damage", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("saboteur_chain", "Цепная реакция", "+2% урона и -2% времени восстановления навыков.", TalentBranch.PathfinderSaboteur, 1, 2, 35, "saboteur_attack", 2, 1, 3),
            new TalentDef("saboteur_legendary", "Легенда: Разумный крюк", "Даёт Sentient Meat Hook: попадания могут зацепить, притянуть и повредить соседних врагов.", TalentBranch.PathfinderSaboteur, 1, 3, 45, "saboteur_chain", 1, 1, 4),

            // ИСКАТЕЛЬ — лучший долгосрочный ресурс и utility.
            new TalentDef("seeker_startgold", "Припас", "+10 стартового золота за ранг.", TalentBranch.Seeker, 3, unlockLevel: 10, treeColumn: 1, treeRow: 0),
            new TalentDef("seeker_stagegold", "Запасы", "+10 золота на новой стадии за ранг.", TalentBranch.Seeker, 3, unlockLevel: 10, prerequisiteId: "seeker_startgold", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("seeker_specialgold", "Сборщик", "+10% золота за особых монстров за ранг.", TalentBranch.Seeker, 2, unlockLevel: 10, prerequisiteId: "seeker_startgold", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("seeker_move", "Деловая хватка", "+1% скорости движения за ранг.", TalentBranch.Seeker, 2, unlockLevel: 12, prerequisiteId: "seeker_stagegold", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("seeker_armor", "Осторожный сборщик", "+1 брони за ранг.", TalentBranch.Seeker, 2, unlockLevel: 12, prerequisiteId: "seeker_specialgold", prerequisiteRank: 1, treeColumn: 2, treeRow: 2),
            new TalentDef("seeker_cache", "Тайник экспедиции", "+25 стартового золота.", TalentBranch.Seeker, 1, 2, 15, "seeker_move", 2, 1, 3),
            new TalentDef("seeker_void_capstone", "Пустотный ключ", "Даёт Encrusted Key: на каждой стадии появляется скрытый Void-тайник с наградой.", TalentBranch.Seeker, 1, 3, 18, "seeker_cache", 1, 1, 4),

            // ИСКАТЕЛЬ / ТОРГОВЕЦ — экономика, дроны и покупки.
            new TalentDef("merchant_startgold", "Финансовая подушка", "+15 стартового золота за ранг.", TalentBranch.SeekerMerchant, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("merchant_stagegold", "Инвестиция", "+15 золота на новой стадии за ранг.", TalentBranch.SeekerMerchant, 3, unlockLevel: 20, prerequisiteId: "merchant_startgold", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("merchant_specialgold", "Выгодный трофей", "+10% золота за особых монстров за ранг.", TalentBranch.SeekerMerchant, 2, unlockLevel: 22, prerequisiteId: "merchant_startgold", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("merchant_reserve", "Резерв", "+25 золота после завершения каждой стадии.", TalentBranch.SeekerMerchant, 1, unlockLevel: 25, prerequisiteId: "merchant_stagegold", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("merchant_capital", "Капитал", "На новой стадии получить ещё 10 × номер стадии золота.", TalentBranch.SeekerMerchant, 1, 2, 35, "merchant_reserve", 1, 1, 3),
            new TalentDef("merchant_legendary", "Легенда: Военные облигации", "Даёт War Bonds: в боях с боссами вызывает ракетный обстрел; чем больше золота заработано заранее, тем больше ракет.", TalentBranch.SeekerMerchant, 1, 3, 45, "merchant_capital", 1, 1, 4),

            // ИСКАТЕЛЬ / СТАРАТЕЛЬ — трофеи и редкие находки.
            new TalentDef("prospector_specialgold", "Искатель сокровищ", "+10% золота за особых монстров за ранг.", TalentBranch.SeekerProspector, 2, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("prospector_white3", "Удачная партия", "После каждых 3 завершённых стадий получить случайный белый предмет.", TalentBranch.SeekerProspector, 1, unlockLevel: 20, prerequisiteId: "prospector_specialgold", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("prospector_legendwhite", "Ценный трофей", "Первый легендарный+ враг за забег даёт случайный белый предмет.", TalentBranch.SeekerProspector, 1, unlockLevel: 22, prerequisiteId: "prospector_specialgold", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("prospector_mythgreen", "Большая находка", "Первый мифический+ враг за забег имеет 25% шанс дать случайный зелёный предмет.", TalentBranch.SeekerProspector, 1, 2, 25, "prospector_legendwhite", 1, 2, 2),
            new TalentDef("prospector_goldmine", "Золотая жила", "Каждая пятая завершённая стадия даёт случайный зелёный предмет.", TalentBranch.SeekerProspector, 1, 2, 35, "prospector_white3", 1, 1, 3),
            new TalentDef("prospector_legendary", "Легенда: 57-листный клевер", "Даёт 57 Leaf Clover: неблагоприятные случайные результаты перебрасываются в твою пользу.", TalentBranch.SeekerProspector, 1, 3, 45, "prospector_goldmine", 1, 1, 4),

            // ИСКАТЕЛЬ / РЕЛИКВАРИЙ — снаряжение и универсальная полезность.
            new TalentDef("relic_cooldown", "Знание реликвий", "-1% времени восстановления навыков за ранг.", TalentBranch.SeekerRelicKeeper, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("relic_move", "Быстрый доступ", "+1.5% скорости движения за ранг.", TalentBranch.SeekerRelicKeeper, 2, unlockLevel: 20, prerequisiteId: "relic_cooldown", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("relic_armor", "Защитный футляр", "+2 брони за ранг.", TalentBranch.SeekerRelicKeeper, 2, unlockLevel: 20, prerequisiteId: "relic_cooldown", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("relic_attack", "Настроенный механизм", "+1.5% скорости атаки за ранг.", TalentBranch.SeekerRelicKeeper, 2, unlockLevel: 25, prerequisiteId: "relic_move", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("relic_attunement", "Настройка реликвии", "+2% скорости движения и -2% времени восстановления навыков.", TalentBranch.SeekerRelicKeeper, 1, 2, 35, "relic_attack", 2, 1, 3),
            new TalentDef("relic_legendary", "Легенда: Связанный душой катализатор", "Даёт Soulbound Catalyst: убийства сокращают перезарядку экипировки.", TalentBranch.SeekerRelicKeeper, 1, 3, 45, "relic_attunement", 1, 1, 4)
            ,

            // АРКАНИСТ — навыки, ротация и ослабление целей; почти без защиты.
            new TalentDef("arcanist_cooldown", "Поток эфира", "-1% времени восстановления навыков за ранг.", TalentBranch.Arcanist, 3, unlockLevel: 10, treeColumn: 1, treeRow: 0),
            new TalentDef("arcanist_damage", "Арканный импульс", "+1% урона за ранг.", TalentBranch.Arcanist, 3, unlockLevel: 10, prerequisiteId: "arcanist_cooldown", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("arcanist_attack", "Быстрый каст", "+1% скорости атаки за ранг.", TalentBranch.Arcanist, 3, unlockLevel: 10, prerequisiteId: "arcanist_cooldown", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("arcanist_move", "Скольжение", "+1.5% скорости движения за ранг.", TalentBranch.Arcanist, 2, unlockLevel: 12, prerequisiteId: "arcanist_damage", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("arcanist_crit", "Фокусировка", "+0.75% критического шанса за ранг.", TalentBranch.Arcanist, 2, unlockLevel: 12, prerequisiteId: "arcanist_attack", prerequisiteRank: 2, treeColumn: 2, treeRow: 2),
            new TalentDef("arcanist_convergence", "Схождение потоков", "+1% урона и -2% времени восстановления навыков.", TalentBranch.Arcanist, 1, 2, 15, "arcanist_move", 2, 1, 3),
            new TalentDef("arcanist_void_capstone", "Лизатная ячейка", "Даёт Lysate Cell: добавляет дополнительный заряд специального навыка.", TalentBranch.Arcanist, 1, 3, 18, "arcanist_convergence", 1, 1, 4),

            // АРКАНИСТ / ХРОНОМАНТ — самая гибкая ротация навыков, но без защитной страховки.
            new TalentDef("chrono_cooldown", "Сжатие времени", "-1.5% времени восстановления навыков за ранг.", TalentBranch.ArcanistChronomancer, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("chrono_attack", "Ускорение", "+1.5% скорости атаки за ранг.", TalentBranch.ArcanistChronomancer, 2, unlockLevel: 20, prerequisiteId: "chrono_cooldown", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("chrono_move", "Сдвиг фазы", "+1.5% скорости движения за ранг.", TalentBranch.ArcanistChronomancer, 2, unlockLevel: 20, prerequisiteId: "chrono_cooldown", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("chrono_damage", "Повторный импульс", "+1% урона за ранг.", TalentBranch.ArcanistChronomancer, 2, unlockLevel: 25, prerequisiteId: "chrono_attack", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("chrono_overclock", "Вне времени", "+2% скорости атаки и -2% времени восстановления навыков.", TalentBranch.ArcanistChronomancer, 1, 2, 35, "chrono_damage", 2, 1, 3),
            new TalentDef("chrono_legendary", "Легенда: Мозговые стебли", "Даёт Brainstalks: убийство элиты на короткое время почти убирает перезарядки навыков.", TalentBranch.ArcanistChronomancer, 1, 3, 45, "chrono_overclock", 1, 1, 4),

            // АРКАНИСТ / ПРОКЛИНАТЕЛЬ — сильнее против уже опасных и редких целей.
            new TalentDef("hex_damage", "Порча", "+1% урона за ранг.", TalentBranch.ArcanistHexer, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("hex_cooldown", "Зловещий ритм", "-1% времени восстановления навыков за ранг.", TalentBranch.ArcanistHexer, 2, unlockLevel: 20, prerequisiteId: "hex_damage", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("hex_crit", "Слабое место", "+0.75% критического шанса за ранг.", TalentBranch.ArcanistHexer, 2, unlockLevel: 20, prerequisiteId: "hex_damage", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("hex_hunt", "Печать охоты", "+2% урона боссам и особым монстрам за ранг.", TalentBranch.ArcanistHexer, 2, unlockLevel: 25, prerequisiteId: "hex_cooldown", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("hex_corrosion", "Разъедающая печать", "Ещё +3% урона боссам и особым монстрам.", TalentBranch.ArcanistHexer, 1, 2, 35, "hex_hunt", 2, 1, 3),
            new TalentDef("hex_legendary", "Легенда: Симбиотический скорпион", "Даёт Symbiotic Scorpion: каждое попадание всё сильнее снижает броню цели, пока она жива.", TalentBranch.ArcanistHexer, 1, 3, 45, "hex_corrosion", 1, 1, 4),

            // АРКАНИСТ / БУРЕВЕСТНИК — темп, атаки и зачистка пачек.
            new TalentDef("storm_attack", "Грозовой темп", "+1.5% скорости атаки за ранг.", TalentBranch.ArcanistStormcaller, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("storm_move", "Порыв ветра", "+1.5% скорости движения за ранг.", TalentBranch.ArcanistStormcaller, 2, unlockLevel: 20, prerequisiteId: "storm_attack", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("storm_damage", "Разряд", "+1% урона за ранг.", TalentBranch.ArcanistStormcaller, 2, unlockLevel: 20, prerequisiteId: "storm_attack", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("storm_cooldown", "Грозовой цикл", "-1% времени восстановления навыков за ранг.", TalentBranch.ArcanistStormcaller, 2, unlockLevel: 25, prerequisiteId: "storm_move", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("storm_charge", "Перегрузка", "+2% скорости атаки и +1% урона.", TalentBranch.ArcanistStormcaller, 1, 2, 35, "storm_cooldown", 2, 1, 3),
            new TalentDef("storm_legendary", "Легенда: Резонансный диск", "Даёт Resonance Disc: быстрые убийства заряжают диск, который сам пронзает врагов и мощно взрывается.", TalentBranch.ArcanistStormcaller, 1, 3, 45, "storm_charge", 1, 1, 4),

            // ТЕХНОМАНТ — стеклянный класс: скорость и внешняя огневая мощь вместо собственной защиты.
            new TalentDef("tech_attack", "Разогнанные сервоприводы", "+1.5% скорости атаки за ранг.", TalentBranch.Technomancer, 3, unlockLevel: 10, treeColumn: 1, treeRow: 0),
            new TalentDef("tech_move", "Лёгкое шасси", "+1.5% скорости движения за ранг.", TalentBranch.Technomancer, 3, unlockLevel: 10, prerequisiteId: "tech_attack", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("tech_cooldown", "Контур управления", "-1% времени восстановления навыков за ранг.", TalentBranch.Technomancer, 2, unlockLevel: 10, prerequisiteId: "tech_attack", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("tech_minion_damage", "Боевой протокол", "+5% урона подконтрольных существ за ранг.", TalentBranch.Technomancer, 2, unlockLevel: 12, prerequisiteId: "tech_move", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("tech_minion_health", "Усиленные корпуса", "+5% здоровья подконтрольных существ за ранг.", TalentBranch.Technomancer, 2, unlockLevel: 12, prerequisiteId: "tech_cooldown", prerequisiteRank: 2, treeColumn: 2, treeRow: 2),
            new TalentDef("tech_core", "Сетевая синхронизация", "+2% скорости атаки и движения владельца; +5% урона миньонов.", TalentBranch.Technomancer, 1, 2, 15, "tech_minion_damage", 2, 1, 3),
            new TalentDef("tech_void_capstone", "Плазменная креветка", "Даёт Plasma Shrimp: пока есть щит, попадания запускают самонаводящиеся ракеты.", TalentBranch.Technomancer, 1, 3, 18, "tech_core", 1, 1, 4),

            // ТЕХНОМАНТ / ДРОНОВОД — максимум силы переносится на дронов и миньонов.
            new TalentDef("drone_damage", "Боевые прошивки", "+6% урона подконтрольных существ за ранг.", TalentBranch.TechnomancerDroneMaster, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("drone_health", "Резервные детали", "+6% здоровья подконтрольных существ за ранг.", TalentBranch.TechnomancerDroneMaster, 2, unlockLevel: 20, prerequisiteId: "drone_damage", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("drone_attack", "Синхронный огонь", "+5% скорости атаки подконтрольных существ за ранг.", TalentBranch.TechnomancerDroneMaster, 2, unlockLevel: 20, prerequisiteId: "drone_damage", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("drone_move", "Протокол преследования", "+5% скорости движения подконтрольных существ за ранг.", TalentBranch.TechnomancerDroneMaster, 2, unlockLevel: 25, prerequisiteId: "drone_health", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("drone_network", "Рой", "Подконтрольные существа получают ещё +10% урона.", TalentBranch.TechnomancerDroneMaster, 1, 2, 35, "drone_move", 2, 1, 3),
            new TalentDef("drone_legendary", "Легенда: Запчасти дронов", "Даёт Spare Drone Parts: усиливает дронов, добавляет им ракеты и цепную пушку, а также даёт боевого дрона-компаньона.", TalentBranch.TechnomancerDroneMaster, 1, 3, 45, "drone_network", 1, 1, 4),

            // ТЕХНОМАНТ / ПЕРЕХВАТЧИК — сам хрупкий, но быстрее и защищён автоматической ПРО.
            new TalentDef("interceptor_attack", "Система наведения", "+1.5% скорости атаки за ранг.", TalentBranch.TechnomancerInterceptor, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("interceptor_move", "Уклоняющее шасси", "+1.5% скорости движения за ранг.", TalentBranch.TechnomancerInterceptor, 2, unlockLevel: 20, prerequisiteId: "interceptor_attack", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("interceptor_cooldown", "Быстрый цикл", "-1% времени восстановления навыков за ранг.", TalentBranch.TechnomancerInterceptor, 2, unlockLevel: 20, prerequisiteId: "interceptor_attack", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("interceptor_damage", "Ответный импульс", "+1% урона за ранг.", TalentBranch.TechnomancerInterceptor, 2, unlockLevel: 25, prerequisiteId: "interceptor_move", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("interceptor_matrix", "Матрица перехвата", "+2% скорости атаки и движения.", TalentBranch.TechnomancerInterceptor, 1, 2, 35, "interceptor_damage", 2, 1, 3),
            new TalentDef("interceptor_legendary", "Легенда: Защитные микроботы", "Даёт Defensive Microbots: автоматически сбивает ближайшие вражеские снаряды; перезарядка ускоряется от скорости атаки.", TalentBranch.TechnomancerInterceptor, 1, 3, 45, "interceptor_matrix", 1, 1, 4),

            // ТЕХНОМАНТ / КОНСТРУКТОР — один сильный компаньон и усиление механических союзников.
            new TalentDef("constructor_minion_damage", "Силовое ядро", "+5% урона подконтрольных существ за ранг.", TalentBranch.TechnomancerConstructor, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("constructor_minion_health", "Утолщённый корпус", "+7% здоровья подконтрольных существ за ранг.", TalentBranch.TechnomancerConstructor, 2, unlockLevel: 20, prerequisiteId: "constructor_minion_damage", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("constructor_move", "Мобильная платформа", "+1.5% скорости движения владельца за ранг.", TalentBranch.TechnomancerConstructor, 2, unlockLevel: 20, prerequisiteId: "constructor_minion_damage", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("constructor_cooldown", "Сборочный цикл", "-1% времени восстановления навыков за ранг.", TalentBranch.TechnomancerConstructor, 2, unlockLevel: 25, prerequisiteId: "constructor_minion_health", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("constructor_core", "Прототип", "Подконтрольные существа получают +10% скорости атаки и движения.", TalentBranch.TechnomancerConstructor, 1, 2, 35, "constructor_cooldown", 2, 1, 3),
            new TalentDef("constructor_legendary", "Легенда: Осиротевшее ядро", "Даёт Orphaned Core: призывает дружелюбного Solus, который таранит врагов, наследует предметы скорости и сильнее бьёт на ходу.", TalentBranch.TechnomancerConstructor, 1, 3, 45, "constructor_core", 1, 1, 4),

            // КОМАНДИР — универсальная тактика, метки целей и союзники; не превосходит специализированные классы в их нишах.
            new TalentDef("commander_damage", "Боевой приказ", "+1% урона за ранг.", TalentBranch.Commander, 3, unlockLevel: 10, treeColumn: 1, treeRow: 0),
            new TalentDef("commander_move", "Манёвр", "+1% скорости движения за ранг.", TalentBranch.Commander, 2, unlockLevel: 10, prerequisiteId: "commander_damage", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("commander_attack", "Темп отряда", "+1% скорости атаки за ранг.", TalentBranch.Commander, 2, unlockLevel: 10, prerequisiteId: "commander_damage", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("commander_armor", "Дисциплина", "+1 брони за ранг.", TalentBranch.Commander, 2, unlockLevel: 12, prerequisiteId: "commander_move", prerequisiteRank: 1, treeColumn: 0, treeRow: 2),
            new TalentDef("commander_priority", "Приоритетная цель", "+2% урона боссам и особым монстрам за ранг.", TalentBranch.Commander, 2, unlockLevel: 12, prerequisiteId: "commander_attack", prerequisiteRank: 1, treeColumn: 2, treeRow: 2),
            new TalentDef("commander_order", "Слаженность", "+1% урона, скорости атаки и движения.", TalentBranch.Commander, 1, 2, 15, "commander_priority", 2, 1, 3),
            new TalentDef("commander_void_capstone", "Сингулярная полоса", "Даёт Singularity Band: сильные попадания создают чёрную дыру, стягивающую врагов и затем взрывающуюся.", TalentBranch.Commander, 1, 3, 18, "commander_order", 1, 1, 4),

            // КОМАНДИР / ТАКТИК — сильнее против приоритетных целей, но слабее Палача по чистому DPS.
            new TalentDef("tactician_boss", "Разбор цели", "+2% урона боссам за ранг.", TalentBranch.CommanderTactician, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("tactician_rare", "Опасная цель", "+2% урона особым монстрам за ранг.", TalentBranch.CommanderTactician, 2, unlockLevel: 20, prerequisiteId: "tactician_boss", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("tactician_crit", "Окно атаки", "+0.75% критического шанса за ранг.", TalentBranch.CommanderTactician, 2, unlockLevel: 20, prerequisiteId: "tactician_boss", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("tactician_move", "Перестроение", "+1% скорости движения за ранг.", TalentBranch.CommanderTactician, 2, unlockLevel: 25, prerequisiteId: "tactician_rare", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("tactician_mark", "Отмеченная цель", "Ещё +3% урона боссам и особым монстрам.", TalentBranch.CommanderTactician, 1, 2, 35, "tactician_move", 2, 1, 3),
            new TalentDef("tactician_legendary", "Легенда: Нектар роста", "Даёт Growth Nectar: каждый активный уникальный бафф повышает все основные характеристики, вплоть до четырёх баффов.", TalentBranch.CommanderTactician, 1, 3, 45, "tactician_mark", 1, 1, 4),

            // КОМАНДИР / ЗНАМЕНОСЕЦ — умеренная универсальная аура без танковой специализации.
            new TalentDef("standard_health", "Знамя стойкости", "+1% максимального здоровья за ранг.", TalentBranch.CommanderStandardBearer, 2, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("standard_attack", "Знамя натиска", "+1.5% скорости атаки за ранг.", TalentBranch.CommanderStandardBearer, 2, unlockLevel: 20, prerequisiteId: "standard_health", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("standard_move", "Знамя манёвра", "+1.5% скорости движения за ранг.", TalentBranch.CommanderStandardBearer, 2, unlockLevel: 20, prerequisiteId: "standard_health", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("standard_armor", "Удерживать строй", "+1 брони за ранг.", TalentBranch.CommanderStandardBearer, 2, unlockLevel: 25, prerequisiteId: "standard_attack", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("standard_banner", "Боевой штандарт", "+1% урона, скорости атаки и движения.", TalentBranch.CommanderStandardBearer, 1, 2, 35, "standard_armor", 2, 1, 3),
            new TalentDef("standard_legendary", "Легенда: Межзвёздное растение", "Даёт Interstellar Desk Plant: убийства выращивают лечебные растения, создающие область восстановления здоровья для союзников.", TalentBranch.CommanderStandardBearer, 1, 3, 45, "standard_banner", 1, 1, 4),

            // КОМАНДИР / ПОЛКОВОДЕЦ — армия призванных союзников; слабее Техноманта в чистом усилении дронов.
            new TalentDef("warlord_minion_damage", "Боевой строй", "+4% урона подконтрольных существ за ранг.", TalentBranch.CommanderWarlord, 3, unlockLevel: 20, treeColumn: 1, treeRow: 0),
            new TalentDef("warlord_minion_health", "Резерв", "+4% здоровья подконтрольных существ за ранг.", TalentBranch.CommanderWarlord, 2, unlockLevel: 20, prerequisiteId: "warlord_minion_damage", prerequisiteRank: 1, treeColumn: 0, treeRow: 1),
            new TalentDef("warlord_damage", "Личный пример", "+1% урона владельца за ранг.", TalentBranch.CommanderWarlord, 2, unlockLevel: 20, prerequisiteId: "warlord_minion_damage", prerequisiteRank: 1, treeColumn: 2, treeRow: 1),
            new TalentDef("warlord_move", "Марш", "+1.5% скорости движения владельца за ранг.", TalentBranch.CommanderWarlord, 2, unlockLevel: 25, prerequisiteId: "warlord_minion_health", prerequisiteRank: 2, treeColumn: 0, treeRow: 2),
            new TalentDef("warlord_army", "Армия", "Подконтрольные существа получают +8% скорости атаки.", TalentBranch.CommanderWarlord, 1, 2, 35, "warlord_move", 2, 1, 3),
            new TalentDef("warlord_legendary", "Легенда: Счастливая маска", "Даёт Happiest Mask: убийства иногда создают временного усиленного призрака убитого врага на твоей стороне.", TalentBranch.CommanderWarlord, 1, 3, 45, "warlord_army", 1, 1, 4)
        };

        private static readonly Dictionary<string, TalentDef> ById = All.ToDictionary(t => t.Id, t => t);

        public static TalentDef Find(string id) => id != null && ById.TryGetValue(id, out var def) ? def : null;

        public static int GetRank(ProfileProgress profile, string id)
        {
            if (profile?.talents == null || string.IsNullOrEmpty(id)) return 0;
            return profile.talents.FirstOrDefault(t => t.id == id)?.rank ?? 0;
        }

        public static int GetSpentPoints(ProfileProgress profile)
        {
            if (profile?.talents == null) return 0;
            var total = 0;
            foreach (var record in profile.talents)
            {
                var def = Find(record.id);
                if (def != null) total += System.Math.Min(record.rank, def.MaxRank) * def.PointCost;
            }
            return total;
        }

        public static bool PrerequisiteMet(ProfileProgress profile, TalentDef def)
        {
            if (def == null || string.IsNullOrEmpty(def.PrerequisiteId)) return true;
            return GetRank(profile, def.PrerequisiteId) >= def.PrerequisiteRank;
        }

        public static string ClassForBranch(TalentBranch branch)
        {
            switch (branch)
            {
                case TalentBranch.Guardian:
                case TalentBranch.GuardianBastion:
                case TalentBranch.GuardianUnbroken:
                case TalentBranch.GuardianVanguard: return "Guardian";
                case TalentBranch.Slayer:
                case TalentBranch.SlayerExecutioner:
                case TalentBranch.SlayerHunter:
                case TalentBranch.SlayerBerserker: return "Slayer";
                case TalentBranch.Pathfinder:
                case TalentBranch.PathfinderScout:
                case TalentBranch.PathfinderAcrobat:
                case TalentBranch.PathfinderSaboteur: return "Pathfinder";
                case TalentBranch.Seeker:
                case TalentBranch.SeekerMerchant:
                case TalentBranch.SeekerProspector:
                case TalentBranch.SeekerRelicKeeper: return "Seeker";
                case TalentBranch.Arcanist:
                case TalentBranch.ArcanistChronomancer:
                case TalentBranch.ArcanistHexer:
                case TalentBranch.ArcanistStormcaller: return "Arcanist";
                case TalentBranch.Technomancer:
                case TalentBranch.TechnomancerDroneMaster:
                case TalentBranch.TechnomancerInterceptor:
                case TalentBranch.TechnomancerConstructor: return "Technomancer";
                case TalentBranch.Commander:
                case TalentBranch.CommanderTactician:
                case TalentBranch.CommanderStandardBearer:
                case TalentBranch.CommanderWarlord: return "Commander";
                default: return string.Empty;
            }
        }

        public static string SpecForBranch(TalentBranch branch)
        {
            switch (branch)
            {
                case TalentBranch.GuardianBastion: return "Bastion";
                case TalentBranch.GuardianUnbroken: return "Unbroken";
                case TalentBranch.GuardianVanguard: return "Vanguard";
                case TalentBranch.SlayerExecutioner: return "Executioner";
                case TalentBranch.SlayerHunter: return "Hunter";
                case TalentBranch.SlayerBerserker: return "Berserker";
                case TalentBranch.PathfinderScout: return "Scout";
                case TalentBranch.PathfinderAcrobat: return "Acrobat";
                case TalentBranch.PathfinderSaboteur: return "Saboteur";
                case TalentBranch.SeekerMerchant: return "Merchant";
                case TalentBranch.SeekerProspector: return "Prospector";
                case TalentBranch.SeekerRelicKeeper: return "RelicKeeper";
                case TalentBranch.ArcanistChronomancer: return "Chronomancer";
                case TalentBranch.ArcanistHexer: return "Hexer";
                case TalentBranch.ArcanistStormcaller: return "Stormcaller";
                case TalentBranch.TechnomancerDroneMaster: return "DroneMaster";
                case TalentBranch.TechnomancerInterceptor: return "Interceptor";
                case TalentBranch.TechnomancerConstructor: return "Constructor";
                case TalentBranch.CommanderTactician: return "Tactician";
                case TalentBranch.CommanderStandardBearer: return "StandardBearer";
                case TalentBranch.CommanderWarlord: return "Warlord";
                default: return string.Empty;
            }
        }

        public static bool IsBranchActive(ProfileProgress profile, TalentBranch branch)
        {
            if (branch == TalentBranch.Legacy) return true;
            if (profile == null) return false;
            var classId = ClassForBranch(branch);
            if (profile.activeClass != classId) return false;
            var specId = SpecForBranch(branch);
            return string.IsNullOrEmpty(specId) || profile.activeSpecialization == specId;
        }

        public static int GetActiveRank(ProfileProgress profile, string id)
        {
            var def = Find(id);
            return def != null && IsBranchActive(profile, def.Branch) ? GetRank(profile, id) : 0;
        }
    }
}
