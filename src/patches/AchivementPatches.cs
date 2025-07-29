using HarmonyLib;
using Kingmaker;
using Kingmaker.Achievements;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;

namespace Purps.RogueTrader.Patches
{
    public static class AchievementPatches
    {
        public static Settings settings = Main.settings;

        // https://github.com/xADDBx/ToyBox-RogueTrader/blob/b2f773fe69ad1537919b35521ae0076f47b9143e/ToyBox/Classes/MonkeyPatchin/BagOfPatches/MiscRT.cs#L137-L150
        [HarmonyPatch(typeof(AchievementEntity), nameof(AchievementEntity.IsDisabled), MethodType.Getter)]
        public static class AchievementEntity_IsDisabled_Patch
        {
            public static void Postfix(ref bool __result, AchievementEntity __instance)
            {
                if (!settings.toggleAllowAchievementsDuringModdedGame) return;

                if (!__instance.Data.OnlyMainCampaign && Game.Instance.Player.Campaign && !Game.Instance.Player.Campaign.IsMainGameContent)
                {
                    __result = true;
                    return;
                }
                BlueprintCampaignReference specificCampaign = __instance.Data.SpecificCampaign;
                BlueprintCampaign blueprintCampaign = ((specificCampaign != null) ? specificCampaign.Get() : null);
                __result = !__instance.Data.OnlyMainCampaign && blueprintCampaign != null && Game.Instance.Player.Campaign != blueprintCampaign;
            }
        }

        // https://github.com/xADDBx/ToyBox-RogueTrader/blob/b2f773fe69ad1537919b35521ae0076f47b9143e/ToyBox/Classes/MonkeyPatchin/BagOfPatches/MiscRT.cs#L152-L161
        [HarmonyPatch(typeof(Player), nameof(Player.ModsUser), MethodType.Getter)]
        public static class Player_ModsUser_Patch
        {
            public static void Postfix(ref bool __result)
            {
                if (settings.toggleAllowAchievementsDuringModdedGame)
                {
                    __result = false;
                    return;
                }
            }
        }
    }
}