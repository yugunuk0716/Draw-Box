using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ModeManager : MonoBehaviour
{
    public static ModeManager instance;


    private int min = 5;
    private float sec = 1f;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("�ټ��� ModeManager�� ������");
            return;
        }
        instance = this;
        StageOrRank(GameManager.instance.isStage, GameManager.instance.stageIndex);
    }


    public void StageOrRank(bool isStage, int stageIdx = 0) //���������� ��ũ������� Ȯ���ϰ� �׿� ���� �۾��� ���ִ� ��
    {
        if(isStage)
        {
            if(stageIdx != 0) //Ȥ�ø𸣴� ����ó��
            {
                GameManager.instance.SetRemainBox();
            }
        }
        else
        {
            //��ũ���
            Timer();
        }
    }
    

    public void Timer() //��ũ ��带���� Ÿ�̸�
    {
        sec = Mathf.Clamp(sec -= Time.deltaTime, 0, 59);

        TimeCompare();
    }
    public void SetTime(bool addTime) //Ÿ�� ������
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
            if(min % 2 == 0)
            {
                PoolManager.instance.ObstacleSpawn();
            }
            switch (min)
            {
                case 0:
                    PoolManager.instance.SetBoxSpeed(0.05f);
                    break;
                case 1:
                    PoolManager.instance.SetBoxSpeed(0.04f);
                    break;
                case 2:
                    PoolManager.instance.SetBoxSpeed(0.03f);
                    break;
                case 3:
                    PoolManager.instance.SetBoxSpeed(0.02f);
                    break;
                default:
                    break;
            }
        }
        else if (sec >= 59f)
        {
            sec = 0f;
            min++;
        }

        if (min <= 0)
        {
            GameManager.instance.isGameover = true;
        }

        //���⼭ �ؽ�Ʈ�� �ٲ�����ҵ�?
    }
}
