using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance; // 싱글톤

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


    public Dictionary<int, int> stageBox = new Dictionary<int, int>(); //스테이지별 박스갯수
    public Dictionary<int, List<int>> stageStar = new Dictionary<int, List<int>>(); // 스테이지 별 개수
    public Dictionary<Line, Sprite> lineSpriteDic; //라인별 박스 스프라이트 딕셔너리


    public Queue<int> boxIdxQueue = new Queue<int>();
    public List<Sprite> boxSprite = new List<Sprite>();

    public bool isGameover = false; // 게임오버 체크
    public bool isStage = false; //스테이지 모드인지 무한모드인지 체크하기 위한 변수
    public bool isFever = false; //피버인지 체크하기 위한 변수

    public int stageIndex = 0; //스테이지 인덱스 - 이에 따른 스테이지들로 실행해야함
    public int boxCount = 0; //일단 박스가 들어간만큼 올라가는 변수
    public int remainBox = 0; // 박스 갯수 (생성용)
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
            stageStar.Add(i, new List<int>() {(int)(stageBox[i] * 0.5f), (int)(stageBox[i] * 0.75f), stageBox[i]}); //3별:100% 넣었을경우 2별:75% 1별:50%
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
        UIManager.instance.ChangeScoreAndBoxText($"남은 박스 : {remainBox}");
    }

    public void Init()
    {
        boxCount = 0;
        boxIdxQueue.Clear();
        isFever = false;
        isGameover = false;
        print("초기화");
    }
    public void AddScore(int score) //박스갯수를 더해주며 
    {
        this.boxCount += score;
        this.tempBox -= score;
        if(!isStage)
        {
            UIManager.instance.ChangeScoreAndBoxText($"{(boxCount * 100)}점");
        }
    }
    public void RemainBox(int score)
    {
        if (isStage)
        {
            this.remainBox -= score;
            UIManager.instance.ChangeScoreAndBoxText($"남은 박스 : {remainBox}");
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
