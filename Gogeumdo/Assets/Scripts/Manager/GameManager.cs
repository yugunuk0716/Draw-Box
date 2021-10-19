using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤

    //public int boxCount = 0;
    public bool isGameover = false; // 게임오버 체크
    public bool isStage = false; //스테이지 모드인지 무한모드인지 체크하기 위한 변수
    public bool isFever = false; //피버인지 체크하기 위한 변수
    public int stageIndex = 0; //스테이지 인덱스 - 이에 따른 스테이지들로 실행해야함
    public int boxCount = 0; //박스 카운트 - 이것에 따라 피버 상자가 나옴

    private void Awake()
    {
        if(instance != null)// 싱글톤 중복 체크
        {
            Debug.LogError("다수의 게임매니저가 실행중");
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            LoadManager.LoadScene("InGame");
        }
    }
}
