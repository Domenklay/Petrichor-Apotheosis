using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace PetrichorLegacy.Progression
{
    public static class PerfectionRuntimeEffects
    {
        public static void Install()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.TakeDamageProcess += HealthComponent_TakeDamageProcess;
            Stage.onStageStartGlobal += OnStageStart;
        }

        public static void Uninstall()
        {
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
            On.RoR2.HealthComponent.TakeDamageProcess -= HealthComponent_TakeDamageProcess;
            Stage.onStageStartGlobal -= OnStageStart;
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self == null || self.master == null || self.master.playerCharacterMasterController == null) return;
            var progress = ServerProgressionState.Get(self.master);
            if (progress == null) return;

            if (progress.PursuitUntil > Time.fixedTime && ServerProgressionState.Perfection(self.master, "perfection_pursuit") > 0)
            {
                self.moveSpeed *= 1.03f;
                self.attackSpeed *= 1.02f;
            }
            if (ServerProgressionState.Perfection(self.master, "perfection_recovery") > 0) self.regen *= 1.05f;
            if (ServerProgressionState.Perfection(self.master, "perfection_ironwill") > 0 && self.healthComponent != null && self.healthComponent.combinedHealthFraction < 0.35f)
                self.armor += 6f;
            if (ServerProgressionState.Perfection(self.master, "perfection_apex") > 0)
            {
                self.damage *= 1.02f;
                self.crit += 1f;
            }

            var total = ServerProgressionState.MasteryTotalEffectiveRanks(self.master);
            if (ServerProgressionState.MasteryRank(self.master, "mastery_guardian_hp") >= 100) self.maxHealth *= 1.02f;
            if (ServerProgressionState.MasteryRank(self.master, "mastery_guardian_armor") >= 100) self.armor += 2f;

            if (ServerProgressionState.MasteryRank(self.master, "mastery_slayer_attack") >= 100) self.attackSpeed *= 1.02f;
            if (ServerProgressionState.MasteryRank(self.master, "mastery_slayer_cooldown") >= 100) ApplyCooldownScale(self.skillLocator, 0.01f);
            if (ServerProgressionState.MasteryRank(self.master, "mastery_slayer_crit") >= 100) self.crit += 1f;

            if (ServerProgressionState.MasteryRank(self.master, "mastery_path_move") >= 100) self.moveSpeed *= 1.02f;
            if (ServerProgressionState.MasteryRank(self.master, "mastery_path_sprint") >= 100 && self.isSprinting) self.armor += 1f;
            if (ServerProgressionState.MasteryRank(self.master, "mastery_path_jump") >= 100) self.maxJumpCount += 1;
            if (ServerProgressionState.MasteryRank(self.master, "mastery_path_control") >= 100) ApplyCooldownScale(self.skillLocator, 0.01f);

            if (ServerProgressionState.MasteryRank(self.master, "mastery_arcanist_attack") >= 100) self.attackSpeed *= 1.02f;
            if (ServerProgressionState.MasteryRank(self.master, "mastery_arcanist_cooldown") >= 100) ApplyCooldownScale(self.skillLocator, 0.015f);
            if (ServerProgressionState.MasteryRank(self.master, "mastery_arcanist_move") >= 100) self.moveSpeed *= 1.02f;

            if (ServerProgressionState.MasteryRank(self.master, "mastery_tech_attack") >= 100) self.attackSpeed *= 1.02f;
            if (ServerProgressionState.MasteryRank(self.master, "mastery_tech_move") >= 100) self.moveSpeed *= 1.02f;

            if (ServerProgressionState.MasteryRank(self.master, "mastery_commander_move") >= 100) { self.moveSpeed *= 1.015f; self.armor += 1f; }
            if (ServerProgressionState.MasteryRank(self.master, "mastery_commander_armor") >= 100) self.armor += 2f;

            switch (progress.ActiveClass)
            {
                case "Guardian":
                    if (total >= 25) self.armor += 2f;
                    if (total >= 60) self.maxHealth *= 1.02f;
                    if (total >= 120) { self.regen *= 1.05f; self.maxShield += self.baseMaxHealth * 0.03f; }
                    if (total >= 220) { self.armor += 4f; self.maxHealth *= 1.02f; }
                    break;
                case "Slayer":
                    if (total >= 25) self.damage *= 1.01f;
                    if (total >= 60) self.attackSpeed *= 1.015f;
                    if (total >= 220) { self.damage *= 1.02f; self.attackSpeed *= 1.01f; }
                    break;
                case "Pathfinder":
                    if (total >= 25) self.moveSpeed *= 1.015f;
                    if (total >= 60) self.sprintingSpeedMultiplier *= 1.02f;
                    if (total >= 120) { self.jumpPower *= 1.05f; self.armor += 1f; }
                    if (total >= 220) { self.moveSpeed *= 1.02f; self.maxJumpCount += 1; }
                    break;
                case "Arcanist":
                    if (total >= 60) self.damage *= 1.01f;
                    if (total >= 120) self.attackSpeed *= 1.015f;
                    ApplyCooldownScale(self.skillLocator, total >= 220 ? 0.03f : total >= 25 ? 0.01f : 0f);
                    break;
                case "Technomancer":
                    if (total >= 220) { self.attackSpeed *= 1.02f; self.moveSpeed *= 1.02f; }
                    break;
                case "Commander":
                    if (total >= 25) self.armor += 1f;
                    if (total >= 120) { self.damage *= 1.015f; self.moveSpeed *= 1.015f; }
                    if (total >= 220) self.armor += 2f;
                    break;
            }
        }

        private static void HealthComponent_TakeDamageProcess(On.RoR2.HealthComponent.orig_TakeDamageProcess orig, HealthComponent self, DamageInfo damageInfo)
        {
            var attacker = damageInfo.attacker ? damageInfo.attacker.GetComponent<CharacterBody>() : null;
            var victim = self != null ? self.body : null;
            if (attacker != null && attacker.master != null && victim != null)
            {
                if (ServerProgressionState.Perfection(attacker.master, "perfection_execution") > 0 && victim.healthComponent != null && victim.healthComponent.combinedHealthFraction < 0.25f)
                    damageInfo.damage *= 1.05f;
                if (ServerProgressionState.Perfection(attacker.master, "perfection_relentless") > 0 && (victim.isBoss || victim.GetComponent<MonsterRarityMarker>() != null))
                    damageInfo.damage *= 1.02f;
                var progress = ServerProgressionState.Get(attacker.master);
                var specialTarget = victim.isBoss || victim.GetComponent<MonsterRarityMarker>() != null;
                if (specialTarget)
                {
                    var prestigeDamage = progress != null ? PrestigeCatalog.HunterDamageBonus(progress.Level) : 0f;
                    if (prestigeDamage > 0f) damageInfo.damage *= 1f + prestigeDamage;
                    if (ServerProgressionState.MasteryRank(attacker.master, "mastery_slayer_damage") >= 100) damageInfo.damage *= 1.03f;
                    if (ServerProgressionState.MasteryRank(attacker.master, "mastery_arcanist_damage") >= 100) damageInfo.damage *= 1.03f;
                    if (ServerProgressionState.MasteryRank(attacker.master, "mastery_commander_damage") >= 100) damageInfo.damage *= 1.02f;
                }
                if (progress != null && progress.ActiveClass == "Slayer" && ServerProgressionState.MasteryTotalEffectiveRanks(attacker.master) >= 150 &&
                    (victim.isBoss || victim.GetComponent<MonsterRarityMarker>() != null))
                    damageInfo.damage *= 1.03f;
            }
            orig(self, damageInfo);
        }

        private static void OnStageStart(Stage stage)
        {
            if (!NetworkServer.active || Run.instance == null) return;
            var stageIndex = Run.instance.stageClearCount;
            foreach (var pcmc in PlayerCharacterMasterController.instances)
            {
                var master = pcmc?.master;
                var body = master?.GetBody();
                if (master == null || body == null || body.healthComponent == null) continue;

                if (ServerProgressionState.Perfection(master, "perfection_bulwark") > 0)
                    body.healthComponent.AddBarrier(body.maxHealth * 0.06f);
                if (ServerProgressionState.Perfection(master, "perfection_laststand") > 0)
                {
                    body.healthComponent.HealFraction(0.08f, default(ProcChainMask));
                    body.healthComponent.AddBarrier(body.maxHealth * 0.08f);
                }

                if (ServerProgressionState.MasteryRank(master, "mastery_guardian_regen") >= 100)
                    body.healthComponent.HealFraction(0.03f, default(ProcChainMask));
                if (ServerProgressionState.MasteryRank(master, "mastery_guardian_shield") >= 100)
                    body.healthComponent.AddBarrier(body.maxHealth * 0.05f);

                var gold = 0f;
                if (stageIndex == 0 && ServerProgressionState.Perfection(master, "perfection_capital") > 0) gold += 25f;
                if (stageIndex > 0 && ServerProgressionState.Perfection(master, "perfection_supply") > 0) gold += 15f;

                var progress = ServerProgressionState.Get(master);
                if (progress != null && progress.ActiveClass == "Seeker")
                {
                    var total = ServerProgressionState.MasteryTotalEffectiveRanks(master);
                    if (stageIndex > 0 && total >= 120) gold += 15f;
                    if (stageIndex == 0 && total >= 220) gold += 40f;
                }
                if (stageIndex > 0 && ServerProgressionState.MasteryRank(master, "mastery_seeker_stagegold") >= 100) gold += 20f;
                if (stageIndex == 0 && ServerProgressionState.MasteryRank(master, "mastery_seeker_startgold") >= 100) gold += 30f;
                if (gold > 0f) master.GiveMoney((uint)Mathf.RoundToInt(gold));
            }
        }

        private static void ApplyCooldownScale(SkillLocator locator, float reduction)
        {
            if (locator == null || reduction <= 0f) return;
            var mult = Mathf.Clamp(1f - reduction, 0.70f, 1f);
            ApplySkill(locator.primary, mult); ApplySkill(locator.secondary, mult); ApplySkill(locator.utility, mult); ApplySkill(locator.special, mult);
        }

        private static void ApplySkill(GenericSkill skill, float mult)
        {
            if (skill == null) return;
            var type = typeof(GenericSkill);
            var field = type.GetField("cooldownScale", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
            if (field != null && field.FieldType == typeof(float)) field.SetValue(skill, Mathf.Clamp((float)field.GetValue(skill) * mult, 0.70f, 2f));
        }
    }
}
