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
        boxPool = new Pool<Box>(new PrefabFactory<Box>(boxPrefab), 25); //15����ŭ �̸� �����
        boxPool.members.ForEach(b => b.gameObject.SetActive(false)); //���� ���α�

        StartCoroutine(SpawnBox()); //�ڷ�ƾ ����
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

        handler = (s, e) =>
        {
            GameManager.instance.BoxIncrease(0, -1);
            boxPool.Release(box);
            box.nAnswer -= handler;
        };
        box.nAnswer += handler;

        //������ �� ������ ������ �ʿ��Ұ�� ���⼭ �������.
        box.gameObject.SetActive(true); //��Ƽ�긦 ����
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

        handler = (s, e) =>
        {
            GameManager.instance.BoxIncrease(0, -1);
            boxPool.Release(box);
            box.nAnswer -= handler;
        };
        box.nAnswer += handler;

        //������ �� ������ ������ �ʿ��Ұ�� ���⼭ �������.
        box.gameObject.SetActive(true); //��Ƽ�긦 ����
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

        handler = (s, e) =>
        {
            ModeManager.instance.SetTime(false);
            GameManager.instance.BoxIncrease(0, -1);
            boxPool.Release(box);
            box.nAnswer -= handler;
        };
        box.nAnswer += handler;

        //������ �� ������ ������ �ʿ��Ұ�� ���⼭ �������.
        box.gameObject.SetActive(true); //��Ƽ�긦 ����
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
}
