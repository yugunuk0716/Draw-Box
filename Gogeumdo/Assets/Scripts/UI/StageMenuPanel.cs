using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMenuPanel : PanelScript
{
    public Button continueBtn;
    public Button retryBtn;
    public Button homeBtn;
    public Button creditBtn;
    public Button backBtn;

    public CanvasGroup creditPanel;
    
    public Text stageIdxText;

    CanvasGroup cv;
    protected override void Awake()
    {
        base.Awake();
    }


    private void Start()
    {
        cv = GetComponent<CanvasGroup>();
        continueBtn.onClick.AddListener(() => 
        {
            Close();
            Time.timeScale = 1;
        });
        homeBtn.onClick.AddListener(() => OnClickHomeBtn("Stage"));
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        creditBtn.onClick.AddListener(() => CreditPanel(true));
        backBtn.onClick.AddListener(() => CreditPanel(false));
       
        stageIdxText.text = $"{GameManager.instance.stageIndex} 스테이지";
        

    }

    public override void Open(object data = null, int closeCount = 1) // UI 활성화 함수
    {
        base.Open();
        Time.timeScale = 0f;

    }

    public void CreditPanel(bool on) // 크레딧 패널 활성화 및 비활성화 함수
    {
        creditPanel.alpha = on ? 1 : 0;
        creditPanel.interactable = on;
        creditPanel.blocksRaycasts = on;
    }

    public override void SetAlpha(bool on)// 스테이지 매뉴 패널 활성화 및 비활성화 함수
    {
        cv.alpha = on ? 1f : 0f;
        cv.interactable = on;
        cv.blocksRaycasts = on;
    }
}
