using System;
using System.Collections.Generic;
using System.Text;
using Kingmaker;
using Kingmaker.Code.UI.MVVM;
using Kingmaker.Code.UI.MVVM.VM.ServiceWindows;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Parts;
using UnityEngine;
using static Kingmaker.UnitLogic.Parts.UnitPartBonusAbility;

namespace Purps.RogueTrader.Behaviours
{
    public class DebugOverlayBehaviour : MonoBehaviour
    {

        private Texture2D blackTexture;
        private GUIStyle featureStyle;
        private GUIStyle labelStyle;
        private Vector2 scrollPos;

        private static readonly StringBuilder featuresSb = new StringBuilder();

        public void Awake()
        {
            if (blackTexture == null)
            {
                blackTexture = new Texture2D(1, 1);
                blackTexture.SetPixel(0, 0, Color.black);
                blackTexture.Apply();
            }

            featureStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 20,
                wordWrap = true
            };
            featureStyle.normal.textColor = Color.white;

            labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20
            };
            labelStyle.normal.textColor = Color.white;
        }

        public void Update()
        {
            featuresSb.Clear();

            BaseUnitEntity entity = null;
            Game instance = Game.Instance;
            if (instance != null)
            {
                PersistentState state = instance.State;
                if (state != null)
                {
                    EntityPool<BaseUnitEntity> allBaseUnits = state.AllBaseUnits;

                    foreach (BaseUnitEntity baseUnit in allBaseUnits)
                    {
                        if (baseUnit.IsDirectlyControllable && baseUnit.IsSelected)
                        {
                            entity = baseUnit;
                            break;
                        }
                    }
                }
            }

            if (entity != null)
            {
                foreach (var feature in entity.Progression.Features)
                {
                    string name = feature.Blueprint.Name;
                    featuresSb.AppendLine($"{(string.IsNullOrEmpty(name) ? "XXXXXXXX" : name)} ({feature.Blueprint.AssetGuid})");
                }
            }
        }

        public void OnGUI()
        {
            DrawFeatures();
        }

        private void DrawFeatures()
        {
            // Calculate label size dynamically
            string labelText = "Features";
            Vector2 labelSize = labelStyle.CalcSize(new GUIContent(labelText));
            float labelX = 20f;
            float labelY = 20f;
            float labelWidth = labelSize.x;
            float labelHeight = labelSize.y;

            // Draw the label
            GUI.Label(new Rect(labelX, labelY, labelWidth, labelHeight), labelText, labelStyle);

            // Prepare feature list
            string[] lines = featuresSb.ToString().Split('\n');
            float lineHeight = featureStyle.CalcSize(new GUIContent("Sample")).y + 8f; // Add a bit of padding

            // Position scroll area directly below label
            float scrollX = labelX;
            float scrollY = labelY + labelHeight + 10f; // 10px padding below label
            float scrollWidth = 800f; // Or Screen.width - 40f for dynamic width
            float maxScrollHeight = 600f; // Or Screen.height - scrollY - 20f

            float totalScrollHeight = lineHeight * lines.Length;
            float scrollHeight = Mathf.Min(maxScrollHeight, totalScrollHeight);

            Rect outerRect = new Rect(scrollX, scrollY, scrollWidth, scrollHeight);
            Rect innerRect = new Rect(0, 0, scrollWidth - 20f, totalScrollHeight);

            GUI.DrawTexture(outerRect, blackTexture);

            scrollPos = GUI.BeginScrollView(outerRect, scrollPos, innerRect);

            for (int i = 0; i < lines.Length; i++)
            {
                Rect lineRect = new Rect(0, i * lineHeight, innerRect.width, lineHeight);
                if (GUI.Button(lineRect, lines[i], featureStyle))
                {
                    GUIUtility.systemCopyBuffer = lines[i];
                    Debug.Log("Copied GUID: " + lines[i]);
                }
            }

            GUI.EndScrollView();
        }
    }
}