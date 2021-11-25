using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModeManager : MonoBehaviour
{
    public static ModeManager instance;

    [Header("Ÿ�̸� ����")]
    private bool isTimer = false;
    private float limitTime = 300f;
    private int min = 5;
    private float sec = 1f;

    private bool check = false;

    private int limitObstacle = 2; //�⺻ 2�� + ���� ��ֹ� 2��

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� ModeManager�� ������");
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

    public void StageOrRank(bool isStage, int stageIdx = 0) //���������� ��ũ������� Ȯ���ϰ� �׿� ���� �۾��� ���ִ� ��
    {
        if(isStage)
        {
            if(stageIdx != 0) //Ȥ�ø𸣴� ����ó��
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
            //��ũ���
            isTimer = true;
        }
        EventManager.Invoke("InitSpawn");
        //SoundManager.instance.PlayBgmSound(SoundManager.instance.inGameBgm,0.1f);
        SoundManager.instance.ChangeBgmSound(0.07f); //��� �Ҹ��ٲٱ�
    }
    public int GetMin() //���� �� ��������
    {
        return (int)(limitTime / 60);
    }

    public void Timer() //��ũ ��带���� Ÿ�̸�
    {
        limitTime -= Time.deltaTime;
        min = (int)(limitTime / 60);
        sec = limitTime % 60;

        if (limitTime <= 0f && !check)
        {
            GameManager.instance.RankClear();
            check = true;
        }
        //�п� ���� ����� �Ұ͵�
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
    public void SetTime(bool addTime) //Ÿ�� ������
    {
        limitTime += addTime ? 5f : -5f;
        min = (int)(limitTime / 60);
        sec = limitTime % 60;
        UIManager.instance.ChangeTimerText($"{min}:{(int)sec}");
    }
}
