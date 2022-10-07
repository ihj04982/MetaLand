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

    //��ü ���
    public Dictionary<string, List<GameObject>> dic;
    //��Ȱ��ȭ ���
    public Dictionary<string, List<GameObject>> deactiveDic;

    //�Ѿ��� �� �� ���� List ���� Ž���ϴ� ���� ȿ�������� ���ϱ� ������ �� �� �ִ� ������ �Ѿ� (��Ȱ��ȭ ����)�� �ٷ� list�� ��� list[0]���� ���� Active ���°� �Ǹ� ���� �� ��
    // ���� ���� �� list 1���� list 0������ �ε����� �ٲ�� �ǹǷ� list�� ����� ������ ��� list 0���� �����ٰ� ���� ������ �����ϸ� Ž������ �ʾƵ� ��
    // class�� ������ �͵��� list�� ���� ��  value (x) , reference(o) �ּڰ��� ������ ����
    // GameObject go = instatiate(factory);
    // list1, list 2�� ���� �� 
    // GameObject go = Instatiate();
    // list1.Add(go);
    // list2.Add(go);
    // �ϰ� �Ǹ� �Ѵ� ���� ��ü�� �ּڰ��� ������ ���� ��ü�� 1��
    // ũ�� : �迭 array.lengths , list �� list.count
    public void CreateInstance(string prefabName)
    {
        GameObject factory = Resources.Load<GameObject>(prefabName);
        
        string key = prefabName;

        if (dic != null && dic.ContainsKey(key))
        {
            //�Լ� �ٷ� ����
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

        //key ���� Zipline�� ��� ObjectPool �Ѱ��� ����
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
        // ����ȯ
        //(GameObject)Resources.Load(name);
        //Resources.Load<GameObject>(name);
        //Resources.Load(name) as Gameobject;

        //��� ���� Ŭ�����δ� ����ȯ ����
    }
    //list�� ����ִ� ���� ������Ʈ�߿� ��Ȱ��ȭ �Ȱ� ������ �װ��� ��ȯ �ϰ� �ʹ�.

    public GameObject GetDeactiveObject(string key)
    {
        //deactiveDic ��ųʸ��� �ƹ��͵� ���ٸ�
        if (deactiveDic[key].Count == 0)
        {
            //Resources �������� key�̸����� �� prefep�� �����ͼ� factory�� ����.
            GameObject factory = Resources.Load<GameObject>(key);

            // factory�� ���� �� �������� �ν��Ͻ� ȭ �ؼ� ggg�� ����
            GameObject ggg = Instantiate(factory);
            
            // ggg ���ӿ�����Ʈ�� �̸��� key ��(�о�� ������ �̸�)���� �̸��� �޾���
            ggg.name = key;
            // go.name.Replace("(Clone)", "");

            //���� �ν��Ͻ��� Active�� ����
            ggg.SetActive(false);
            // ������ ������ ������� ��ųʸ��� ���� �ν��Ͻ� ggg�� �߰�
            dic[key].Add(ggg);
            // ggg���� �ϰ� ����
            return ggg;
        }
        // ��Ȱ��ȭ�� ������Ʈ ��ųʸ��� Ű���� �ش��ϴ� ����Ʈ�� ù��° ���� go�� �����ϰ�
        GameObject go = deactiveDic[key][0];
        //deactiveDic[key]�� ù��° �ν��Ͻ��� ��ųʸ����� ����
        deactiveDic[key].RemoveAt(0);
        //deactiveDic.Remove(go); 
        //������ �ν��Ͻ� ����
        return go;
    }
    internal GameObject GetDeactiveObjectOld(string key)
    {
        for (int i = 0; i < maxCount; i++)
        {
            // ���� list[i]�� ��Ȱ��ȭ �Ǿ��ִٸ�
            if (false == dic[key][i].activeSelf)
                // ��ȯ�ϰ�ʹ�.
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
