using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModeManager : MonoBehaviour
{
    public static ModeManager instance;

    [Header("Ÿ�̸� ����")]
    private bool isTimer = false;
    private float limitTime = 20f;
    private int min = 5;
    private float sec = 1f;

    private bool check = false;

    private int limitObstacle = 2;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� ModeManager�� ������");
            return;
        }
        instance = this;

        EventManager.AddEvent("StageOrRank",() => StageOrRank(GameManager.instance.isStage, GameManager.instance.stageIndex));
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
                //print(GameManager.instance.boxIdxQueue.Count + "������");
                GameManager.instance.SetRemainBox();
            }
        }
        else
        {
            //��ũ���
            isTimer = true;
        }
        EventManager.Invoke("BoxSpawn");
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
                    PoolManager.instance.SetBoxSpeed(0.04f);
                    if(limitObstacle == 1)
                    {
                        PoolManager.instance.ObstacleSpawn();
                        limitObstacle--;
                    }
                    break;
                case 2:
                    PoolManager.instance.SetBoxSpeed(0.03f);
                    break;
                case 3:
                    PoolManager.instance.SetBoxSpeed(0.02f);
                    if (limitObstacle == 2)
                    {
                        PoolManager.instance.ObstacleSpawn();
                        limitObstacle--;
                    }
                    break;
                default:
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
