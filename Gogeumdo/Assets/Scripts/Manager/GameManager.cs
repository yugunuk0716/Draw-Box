using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱���

    public Dictionary<int, int> stageBox = new Dictionary<int, int>(); //���������� �ڽ�����
    public Dictionary<int, List<int>> stageStar = new Dictionary<int, List<int>>(); // �������� �� ����
    public Dictionary<Line, Color> lineColorDic; //���κ� �÷� ��ųʸ�

    public Queue<int> boxIdxQueue = new Queue<int>();

    public bool isGameover = false; // ���ӿ��� üũ
    public bool isStage = false; //�������� ������� ���Ѹ������ üũ�ϱ� ���� ����
    public bool isFever = false; //�ǹ����� üũ�ϱ� ���� ����

    public int stageIndex = 0; //�������� �ε��� - �̿� ���� ����������� �����ؾ���
    public int boxCount = 0; //�ϴ� �ڽ��� ����ŭ �ö󰡴� ����
    public int remainBox = 0; // �ڽ� ���� (������)
    public int tempBox = 0;

    public int count = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) 
        {
            for (int i = 0; i < stageBox[1]; i++)
            {
                boxIdxQueue.Enqueue(Random.Range(0, 5));
            }
            stageIndex = 1;
            LoadManager.LoadScene("InGame");

        }
    }

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
                stageBox[i] += 4;
            }
        }
        for (int i = 1; i < 4; i++)
        {
            stageStar.Add(i, new List<int>() {(int)(stageBox[i] * 0.5f), (int)(stageBox[i] * 0.75f), stageBox[i]}); //3��:100% �־������ 2��:75% 1��:50%
        }
    }
    public bool CompareCount(int stageIdx)
    {
        return stageStar[stageIndex][stageIdx] >= boxCount;
    }
    public void SetRemainBox()
    {
        if(boxIdxQueue.Count < stageStar[stageIndex][0])
        {
            isGameover = true;
            return;
        }
        remainBox = boxIdxQueue.Count;
        tempBox = remainBox;
    }

    public void Init()
    {
        boxCount = 0;
        boxIdxQueue.Clear();
        isFever = false;
        isGameover = false;
        print("�ʱ�ȭ");
    }
    public void AddScore(int score) //�ڽ������� �����ָ� 
    {
        this.boxCount += score;
        this.tempBox -= score;
        if(!isStage)
        {
            UIManager.instance.ChangeScoreAndBoxText($"{(boxCount * 100)}��");
        }
    }
    public void RemainBox(int score)
    {
        if (isStage)
        {
            this.remainBox -= score;
            UIManager.instance.ChangeScoreAndBoxText($"���� �ڽ� : {remainBox}");
            if (stageStar[stageIndex][1] >= boxCount)
            {
                PoolManager.instance.SetBoxSpeed(0.03f);
            }
            else if (stageStar[stageIndex][0] >= boxCount)
            {
                PoolManager.instance.SetBoxSpeed(0.02f);
            }
        }
    }

    public void StageClear()
    {
        if (!isStage) return;
        if(tempBox <= 0)
        {

        }
        //�������� ����� ���
    }
    public void RankClear()
    {
        if (isStage) return;

        //��ũ����� ���
    }
}
