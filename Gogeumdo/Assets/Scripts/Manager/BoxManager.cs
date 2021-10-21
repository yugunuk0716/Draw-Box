using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance; //�̱���

    private float feverTime = 5f;
    private WaitForSeconds feverWs;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� �ڽ��Ŵ����� ������");
            return;
        }
        instance = this;

        feverWs = new WaitForSeconds(feverTime);
    }

    private void Update()
    {
        if (GameManager.instance.boxCount > 5)
        {
            PoolManager.instance.FeverBoxSpawn();
            GameManager.instance.boxCount -= 5;
        }
    }

    public IEnumerator Fever()
    {
        if (GameManager.instance.isFever) yield break;
        GameManager.instance.isFever = true; //�ǹ� ����

        yield return feverWs;

        GameManager.instance.isFever = false; //�ǹ� ����
    }
}
