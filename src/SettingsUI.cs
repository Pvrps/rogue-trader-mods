using System;
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
                GUILayout.Width(300)
            );

            GUILayout.EndVertical();
        }
    }
}