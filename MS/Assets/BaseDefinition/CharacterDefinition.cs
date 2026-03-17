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
        private int characterId;
        private float hp;
        private float attack_Point;
        private float mp;
        private float attack_Speed;

        private string name;
        private RaceType raceType;

    }

    public class PlayableCharacter : CharacterBase
    {
        private float loyalty;
        private float dependency;
        private float relationship;

        private float needed_food_at_once;
    }

    struct PartyStruct
    {
        private PlayableCharacter character;
        private int battlePosition;
        private int sanity;
        private int energy;
        private float Tiredness;
    }

}