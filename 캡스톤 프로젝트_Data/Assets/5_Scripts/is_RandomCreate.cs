using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class is_RandomCreate : MonoBehaviour
{
    public GameObject[] prefabs;
    public GameObject[] areas;

    public int count = 20;    //찍어낼 게임 오브젝트 갯수

    private List<GameObject> newObject = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < count; ++i)//count 수 만큼 생성한다
        {
            Spawn();//생성 + 스폰위치를 포함하는 함수
        }

    }

    private Vector3 GetRandomPosition()
    {
        int randArea = Random.Range(0, areas.Length);
        GameObject selectedarea = areas[randArea];

        MeshRenderer renderer = areas[randArea].GetComponent<MeshRenderer>();
        Vector3 size = renderer.bounds.size;

        Vector3 basePosition = selectedarea.transform.position;


        float posX = basePosition.x + Random.Range(-size.x / 2f, size.x / 2f);
        //float posY = basePosition.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posZ = basePosition.z + Random.Range(-size.z / 2f, size.z / 2f);

        Vector3 spawnPos = new Vector3(posX, 3, posZ);

        return spawnPos;
    }

    private GameObject selectPrefab()
    {
        int randPrepab = Random.Range(0, prefabs.Length);
        GameObject selectedPrefab = prefabs[randPrepab];

        return selectedPrefab;
    }

    private void Spawn()
    {
        Vector3 spawnPos = GetRandomPosition();
        GameObject selectedPrefab = selectPrefab();

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        instance.transform.parent = GameObject.Find("items").GetComponent<Transform>();// 묶어주기
        newObject.Add(instance);
        Debug.Log(newObject.Count);

        Debug.Log("seffsef");
    }


}
