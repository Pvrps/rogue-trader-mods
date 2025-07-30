using System.Collections.Generic;
using System.Text;
using Kingmaker.UnitLogic.Buffs;
using Purps.RogueTrader.API.Menu;
using Purps.RogueTrader.API.Unit;
using UnityEngine;
using static Kingmaker.UnitLogic.Parts.UnitPartBonusAbility;

namespace Purps.RogueTrader.Behaviours
{
    public class CombatOverlayBehaviour : MonoBehaviour
    {
        private static readonly Dictionary<string, Buff> buffs = new Dictionary<string, Buff>();
        private static readonly Dictionary<string, BonusAbilityData> bonuses = new Dictionary<string, BonusAbilityData>();

        public void Update()
        {
            buffs.Clear();
            bonuses.Clear();

            if (RTMenu.IsOpen() || !RTUnit.IsInCombat())
            {
                return;
            }

            var buff = RTUnit.GetBuff(Constants.PyromancyHeartOfMagmaTalentCounterBuff);
            if (buff != null)
            {
                buffs[buff.Blueprint.AssetGuid] = buff;
            }

            var bonus = RTUnit.GetBonusAbility(Constants.PyromancyHeartOfMagmaTalentFeature) ??
                        RTUnit.GetBonusAbility(Constants.PyromancyHeartOfMagmaTalentChargeBuff);
            if (bonus != null)
            {
                bonuses[bonus.Source.Blueprint.AssetGuid] = bonus;
            }
        }

        public void OnGUI()
        {
            if (RTMenu.IsOpen() || !RTUnit.IsInCombat())
            {
                return;
            }

            StringBuilder sb = new StringBuilder();

            if (RTUnit.HasFeature(Constants.PyromancyHeartOfMagmaTalentFeature))
            {
                sb.Append($"<color=white>Fire Within ({(buffs.TryGetValue(Constants.PyromancyHeartOfMagmaTalentCounterBuff, out var buff) ? buff?.Rank ?? 0 : 0)})</color> ");

                var bonusEnabled = bonuses.ContainsKey(Constants.PyromancyHeartOfMagmaTalentFeature)
                                || bonuses.ContainsKey(Constants.PyromancyHeartOfMagmaTalentChargeBuff);
                sb.AppendLine($"<color={(bonusEnabled ? "green" : "red")}>{(bonusEnabled ? "Enabled" : "Disabled")}</color>");
            }

            GUI.Label(new Rect(20, 20, 1000, 40), $"<size=32>{sb}</size>");
        }
    }
}