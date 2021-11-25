using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Transform panelParent;
    public StageMenuPanel stageMenuPanel;
    public RankMenuPanel rankMenuPanel;
    public StageClearPanel stageClearPanel;
    public RankClearPanel rankClearPanel;
    public AlertPopup alertPopup;
    public GameOverPanel gameOverPanel;

    public Sprite[] starSprite;

    public Button menuBtn;

    public Text scoreAndBoxText;
    public Text timerText;

    private CanvasGroup cv;
    private readonly string packagerSceneName = "InGamePackager"; 
    Dictionary<string, PanelScript> panelDic = new Dictionary<string, PanelScript>();
    Stack<PanelScript> panelStack = new Stack<PanelScript>();


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("다수의 UIManager가 실행중입니다");
        }
        instance = this;
    }

    void Start()
    {
        cv = panelParent.GetComponent<CanvasGroup>();
        if (cv == null)
        {
            cv = panelParent.gameObject.AddComponent<CanvasGroup>();
        }
        cv.alpha = 0;
        cv.interactable = false;
        cv.blocksRaycasts = false;

        //딕셔너리에 UI 넣기
        panelDic.Add("stageMenu", Instantiate(stageMenuPanel, panelParent));
        panelDic.Add("rankMenu", Instantiate(rankMenuPanel, panelParent));
        panelDic.Add("stageClear", Instantiate(stageClearPanel, panelParent));
        panelDic.Add("rankClear", Instantiate(rankClearPanel, panelParent));
        panelDic.Add("gameOver", Instantiate(gameOverPanel, panelParent));


        panelDic.Add("alert", Instantiate(alertPopup, panelParent)); //alert는 항상 맨 밑에 있어야 함

        
        //버튼에 이벤트 추가
        menuBtn.onClick.AddListener(() =>
        {
            if(!TutorialManager.instance.isTuto)
            {
                if (GameManager.instance.isStage)
                {
                    OpenPanel("stageMenu");
                }
                else
                {
                    OpenPanel("rankMenu");
                }
                Time.timeScale = 0;
            }
        });

        // 스테이지 기본 UI
        if (!GameManager.instance.isStage)
        {
            ChangeScoreAndBoxText($"0점");
            timerText.gameObject.SetActive(true);
        }
        // 랭크모드 기본 UI
        else
        {
            ChangeScoreAndBoxText($"남은 박스 : ?");
        }
    }

    public void ChangeScoreAndBoxText(string str) // 상자 텍스트 갱신 함수
    {
        if (SceneManager.GetActiveScene().name == packagerSceneName)
            return;
        scoreAndBoxText.text = str;
    }

    public void ChangeTimerText(string str) // 타이머 텍스트 갱신 함수
    {
        timerText.text = str;
    }
    public Sprite ChangeStarSprite(int idx) // 상자 스프라이트 교체 함수
    {
        return GameManager.instance.CompareCount(idx) ? starSprite[1] : starSprite[0];
    }

    public void OpenPanel(string name, object data = null, int closeCount = 1) // UI 활성화 함수
    {
        if (panelStack.Count == 0)
        {
            cv.alpha = 1f;
            cv.interactable = true;
            cv.blocksRaycasts = true;
        }
        panelStack.Push(panelDic[name]);
        panelDic[name].Open(data, closeCount);
    }

    public void ClosePanel() // UI 비활성화 함수
    {
        //cv = panelStack.Peek().GetComponent<CanvasGroup>();
        panelStack.Pop().Close();
        if (panelStack.Count == 0)
        {
            cv.alpha = 0f;
            cv.interactable = false;
            cv.blocksRaycasts = false;
        }
    }

    


}
