using System;
using System.Collections.Generic;
using System.Text;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Code.UI.MVVM;
using Kingmaker.Code.UI.MVVM.VM.EscMenu;
using Kingmaker.Code.UI.MVVM.VM.ServiceWindows;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;
using Purps.RogueTrader.Logging;
using UnityEngine;
using static Kingmaker.UnitLogic.Parts.UnitPartBonusAbility;

namespace Purps.RogueTrader.Behaviours
{
    public class OverlayBehaviour : MonoBehaviour
    {
        private static readonly List<Buff> buffs = new List<Buff>();
        private static readonly List<EntityFactSource> bonuses = new List<EntityFactSource>();

        private bool isInCombat = false;

        public void Update()
        {
            buffs.Clear();
            bonuses.Clear();
            isInCombat = false;

            if (!ShouldDrawOverlay())
            {
                return;
            }

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
                TrackCombatStuff(entity);
            }
        }

        private bool ShouldDrawOverlay()
        {
            return
            !RootUIContext.Instance.IsBlockedFullScreenUIType()
            && RootUIContext.Instance.CurrentServiceWindow == ServiceWindowsType.None
            && RootUIContext.Instance.FullScreenUIType == Kingmaker.UI.Models.FullScreenUIType.Unknown
            && !RootUIContext.Instance.GroupChangerIsShown
            && !RootUIContext.Instance.IsInventoryShow
            && !RootUIContext.Instance.IsMainMenu
            && !RootUIContext.Instance.IsLoadingScreen
            && !RootUIContext.Instance.IsCharInfoAbilitiesChooseMode
            && !RootUIContext.Instance.IsCharInfoLevelProgression
            ;
        }

        private void TrackCombatStuff(BaseUnitEntity entity)
        {
            isInCombat = entity.IsInCombat;
            if (!isInCombat)
            {
                return;
            }

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

        public void OnGUI()
        {
            StringBuilder sb = new StringBuilder();

            DisplayCombatStuff(sb);

            GUI.Label(new Rect(20, 20, 1000, 40), $"<size=32>{sb}</size>");
        }

        private void DisplayCombatStuff(StringBuilder sb)
        {
            if (!isInCombat)
            {
                return;
            }

            var fireWithinBlueprint = buffs.Find(buff => buff.Blueprint.AssetGuid == Constants.PyromancyHeartOfMagmaTalentCounterBuff);
            sb.Append($"<color=white>Fire Within ({fireWithinBlueprint?.Rank ?? 0})</color> ");

            var bonusEnabled = bonuses.Exists(bonus =>
                bonus.Blueprint.AssetGuid == Constants.PyromancyHeartOfMagmaTalentFeature ||
                bonus.Blueprint.AssetGuid == Constants.PyromancyHeartOfMagmaTalentChargeBuff);
            sb.AppendLine($"<color={(bonusEnabled ? "green" : "red")}>{(bonusEnabled ? "Enabled" : "Disabled")}</color>");
        }
    }
}