using GroupDefinition;
using System;
using DataDefinition;
using UnityEngine;

namespace CharacterDefinition
{
    public enum RaceType
    {
        Human, //인간족
        Dragon, //용인족
        Beast, //수인족
        Long_Ear, //장이족=엘프
        Iron_Hander, //제장족=드워프
        Vampir, //취혈족=벰파이어
        Fathring, //신환족=천사
        Succuber, //몽열족=서큐버스(몽마)
        Sprit, //정령족
        Seaborn, //어인족
        Giant, //거인족


    }
    }
    
    public class CharacterDataContainer<T> : BaseDataContainer<T> where T : CharacterScriptableObject, IIdentifiable
    {

    }

    #region 캐릭터 스크립터블 테이블 정의

    public abstract class CharacterScriptableObject : CustomBaseScriptableObject
    {
    }
    #region 종합 캐릭터 테이블 정의

    [CreateAssetMenu(fileName = "CharacterTable", menuName = "Scriptable Objects/Character/CharacterTable")]
    public class CharacterTable : CharacterScriptableObject
    {
        public CharacterData characterData; //캐릭터 데이터
        public CharacterStringData characterStringData; //캐릭터 String 데이터
    }

    #endregion

    #region 캐릭터 데이터 스크립터블 테이블 정의

    [Serializable]
    public class CharacterData
    {

        public float hpMax; //최대 체력
        public float attack_Point; //공격력
        public float mpMax; //최대 마나 포인트
        public float dpMax; //최대 신성력
        public float battleSpeed; //전투 속도
        public float baseAttackSpeed; //공격 속도
        public float sanityMax; //정신력 최대치
        public float energyMax; //활력 최대치

        public RaceType baseRaceType; //종족
        public DependencyStyle baseRaceIsolation; //종족 배타성

        public Contry baseContry; //기본 국가
        public DependencyStyle basePatriotism; //기본 애국심

        public BaseGroup baseGroup; //기본 소속 단체 -> 플레이어 그룹 이외
        public DependencyStyle groupLoyalty; //기본 충성도 -> 플레이어 그룹 이외 

        public Religion baseReligion; //기본 종교
        public DependencyStyle baseFath; //기본 신앙 정도

        public SocialSystem baseSystem; //기본 사회사상
        public DependencyStyle politicTendency; //정치 편향성
    }

    [CreateAssetMenu(fileName = "CharacterDataTable", menuName = "Scriptable Objects/Character/CharacterDataTable")]
    public class CharacterDataTable : CharacterScriptableObject
    {
        public CharacterData characterData; //캐릭터 데이터
    }
    #endregion

    #region 캐릭터 String 스크립터블 테이블 정의

    [Serializable]
    public class CharacterStringData
    {
        public string name; //이름
        public string description; //설명
    }

    [CreateAssetMenu(fileName = "CharacterStringTable", menuName = "Scriptable Objects/Character/CharacterStringTable")]
    public class CharacterStringTable : CharacterScriptableObject
    {
        public CharacterStringData characterStringData; //캐릭터 String 데이터
    }
    #endregion

    #region 플레이어블 캐릭터 데이터 스크립터블 테이블 정의

    [Serializable]
    public class PlayableData
    {
        public float base_needed_food; //1회 음식 기본 소모량
        public float world_Speed; //캐릭터 월드 이동 속도
    }
    [CreateAssetMenu(fileName = "PlayableDataTable", menuName = "Scriptable Objects/Character/PlayableDataTable")]
    public class PlayableDataTable : CharacterDataTable
    {
        public PlayableData playableData; //플레이어블 캐릭터 데이터
    }

    #endregion

    #endregion

    public abstract class CharacterBase
    {
        public CharacterDataTable characterDataTable; //캐릭터 데이터 테이블
        public CharacterStringTable characterStringTable; //캐릭터 String 테이블

        public int id; //캐릭터 고유 아이디
        public float curhp; //체력
        public float curmp; //마나 포인트
        public float curdp; //신성력

        //캐릭터 상태 관리
        public float loyalty; //충성도
        public float dependency; //종속도
        public float relationship; //친밀도


        /// <summary>
        /// 0: 배치 없음, 1~9:배치
        /// </summary>
        public int battlePosition; //전투 위치
        public float curSanity; //정신력
        public float curEnergy; //활력


    }

    [Serializable]
    public class PlayableCharacter : CharacterBase
    {
        protected PlayableDataTable playableTable; //플레이어블 캐릭터 데이터 테이블

        public PlayableCharacter(int id)
        {
            this.id = id;
            Init();
        }

        private void Init()
        {


        }

    }

    [Serializable]
    public class EnemyCharacter : CharacterBase
    {

        public EnemyCharacter(int id)
        {
            this.id = id;
            Init();
        }
        private void Init()
        {
        }
    }

}