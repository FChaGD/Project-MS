using CharacterDefinition;
using DataDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BattleDefinition
{
    /// <summary>
    /// BattleController와 하위 컴포넌트에서 사용될 정보 컨테이너 클래스
    /// </summary>
    [Serializable]
    public class BattleContainer
    {
        public int round;
        public List<PlayableCharacter> heroList;
        public List<EnemyCharacter> enemyList;
        public BattleFieldManager battleFieldManager;
    }

    /// <summary>
    /// 로직 사이 배틀 정보를 전달하는 컨텍스트 구조체
    /// </summary>
    public readonly struct BattleContext
    {
        public readonly IReadOnlyList<CharacterBase> Allies;
        public readonly IReadOnlyList<CharacterBase> Enemies;
        public readonly int RoundCount;

        public BattleContext(IReadOnlyList<CharacterBase> allies,
                              IReadOnlyList<CharacterBase> enemies,
                              int roundCount)
        {
            Allies = allies; Enemies = enemies; RoundCount = roundCount;
        }
    }



    public interface IBattleFunc
    {
        void BattleStartSetting();
    }

    public struct BattleOrderContainer
    {
        public CharacterBase orderCharacter;
        public String orderAction;
    }

    #region 스킬 관련
    #region 베이스 조상 클래스 정의
    public abstract class SkillBase : IIdentifiable
    {
        private int _id;
        public int id => _id;
    }

    public abstract class PassiveSkillBase : SkillBase
    {

        public abstract void OnEquip();
        public abstract void OnUnequip();
    }

    public abstract class ActiveSkillBase : SkillBase
    {
        protected ITargetPattern TargetPattern { get; }
        public abstract bool CanUse();
        public abstract void Execute();
    }
    #endregion

    #region 액티브 타겟 결정 유형 
    public interface ITargetPattern
    {
        // 시전자 위치, 스킬이 조준한 지점, 대상 진형을 받아
        // 실제로 맞는 좌표 목록을 계산해서 반환
        IReadOnlyList<Vector2Int> Resolve(Vector2Int casterPos, Vector2Int aimedPos);
    }

    public class SingleTargetPattern : ITargetPattern
    {
        public IReadOnlyList<Vector2Int> Resolve(Vector2Int casterPos, Vector2Int aimedPos)
            => new[] { aimedPos };
    }

    public class RowTargetPattern : ITargetPattern // 가로 한 줄 (예: 전열 전체)
    {
        public IReadOnlyList<Vector2Int> Resolve(Vector2Int casterPos, Vector2Int aimedPos)
            => Enumerable.Range(0, 3).Select(c => new Vector2Int(aimedPos.x, c)).ToList();
    }

    public class ColumnTargetPattern : ITargetPattern // 세로 한 줄 (관통형 공격 등)
    {
        public IReadOnlyList<Vector2Int> Resolve(Vector2Int casterPos, Vector2Int aimedPos)
            => Enumerable.Range(0, 3).Select(r => new Vector2Int(r, aimedPos.y)).ToList();
    }

    public class CrossTargetPattern : ITargetPattern // 십자 범위
    {
        public IReadOnlyList<Vector2Int> Resolve(Vector2Int casterPos, Vector2Int aimedPos) =>
            new[] { aimedPos,
                 new Vector2Int(aimedPos.x, aimedPos.y-1),
                 new Vector2Int(aimedPos.x, aimedPos.y+1),
                 new Vector2Int(aimedPos.x-1, aimedPos.y),
                 new Vector2Int(aimedPos.x+1, aimedPos.y) }
            .Where(IsValid).ToList();

        private bool IsValid(Vector2Int p) => p.x is >= 0 and < 3 && p.y is >= 0 and < 3;
    }

    public class AllTargetPattern : ITargetPattern // 광역 전체
    {
        public IReadOnlyList<Vector2Int> Resolve(Vector2Int casterPos, Vector2Int aimedPos)
        { return new[] { new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2),
                       new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2),
                       new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2) }; }
        
    }
    #endregion

    #endregion

    #region 전투 트리거 이벤트
    public static class BattleEvent<T> where T : BattleTriggerEventBase
    {
        private static Action startTrigger;
        private static Action middletrigger;
        private static Action endTrigger;
        public static void Publish()
        {
            startTrigger?.Invoke();
            middletrigger?.Invoke();
            endTrigger?.Invoke();
        }

        public static void SubscribeStart(Action addTarget)
        {
            startTrigger += addTarget;
        }
        public static void SubscribeMiddle(Action addTarget)
        {
            middletrigger += addTarget;
        }
        public static void SubscribeEnd(Action addTarget)
        {
            endTrigger += addTarget;
        }

        public static void Unsubscribe(Action removeTarget)
        {
            startTrigger -= removeTarget;
            middletrigger -= removeTarget;
            endTrigger -= removeTarget;
        }
    }
    //BattleStart
    //└─ RoundStart
    //   └─ TurnStart
    //         └─ CombatStart
    //         └─ CombatEnd
    //    └─ TurnEnd
    //  └─ RoundEnd
    //BattleEnd
    public class BattleTriggerEventBase
    {

    }

    public sealed class BattleStart : BattleTriggerEventBase
    {
    }

    public sealed class BattleEnd : BattleTriggerEventBase
    {
    }

    public sealed class RoundStart : BattleTriggerEventBase
    {
    }

    public sealed class RoundEnd : BattleTriggerEventBase
    {
    }
    public sealed class CombatStart : BattleTriggerEventBase
    {

    }
    public sealed class CombatEnd : BattleTriggerEventBase
    {

    }
    public sealed class TurnStart : BattleTriggerEventBase
    {

    }
    public sealed class TurnEnd : BattleTriggerEventBase
    {
    }

    #endregion

}
