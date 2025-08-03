using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;
using Purps.RogueTrader.API.Menu;
using Purps.RogueTrader.API.Unit;
using UnityEngine;
using static Kingmaker.UnitLogic.Parts.UnitPartBonusAbility;
using Color = UnityEngine.Color;

namespace Purps.RogueTrader.Behaviours
{
    public class CombatOverlayBehaviour : MonoBehaviour
    {
        private static readonly Dictionary<string, Buff> buffs = new Dictionary<string, Buff>();
        private static readonly Dictionary<string, BonusAbilityData> bonuses = new Dictionary<string, BonusAbilityData>();

        private Texture2D backgroundTexture;
        private GUIStyle labelStyle;

        public void Update()
        {
            buffs.Clear();
            CombatOverlayBehaviour.bonuses.Clear();

            BaseUnitEntity selectedUnit = RTUnit.GetSelectedUnit();

            if (RTMenu.IsOpen() || !RTUnit.IsInCombat(selectedUnit) || !selectedUnit.CanAct)
            {
                return;
            }

            var buff = RTUnit.GetBuff(Constants.PyromancyHeartOfMagmaTalentCounterBuff, selectedUnit);
            if (buff != null)
            {
                buffs[buff.Blueprint.AssetGuid] = buff;
            }

            List<BonusAbilityData> bonuses = RTUnit.GetBonusAbilities(new[] { Constants.PyromancyHeartOfMagmaTalentFeature, Constants.PyromancyHeartOfMagmaTalentChargeBuff }, selectedUnit);
            if (bonuses != null && bonuses.Count > 0)
            {
                foreach (var bonus in bonuses)
                {
                    CombatOverlayBehaviour.bonuses[bonus.Source.Blueprint.AssetGuid] = bonus;
                }
            }
        }

        public void OnGUI()
        {
            BaseUnitEntity selectedUnit = RTUnit.GetSelectedUnit();

            if (RTMenu.IsOpen() || !RTUnit.IsInCombat(selectedUnit) || !selectedUnit.CanAct)
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            if (RTUnit.HasFeature(Constants.PyromancyHeartOfMagmaTalentFeature, selectedUnit))
            {
                sb.Append($"<color=white>Fire Within ({(buffs.TryGetValue(Constants.PyromancyHeartOfMagmaTalentCounterBuff, out var buff) ? buff?.Rank ?? 0 : 0)})</color> ");

                var keysToCheck = new[] {
                    Constants.PyromancyHeartOfMagmaTalentFeature,
                    Constants.PyromancyHeartOfMagmaTalentChargeBuff
                };

                int enabledCount = keysToCheck.Count(key => bonuses.ContainsKey(key));

                string color = enabledCount > 0 ? "green" : "red";
                string statusText = enabledCount > 0 ? $"Enabled ({enabledCount})" : "Disabled";

                sb.AppendLine($"<color={color}>{statusText}</color>");
            }

            EnsureInitialized();

            string richText = sb.ToString();
            Vector2 size = labelStyle.CalcSize(new GUIContent(richText));
            Rect rect = new Rect(20, 20, size.x, 45);

            GUI.Label(rect, sb.ToString(), labelStyle);
        }

        private void EnsureInitialized()
        {
            if (backgroundTexture == null)
            {
                backgroundTexture = new Texture2D(1, 1);
                backgroundTexture.SetPixel(0, 0, new Color(0, 0, 0, 0.75f)); // semi-transparent black
                backgroundTexture.Apply();
            }

            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.box)
                {
                    fontSize = 32,
                    richText = true,
                    padding = new RectOffset(10, 10, 5, 5),
                    normal = { background = backgroundTexture }
                };
            }
        }
    }
}