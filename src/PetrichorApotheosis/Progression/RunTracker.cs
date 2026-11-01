using System;
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
