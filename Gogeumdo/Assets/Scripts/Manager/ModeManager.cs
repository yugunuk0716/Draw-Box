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
            Debug.LogError("�ټ��� ModeManager�� ������");
            return;
        }
        instance = this;
    }


    public void StageOrRank(bool isStage, int stageIdx = 0) //���������� ��ũ������� Ȯ���ϰ� �׿� ���� �۾��� ���ִ� ��
    {
        if(isStage)
        {
            //��������
            switch (stageIdx)
            {
                default:
                    break;
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
        }
        else if (sec >= 59f)
        {
            sec = 0f;
            min++;
        }

        if (min <= 0)
        {
            //���ӿ���
            GameManager.instance.isGameover = true;
        }

        //���⼭ �ؽ�Ʈ�� �ٲ�����ҵ�?
    }
}
