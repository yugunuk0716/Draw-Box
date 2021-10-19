using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverManager : MonoBehaviour
{
    public static FeverManager instance; //�̱���

    private float feverTime = 5f;
    private WaitForSeconds feverWs;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� �ǹ��Ŵ����� ������");
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
        GameManager.instance.boxCount++; //�ڽ� ī��Ʈ ����
        GameManager.instance.score++;
        
        GameManager.instance.isFever = true; //�ǹ� ����

        yield return feverWs;

        GameManager.instance.isFever = false; //�ǹ� ����
    }
}
