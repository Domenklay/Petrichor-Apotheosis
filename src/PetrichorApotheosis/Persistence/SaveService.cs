using System;
using System.IO;
using BepInEx;
using PetrichorLegacy.Models;
using PetrichorLegacy.Progression;
using RoR2;
using UnityEngine;

namespace PetrichorLegacy.Persistence
{
    public static class SaveService
    {
        private static string _currentProfileKey;
        public static ProfileProgress Current { get; private set; } = new ProfileProgress();

        // Kept intentionally for compatibility with Petrichor Legacy 0.7.x saves.
        private static string SaveDirectory => System.IO.Path.Combine(Paths.ConfigPath, "PetrichorLegacy");

        public static bool RefreshProfile()
        {
            var local = LocalUserManager.GetFirstLocalUser();
            var profile = local?.userProfile;
            if (profile == null) return false;

            var key = string.IsNullOrWhiteSpace(profile.fileName) ? "default" : Sanitize(profile.fileName);
            if (key == _currentProfileKey) return true;

            _currentProfileKey = key;
            LoadCurrent();
            return true;
        }

        private static string CurrentPath => System.IO.Path.Combine(SaveDirectory, (_currentProfileKey ?? "default") + ".json");

        public static void LoadCurrent()
        {
            Directory.CreateDirectory(SaveDirectory);
            try
            {
                if (!File.Exists(CurrentPath))
                {
                    Current = new ProfileProgress();
                    SaveCurrent();
                    return;
                }

                var json = File.ReadAllText(CurrentPath);
                Current = JsonUtility.FromJson<ProfileProgress>(json) ?? new ProfileProgress();
                Normalize(Current);
                SaveCurrent();
            }
            catch (Exception e)
            {
                PetrichorApotheosisPlugin.Log.LogError($"Failed to load progression: {e}");
                Current = new ProfileProgress();
            }
        }

        public static void SaveCurrent()
        {
            if (Current == null || string.IsNullOrEmpty(_currentProfileKey)) return;
            Directory.CreateDirectory(SaveDirectory);
            try
            {
                Normalize(Current);
                File.WriteAllText(CurrentPath, JsonUtility.ToJson(Current, true));
            }
            catch (Exception e)
            {
                PetrichorApotheosisPlugin.Log.LogError($"Failed to save progression: {e}");
            }
        }

        private static void Normalize(ProfileProgress p)
        {
            p.level = Math.Max(1, p.level);
            p.currentXp = Math.Max(0, p.currentXp);
            p.unspentTalentPoints = Math.Max(0, p.unspentTalentPoints);
            p.unspentMasteryPoints = Math.Max(0, p.unspentMasteryPoints);
            p.unspentPerfectionPoints = Math.Max(0, p.unspentPerfectionPoints);
            if (p.talents == null) p.talents = new System.Collections.Generic.List<TalentRankRecord>();
            if (p.masteries == null) p.masteries = new System.Collections.Generic.List<MasteryRankRecord>();
            if (p.perfections == null) p.perfections = new System.Collections.Generic.List<PerfectionRankRecord>();
            if (p.startingLoadout == null) p.startingLoadout = new System.Collections.Generic.List<LoadoutItemRecord>();
            if (p.activeClass == null) p.activeClass = string.Empty;
            if (p.activeSpecialization == null) p.activeSpecialization = string.Empty;

            var migratedFromPreV3 = p.schemaVersion < 3;

            if (p.schemaVersion < 3)
            {
                var oldSpent = 0;
                foreach (var record in p.talents)
                {
                    if (record != null) oldSpent += Math.Max(0, record.rank);
                }
                p.talents.Clear();
                p.masteries.Clear();
                p.perfections.Clear();
                p.activeClass = string.Empty;
                p.activeSpecialization = string.Empty;

                p.unspentTalentPoints = Math.Max(p.unspentTalentPoints + oldSpent,
                    ProgressionService.EarnedTalentPointsForLevel(p.level));
                p.unspentMasteryPoints = Math.Max(p.unspentMasteryPoints,
                    ProgressionService.EarnedMasteryPointsForLevel(p.level));
                p.unspentPerfectionPoints = Math.Max(p.unspentPerfectionPoints,
                    ProgressionService.EarnedPerfectionPointsForLevel(p.level));
                p.startingLoadout.Clear();
                p.schemaVersion = 3;
            }

            if (p.schemaVersion < 4)
            {
                // The Legacy tree was redesigned. Refund only its old ranks; class/spec progress stays intact.
                var legacyRefund = 0;
                for (var i = p.talents.Count - 1; i >= 0; i--)
                {
                    var record = p.talents[i];
                    if (record == null) continue;
                    var def = TalentCatalog.Find(record.id);
                    if (def == null || def.Branch != TalentBranch.Legacy) continue;
                    legacyRefund += Math.Max(0, record.rank);
                    p.talents.RemoveAt(i);
                }

                p.unspentTalentPoints += legacyRefund;
                // v0.5.1 gives a real level-1 talent point. Pre-v3 migration already used the new earned-point formula.
                if (!migratedFromPreV3) p.unspentTalentPoints += 1;
                p.schemaVersion = 4;
            }

            if (p.schemaVersion < 5)
            {
                var combat = false;
                var survival = false;
                var mobility = false;
                for (var i = 0; i < p.talents.Count; i++)
                {
                    var r = p.talents[i];
                    if (r == null || r.rank <= 0) continue;
                    switch (r.id)
                    {
                        case "legacy_damage": case "legacy_attack": case "legacy_hunter": combat = true; break;
                        case "legacy_hp": case "legacy_regen": case "legacy_armor": survival = true; break;
                        case "legacy_move": case "legacy_sprint": case "legacy_jump": mobility = true; break;
                    }
                }

                var pathCount = (combat ? 1 : 0) + (survival ? 1 : 0) + (mobility ? 1 : 0);
                if (pathCount > 1)
                {
                    // Refund only path-specific Legacy nodes. Shared backpack/gold nodes remain untouched.
                    var refund = 0;
                    for (var i = p.talents.Count - 1; i >= 0; i--)
                    {
                        var r = p.talents[i];
                        if (r == null) continue;
                        var id = r.id ?? string.Empty;
                        var pathNode = id == "legacy_damage" || id == "legacy_attack" || id == "legacy_hunter" ||
                                       id == "legacy_hp" || id == "legacy_regen" || id == "legacy_armor" ||
                                       id == "legacy_move" || id == "legacy_sprint" || id == "legacy_jump";
                        if (!pathNode) continue;
                        refund += Math.Max(0, r.rank);
                        p.talents.RemoveAt(i);
                    }
                    p.unspentTalentPoints += refund;
                }
                p.schemaVersion = 5;
            }

            if (p.schemaVersion < 6)
            {
                // New Legacy nodes extend the already chosen direction. Existing purchases stay intact.
                p.schemaVersion = 6;
            }

            if (p.schemaVersion < 7)
            {
                // Perfection became a prerequisite-based constellation. Refund old choices only.
                var refund = 0;
                if (p.perfections != null)
                {
                    foreach (var r in p.perfections) if (r != null) refund += Math.Max(0, r.rank);
                    p.perfections.Clear();
                }
                p.unspentPerfectionPoints += refund;
                p.schemaVersion = 7;
            }
        }

        private static string Sanitize(string value)
        {
            foreach (var c in System.IO.Path.GetInvalidFileNameChars()) value = value.Replace(c, '_');
            return value;
        }
    }
}
