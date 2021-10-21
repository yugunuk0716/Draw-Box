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
    public Sprite[] boxSprite; //�ڽ��� ��������Ʈ�� //0 = �⺻ �ڽ� 1 = �ǹ� �ڽ� 2 = �ð� ���� �ڽ� �� �� ����

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� Ǯ�Ŵ����� ������");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        boxPool = new Pool<Box>(new PrefabFactory<Box>(boxPrefab), 25); //25����ŭ �̸� �����
        boxPool.members.ForEach(b => b.gameObject.SetActive(false)); //���� ���α�

        obstaclePool = new Pool<Obstacle>(new PrefabFactory<Obstacle>(obstaclePrefab), 25);
        obstaclePool.members.ForEach(x => x.gameObject.SetActive(false));

        StartCoroutine(InitSpawn());
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

        ob.gameObject.SetActive(true);
        ob.gameObject.transform.position = new Vector2(MovementManager.instance.lineTrm[ob.lineIdx].position.x, spawnPoint.position.y);
    }

    public void BoxSpawn()
    {
        Box box = boxPool.Allocate(); //�ڽ��� Ǯ�� �ڽ����ִٸ� �������� ���ٸ� ���� �����

        EventHandler handler = null;
        handler = (s, e) =>
        {
            GameManager.instance.BoxIncrease(1, 1);
            boxPool.Release(box); //�ڽ��� �ʱ�ȭ
            box.Death -= handler; //������ ���ֱ�
        };
        box.Death += handler; //������ �ڽ��� Death�� �߰�����


        //������ �� ������ ������ �ʿ��Ұ�� ���⼭ �������.
        box.gameObject.SetActive(true); //��Ƽ�긦 ����
        //box.GetComponent<SpriteRenderer>().sprite = boxSprite[0];
        box.gameObject.transform.position = new Vector2(MovementManager.instance.lineTrm[box.lineIdx].position.x, spawnPoint.position.y); //�ڽ��� �������� ��������Ʈ�� ���ְ�
    }
    public void FeverBoxSpawn()
    {
        Box box =  boxPool.Allocate();

        EventHandler handler = null;

        handler = (s, e) =>
        {
            //�ǹ� ����
            
            GameManager.instance.BoxIncrease(0, 1);
            StartCoroutine(BoxManager.instance.Fever());
            boxPool.Release(box);
            box.Death -= handler;
        };
        box.Death += handler; //������ �ڽ��� Death�� �߰�����


        //������ �� ������ ������ �ʿ��Ұ�� ���⼭ �������.
        box.gameObject.SetActive(true); //��Ƽ�긦 ����
        //box.GetComponent<SpriteRenderer>().sprite = boxSprite[1];
        box.gameObject.transform.position = new Vector2(MovementManager.instance.lineTrm[box.lineIdx].position.x, spawnPoint.position.y); //�ڽ��� �������� ��������Ʈ�� ���ְ�
    }
    public void TimeIncreaseBoxSpawn()
    {
        Box box = boxPool.Allocate(); //�ڽ��� Ǯ�� �ڽ����ִٸ� �������� ���ٸ� ���� �����

        EventHandler handler = null;
        handler = (s, e) =>
        {
            ModeManager.instance.SetTime(true);
            GameManager.instance.BoxIncrease(1, 1);
            boxPool.Release(box); //�ڽ��� �ʱ�ȭ

            box.Death -= handler; //������ ���ֱ�
        };
        box.Death += handler; //������ �ڽ��� Death�� �߰�����


        //������ �� ������ ������ �ʿ��Ұ�� ���⼭ �������.
        box.gameObject.SetActive(true); //��Ƽ�긦 ����
        //box.GetComponent<SpriteRenderer>().sprite = boxSprite[2];
        box.gameObject.transform.position = new Vector2(MovementManager.instance.lineTrm[box.lineIdx].position.x, spawnPoint.position.y); //�ڽ��� �������� ��������Ʈ�� ���ְ�
    }
    

    IEnumerator SpawnBox()
    {
        while (!GameManager.instance.isGameover) //���ӿ����� �ƴҶ����� 
        {
            BoxSpawn();
            yield return new WaitForSeconds(4f); //���߿� �������ش�.
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
        StartCoroutine(SpawnBox()); //�ڷ�ƾ ����

        yield return new WaitForSeconds(2f);

        StartCoroutine(InitObstacle());   
    }
}
