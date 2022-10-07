using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // Start is called before the first frame update
    public static ObjectPool instance;
    private void Awake()
    {
        instance = this;
    }

    public int maxCount = 10;

    //전체 목록
    public Dictionary<string, List<GameObject>> dic;
    //비활성화 목록
    public Dictionary<string, List<GameObject>> deactiveDic;

    //총알을 쏠 때 마다 List 전부 탐색하는 것은 효율적이지 못하기 때문에 쏠 수 있는 상태의 총알 (비활성화 상태)만 다로 list에 담아 list[0]번을 쓰고 Active 상태가 되면 삭제 해 줌
    // 삭제 했을 때 list 1번이 list 0번으로 인덱스가 바뀌게 되므로 list가 비워질 때까지 계속 list 0번을 가져다가 쓰는 식으로 구현하면 탐색하지 않아도 됌
    // class로 구현된 것들을 list에 넣을 때  value (x) , reference(o) 주솟값을 가지고 있음
    // GameObject go = instatiate(factory);
    // list1, list 2가 있을 때 
    // GameObject go = Instatiate();
    // list1.Add(go);
    // list2.Add(go);
    // 하게 되면 둘다 같은 객체의 주솟값을 가지고 있음 객체는 1개
    // 크기 : 배열 array.lengths , list 는 list.count
    public void CreateInstance(string prefabName)
    {
        GameObject factory = Resources.Load<GameObject>(prefabName);
        
        string key = prefabName;

        if (dic != null && dic.ContainsKey(key))
        {
            //함수 바로 종료
            return;
        }

        if (dic == null)
        {
            dic = new Dictionary<string, List<GameObject>>();
            deactiveDic = new Dictionary<string, List<GameObject>>();
        }

        //  dic = new Dictionary<string, List<GameObject>>();

        dic.Add(key, new List<GameObject>());
        deactiveDic.Add(key, new List<GameObject>());

        //key 값이 Zipline일 경우 ObjectPool 한개만 생성
        if (key == "Zipline")
        {
            maxCount = 2;
        }
        for (int i = 0; i < maxCount; i++)
        {
            GameObject go = Instantiate(factory);
            go.name = key;
            // go.name.Replace("(Clone)", "");
            go.SetActive(false);
            dic[key].Add(go);
            deactiveDic[key].Add(go);
        }
        // 형변환
        //(GameObject)Resources.Load(name);
        //Resources.Load<GameObject>(name);
        //Resources.Load(name) as Gameobject;

        //상속 받은 클래스로는 형변환 가능
    }
    //list에 들어있는 게임 오브젝트중에 비활성화 된것 있으면 그것을 반환 하고 싶다.

    public GameObject GetDeactiveObject(string key)
    {
        //deactiveDic 딕셔너리에 아무것도 없다면
        if (deactiveDic[key].Count == 0)
        {
            //Resources 폴더에서 key이름으로 된 prefep을 가져와서 factory에 저장.
            GameObject factory = Resources.Load<GameObject>(key);

            // factory에 저장 된 프리펩을 인스턴스 화 해서 ggg에 저장
            GameObject ggg = Instantiate(factory);
            
            // ggg 게임오브젝트에 이름을 key 값(읽어온 프리펩 이름)으로 이름을 달아줌
            ggg.name = key;
            // go.name.Replace("(Clone)", "");

            //만든 인스턴스의 Active를 꺼줌
            ggg.SetActive(false);
            // 꺼지든 켜지든 상관없는 딕셔너리에 만든 인스턴스 ggg를 추가
            dic[key].Add(ggg);
            // ggg리턴 하고 종료
            return ggg;
        }
        // 비활성화된 오브젝트 딕셔너리의 키값에 해당하는 리스트의 첫번째 값을 go에 저장하고
        GameObject go = deactiveDic[key][0];
        //deactiveDic[key]의 첫번째 인스턴스를 딕셔너리에서 제거
        deactiveDic[key].RemoveAt(0);
        //deactiveDic.Remove(go); 
        //꺼내온 인스턴스 리턴
        return go;
    }
    internal GameObject GetDeactiveObjectOld(string key)
    {
        for (int i = 0; i < maxCount; i++)
        {
            // 만약 list[i]이 비활성화 되어있다면
            if (false == dic[key][i].activeSelf)
                // 반환하고싶다.
                return dic[key][i];
        }
        return null;
    }

    /*
     public int[] a = new int[5];
           for (int i = 0; i<a.Length; i++)
       {
           a[i] = 0;
       }
*/





    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
