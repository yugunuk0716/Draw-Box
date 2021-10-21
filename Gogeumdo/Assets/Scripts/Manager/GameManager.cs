using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱���

    public Dictionary<int, int> stageBox = new Dictionary<int, int>(); //���������� �ڽ�����
    public Dictionary<Line, Color> lineColorDic; //���κ� �÷� ��ųʸ�

    public Queue<int> boxIdxQueue = new Queue<int>();

    public bool isGameover = false; // ���ӿ��� üũ
    public bool isStage = false; //�������� ������� ���Ѹ������ üũ�ϱ� ���� ����
    public bool isFever = false; //�ǹ����� üũ�ϱ� ���� ����
    public int stageIndex = 0; //�������� �ε��� - �̿� ���� ����������� �����ؾ���
    public int boxCount = 0; //�ϴ� �ڽ��� �������ŭ �ö󰡴� ���� (���Ѹ���)
    public int remainBox = 0; // �ڽ� ���� (����������)

    private void Awake()
    {
        if(instance != null)// �̱��� �ߺ� üũ
        {
            Debug.LogError("�ټ��� ���ӸŴ����� ������");
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        lineColorDic = new Dictionary<Line, Color>()
        {
            {Line.a,Color.red },
            {Line.b,Color.yellow },
            {Line.c,Color.green },
            {Line.d,Color.blue },
            {Line.e, new Color(161f/255f,0f,255f/255f) /*�����*/}
        };

        for (int i = 1; i < 4; i++)
        {
            
            stageBox.Add(i, 10 + (i*2));
            if (i % 10 == 0)
            {
                stageBox[i] += 3;
            }
        }
    }

    public void SetRemainBox()
    {
        remainBox = stageBox[stageIndex];
    }

    public void Init() 
    {
        boxCount = 0;
        isFever = false;
        isGameover = false;
    }
    public void AddScore(int score)
    {
        this.boxCount += score;
    }
}
