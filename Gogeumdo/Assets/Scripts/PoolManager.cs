using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class PoolManager : MonoBehaviour
{
    Pool<Box> boxPool;
    public GameObject boxPrefab;
    public Transform spawnPoint;

    private void Start()
    {
        boxPool = new Pool<Box>(new PrefabFactory<Box>(boxPrefab), 15);
        boxPool.members.ForEach(b => b.gameObject.SetActive(false));

        StartCoroutine(SpawnBox());
    }

    public void BoxSpawn()
    {
        Box box = boxPool.Allocate();

        EventHandler handler = null;
        handler = (s, e) =>
        {
            //GameManager.instance.boxCount--;
            boxPool.Release(box);
            box.Death -= handler;
        };
        box.Death += handler;
        //GameManager.instance.boxCount++;

        //생성한 후 포지션 변경이 필요할경우 여기서 해줘야함.
        box.gameObject.transform.position = spawnPoint.position;
        box.gameObject.SetActive(true);
    }

    IEnumerator SpawnBox()
    {
        while (!GameManager.instance.isGameover) //게임오버가 아닐때까지
        {
            BoxSpawn();
            yield return new WaitForSeconds(4f); //나중에 조절해준다.
        }
    }
}
