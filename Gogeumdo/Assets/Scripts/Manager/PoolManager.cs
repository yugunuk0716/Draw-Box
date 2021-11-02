using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    Pool<Box> boxPool; //박스의 풀
    Pool<Obstacle> obstaclePool; //장애물 풀

    public GameObject boxPrefab; //박스의 프리팹
    public GameObject obstaclePrefab; //장애물의 프리팹
    public Transform spawnPoint; //박스의 소환지점

    public int obstacleCount = 0;

    [Header("박스관련 배열들")]
    public int[] boxCount;
    public Sprite[] boxSprite; //박스의 스프라이트들 
    //0 = 피버 박스 1 = 시간 증가 박스 로 할 예정

    private WaitForSeconds Before10 = new WaitForSeconds(4f);
    private WaitForSeconds Before20 = new WaitForSeconds(3f);
    private WaitForSeconds After20 = new WaitForSeconds(2f);

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 풀매니저가 실행중");
            return;
        }
        instance = this;

        boxCount = new int[2];
        Before10 = new WaitForSeconds(4f);
        Before20 = new WaitForSeconds(3f);
        After20 = new WaitForSeconds(2f);
    }

    private void Start()
    {
        boxPool = new Pool<Box>(new PrefabFactory<Box>(boxPrefab),25);
        //boxPool = new Pool<Box>(new PrefabFactory<Box>(boxPrefab), 25); //25개만큼 미리 만들고
        boxPool.members.ForEach(b => b.gameObject.SetActive(false)); //전부 꺼두기

        obstaclePool = new Pool<Obstacle>(new PrefabFactory<Obstacle>(obstaclePrefab), 25);
        obstaclePool.members.ForEach(x => x.gameObject.SetActive(false));

        StartCoroutine(InitSpawn());
    }

    public void EventBoxSpawn() //박스가 몇개씩 사라지고 생성되는 아이템인지 체크
    {

        if (boxCount[0] >= 5)
        {
            FeverBoxSpawn();
            boxCount[0] -= 5;
        }
        if (boxCount[1] > 30)
        {
            TimeIncreaseBoxSpawn();
            boxCount[1] -= 30;
        }
    }

    public void ObstacleSpawn()
    {
        Obstacle ob = obstaclePool.Allocate();

        EventHandler handler = null;

        handler = (s, e) =>
        {
            ObstacleSpawn();
            obstaclePool.Release(ob);
            ob.Death -= handler;
        };
        ob.Death += handler;
        StartCoroutine(Wait(ob));
    }
    IEnumerator Wait(Obstacle ob)
    {
        RaycastHit2D hit; Vector2 dest; int idx;
        do
        {
            Debug.Log("장애물 충돌");
            dest = new Vector2(BoxManager.instance.lineTrm[ob.lineIdx].position.x,spawnPoint.position.y);
            hit = Physics2D.BoxCast(dest, gameObject.transform.lossyScale * 0.2f, 0, new Vector2(0, 0));
            yield return new WaitForSeconds(0.3f);
        } while (hit.collider != null);
        yield return null;
        ob.gameObject.SetActive(true);
        ob.gameObject.transform.position = new Vector2(BoxManager.instance.lineTrm[ob.lineIdx].position.x, spawnPoint.position.y);
    }

    public void AddBoxCount(bool add)
    {
        for (int i = 0; i < boxCount.Length; i++)
        {
            boxCount[i] += add ? 1 : -1;
        }
    }
    public void SetBoxSpeed(float speed)
    {
        if (boxPool.members[0].moveTime == speed) return;

        for (int i = 0; i < boxPool.members.Count; i++)
        {
            boxPool.members[i].SetMoveSpeed(speed);
        }
    }
    public void BoxSpawn()
    {
        Box box = boxPool.Allocate(); //박스의 풀에 박스가있다면 가져오고 없다면 새로 만들기
        GameManager.instance.RemainBox(1);
        EventHandler handler = null;
        handler = (s, e) =>
        {
            GameManager.instance.AddScore(1);
            GameManager.instance.StageClear();
            AddBoxCount(true);
            boxPool.Release(box); //박스의 초기화
            box.Death -= handler; //했으면 빼주기
        };
        box.Death += handler; //생성된 박스의 Death에 추가해줌


        //생성한 후 포지션 변경이 필요할경우 여기서 해줘야함.
        box.gameObject.SetActive(true); //액티브를 켜줌
        //box.GetComponent<SpriteRenderer>().sprite = boxSprite[0];
        box.gameObject.transform.position = new Vector2(BoxManager.instance.lineTrm[box.lineIdx].position.x, spawnPoint.position.y); //박스의 포지션을 스폰포인트로 해주고
    }
    public void FeverBoxSpawn()
    {
        Box box =  boxPool.Allocate();

        EventHandler handler = null;

        handler = (s, e) =>
        {
            //피버 실행
            
            StartCoroutine(BoxManager.instance.Fever());
            boxPool.Release(box);
            box.Death -= handler;
        };
        box.Death += handler; //생성된 박스의 Death에 추가해줌


        //생성한 후 포지션 변경이 필요할경우 여기서 해줘야함.
        box.gameObject.SetActive(true); //액티브를 켜줌
        //box.spriteRenderer.sprite = boxSprite[0];
        box.gameObject.transform.position = new Vector2(BoxManager.instance.lineTrm[box.lineIdx].position.x, spawnPoint.position.y); //박스의 포지션을 스폰포인트로 해주고
    }
    public void TimeIncreaseBoxSpawn()
    {
        Box box = boxPool.Allocate(); //박스의 풀에 박스가있다면 가져오고 없다면 새로 만들기

        EventHandler handler = null;
        handler = (s, e) =>
        {
            ModeManager.instance.SetTime(true);
            boxPool.Release(box); //박스의 초기화

            box.Death -= handler; //했으면 빼주기
        };
        box.Death += handler; //생성된 박스의 Death에 추가해줌


        //생성한 후 포지션 변경이 필요할경우 여기서 해줘야함.
        box.gameObject.SetActive(true); //액티브를 켜줌
        //box.spriteRenderer.sprite = boxSprite[1];
        box.gameObject.transform.position = new Vector2(BoxManager.instance.lineTrm[box.lineIdx].position.x, spawnPoint.position.y); //박스의 포지션을 스폰포인트로 해주고
    }
    

    IEnumerator SpawnBox()
    {
        while (!GameManager.instance.isGameover) //게임오버가 아닐때까지 
        {
            if(GameManager.instance.isStage && GameManager.instance.remainBox > 0)
            {
                print("정상");
                BoxSpawn();
            }
            else if(!GameManager.instance.isStage)
            {
                print("랭크일떄");
                BoxSpawn();
            }

            if(GameManager.instance.stageIndex < 10)
            {
                print("이게 왜 안됨");
                yield return Before10; //나중에 조절해준다.
            }
            else if(GameManager.instance.stageIndex < 20)
            {
                yield return Before20;
            }
            else
            {
                yield return After20;
            }
            yield return null;
        }
    }
    IEnumerator InitObstacle()
    {
        for (int i = 0; i < 2; i++)
        {
            ObstacleSpawn();
            yield return new WaitForSeconds(4f);
        }
    }

    IEnumerator InitSpawn()
    {
        StartCoroutine(SpawnBox()); //코루틴 시작

        yield return new WaitForSeconds(2f);

        StartCoroutine(InitObstacle());   
    }
}
