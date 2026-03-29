using System.Text;
using UnityEngine;

namespace GroupDefinition
{
    public abstract class BaseGroup : ScriptableObject
    {
        public StringBuilder groupName;
        public GroupType groupType;
    }

    public class Contry : BaseGroup
    {
    }

    public abstract class Admin : BaseGroup
    {
        public Contry belongingContry; //소속 국가

    }

    public class Goverment : Admin
    {

    }

    public class CityHall : Admin
    {

    }

    public class Religion : BaseGroup
    {

    }

    public class Merchant : BaseGroup
    {

    }

    public class Mercenary : BaseGroup
    {

    }

    public class Adventurer : BaseGroup
    {

    }

    public class Crime : BaseGroup
    {

    }



    public enum GroupType
    {
        None,
        Goverment, //정부 조직
        Admin, //지방 행정조직
        Religion, //종교조직
        Merchant, //상단조직
        Mercenary, //용병조직
        Adventurer, //탐험 조직
        Crime, //범죄조직

    }

    //의존 수준, 신앙 정도, 배타성, 정치 배타성 등등
    public enum DependencyStyle
    {
        VeryLow = 0, //매우 낮음
        Low, //낮음
        LowMiddle, //약간 낮음
        Middle, //중간
        MiddleHigh, //약간 높음
        High, //높음
        VeryHigh, //매우 높음
    }


    public enum SocialSystem
    {
        None = 0,
        Kingdom, // 군주제
        Feudal, // 봉건제
        Republic, //공화제
        Military, //군정
        Anarchy, // 무정부주의
        Socialism, //사회주의
        Theocracy, //신권주의
        Traditional, //전통주의

    }

}
