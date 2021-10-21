using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance; //싱글톤

    private float feverTime = 5f;
    private WaitForSeconds feverWs;
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 박스매니저가 실행중");
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
        GameManager.instance.isFever = true; //피버 시작

        yield return feverWs;

        GameManager.instance.isFever = false; //피버 종료
    }
}
