using System.Reflection;
using RoR2;
using UnityEngine;

namespace PetrichorLegacy.Progression
{
    public static class AdditionalSpecEffects
    {
        public static void Install()
        {
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        public static void Uninstall()
        {
            On.RoR2.CharacterBody.RecalculateStats -= CharacterBody_RecalculateStats;
        }

        private static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);
            if (self == null || self.master == null) return;

            // Minions inherit only the deliberately small minion bonuses of the active Technomancer/Commander build.
            if (self.master.playerCharacterMasterController == null)
            {
                var ownership = self.master.GetComponent<MinionOwnership>();
                var owner = ownership != null ? ownership.ownerMaster : null;
                if (owner == null || ServerProgressionState.Get(owner) == null) return;
                var dmg = 0f;
                var hp = 0f;
                var atk = 0f;
                var mov = 0f;
                dmg += ServerProgressionState.Rank(owner, "tech_minion_damage") * 0.05f;
                hp += ServerProgressionState.Rank(owner, "tech_minion_health") * 0.05f;
                if (ServerProgressionState.Rank(owner, "tech_core") > 0) dmg += 0.05f;
                dmg += ServerProgressionState.Rank(owner, "drone_damage") * 0.06f;
                hp += ServerProgressionState.Rank(owner, "drone_health") * 0.06f;
                atk += ServerProgressionState.Rank(owner, "drone_attack") * 0.05f;
                mov += ServerProgressionState.Rank(owner, "drone_move") * 0.05f;
                if (ServerProgressionState.Rank(owner, "drone_network") > 0) dmg += 0.10f;
                dmg += ServerProgressionState.Rank(owner, "constructor_minion_damage") * 0.05f;
                hp += ServerProgressionState.Rank(owner, "constructor_minion_health") * 0.07f;
                if (ServerProgressionState.Rank(owner, "constructor_core") > 0) { atk += 0.10f; mov += 0.10f; }
                dmg += ServerProgressionState.Rank(owner, "warlord_minion_damage") * 0.04f;
                hp += ServerProgressionState.Rank(owner, "warlord_minion_health") * 0.04f;
                if (ServerProgressionState.Rank(owner, "warlord_army") > 0) atk += 0.08f;
                dmg += ServerProgressionState.MasteryValue(owner, "mastery_tech_minion_damage");
                hp += ServerProgressionState.MasteryValue(owner, "mastery_tech_minion_health");
                dmg += ServerProgressionState.MasteryValue(owner, "mastery_commander_minion");
                if (ServerProgressionState.MasteryRank(owner, "mastery_tech_minion_damage") >= 100) dmg += 0.08f;
                if (ServerProgressionState.MasteryRank(owner, "mastery_tech_minion_health") >= 100) hp += 0.08f;
                if (ServerProgressionState.MasteryRank(owner, "mastery_commander_minion") >= 100) { dmg += 0.05f; hp += 0.05f; }
                var ownerProgress = ServerProgressionState.Get(owner);
                var masteryTotal = ServerProgressionState.MasteryTotalEffectiveRanks(owner);
                if (ownerProgress != null && ownerProgress.ActiveClass == "Technomancer")
                {
                    if (masteryTotal >= 25) dmg += 0.04f;
                    if (masteryTotal >= 60) hp += 0.04f;
                    if (masteryTotal >= 120) { atk += 0.05f; mov += 0.05f; }
                }
                if (ownerProgress != null && ownerProgress.ActiveClass == "Commander")
                {
                    if (masteryTotal >= 60) { dmg += 0.03f; hp += 0.03f; }
                    if (masteryTotal >= 220) { atk += 0.05f; mov += 0.05f; }
                }
                self.damage *= 1f + dmg;
                self.maxHealth *= 1f + hp;
                self.attackSpeed *= 1f + atk;
                self.moveSpeed *= 1f + mov;
                return;
            }
            if (ServerProgressionState.Get(self.master) == null) return;

            // Guardian / Vanguard — modest offense without reaching Slayer output.
            self.damage *= 1f + ServerProgressionState.Rank(self.master, "vanguard_damage") * 0.01f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "vanguard_move") * 0.015f;
            self.armor += ServerProgressionState.Rank(self.master, "vanguard_armor") * 2f;
            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "vanguard_attack") * 0.015f;
            if (ServerProgressionState.Rank(self.master, "vanguard_breaker") > 0)
            {
                self.damage *= 1.02f;
                self.armor += 2f;
            }

            // Slayer / Berserker — highest tempo, no extra defensive scaling.
            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "berserker_attack") * 0.02f;
            self.damage *= 1f + ServerProgressionState.Rank(self.master, "berserker_damage") * 0.015f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "berserker_move") * 0.015f;
            ApplyCooldownScale(self.skillLocator, ServerProgressionState.Rank(self.master, "berserker_cooldown") * 0.01f);
            if (ServerProgressionState.Rank(self.master, "berserker_frenzy") > 0)
            {
                self.damage *= 1.02f;
                self.attackSpeed *= 1.02f;
            }

            // Pathfinder / Saboteur — skill frequency and crowd-control oriented damage.
            ApplyCooldownScale(self.skillLocator, ServerProgressionState.Rank(self.master, "saboteur_cooldown") * 0.01f);
            self.damage *= 1f + ServerProgressionState.Rank(self.master, "saboteur_damage") * 0.01f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "saboteur_move") * 0.015f;
            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "saboteur_attack") * 0.015f;
            if (ServerProgressionState.Rank(self.master, "saboteur_chain") > 0)
            {
                self.damage *= 1.02f;
                ApplyCooldownScale(self.skillLocator, 0.02f);
            }

            // Seeker / Relic Keeper — utility stats; the legendary reward handles equipment identity.
            ApplyCooldownScale(self.skillLocator, ServerProgressionState.Rank(self.master, "relic_cooldown") * 0.01f);
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "relic_move") * 0.015f;
            self.armor += ServerProgressionState.Rank(self.master, "relic_armor") * 2f;
            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "relic_attack") * 0.015f;
            if (ServerProgressionState.Rank(self.master, "relic_attunement") > 0)
            {
                self.moveSpeed *= 1.02f;
                ApplyCooldownScale(self.skillLocator, 0.02f);
            }

            // Arcanist — skill-centric glass cannon.
            self.damage *= 1f + ServerProgressionState.Rank(self.master, "arcanist_damage") * 0.01f;
            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "arcanist_attack") * 0.01f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "arcanist_move") * 0.015f;
            self.crit += ServerProgressionState.Rank(self.master, "arcanist_crit") * 0.75f;
            ApplyCooldownScale(self.skillLocator, ServerProgressionState.Rank(self.master, "arcanist_cooldown") * 0.01f);
            if (ServerProgressionState.Rank(self.master, "arcanist_convergence") > 0) { self.damage *= 1.01f; ApplyCooldownScale(self.skillLocator, 0.02f); }

            ApplyCooldownScale(self.skillLocator, ServerProgressionState.Rank(self.master, "chrono_cooldown") * 0.015f);
            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "chrono_attack") * 0.015f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "chrono_move") * 0.015f;
            self.damage *= 1f + ServerProgressionState.Rank(self.master, "chrono_damage") * 0.01f;
            if (ServerProgressionState.Rank(self.master, "chrono_overclock") > 0) { self.attackSpeed *= 1.02f; ApplyCooldownScale(self.skillLocator, 0.02f); }

            self.damage *= 1f + ServerProgressionState.Rank(self.master, "hex_damage") * 0.01f;
            self.crit += ServerProgressionState.Rank(self.master, "hex_crit") * 0.75f;
            ApplyCooldownScale(self.skillLocator, ServerProgressionState.Rank(self.master, "hex_cooldown") * 0.01f);

            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "storm_attack") * 0.015f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "storm_move") * 0.015f;
            self.damage *= 1f + ServerProgressionState.Rank(self.master, "storm_damage") * 0.01f;
            ApplyCooldownScale(self.skillLocator, ServerProgressionState.Rank(self.master, "storm_cooldown") * 0.01f);
            if (ServerProgressionState.Rank(self.master, "storm_charge") > 0) { self.attackSpeed *= 1.02f; self.damage *= 1.01f; }

            // Technomancer — personal stats stay light; most power is external.
            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "tech_attack") * 0.015f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "tech_move") * 0.015f;
            ApplyCooldownScale(self.skillLocator, ServerProgressionState.Rank(self.master, "tech_cooldown") * 0.01f);
            if (ServerProgressionState.Rank(self.master, "tech_core") > 0) { self.attackSpeed *= 1.02f; self.moveSpeed *= 1.02f; }
            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "interceptor_attack") * 0.015f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "interceptor_move") * 0.015f;
            self.damage *= 1f + ServerProgressionState.Rank(self.master, "interceptor_damage") * 0.01f;
            ApplyCooldownScale(self.skillLocator, ServerProgressionState.Rank(self.master, "interceptor_cooldown") * 0.01f);
            if (ServerProgressionState.Rank(self.master, "interceptor_matrix") > 0) { self.attackSpeed *= 1.02f; self.moveSpeed *= 1.02f; }
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "constructor_move") * 0.015f;
            ApplyCooldownScale(self.skillLocator, ServerProgressionState.Rank(self.master, "constructor_cooldown") * 0.01f);

            // Commander — broad but deliberately smaller bonuses than dedicated classes.
            self.damage *= 1f + ServerProgressionState.Rank(self.master, "commander_damage") * 0.01f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "commander_move") * 0.01f;
            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "commander_attack") * 0.01f;
            self.armor += ServerProgressionState.Rank(self.master, "commander_armor");
            if (ServerProgressionState.Rank(self.master, "commander_order") > 0) { self.damage *= 1.01f; self.moveSpeed *= 1.01f; self.attackSpeed *= 1.01f; }
            self.crit += ServerProgressionState.Rank(self.master, "tactician_crit") * 0.75f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "tactician_move") * 0.01f;
            self.maxHealth *= 1f + ServerProgressionState.Rank(self.master, "standard_health") * 0.01f;
            self.attackSpeed *= 1f + ServerProgressionState.Rank(self.master, "standard_attack") * 0.015f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "standard_move") * 0.015f;
            self.armor += ServerProgressionState.Rank(self.master, "standard_armor");
            if (ServerProgressionState.Rank(self.master, "standard_banner") > 0) { self.damage *= 1.01f; self.attackSpeed *= 1.01f; self.moveSpeed *= 1.01f; }
            self.damage *= 1f + ServerProgressionState.Rank(self.master, "warlord_damage") * 0.01f;
            self.moveSpeed *= 1f + ServerProgressionState.Rank(self.master, "warlord_move") * 0.015f;

            self.damage *= 1f + ServerProgressionState.MasteryValue(self.master, "mastery_arcanist_damage");
            self.attackSpeed *= 1f + ServerProgressionState.MasteryValue(self.master, "mastery_arcanist_attack");
            self.moveSpeed *= 1f + ServerProgressionState.MasteryValue(self.master, "mastery_arcanist_move");
            ApplyCooldownScale(self.skillLocator, ServerProgressionState.MasteryValue(self.master, "mastery_arcanist_cooldown"));
            self.attackSpeed *= 1f + ServerProgressionState.MasteryValue(self.master, "mastery_tech_attack");
            self.moveSpeed *= 1f + ServerProgressionState.MasteryValue(self.master, "mastery_tech_move");
            self.damage *= 1f + ServerProgressionState.MasteryValue(self.master, "mastery_commander_damage");
            self.moveSpeed *= 1f + ServerProgressionState.MasteryValue(self.master, "mastery_commander_move");
            self.armor += ServerProgressionState.MasteryValue(self.master, "mastery_commander_armor");
        }

        private static void ApplyCooldownScale(SkillLocator locator, float reduction)
        {
            if (locator == null || reduction <= 0f) return;
            var mult = Mathf.Clamp(1f - reduction, 0.70f, 1f);
            ApplyCooldownScale(locator.primary, mult);
            ApplyCooldownScale(locator.secondary, mult);
            ApplyCooldownScale(locator.utility, mult);
            ApplyCooldownScale(locator.special, mult);
        }

        private static void ApplyCooldownScale(GenericSkill skill, float multiplier)
        {
            if (skill == null) return;
            var type = typeof(GenericSkill);
            var field = type.GetField("cooldownScale", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (field != null && field.FieldType == typeof(float))
            {
                var value = (float)field.GetValue(skill);
                field.SetValue(skill, Mathf.Clamp(value * multiplier, 0.70f, 2f));
                return;
            }
            var prop = type.GetProperty("cooldownScale", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (prop != null && prop.CanRead && prop.CanWrite && prop.PropertyType == typeof(float))
            {
                var value = (float)prop.GetValue(skill, null);
                prop.SetValue(skill, Mathf.Clamp(value * multiplier, 0.70f, 2f), null);
            }
        }
    }
}
