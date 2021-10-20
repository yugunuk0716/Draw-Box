using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    Pool<Box> boxPool; //박스의 풀
    //Pool<FeverBox> feverBoxPool; //피버 박스의 풀
    public GameObject boxPrefab; //박스의 프리팹
    //public GameObject feverBoxPrefab; //피버 박스의 프리팹
    public Transform spawnPoint; //박스의 소환지점

    public Sprite[] boxSprite; //박스의 스프라이트들

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 풀매니저가 실행중");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        boxPool = new Pool<Box>(new PrefabFactory<Box>(boxPrefab), 25); //15개만큼 미리 만들고
        boxPool.members.ForEach(b => b.gameObject.SetActive(false)); //전부 꺼두기

        StartCoroutine(SpawnBox()); //코루틴 시작
    }

    public void BoxSpawn()
    {
        Box box = boxPool.Allocate(); //박스의 풀에 박스가있다면 가져오고 없다면 새로 만들기

        EventHandler handler = null;
        handler = (s, e) =>
        {
            GameManager.instance.BoxIncrease(1, 1);
            boxPool.Release(box); //박스의 초기화
            box.Death -= handler; //했으면 빼주기
        };
        box.Death += handler; //생성된 박스의 Death에 추가해줌

        handler = (s, e) =>
        {
            GameManager.instance.BoxIncrease(0, -1);
            boxPool.Release(box);
            box.nAnswer -= handler;
        };
        box.nAnswer += handler;

        //생성한 후 포지션 변경이 필요할경우 여기서 해줘야함.
        box.gameObject.SetActive(true); //액티브를 켜줌
        box.gameObject.transform.position = new Vector2(MovementManager.instance.lineTrm[box.lineIdx].position.x, spawnPoint.position.y); //박스의 포지션을 스폰포인트로 해주고
    }
    public void FeverBoxSpawn()
    {
        Box box =  boxPool.Allocate();

        EventHandler handler = null;

        handler = (s, e) =>
        {
            //피버 실행
            
            GameManager.instance.BoxIncrease(0, 1);
            StartCoroutine(BoxManager.instance.Fever());
            boxPool.Release(box);
            box.Death -= handler;
        };
        box.Death += handler; //생성된 박스의 Death에 추가해줌

        handler = (s, e) =>
        {
            GameManager.instance.BoxIncrease(0, -1);
            boxPool.Release(box);
            box.nAnswer -= handler;
        };
        box.nAnswer += handler;

        //생성한 후 포지션 변경이 필요할경우 여기서 해줘야함.
        box.gameObject.SetActive(true); //액티브를 켜줌
        box.gameObject.transform.position = new Vector2(MovementManager.instance.lineTrm[box.lineIdx].position.x, spawnPoint.position.y); //박스의 포지션을 스폰포인트로 해주고
    }
    public void TimeIncreaseBoxSpawn()
    {
        Box box = boxPool.Allocate(); //박스의 풀에 박스가있다면 가져오고 없다면 새로 만들기

        EventHandler handler = null;
        handler = (s, e) =>
        {
            ModeManager.instance.SetTime(true);
            GameManager.instance.BoxIncrease(1, 1);
            boxPool.Release(box); //박스의 초기화

            box.Death -= handler; //했으면 빼주기
        };
        box.Death += handler; //생성된 박스의 Death에 추가해줌

        handler = (s, e) =>
        {
            ModeManager.instance.SetTime(false);
            GameManager.instance.BoxIncrease(0, -1);
            boxPool.Release(box);
            box.nAnswer -= handler;
        };
        box.nAnswer += handler;

        //생성한 후 포지션 변경이 필요할경우 여기서 해줘야함.
        box.gameObject.SetActive(true); //액티브를 켜줌
        box.gameObject.transform.position = new Vector2(MovementManager.instance.lineTrm[box.lineIdx].position.x, spawnPoint.position.y); //박스의 포지션을 스폰포인트로 해주고
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
