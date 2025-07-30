using System.Collections.Generic;
using System.Linq;
using Kingmaker.EntitySystem.Entities;
using Purps.RogueTrader.API.Menu;
using Purps.RogueTrader.API.Unit;
using UnityEngine;

namespace Purps.RogueTrader.Behaviours
{
    public class DebugOverlayBehaviour : MonoBehaviour
    {
        private Texture2D blackTexture;
        private GUIStyle tabButtonStyle;
        private GUIStyle tableButtonStyle;
        private GUIStyle labelStyle;
        private Vector2 tableScrollPos;

        private class DebugTableTab
        {
            public string Label;
            public List<string> Lines = new List<string>();
        }

        private List<DebugTableTab> tabs = new List<DebugTableTab>();
        private int activeTabIndex = 0;

        public void Update()
        {
            EnsureInitialized();

            tabs.Clear();

            if (RTMenu.IsOpen())
            {
                return;
            }

            BaseUnitEntity entity = RTUnit.GetSelectedUnit();
            if (entity == null)
            {
                return;
            }

            var featuresTab = new DebugTableTab { Label = "Features" };
            foreach (var feature in entity.Progression.Features)
            {
                string name = feature.Blueprint.Name;
                featuresTab.Lines.Add($"{(string.IsNullOrEmpty(name) ? "XXXXXXXX" : name)} ({feature.Blueprint.AssetGuid})");
            }
            tabs.Add(featuresTab);

            var bonusesTab = new DebugTableTab { Label = "Bonuses" };
            foreach (var bonus in RTUnit.GetBonusAbilities())
            {
                string name = bonus.Source.Blueprint.name;
                bonusesTab.Lines.Add($"{(string.IsNullOrEmpty(name) ? "XXXXXXXX" : name)} ({bonus.Source.Blueprint.AssetGuid})");
            }
            tabs.Add(bonusesTab);

            if (activeTabIndex >= tabs.Count) activeTabIndex = 0;
        }

        public void OnGUI()
        {
            EnsureInitialized();

            if (tabs.Count == 0) return;

            float startX = 20f;
            float startY = 20f;
            float tabWidth = 160f;
            string longestTabLabel = tabs.Max(tab => tab.Label.Length) > 0
                ? tabs.OrderByDescending(tab => tab.Label.Length).First().Label
                : "Sample";
            float tabHeight = tabButtonStyle.CalcSize(new GUIContent(longestTabLabel)).y + 8f;
            float tabSpacing = 6f;

            // Draw tabs
            for (int i = 0; i < tabs.Count; i++)
            {
                float tabPosX = startX + i * (tabWidth + tabSpacing);
                bool isActive = i == activeTabIndex;

                Color origColor = GUI.backgroundColor;
                if (isActive)
                    GUI.backgroundColor = Color.gray;

                if (GUI.Button(new Rect(tabPosX, startY, tabWidth, tabHeight), tabs[i].Label, tabButtonStyle))
                {
                    activeTabIndex = i;
                }
                GUI.backgroundColor = origColor;
            }

            // Draw the active tab's table
            var activeTab = tabs[activeTabIndex];
            float tableX = startX;
            float tableY = startY + tabHeight + 16f;
            float tableWidth = 800f;
            float maxTableHeight = 600f;
            string sampleCell = activeTab.Lines.Count > 0
                ? activeTab.Lines.OrderByDescending(s => s.Length).First()
                : "Sample";
            float cellHeight = tableButtonStyle.CalcSize(new GUIContent(sampleCell)).y + 8f;
            float totalHeight = cellHeight * activeTab.Lines.Count;
            float scrollHeight = Mathf.Min(maxTableHeight, totalHeight);

            Rect tableRect = new Rect(tableX, tableY, tableWidth, scrollHeight);
            Rect innerRect = new Rect(0, 0, tableWidth - 20f, totalHeight);
            GUI.DrawTexture(tableRect, blackTexture);

            tableScrollPos = GUI.BeginScrollView(tableRect, tableScrollPos, innerRect);

            for (int i = 0; i < activeTab.Lines.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(activeTab.Lines[i])) continue;
                Rect lineRect = new Rect(0, i * cellHeight, innerRect.width, cellHeight);
                if (GUI.Button(lineRect, activeTab.Lines[i], tableButtonStyle))
                {
                    GUIUtility.systemCopyBuffer = activeTab.Lines[i];
                }
            }
            GUI.EndScrollView();
        }

        private void EnsureInitialized()
        {
            if (blackTexture == null)
            {
                blackTexture = new Texture2D(1, 1);
                blackTexture.SetPixel(0, 0, Color.black);
                blackTexture.Apply();
            }
            if (tabButtonStyle == null)
            {
                tabButtonStyle = new GUIStyle(GUI.skin.button) { fontSize = 20, wordWrap = false, margin = new RectOffset(4, 4, 0, 0), padding = new RectOffset(10, 10, 6, 6) };
                tabButtonStyle.normal.textColor = Color.white;
            }
            if (tableButtonStyle == null)
            {
                tableButtonStyle = new GUIStyle(GUI.skin.button) { fontSize = 18, wordWrap = true };
                tableButtonStyle.normal.textColor = Color.white;
            }
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label) { fontSize = 20 };
                labelStyle.normal.textColor = Color.white;
            }
        }
    }
}