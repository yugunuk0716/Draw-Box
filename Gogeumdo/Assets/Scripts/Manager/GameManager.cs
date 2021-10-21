using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱���

    public Dictionary<Line, Color> lineColorDic; //���κ� �÷� ��ųʸ�

    public bool isGameover = false; // ���ӿ��� üũ
    public bool isStage = false; //�������� ������� ���Ѹ������ üũ�ϱ� ���� ����
    public bool isFever = false; //�ǹ����� üũ�ϱ� ���� ����
    public int stageIndex = 0; //�������� �ε��� - �̿� ���� ����������� �����ؾ���
    public int boxCount = 0; //�ڽ� ī��Ʈ - �̰Ϳ� ���� �ǹ� ���ڰ� ����
    public int score = 0;

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
    }
    private void Update()
    {
        
    }

    public void Init() 
    {
        boxCount = 0;
        score = 0;
        isFever = false;
        isGameover = false;
    }
    public void BoxIncrease(int boxCount, int score)
    {
        this.boxCount += boxCount;
        this.score += score;
    }
}
