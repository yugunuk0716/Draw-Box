using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    Pool<Box> boxPool; //�ڽ��� Ǯ
    //Pool<FeverBox> feverBoxPool; //�ǹ� �ڽ��� Ǯ
    public GameObject boxPrefab; //�ڽ��� ������
    //public GameObject feverBoxPrefab; //�ǹ� �ڽ��� ������
    public Transform spawnPoint; //�ڽ��� ��ȯ����

    public Sprite[] boxSprite; //�ڽ��� ��������Ʈ��

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
        boxPool = new Pool<Box>(new PrefabFactory<Box>(boxPrefab), 15); //15����ŭ �̸� �����
        //feverBoxPool = new Pool<FeverBox>(new PrefabFactory<FeverBox>(feverBoxPrefab), 5); //5����ŭ �̸� �����
        boxPool.members.ForEach(b => b.gameObject.SetActive(false)); //���� ���α�
        //feverBoxPool.members.ForEach(x => x.gameObject.SetActive(false));

        StartCoroutine(SpawnBox()); //�ڷ�ƾ ����
    }

    public void BoxSpawn()
    {
        Box box = boxPool.Allocate(); //�ڽ��� Ǯ�� �ڽ����ִٸ� �������� ���ٸ� ���� �����

        EventHandler handler = null;
        handler = (s, e) =>
        {
            //GameManager.instance.boxCount--;
            boxPool.Release(box); //�ڽ��� �ʱ�ȭ
            box.Death -= handler; //������ ���ֱ�
        };
        box.Death += handler; //������ �ڽ��� Death�� �߰�����

        handler = (s, e) =>
        {
            boxPool.Release(box);
            box.nAnswer -= handler;
        };
        box.nAnswer += handler;

        //������ �� ������ ������ �ʿ��Ұ�� ���⼭ �������.
        int idx = UnityEngine.Random.Range(0, 9);
        box.gameObject.transform.position = new Vector2(MovementManager.instance.lineTrm[idx].position.x, spawnPoint.position.y); //�ڽ��� �������� ��������Ʈ�� ���ְ�
        box.gameObject.SetActive(true); //��Ƽ�긦 ����
    }
    public void FeverBoxSpawn()
    {
        Box box =  boxPool.Allocate();

        EventHandler handler = null;

        handler = (s, e) =>
        {
            //�ǹ� ����
            StartCoroutine(FeverManager.instance.Fever());
            boxPool.Release(box);
            box.Death -= handler;
        };
        box.Death += handler; //������ �ڽ��� Death�� �߰�����

        handler = (s, e) =>
        {
            GameManager.instance.score--;
            boxPool.Release(box);
            box.nAnswer -= handler;
        };
        box.nAnswer += handler;

        //������ �� ������ ������ �ʿ��Ұ�� ���⼭ �������.
        box.gameObject.transform.position = spawnPoint.position; //�ڽ��� �������� ��������Ʈ�� ���ְ�
        box.gameObject.SetActive(true); //��Ƽ�긦 ����
    }
    public void TimeIncreaseBoxSpawn()
    {

    }
    

    IEnumerator SpawnBox()
    {
        while (!GameManager.instance.isGameover) //���ӿ����� �ƴҶ����� 
        {
            BoxSpawn();
            yield return new WaitForSeconds(4f); //���߿� �������ش�.
        }
    }
}
