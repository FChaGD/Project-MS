using GroupDefinition;
using System;
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
        fathring, //신환족=천사


    }

    [CreateAssetMenu(fileName = "CharacterDataTable", menuName = "Scriptable Objects/Character")]
    public class CharacterDataTable : ScriptableObject
    {
        public int characterId; //ID
        public float hpMax; //최대 체력
        public float attack_Point; //공격력
        public float mpMax; //최대 마나 포인트
        public float dpMax; //최대 신성력
        public float baseAttackSpeed; //공격 속도

        public RaceType baseRaceType; //종족
        public DependencyStyle baseRaceIsolation; //종족 배타성

        public Contry baseContry; //기본 국가
        public DependencyStyle basePatriotism; //기본 애국심

        public BaseGroup baseGroup; //기본 소속 단체 -> 플레이어 그룹 이외
        public DependencyStyle groupLoyalty; //기본 충성도 -> 플레이어 그룹 이외 

        public Religion baseReligion; //기본 종교
        public DependencyStyle baseFath; //기본 신앙 정도
        public DependencyStyle baseIsolation; //종교 기본 배타성
        
        public SocialSystem baseSystem; //기본 사회사상
        public DependencyStyle politicTendency; //정치 편향성
    }

    [CreateAssetMenu(fileName = "CharacterStringTable", menuName = "Scriptable Objects/Character")]
    public class CharacterStringTable : ScriptableObject
    {
        public string name; //이름
    }

    [CreateAssetMenu(fileName = "PlayableTable", menuName = "Scriptable Objects/Character")]
    public class PlayableTable : ScriptableObject
    {

    }

    public abstract class CharacterBase
    {
        public float curhp; //체력
        public float curmp; //마나 포인트
        public float curdp; //신성력


    }

    [Serializable]
    public class PlayableCharacter : CharacterBase
    {
        //캐릭터 상태 관리
        public float loyalty; //충성도
        public float dependency; //종속도
        public float relationship; //친밀도


        public int battlePosition; //전투 위치
        public int sanity; //정신력
        public int energy; //활력
        public float Tiredness; //피로도

        public float base_needed_food; //1회 음식 기본 소모량
        public float base_Speed; //캐릭터 월드 이동 속도
    }

}