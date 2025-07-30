using System.Collections.Generic;
using System.Linq;
using Kingmaker;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Parts;
using static Kingmaker.UnitLogic.Parts.UnitPartBonusAbility;

namespace Purps.RogueTrader.API.Unit
{
    public static class RTUnit
    {
        public static BaseUnitEntity GetSelectedUnit()
        {
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
                            return baseUnit;
                        }
                    }
                }
            }

            return null;
        }

        public static bool HasFeature(string guid, BaseUnitEntity unit = null)
        {
            return (unit ?? GetSelectedUnit())?
                .Progression
                .Features
                .Enumerable
                .Exists(feature => feature.Blueprint.AssetGuid == guid) ?? false;
        }

        public static bool IsInCombat(BaseUnitEntity unit = null)
        {
            return (unit ?? GetSelectedUnit())?.IsInCombat ?? false;
        }

        public static Buff GetBuff(string guid, BaseUnitEntity unit = null)
        {
            return (unit ?? GetSelectedUnit())?.Buffs.Enumerable.FirstOrDefault(buff => buff.Blueprint.AssetGuid == guid) ?? null;
        }

        public static List<BonusAbilityData> GetBonusAbilities(BaseUnitEntity unit = null)
        {
            UnitPartBonusAbility bonusAbility = (unit ?? GetSelectedUnit()).GetBonusAbilityUseOptional();
            var field = typeof(UnitPartBonusAbility).GetField("m_Bonuses", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field.GetValue(bonusAbility) as List<BonusAbilityData>;
        }

        public static BonusAbilityData GetBonusAbility(string guid, BaseUnitEntity unit = null)
        {
            foreach (var bonus in GetBonusAbilities(unit))
            {
                if (bonus.Source.Blueprint.AssetGuid == guid)
                {
                    return bonus;
                }
            }

            return null;
        }
    }
}