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
    public void Timer()
    {
        sec = Mathf.Clamp(sec -= Time.deltaTime, 0, 59);

        if(sec <= 0)
        {
            sec = 59;
            min--;
        }

        if(min <= 0)
        {
            //���ӿ���
            GameManager.instance.isGameover = true;
        }
    }
}
