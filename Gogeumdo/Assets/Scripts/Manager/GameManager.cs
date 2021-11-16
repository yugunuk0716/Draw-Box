using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; // �̱���

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("GameManager");
                obj.AddComponent<GameManager>();
                obj.AddComponent<NetworkManager>();
                DontDestroyOnLoad(obj);
                _instance = obj.GetComponent<GameManager>();
            }

            return _instance;
        }
    }


    public Dictionary<int, int> stageBox = new Dictionary<int, int>(); //���������� �ڽ�����
    public Dictionary<int, List<int>> stageStar = new Dictionary<int, List<int>>(); // �������� �� ����
    public Dictionary<Line, Sprite> lineSpriteDic; //���κ� �ڽ� ��������Ʈ ��ųʸ�


    public Queue<int> boxIdxQueue = new Queue<int>();
    public List<Sprite> boxSprite = new List<Sprite>();

    public bool isGameover = false; // ���ӿ��� üũ
    public bool isStage = false; //�������� ������� ���Ѹ������ üũ�ϱ� ���� ����
    public bool isFever = false; //�ǹ����� üũ�ϱ� ���� ����

    public int stageIndex = 0; //�������� �ε��� - �̿� ���� ����������� �����ؾ���
    public int boxCount = 0; //�ϴ� �ڽ��� ����ŭ �ö󰡴� ����
    public int remainBox = 0; // �ڽ� ���� (������)
    public int tempBox = 0;

    public int count = 0;

    private GameObject boxSpriteObj;

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O)) 
        //{
        //    for (int i = 0; i < stageBox[1]; i++)
        //    {
        //        boxIdxQueue.Enqueue(Random.Range(0, 5));
        //    }
        //    stageIndex = 1;
        //    LoadManager.LoadScene("InGame");

        //}
    }

    private void Awake()
    {
        if(_instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        if (boxSprite.Count == 0) 
        {
            boxSpriteObj = Resources.Load<GameObject>("BoxSprites");
            SpriteRenderer[] sprites = boxSpriteObj.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < sprites.Length; i++)
            {
                boxSprite.Add(sprites[i].sprite);
            }
            
        }

        lineSpriteDic = new Dictionary<Line, Sprite>();
        for (int i = 0; i < 5; i++)
        {
            lineSpriteDic.Add((Line)i, boxSprite[i]);
        }

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
    private void Start()
    {
    }
    public bool CompareCount(int stageIdx)
    {
        print(boxCount + " " + stageStar[stageIndex][stageIdx]);
        return stageStar[stageIndex][stageIdx] <= boxCount;
    }
    public void SetRemainBox()
    {
        if(boxIdxQueue.Count < stageStar[stageIndex][0])
        {
            isGameover = true;
            return;
        }
        tempBox = remainBox = boxIdxQueue.Count;
        UIManager.instance.ChangeScoreAndBoxText($"���� �ڽ� : {remainBox}");
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
            UIManager.instance.OpenPanel("stageClear");
        }
    }
    public void RankClear()
    {
        if (isStage) return;
        int score = boxCount * 100;
        ScoreVO vo = new ScoreVO("", score);
        string json = JsonUtility.ToJson(vo);
        NetworkManager.instance.SendPostRequest("scorerecord", json, res =>
        {
            ResponseVO vo = JsonUtility.FromJson<ResponseVO>(res);
            if (vo.result)
            {
                print(vo.payload);
            }
            else
            {
                print(vo.payload);
            }
        });
        UIManager.instance.OpenPanel("rankClear");
    }
}
