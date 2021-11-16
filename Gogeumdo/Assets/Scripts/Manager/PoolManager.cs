using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    Pool<Box> boxPool; //�ڽ��� Ǯ
    Pool<Obstacle> obstaclePool; //��ֹ� Ǯ

    public GameObject boxPrefab; //�ڽ��� ������
    public GameObject obstaclePrefab; //��ֹ��� ������
    public Transform spawnPoint; //�ڽ��� ��ȯ����

    public int obstacleCount = 0;

    [Header("�ڽ����� �迭��")]
    public int[] boxCount;
    public Sprite[] feverBoxSprite; 
    public Sprite[] timeBoxSprite;

    private WaitForSeconds Before10 = new WaitForSeconds(4f);
    private WaitForSeconds Before20 = new WaitForSeconds(3f);
    private WaitForSeconds After20 = new WaitForSeconds(2f);

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("�ټ��� Ǯ�Ŵ����� ������");
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
        boxPool = new Pool<Box>(new PrefabFactory<Box>(boxPrefab), 25);
        boxPool.members.ForEach(b => b.gameObject.SetActive(false)); //���� ���α�

        obstaclePool = new Pool<Obstacle>(new PrefabFactory<Obstacle>(obstaclePrefab), 25);
        obstaclePool.members.ForEach(x => x.gameObject.SetActive(false));

        EventManager.AddEvent("InitSpawn", () => StartCoroutine(InitSpawn()));
        EventManager.AddEvent("StageInitSpawn", () => StartCoroutine(StageInitSpawn()));
        EventManager.AddEvent("RankInitSpawn", () => StartCoroutine(RankInitSpawn()));
        EventManager.AddEvent("PackagerEnd", () =>
        {
            StartCoroutine(PackagerEnd());
        });
    }

    public void RemoveBox()
    {
        boxPool.members.FindAll(x => x.gameObject.activeSelf).ForEach(x => {
            boxPool.members.Remove(x);
            Destroy(x.gameObject);
        });
    }

    public void EventBoxSpawn() //�ڽ��� ��� ������� �����Ǵ� ���������� üũ
    {
        if (GameManager.instance.isStage) return;
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
    public void InitCount()
    {
        for (int i = 0; i < boxCount.Length; i++)
        {
            boxCount[i] = 0;
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
    IEnumerator Wait(Box ob)
    {
        RaycastHit2D hit; Vector2 dest;
        do
        {
            yield return new WaitForSeconds(0.1f);
            ob.lineIdx = UnityEngine.Random.Range(0, 5);
            dest = new Vector2(BoxManager.instance.lineTrm[ob.lineIdx].position.x, spawnPoint.position.y);
            hit = Physics2D.BoxCast(dest, new Vector2(transform.lossyScale.x, transform.lossyScale.y * 4f), 0f, Vector2.zero);
            
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
        Box box = boxPool.Allocate(); //�ڽ��� Ǯ�� �ڽ����ִٸ� �������� ���ٸ� ���� �����
        GameManager.instance.RemainBox(1);
        EventHandler handler = null;
        handler = (s, e) =>
        {
            GameManager.instance.AddScore(1);
            GameManager.instance.StageClear();
            AddBoxCount(true);
            EventBoxSpawn();

            boxPool.Release(box); //�ڽ��� �ʱ�ȭ
            box.Death -= handler; //������ ���ֱ�
        };
        box.Death += handler; //������ �ڽ��� Death�� �߰�����

        box.InitBox();
        StartCoroutine(Wait(box));
    }
    public void FeverBoxSpawn()
    {
        Box box = boxPool.Allocate();

        EventHandler handler = null;

        handler = (s, e) =>
        {
            //�ǹ� ����

            StartCoroutine(BoxManager.instance.Fever());
            boxPool.Release(box);
            box.Death -= handler;
        };
        box.Death += handler; //������ �ڽ��� Death�� �߰�����

        box.spriteRenderer.sprite = feverBoxSprite[(int)box.line];
        if(!TutorialManager.instance.isTuto)
        {
            StartCoroutine(Wait(box));
        }
        else
        {
            box.gameObject.SetActive(true);
            box.gameObject.transform.position = new Vector2(BoxManager.instance.lineTrm[0].position.x, spawnPoint.position.y);
        }
    }
    public void TimeIncreaseBoxSpawn()
    {
        Box box = boxPool.Allocate(); //�ڽ��� Ǯ�� �ڽ����ִٸ� �������� ���ٸ� ���� �����

        EventHandler handler = null;
        handler = (s, e) =>
        {
            ModeManager.instance.SetTime(true);
            boxPool.Release(box); //�ڽ��� �ʱ�ȭ

            box.Death -= handler; //������ ���ֱ�
        };
        box.Death += handler; //������ �ڽ��� Death�� �߰�����

        box.spriteRenderer.sprite = timeBoxSprite[(int)box.line];
        if (!TutorialManager.instance.isTuto)
        {
            StartCoroutine(Wait(box));
        }
        else
        {
            box.gameObject.SetActive(true);
            box.gameObject.transform.position = new Vector2(BoxManager.instance.lineTrm[1].position.x, spawnPoint.position.y);
        }
    }


    IEnumerator SpawnBox()
    {
        while (!GameManager.instance.isGameover) //���ӿ����� �ƴҶ����� 
        {
            if (GameManager.instance.isStage && GameManager.instance.remainBox > 0)
            {
                BoxSpawn();
            }
            else if (!GameManager.instance.isStage)
            {
                BoxSpawn();
            }

            if (GameManager.instance.stageIndex < 10)
            {
                yield return Before10; //���߿� �������ش�.
            }
            else if (GameManager.instance.stageIndex < 20)
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

    IEnumerator StageInitSpawn()
    {
        BoxSpawn();

        yield return After20;

        ObstacleSpawn();
        TutorialManager.instance.isObstacle = true;
    }
    IEnumerator RankInitSpawn()
    {
        //yield return After20;

        FeverBoxSpawn();
        TutorialManager.instance.isFever = true;

        yield return new WaitUntil(() => !TutorialManager.instance.isFever);

        TimeIncreaseBoxSpawn();
        TutorialManager.instance.isTime = true;

    }
    IEnumerator InitSpawn()
    {
        StartCoroutine(SpawnBox()); //�ڷ�ƾ ����

        yield return After20;

        StartCoroutine(InitObstacle());
    }
    IEnumerator PackagerEnd()
    {
        StartCoroutine(SpawnBox());
        yield return After20;
        ObstacleSpawn();
    }
}
