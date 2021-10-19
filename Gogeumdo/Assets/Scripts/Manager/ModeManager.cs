using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public static ModeManager instance;

    private int min = 5;
    private float sec = 0f;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 ModeManager가 실행중");
            return;
        }
        instance = this;
    }


    public void StageOrRank(bool isStage, int stageIdx = 0) //스테이지와 랭크모드인지 확인하고 그에 따른 작업을 해주는 곳
    {
        if(isStage)
        {
            //스테이지
            switch (stageIdx)
            {
                default:
                    break;
            }
        }
        else
        {
            //랭크모드
            Timer();
        }
    }
    public void Timer() //랭크 모드를위한 타이머
    {
        sec = Mathf.Clamp(sec -= Time.deltaTime, 0, 59);

        TimeCompare();
    }
    public void SetTime(bool addTime) //타임 아이템
    {
        sec += addTime ? 5f : -5f;

        TimeCompare();
    }

    public void TimeCompare()
    {
        if (sec <= 0f)
        {
            sec = 59f;
            min--;
        }
        else if (sec >= 59f)
        {
            sec = 0f;
            min++;
        }

        if (min <= 0)
        {
            //게임오버
            GameManager.instance.isGameover = true;
        }

        //여기서 텍스트를 바꿔줘야할듯?
    }
}
