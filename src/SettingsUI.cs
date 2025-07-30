using UnityEngine;

namespace Purps.RogueTrader
{
    public partial class SettingsUI
    {
        public static void OnGUI()
        {
            GUILayout.BeginVertical("box");

            Main.settings.ToggleAllowAchievementsDuringModdedGame = GUILayout.Toggle(
                Main.settings.ToggleAllowAchievementsDuringModdedGame,
                "Allow Achievements During Modded Game",
                GUILayout.Width(1000)
            );

            bool oldToggle = Main.settings.ToggleCombatOverlay;
            bool newToggle = Main.settings.ToggleCombatOverlay = GUILayout.Toggle(
                Main.settings.ToggleCombatOverlay,
                "Shows helpful information during combat",
                GUILayout.Width(1000)
            );
            if (oldToggle != newToggle)
            {
                Main.settings.ToggleCombatOverlay = newToggle;
            }

#if DEBUG
            oldToggle = Main.settings.ToggleDebugOverlay;
            newToggle = Main.settings.ToggleDebugOverlay = GUILayout.Toggle(
                Main.settings.ToggleDebugOverlay,
                "Shows debugging information",
                GUILayout.Width(1000)
            );
            if (oldToggle != newToggle)
            {
                Main.settings.ToggleDebugOverlay = newToggle;
            }
#endif

            GUILayout.EndVertical();
        }
    }
}