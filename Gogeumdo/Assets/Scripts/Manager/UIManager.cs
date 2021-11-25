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
            Debug.LogError("�ټ��� UIManager�� �������Դϴ�");
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

        //��ųʸ��� UI �ֱ�
        panelDic.Add("stageMenu", Instantiate(stageMenuPanel, panelParent));
        panelDic.Add("rankMenu", Instantiate(rankMenuPanel, panelParent));
        panelDic.Add("stageClear", Instantiate(stageClearPanel, panelParent));
        panelDic.Add("rankClear", Instantiate(rankClearPanel, panelParent));
        panelDic.Add("gameOver", Instantiate(gameOverPanel, panelParent));


        panelDic.Add("alert", Instantiate(alertPopup, panelParent)); //alert�� �׻� �� �ؿ� �־�� ��

        
        //��ư�� �̺�Ʈ �߰�
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

        // �������� �⺻ UI
        if (!GameManager.instance.isStage)
        {
            ChangeScoreAndBoxText($"0��");
            timerText.gameObject.SetActive(true);
        }
        // ��ũ��� �⺻ UI
        else
        {
            ChangeScoreAndBoxText($"���� �ڽ� : ?");
        }
    }

    public void ChangeScoreAndBoxText(string str) // ���� �ؽ�Ʈ ���� �Լ�
    {
        if (SceneManager.GetActiveScene().name == packagerSceneName)
            return;
        scoreAndBoxText.text = str;
    }

    public void ChangeTimerText(string str) // Ÿ�̸� �ؽ�Ʈ ���� �Լ�
    {
        timerText.text = str;
    }
    public Sprite ChangeStarSprite(int idx) // ���� ��������Ʈ ��ü �Լ�
    {
        return GameManager.instance.CompareCount(idx) ? starSprite[1] : starSprite[0];
    }

    public void OpenPanel(string name, object data = null, int closeCount = 1) // UI Ȱ��ȭ �Լ�
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

    public void ClosePanel() // UI ��Ȱ��ȭ �Լ�
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
