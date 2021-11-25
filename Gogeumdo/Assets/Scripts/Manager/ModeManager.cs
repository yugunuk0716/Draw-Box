using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModeManager : MonoBehaviour
{
    public static ModeManager instance;

    [Header("타이머 관련")]
    private bool isTimer = false;
    private float limitTime = 300f;
    private int min = 5;
    private float sec = 1f;

    private bool check = false;

    private int limitObstacle = 2; //기본 2개 + 남은 장애물 2개

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("다수의 ModeManager가 실행중");
            return;
        }
        instance = this;
        Init();
        EventManager.AddEvent("StageOrRank",() => StageOrRank(GameManager.instance.isStage, GameManager.instance.stageIndex));
    }
    public void Init()
    {
        isTimer = false;
        limitTime = 300f;
        limitObstacle = 2;
        check = false;
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        if(isTimer)
        {
            Timer();
        }
    }

    public void StageOrRank(bool isStage, int stageIdx = 0) //스테이지와 랭크모드인지 확인하고 그에 따른 작업을 해주는 곳
    {
        if(isStage)
        {
            if(stageIdx != 0) //혹시모르니 예외처리
            {
                GameManager.instance.SetRemainBox();
                if(stageIdx == 1)
                {
                    EventManager.Invoke("StageInitSpawn");
                    return;
                }
                //SoundManager.instance.PlayBgmSound(SoundManager.instance.packagerBgm, 0.1f);
                SoundManager.instance.ChangeBgmSound(0.07f);
            }
        }
        else
        {
            //랭크모드
            isTimer = true;
        }
        EventManager.Invoke("InitSpawn");
        //SoundManager.instance.PlayBgmSound(SoundManager.instance.inGameBgm,0.1f);
        SoundManager.instance.ChangeBgmSound(0.07f); //브금 소리바꾸기
    }
    public int GetMin() //남은 분 가져오기
    {
        return (int)(limitTime / 60);
    }

    public void Timer() //랭크 모드를위한 타이머
    {
        limitTime -= Time.deltaTime;
        min = (int)(limitTime / 60);
        sec = limitTime % 60;

        if (limitTime <= 0f && !check)
        {
            GameManager.instance.RankClear();
            check = true;
        }
        //분에 따라서 해줘야 할것들
        {
            switch (min)
            {
                case 0:
                    PoolManager.instance.SetBoxSpeed(0.05f);
                    break;
                case 1:
                    PoolManager.instance.SetBoxSpeed(0.045f);
                    if(limitObstacle == 1)
                    {
                        PoolManager.instance.ObstacleSpawn();
                        limitObstacle--;
                    }
                    break;
                case 2:
                    PoolManager.instance.SetBoxSpeed(0.04f);
                    break;
                case 3:
                    PoolManager.instance.SetBoxSpeed(0.035f);
                    if (limitObstacle == 2)
                    {
                        PoolManager.instance.ObstacleSpawn();
                        limitObstacle--;
                    }
                    break;
            }

        }

        UIManager.instance.ChangeTimerText($"{min}:{(int)sec}");
    }
    public void SetTime(bool addTime) //타임 아이템
    {
        limitTime += addTime ? 5f : -5f;
        min = (int)(limitTime / 60);
        sec = limitTime % 60;
        UIManager.instance.ChangeTimerText($"{min}:{(int)sec}");
    }
}
