using System.Text;
using Kingmaker;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using UnityEngine;

namespace Purps.RogueTrader.Behaviours
{
    public class BuffOverlayBehaviour : MonoBehaviour
    {
        private readonly StringBuilder sb = new StringBuilder();

        public void Update()
        {
            sb.Clear();

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
                    if (buff.Blueprint.AssetGuid == Constants.PyromancyHeartOfMagmaTalentCounterBuff)
                    {
                        sb.AppendLine($"<color=red>{buff.Name} ({buff.Rank})</color>");
                    }
                    else
                    {
                        sb.AppendLine($"<color=yellow>{buff.Name} ({buff.Rank})</color>");
                    }
                }
            }
        }

        public void OnGUI()
        {
            GUI.Label(new Rect(20, 20, 1000, 40), $"<size=28>{sb.ToString()}</size>");
        }
    }
}