using System;
using System.Collections.Generic;
using System.Text;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;
using UnityEngine;
using static Kingmaker.UnitLogic.Parts.UnitPartBonusAbility;

namespace Purps.RogueTrader.Behaviours
{
    public class DebugOverlayBehaviour : MonoBehaviour
    {
        private static readonly List<Buff> buffs = new List<Buff>();
        private static readonly List<EntityFactSource> bonuses = new List<EntityFactSource>();



        public void Update()
        {
            buffs.Clear();
            bonuses.Clear();

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
                foreach (var buff in entity.Buffs)
                {
                    buffs.Add(buff);
                }

                UnitPartBonusAbility bonusAbility = entity.GetBonusAbilityUseOptional();
                var field = typeof(UnitPartBonusAbility).GetField("m_Bonuses", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                foreach (var bonus in field.GetValue(bonusAbility) as List<BonusAbilityData>)
                {
                    bonuses.Add(bonus.Source);
                }


            }
        }

        public void OnGUI()
        {
            StringBuilder sb = new StringBuilder();

            TrackFireWithin(sb);

            GUI.Label(new Rect(20, 20, 1000, 40), $"<size=32>{sb}</size>");
        }

        private void TrackFireWithin(StringBuilder sb)
        {
            var fireWithinBlueprint = buffs.Find(b => b.Blueprint.AssetGuid == Constants.PyromancyHeartOfMagmaTalentCounterBuff);
            sb.Append($"<color=white>Fire Within ({fireWithinBlueprint?.Rank ?? 0})</color> ");

            var bonusEnabled = bonuses.Exists(b => b.Blueprint.AssetGuid == Constants.PyromancyHeartOfMagmaTalentFeature);
            sb.AppendLine($"<color={(bonusEnabled ? "green" : "red")}>{(bonusEnabled ? "Enabled" : "Disabled")}</color>");
        }
    }
}