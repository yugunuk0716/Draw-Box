using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Transform panelParent;
    public StageMenuPanel stageMenuPanel;
    public RankMenuPanel rankMenuPanel;
    public StageClearPanel stageClearPanel;
    public RankClearPanel rankClearPanel;
    public AlertPopup alertPopup;

    public Button menuBtn;

    public Text scoreAndBoxText;
    public Text timerText;

    private CanvasGroup cv;

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

        panelDic.Add("stageMenu", Instantiate(stageMenuPanel, panelParent));
        panelDic.Add("rankMenu", Instantiate(rankMenuPanel, panelParent));
        panelDic.Add("stageClear", Instantiate(stageClearPanel, panelParent));
        panelDic.Add("rankClear", Instantiate(rankClearPanel, panelParent));
        panelDic.Add("alert", Instantiate(alertPopup, panelParent)); //alert는 항상 맨 밑에 있어야 함

        menuBtn.onClick.AddListener(() =>
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
        });

        if (!GameManager.instance.isStage)
        {
            ChangeScoreAndBoxText($"0점");
            timerText.gameObject.SetActive(true);
        }
        else
        {
            ChangeScoreAndBoxText($"남은 박스 : {GameManager.instance.remainBox}");
        }
    }

    public void ChangeScoreAndBoxText(string str)
    {
        scoreAndBoxText.text = str;
    }

    public void ChangeTimerText(string str)
    {
        timerText.text = str;
    }

    public void OpenPanel(string name, object data = null, int closeCount = 1)
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

    public void ClosePanel()
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

    // Update is called once per frame
    void Update()
    {

    }


}
