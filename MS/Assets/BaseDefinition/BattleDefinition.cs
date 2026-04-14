using System;
using UnityEngine;

namespace BattleDefinition
{

    [Serializable]
    public class BattleContainer
    {
        public int round;
    }


    #region 전투 트리거 이벤트
    public class BattleTriggerEventBase
    {
        private Action trigger;
        public void TriggerGenerate()
        {
            trigger?.Invoke();
        }

        public void AddTrigger(Action addTarget)
        {
            trigger += addTarget;
        }
        public void RemoveTrigger(Action removeTarget)
        {
            trigger -= removeTarget;
        }
    }

    public class BattleStart : BattleTriggerEventBase
    {
    }

    public class BattleEnd : BattleTriggerEventBase
    {
    }

    public class RoundStart : BattleTriggerEventBase
    {
    }

    public class RoundEnd : BattleTriggerEventBase
    {
    }
    public class CombatStart : BattleTriggerEventBase
    {

    }
    public class CombatEnd : BattleTriggerEventBase
    {

    }
    public class TurnStart : BattleTriggerEventBase
    {

    }
    public class TurnEnd : BattleTriggerEventBase
    {
    }

        #endregion

}
