using System.Reflection;
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;

namespace Purps.RogueTrader.Patches
{
    public static class RuleBookPatches
    {
        [HarmonyPatch(typeof(RulePerformAttackRoll), "get_ResultIsRighteousFury")]
        public static class RulePerformAttackRoll_ResultIsRighteousFury_Patch
        {
            public static bool Prefix(RulePerformAttackRoll __instance, ref bool __result)
            {
                if (__instance.Initiator is BaseUnitEntity initiator && initiator.IsPlayerFaction)
                {
                    __result = true;  // Player always righteous fury (forces crits)
                    return false;
                }
                else
                {
                    __result = false; // Everything else never righteous fury (forces crits)
                    return false;
                }
            }
        }

        [HarmonyPatch(typeof(RuleDealDamage), "get_ResultIsCritical")]
        public static class RuleDealDamage_ResultIsCritical_Patch
        {
            public static bool Prefix(RuleDealDamage __instance, ref bool __result)
            {
                if (__instance.Initiator is BaseUnitEntity initiator && initiator.IsPlayerFaction)
                {
                    __result = true;  // Player always crit
                    return false;
                }
                else
                {
                    __result = false; // Everything else never crits
                    return false;
                }
            }
        }

        [HarmonyPatch(typeof(RulePerformAttackRoll), "get_ResultIsHit")]
        public static class RulePerformAttackRoll_ResultIsHit_Patch
        {
            public static bool Prefix(RulePerformAttackRoll __instance, ref bool __result)
            {
                if (__instance.Initiator is BaseUnitEntity initiator && initiator.IsPlayerFaction)
                {
                    __result = true;  // Player always hits
                    return false;
                }
                else
                {
                    __result = false; // Everything else never hits
                    return false;
                }
            }
        }
    }
}