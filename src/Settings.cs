using UnityModManagerNet;

namespace Purps.RogueTrader
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool toggleAllowAchievementsDuringModdedGame = true;
        public bool toggleCombatOverlay = true;

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}