from pathlib import Path

ROOT = Path('src/PetrichorApotheosis')

def read(rel):
    return (ROOT / rel).read_text(encoding='utf-8')

def write(rel, text):
    (ROOT / rel).write_text(text, encoding='utf-8')

def replace_once(text, old, new, label):
    if old not in text:
        raise RuntimeError(f'missing anchor: {label}')
    return text.replace(old, new, 1)

# Profile defaults: level 1 has no free talent point. Level 2 earns the first one.
t = read('Models/ProfileProgress.cs')
t = replace_once(t, 'public int schemaVersion = 7;', 'public int schemaVersion = 8;', 'schema')
t = replace_once(t, 'public int unspentTalentPoints = 1;', 'public int unspentTalentPoints = 0;', 'default talent point')
write('Models/ProfileProgress.cs', t)

# Early Legacy = 9 earned points from levels 2..10:
# First Trophy, five nodes of one path, then Backpack/Stash/Fieldcraft.
t = read('Progression/TalentCatalog.cs')
repls = {
'new TalentDef("legacy_pack", "Первый трофей", "+1 вместимость стартового Арсенала. Базовая вместимость до этого равна 0.", TalentBranch.Legacy, 1, 1, 1, null, 0, 1, 0),':
'new TalentDef("legacy_pack", "Первый трофей", "+1 вместимость стартового Арсенала. Базовая вместимость до этого равна 0.", TalentBranch.Legacy, 1, 1, 2, null, 0, 1, 0),',
'new TalentDef("legacy_damage", "Заточенное оружие", "+1.5% урона.", TalentBranch.Legacy, 1, 1, 2, "legacy_pack", 1, 0, 1),':
'new TalentDef("legacy_damage", "Заточенное оружие", "+1.5% урона.", TalentBranch.Legacy, 1, 1, 3, "legacy_pack", 1, 0, 1),',
'new TalentDef("legacy_attack", "Быстрая рука", "+1.5% скорости атаки.", TalentBranch.Legacy, 1, 1, 3, "legacy_damage", 1, 0, 2),':
'new TalentDef("legacy_attack", "Быстрая рука", "+1.5% скорости атаки.", TalentBranch.Legacy, 1, 1, 4, "legacy_damage", 1, 0, 2),',
'new TalentDef("legacy_hunter", "Охотничий инстинкт", "+3% урона боссам и особым монстрам.", TalentBranch.Legacy, 1, 1, 4, "legacy_attack", 1, 0, 3),':
'new TalentDef("legacy_hunter", "Охотничий инстинкт", "+3% урона боссам и особым монстрам.", TalentBranch.Legacy, 1, 1, 5, "legacy_attack", 1, 0, 3),',
'new TalentDef("legacy_precision", "Точный расчёт", "+1% шанса критического удара.", TalentBranch.Legacy, 1, 1, 5, "legacy_hunter", 1, 0, 4),':
'new TalentDef("legacy_precision", "Точный расчёт", "+1% шанса критического удара.", TalentBranch.Legacy, 1, 1, 6, "legacy_hunter", 1, 0, 4),',
'new TalentDef("legacy_finisher", "Добивание", "+6% урона целям ниже 25% здоровья.", TalentBranch.Legacy, 1, 1, 6, "legacy_precision", 1, 0, 5),':
'new TalentDef("legacy_finisher", "Добивание", "+6% урона целям ниже 25% здоровья и +1% скорости движения.", TalentBranch.Legacy, 1, 1, 7, "legacy_precision", 1, 0, 5),',
'new TalentDef("legacy_hp", "Закалка", "+2% максимального здоровья.", TalentBranch.Legacy, 1, 1, 2, "legacy_pack", 1, 1, 1),':
'new TalentDef("legacy_hp", "Закалка", "+2% максимального здоровья.", TalentBranch.Legacy, 1, 1, 3, "legacy_pack", 1, 1, 1),',
'new TalentDef("legacy_regen", "Полевой рацион", "+4% естественной регенерации.", TalentBranch.Legacy, 1, 1, 3, "legacy_hp", 1, 1, 2),':
'new TalentDef("legacy_regen", "Полевой рацион", "+4% естественной регенерации.", TalentBranch.Legacy, 1, 1, 4, "legacy_hp", 1, 1, 2),',
'new TalentDef("legacy_armor", "Крепкая кожа", "+2 брони.", TalentBranch.Legacy, 1, 1, 4, "legacy_regen", 1, 1, 3),':
'new TalentDef("legacy_armor", "Крепкая кожа", "+2 брони.", TalentBranch.Legacy, 1, 1, 5, "legacy_regen", 1, 1, 3),',
'new TalentDef("legacy_resolve", "Упрямство", "Ниже 35% здоровья получаешь +3 брони.", TalentBranch.Legacy, 1, 1, 6, "legacy_recovery", 1, 1, 5),':
'new TalentDef("legacy_resolve", "Упрямство", "Ниже 35% здоровья получаешь +3 брони.", TalentBranch.Legacy, 1, 1, 6, "legacy_armor", 1, 1, 4),',
'new TalentDef("legacy_secondwind", "Резервный щит", "+3% максимального здоровья в виде щита.", TalentBranch.Legacy, 1, 1, 7, "legacy_resolve", 1, 1, 6),':
'new TalentDef("legacy_secondwind", "Резервный щит", "+3% максимального здоровья в виде щита.", TalentBranch.Legacy, 1, 1, 7, "legacy_resolve", 1, 1, 5),',
'new TalentDef("legacy_move", "Лёгкая поступь", "+2% скорости движения.", TalentBranch.Legacy, 1, 1, 2, "legacy_pack", 1, 2, 1),':
'new TalentDef("legacy_move", "Лёгкая поступь", "+2% скорости движения.", TalentBranch.Legacy, 1, 1, 3, "legacy_pack", 1, 2, 1),',
'new TalentDef("legacy_sprint", "Марш-бросок", "+3% скорости спринта.", TalentBranch.Legacy, 1, 1, 3, "legacy_move", 1, 2, 2),':
'new TalentDef("legacy_sprint", "Марш-бросок", "+3% скорости спринта.", TalentBranch.Legacy, 1, 1, 4, "legacy_move", 1, 2, 2),',
'new TalentDef("legacy_jump", "Лёгкое снаряжение", "+8% силы прыжка.", TalentBranch.Legacy, 1, 1, 4, "legacy_sprint", 1, 2, 3),':
'new TalentDef("legacy_jump", "Лёгкое снаряжение", "+8% силы прыжка.", TalentBranch.Legacy, 1, 1, 5, "legacy_sprint", 1, 2, 3),',
'new TalentDef("legacy_stride", "Длинный шаг", "Ещё +2% скорости спринта.", TalentBranch.Legacy, 1, 1, 6, "legacy_evasion", 1, 2, 5),':
'new TalentDef("legacy_stride", "Длинный шаг", "Ещё +2% скорости спринта.", TalentBranch.Legacy, 1, 1, 6, "legacy_jump", 1, 2, 4),',
'new TalentDef("legacy_aircontrol", "Чувство дистанции", "+5% силы прыжка.", TalentBranch.Legacy, 1, 1, 7, "legacy_stride", 1, 2, 6),':
'new TalentDef("legacy_aircontrol", "Чувство дистанции", "+5% силы прыжка и +1.5% скорости движения.", TalentBranch.Legacy, 1, 1, 7, "legacy_stride", 1, 2, 5),',
}
for old, new in repls.items():
    t = replace_once(t, old, new, old[:50])
for obsolete in [
'            new TalentDef("legacy_momentum", "Боевой темп", "+1% урона и +1% скорости движения.", TalentBranch.Legacy, 1, 1, 7, "legacy_finisher", 1, 0, 6),\n',
'            new TalentDef("legacy_recovery", "Запас сил", "+2% максимального здоровья.", TalentBranch.Legacy, 1, 1, 5, "legacy_armor", 1, 1, 4),\n',
'            new TalentDef("legacy_evasion", "Смена позиции", "+1.5% скорости движения.", TalentBranch.Legacy, 1, 1, 5, "legacy_jump", 1, 2, 4),\n',
]:
    if obsolete not in t:
        raise RuntimeError('obsolete Legacy node anchor missing')
    t = t.replace(obsolete, '', 1)
write('Progression/TalentCatalog.cs', t)

# Talent accounting and Legacy completion gate.
t = read('Progression/ProgressionService.cs')
t = replace_once(t,
'''        public static int EarnedTalentPointsForLevel(int level)\n        {\n            level = Math.Max(1, level);\n            if (level <= 40) return level;\n            return 40 + (level - 40) / 2;\n        }''',
'''        public static int EarnedTalentPointsForLevel(int level)\n        {\n            level = Math.Max(1, level);\n            if (level <= 40) return Math.Max(0, level - 1);\n            return 39 + (level - 40) / 2;\n        }''', 'earned talent formula')
t = t.replace('case "legacy_damage": case "legacy_attack": case "legacy_hunter": case "legacy_precision": case "legacy_finisher": case "legacy_momentum": return "combat";',
              'case "legacy_damage": case "legacy_attack": case "legacy_hunter": case "legacy_precision": case "legacy_finisher": return "combat";')
t = t.replace('case "legacy_hp": case "legacy_regen": case "legacy_armor": case "legacy_recovery": case "legacy_resolve": case "legacy_secondwind": return "survival";',
              'case "legacy_hp": case "legacy_regen": case "legacy_armor": case "legacy_resolve": case "legacy_secondwind": return "survival";')
t = t.replace('case "legacy_move": case "legacy_sprint": case "legacy_jump": case "legacy_evasion": case "legacy_stride": case "legacy_aircontrol": return "mobility";',
              'case "legacy_move": case "legacy_sprint": case "legacy_jump": case "legacy_stride": case "legacy_aircontrol": return "mobility";')
t = t.replace('"legacy_damage", "legacy_attack", "legacy_hunter", "legacy_precision", "legacy_finisher", "legacy_momentum",\n                "legacy_hp", "legacy_regen", "legacy_armor", "legacy_recovery", "legacy_resolve", "legacy_secondwind",\n                "legacy_move", "legacy_sprint", "legacy_jump", "legacy_evasion", "legacy_stride", "legacy_aircontrol"',
              '"legacy_damage", "legacy_attack", "legacy_hunter", "legacy_precision", "legacy_finisher",\n                "legacy_hp", "legacy_regen", "legacy_armor", "legacy_resolve", "legacy_secondwind",\n                "legacy_move", "legacy_sprint", "legacy_jump", "legacy_stride", "legacy_aircontrol"')
t = replace_once(t,
'''            if (talentId == "legacy_pack2" && string.IsNullOrEmpty(chosenPath))\n            {\n                error = "Сначала продвинься дальше по Наследию.";\n                return false;\n            }''',
'''            if (talentId == "legacy_pack2" && (string.IsNullOrEmpty(chosenPath) || !LegacyPathComplete(profile, chosenPath)))\n            {\n                error = "Сначала заверши выбранное направление Наследия.";\n                return false;\n            }''', 'pack2 completion gate')
t = replace_once(t,
'''        public static bool CanBuyLegacyPathTalent(ProfileProgress profile, string talentId)\n        {\n            if (profile == null) return false;\n            var chosen = LegacyPathChoice(profile);\n            if (talentId == "legacy_pack2") return !string.IsNullOrEmpty(chosen);\n            var path = LegacyPathForTalent(talentId);\n            return string.IsNullOrEmpty(path) || string.IsNullOrEmpty(chosen) || string.Equals(path, chosen, StringComparison.OrdinalIgnoreCase);\n        }''',
'''        public static bool LegacyPathComplete(ProfileProgress profile, string path)\n        {\n            if (profile == null || string.IsNullOrEmpty(path)) return false;\n            if (string.Equals(path, "combat", StringComparison.OrdinalIgnoreCase)) return TalentCatalog.GetRank(profile, "legacy_finisher") > 0;\n            if (string.Equals(path, "survival", StringComparison.OrdinalIgnoreCase)) return TalentCatalog.GetRank(profile, "legacy_secondwind") > 0;\n            if (string.Equals(path, "mobility", StringComparison.OrdinalIgnoreCase)) return TalentCatalog.GetRank(profile, "legacy_aircontrol") > 0;\n            return false;\n        }\n\n        public static bool CanBuyLegacyPathTalent(ProfileProgress profile, string talentId)\n        {\n            if (profile == null) return false;\n            var chosen = LegacyPathChoice(profile);\n            if (talentId == "legacy_pack2") return !string.IsNullOrEmpty(chosen) && LegacyPathComplete(profile, chosen);\n            var path = LegacyPathForTalent(talentId);\n            return string.IsNullOrEmpty(path) || string.IsNullOrEmpty(chosen) || string.Equals(path, chosen, StringComparison.OrdinalIgnoreCase);\n        }''', 'legacy can buy')
write('Progression/ProgressionService.cs', t)

# Save migration: refund removed early nodes, then restore any missing legitimately-earned points.
t = read('Persistence/SaveService.cs')
anchor = '''            if (p.schemaVersion < 7)\n            {\n                // Perfection became a prerequisite-based constellation. Refund old choices only.\n                var refund = 0;\n                if (p.perfections != null)\n                {\n                    foreach (var r in p.perfections) if (r != null) refund += Math.Max(0, r.rank);\n                    p.perfections.Clear();\n                }\n                p.unspentPerfectionPoints += refund;\n                p.schemaVersion = 7;\n            }'''
insert = anchor + '''\n\n            if (p.schemaVersion < 8)\n            {\n                // 0.8.1 removes the old free level-1 point and compresses each early Legacy route to five nodes.\n                // Removed nodes are refunded; existing legitimate purchases are never taken away.\n                var obsolete = new[] { "legacy_momentum", "legacy_recovery", "legacy_evasion" };\n                for (var i = p.talents.Count - 1; i >= 0; i--)\n                {\n                    var r = p.talents[i];\n                    if (r == null || Array.IndexOf(obsolete, r.id) < 0) continue;\n                    p.unspentTalentPoints += Math.Max(0, r.rank);\n                    p.talents.RemoveAt(i);\n                }\n                p.schemaVersion = 8;\n            }\n\n            // Self-heal lost talent currency from older builds/saves.\n            // Never subtracts grandfathered points: it only restores a deficit.\n            var expectedTalentPoints = ProgressionService.EarnedTalentPointsForLevel(p.level);\n            var accountedTalentPoints = TalentCatalog.GetSpentPoints(p) + p.unspentTalentPoints;\n            if (accountedTalentPoints < expectedTalentPoints)\n                p.unspentTalentPoints += expectedTalentPoints - accountedTalentPoints;'''
t = replace_once(t, anchor, insert, 'schema 7 migration')
write('Persistence/SaveService.cs', t)

# Compact Legacy UI to five nodes per path.
t = read('UI/MetaProgressionUIV5.cs')
t = t.replace('const float contentHeight = 1745f;', 'const float contentHeight = 1595f;')
t = t.replace('var pathH = 960f;', 'var pathH = 810f;')
t = t.replace('new[] { "legacy_damage", "legacy_attack", "legacy_hunter", "legacy_precision", "legacy_finisher", "legacy_momentum" }', 'new[] { "legacy_damage", "legacy_attack", "legacy_hunter", "legacy_precision", "legacy_finisher" }')
t = t.replace('new[] { "legacy_hp", "legacy_regen", "legacy_armor", "legacy_recovery", "legacy_resolve", "legacy_secondwind" }', 'new[] { "legacy_hp", "legacy_regen", "legacy_armor", "legacy_resolve", "legacy_secondwind" }')
t = t.replace('new[] { "legacy_move", "legacy_sprint", "legacy_jump", "legacy_evasion", "legacy_stride", "legacy_aircontrol" }', 'new[] { "legacy_move", "legacy_sprint", "legacy_jump", "legacy_stride", "legacy_aircontrol" }')
t = t.replace('new Rect(w * 0.5f - 210f, 1215f, 420f, 30f)', 'new Rect(w * 0.5f - 210f, 1065f, 420f, 30f)')
t = t.replace('new Rect(w * 0.5f - sharedW * 0.5f, 1260f, sharedW, 140f)', 'new Rect(w * 0.5f - sharedW * 0.5f, 1110f, sharedW, 140f)')
t = t.replace('new Rect(w * 0.5f - sharedW * 0.5f, 1417f, sharedW, 140f)', 'new Rect(w * 0.5f - sharedW * 0.5f, 1267f, sharedW, 140f)')
t = t.replace('new Rect(w * 0.5f - sharedW * 0.5f, 1574f, sharedW, 140f)', 'new Rect(w * 0.5f - sharedW * 0.5f, 1424f, sharedW, 140f)')
for line in [
'            if (id == "legacy_momentum") return new[] { "WarCryOnMultiKill" };\n',
'            if (id == "legacy_recovery") return new[] { "Medkit" };\n',
'            if (id == "legacy_evasion") return new[] { "SprintOutOfCombat" };\n',
]:
    t = t.replace(line, '')
write('UI/MetaProgressionUIV5.cs', t)

# Live XP: bosses immediately, completed stages when the next stage starts, end-of-run only a completion bonus.
runtracker = r'''using System;
using RoR2;
using UnityEngine;
using PetrichorLegacy.Persistence;

namespace PetrichorLegacy.Progression
{
    public static class RunTracker
    {
        private static int _bossKills;
        private static int _lastAwardedStageCount;
        private static bool _awarded;
        private static bool _won;
        private static string _endingName;

        public static void Install()
        {
            Run.onRunStartGlobal += OnRunStart;
            Run.onRunDestroyGlobal += OnRunDestroy;
            Stage.onStageStartGlobal += OnStageStart;
            GlobalEventManager.onCharacterDeathGlobal += OnCharacterDeath;
            On.RoR2.Run.BeginGameOver += Run_BeginGameOver;
        }

        public static void Uninstall()
        {
            Run.onRunStartGlobal -= OnRunStart;
            Run.onRunDestroyGlobal -= OnRunDestroy;
            Stage.onStageStartGlobal -= OnStageStart;
            GlobalEventManager.onCharacterDeathGlobal -= OnCharacterDeath;
            On.RoR2.Run.BeginGameOver -= Run_BeginGameOver;
        }

        private static void OnRunStart(Run run)
        {
            _bossKills = 0;
            _lastAwardedStageCount = 0;
            _awarded = false;
            _won = false;
            _endingName = "Unknown";
            ServerProgressionState.ResetForRun();
            PetrichorApotheosisPlugin.Instance.StartCoroutine(Networking.ProgressionNetwork.SendLocalProgressWhenReady());
        }

        private static void OnStageStart(Stage stage)
        {
            var run = Run.instance;
            if (run == null) return;
            AwardPendingStages(run);
        }

        private static void OnCharacterDeath(DamageReport report)
        {
            if (report?.victimBody == null || !report.victimBody.isBoss) return;
            _bossKills++;
            AwardLiveXp(90f, "Босс");
        }

        private static void Run_BeginGameOver(On.RoR2.Run.orig_BeginGameOver orig, Run self, GameEndingDef ending)
        {
            _won = ending != null && ending.isWin;
            _endingName = ending != null ? ending.cachedName : "Unknown";
            AwardPendingStages(self);
            AwardIfNeeded(self);
            orig(self, ending);
        }

        private static void OnRunDestroy(Run run)
        {
            AwardPendingStages(run);
            AwardIfNeeded(run);
        }

        private static void AwardPendingStages(Run run)
        {
            if (run == null) return;
            var cleared = Mathf.Max(0, run.stageClearCount);
            if (cleared <= _lastAwardedStageCount) return;
            var newlyCleared = cleared - _lastAwardedStageCount;
            _lastAwardedStageCount = cleared;
            AwardLiveXp(260f * newlyCleared, newlyCleared == 1 ? "Пройдена стадия" : $"Пройдено стадий: {newlyCleared}");
        }

        private static void AwardLiveXp(float rawXp, string reason)
        {
            if (rawXp <= 0f || !SaveService.RefreshProfile()) return;
            var run = Run.instance;
            var difficultyMultiplier = 1f;
            if (run != null)
            {
                var diffDef = DifficultyCatalog.GetDifficultyDef(run.selectedDifficulty);
                var scaling = diffDef != null ? Mathf.Max(0.5f, diffDef.scalingValue) : 1f;
                difficultyMultiplier = Mathf.Clamp(0.75f + scaling * 0.25f, 0.75f, 2.5f);
            }
            var xp = Mathf.Max(1, Mathf.RoundToInt(rawXp * difficultyMultiplier));
            ProgressionService.AwardXp(SaveService.Current, xp, reason);
            PetrichorApotheosisPlugin.Log?.LogInfo($"Live meta XP: +{xp} ({reason})");
        }

        private static void AwardIfNeeded(Run run)
        {
            if (_awarded || run == null) return;
            if (!SaveService.RefreshProfile()) return;

            _awarded = true;
            var profile = SaveService.Current;

            var minutes = Mathf.Clamp(run.GetRunStopwatch() / 60f, 0f, 60f);
            var diffDef = DifficultyCatalog.GetDifficultyDef(run.selectedDifficulty);
            var scaling = diffDef != null ? Mathf.Max(0.5f, diffDef.scalingValue) : 1f;

            // Stages and bosses are already rewarded live. End-of-run XP is only the completion/time bonus.
            var raw = 200f + minutes * 5f;
            var difficultyMultiplier = Mathf.Clamp(0.75f + scaling * 0.25f, 0.75f, 2.5f);
            var resultMultiplier = _won ? 1.5f : IsObliteration(_endingName) ? 1.25f : 1f;
            var xp = Mathf.Max(50, Mathf.RoundToInt(raw * difficultyMultiplier * resultMultiplier));

            profile.runsCompleted++;
            if (_won) profile.wins++;
            ProgressionService.AwardXp(profile, xp, _won ? "Победа в забеге" : "Итог забега");
            PetrichorApotheosisPlugin.Log.LogInfo($"Run completion XP: +{xp} (stages={run.stageClearCount}, bosses={_bossKills}, win={_won}, ending={_endingName})");
        }

        private static bool IsObliteration(string endingName)
        {
            return !string.IsNullOrEmpty(endingName) && endingName.IndexOf("Obliter", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
'''
write('Progression/RunTracker.cs', runtracker)

# Version bump.
t = read('Plugin.cs').replace('public const string PluginVersion = "0.8.0";', 'public const string PluginVersion = "0.8.1";')
write('Plugin.cs', t)
t = read('PetrichorApotheosis.csproj').replace('<Version>0.8.0</Version>', '<Version>0.8.1</Version>')
write('PetrichorApotheosis.csproj', t)
t = read('manifest.json').replace('"version_number": "0.8.0"', '"version_number": "0.8.1"')
write('manifest.json', t)

print('Applied Petrichor: Apotheosis 0.8.1 hotfix')
