using FieldDefinition;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DataDefinition
{

    #region 데이터 스크립터블 오브젝트 및 컨테이너 정의

    // Id를 보장하는 인터페이스
    public interface IIdentifiable
    {
        public int id { get; }
    }

    public abstract class CustomBaseScriptableObject : ScriptableObject, IIdentifiable
    {
        [SerializeField] private int _id;
        public int id => _id;  // IIdentifiable 구현
    }

    //커스텀 데이터 컨테이너 클래스 정의
    public abstract class BaseDataContainer<T> where T : ScriptableObject, IIdentifiable
    {

        public static List<T> dataList;

        protected static Dictionary<int, T> _cache;

        public T data;

        //필요한 딕셔너리 구축
        public void Initialize(int id)
        {
            //캐시가 할당되지 않았다면 new로 초기화
            if (_cache == null)
                _cache = new Dictionary<int, T>();

            //데이터 원본이 할당되지 않았다면 new로 초기화 후 데이터 로드
            if (dataList == null)
            {
                dataList = new List<T>();
#warning [캐릭터] 테이블에서 데이터를 불러오는 기능 추가 필요
            }

            // 이미 캐싱된 경우 스킵
            if (_cache.ContainsKey(id)) return;

            //데이터 원본이 비어있다면 경고 로그 출력 후 종료
            if (dataList.Count == 0)
            {
                Debug.LogWarning($"데이터 원본이 비어있습니다. {typeof(T).Name} 데이터를 로드할 수 없습니다.");
                return;
            }

            //데이터 받아오기
            data = dataList.FirstOrDefault(d => d.id == id);

            if (data != null)
                _cache[id] = data;
            else
                Debug.LogWarning($"ID {id} 에 해당하는 {typeof(T).Name}이 없습니다.");
        }

        //ID로 캐릭터 데이터 조회
        public T GetById(int id)
        {
            //캐시에서 조회, 없으면 초기화
            if (_cache == null || !_cache.ContainsKey(id))
                Initialize(id);

            _cache.TryGetValue(id, out var result);
            return result;
        }

    }
    #endregion



    // 필드 데이터 집합 클래스
    public class FieldData
    {
        private bool hasRoad;
        private bool canPass;
        private TileType tileType;
        private string[] eventList;
        private InfluenceData areaData;

    }


}
