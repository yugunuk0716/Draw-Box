using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;
using System;

public class PoolManager : MonoBehaviour
{
    Pool<Box> boxPool; //�ڽ��� Ǯ
    public GameObject boxPrefab; //�ڽ��� ������
    public Transform spawnPoint; //�ڽ��� ��ȯ����

    private void Start()
    {
        boxPool = new Pool<Box>(new PrefabFactory<Box>(boxPrefab), 15); //15����ŭ �̸� �����
        boxPool.members.ForEach(b => b.gameObject.SetActive(false)); //���� ���α�

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
        //GameManager.instance.boxCount++;

        //������ �� ������ ������ �ʿ��Ұ�� ���⼭ �������.
        box.gameObject.transform.position = spawnPoint.position; //�ڽ��� �������� ��������Ʈ�� ���ְ�
        box.gameObject.SetActive(true); //��Ƽ�긦 ����
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
