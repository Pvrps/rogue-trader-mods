using System;
using Purps.RogueTrader.Behaviours;
using Purps.RogueTrader.Logging;
using UnityEngine;

namespace Purps.RogueTrader
{
    public partial class SettingsUI
    {
        public static void OnGUI()
        {
            GUILayout.BeginVertical("box");

            Main.settings.toggleAllowAchievementsDuringModdedGame = GUILayout.Toggle(
                Main.settings.toggleAllowAchievementsDuringModdedGame,
                "Allow Achievements During Modded Game",
                GUILayout.Width(1000)
            );

            bool toggleCombatOverlay = Main.settings.toggleCombatOverlay;
            Main.settings.toggleCombatOverlay = GUILayout.Toggle(
                Main.settings.toggleCombatOverlay,
                "Shows helpful information during combat",
                GUILayout.Width(1000)
            );
            if (Main.settings.toggleCombatOverlay != toggleCombatOverlay)
            {
                if (Main.settings.toggleCombatOverlay)
                {
                    Main.RegisterGameObject<CombatOverlayBehaviour>();
                }
                else
                {
                    Main.UnregisterGameObject<CombatOverlayBehaviour>();
                }
            }

            GUILayout.EndVertical();
        }
    }
}