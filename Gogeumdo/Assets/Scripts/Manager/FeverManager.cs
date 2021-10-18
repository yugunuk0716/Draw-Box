using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverManager : MonoBehaviour
{
    public static FeverManager instance; //싱글톤

    private float feverTime = 5f;
    private WaitForSeconds feverWs;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 피버매니저가 실행중");
            return;
        }
        instance = this;

        feverWs = new WaitForSeconds(feverTime);
    }

    public IEnumerator Fever()
    {
        GameManager.instance.boxCount++;
        if(GameManager.instance.boxCount > 5)
        {
            PoolManager.instance.FeverBoxSpawn();
            GameManager.instance.boxCount--;
        }
        GameManager.instance.isFever = true;

        yield return feverWs;

        GameManager.instance.isFever = false;
    }
}
