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



    }

    public abstract class CharacterBase
    {
        public int characterId; //ID
        public float hpMax; //최대 체력
        public float hp; //체력
        public float attack_Point; //공격력
        public float mpMax; //최대 마나 포인트
        public float mp; //마나 포인트
        public float attack_Speed; //공격 속도

        public string name; //이름
        public RaceType raceType; //종족

    }

    public class PlayableCharacter : CharacterBase
    {
        public float loyalty; //충성도
        public float dependency; //종속도
        public float relationship; //친밀도

        public float needed_food_at_once; //1회 음식 소모량
    }

    public struct PartyStruct
    {
        private PlayableCharacter character;
        private int battlePosition; //전투 위치
        private int sanity; //정신력
        private int energy; //활력
        private float Tiredness; //피로도
    }

}