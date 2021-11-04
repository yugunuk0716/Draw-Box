using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClearPanel : PanelScript
{
    public Button homeBtn;
    public Button retryBtn;
    public Button nextStageBtn;
    public Text stageIdxText;

    CanvasGroup cv;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        cv = GetComponent<CanvasGroup>();
        homeBtn.onClick.AddListener(() => OnClickHomeBtn("Stage"));
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        nextStageBtn.onClick.AddListener(() => OnClickNextStageBtn());
        if (GameManager.instance.isStage) 
        {
            stageIdxText.text = $"Stage {GameManager.instance.stageIndex}";
        }
        
    }

    public override void Open(object data = null, int closeCount = 1)
    {
        base.Open(data, closeCount);
        Time.timeScale = 0f;

    }

    public void OnClickNextStageBtn()
    {
        GameManager.instance.stageIndex++;
        LoadManager.LoadScene("InGamePackage");
        Time.timeScale = 1;

    }

    public override void SetAlpha(bool on)
    {
        cv.alpha = on ? 1f : 0f;
        cv.interactable = on;
        cv.blocksRaycasts = on;
    }
}
