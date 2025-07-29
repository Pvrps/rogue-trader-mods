using System.Reflection;
using HarmonyLib;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Purps.RogueTrader.Behaviours;

namespace Purps.RogueTrader.Patches
{
    public static class BlueprintBuffPatches
    {
        [HarmonyPatch(typeof(BlueprintBuff), "get_IsHiddenInUI")]
        public static class BlueprintBuff_IsHiddenInUI_Patch
        {
            public static bool Prefix(BlueprintBuff __instance, ref bool __result)
            {
                if (__instance.AssetGuid == Constants.PyromancyHeartOfMagmaTalentCounterBuff)
                {
                    __result = false;
                    return false;
                }

                return true;
            }
        }
    }
}