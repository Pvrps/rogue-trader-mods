using UnityModManagerNet;

namespace Purps.RogueTrader
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool toggleAllowAchievementsDuringModdedGame = true;
        public bool toggleCombatOverlay = true;
        public bool toggleDebugOverlay = false;

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}