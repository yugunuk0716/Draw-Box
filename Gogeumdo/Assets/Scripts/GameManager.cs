using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤

    //public int boxCount = 0;
    public bool isGameover = false; // 게임오버 체크

    private void Awake()
    {
        if(instance != null)// 싱글톤 중복 체크
        {
            Debug.LogError("다수의 게임매니저가 실행중");
        }
        instance = this;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            LoadManager.LoadScene("InGame");
        }
    }
}
