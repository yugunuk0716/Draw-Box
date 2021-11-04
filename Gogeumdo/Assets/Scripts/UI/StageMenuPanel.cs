using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMenuPanel : PanelScript
{
    public Button continueBtn;
    public Button retryBtn;
    public Button homeBtn;
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
        if (GameManager.instance.isStage)
        {
            stageIdxText.text = $"Stage {GameManager.instance.stageIndex}";
        }

    }

    public override void Open(object data = null, int closeCount = 1)
    {
        base.Open();
        Time.timeScale = 0f;

    }

    public override void Close()
    {
        base.Close();
    }

    public override void OnClickRetryBtn() 
    {
        base.OnClickRetryBtn();
    }

    public override void SetAlpha(bool on)
    {
        cv.alpha = on ? 1f : 0f;
        cv.interactable = on;
        cv.blocksRaycasts = on;
    }
}
