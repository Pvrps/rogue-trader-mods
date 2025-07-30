using Purps.RogueTrader.Behaviours;
using UnityModManagerNet;

namespace Purps.RogueTrader
{
    public class Settings : UnityModManager.ModSettings
    {
        private bool _toggleCombatOverlay = true;
        private bool _toggleDebugOverlay = false;

        public void OnLoad()
        {
            Settings settings = Main.settings;
            if (settings.ToggleCombatOverlay)
            {
                Main.RegisterGameObject<CombatOverlayBehaviour>();
            }

            if (settings.ToggleDebugOverlay)
            {
                Main.RegisterGameObject<DebugOverlayBehaviour>();
            }
        }

        public bool ToggleAllowAchievementsDuringModdedGame { get; set; }
        public bool ToggleCombatOverlay
        {
            get => _toggleCombatOverlay;
            set
            {
                _toggleCombatOverlay = value;
                if (_toggleCombatOverlay)
                {
                    Main.RegisterGameObject<CombatOverlayBehaviour>();
                }
                else
                {
                    Main.UnregisterGameObject<CombatOverlayBehaviour>();
                }
            }
        }
        public bool ToggleDebugOverlay
        {
            get => _toggleDebugOverlay;
            set
            {
                _toggleDebugOverlay = value;
                if (_toggleDebugOverlay)
                {
                    Main.RegisterGameObject<DebugOverlayBehaviour>();
                }
                else
                {
                    Main.UnregisterGameObject<DebugOverlayBehaviour>();
                }
            }
        }

        public override void Save(UnityModManager.ModEntry modEntry) => Save(this, modEntry);
    }
}