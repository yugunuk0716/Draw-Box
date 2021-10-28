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

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        homeBtn.onClick.AddListener(() => OnClickHomeBtn());
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        nextStageBtn.onClick.AddListener(() => OnClickNextStageBtn());
        if (GameManager.instance.isStage) 
        {
            stageIdxText.text = $"Stage {GameManager.instance.stageIndex}";
        }
        
    }

    public override void Open(object data = null, int closeCount = 1)
    {
        base.Open();
        Time.timeScale = 0;
    }

    public override void Close()
    {
        base.Close();
    }

    public override void OnClickHomeBtn()
    {
        base.OnClickHomeBtn();
        Time.timeScale = 1;
    }
    public override void OnClickRetryBtn()
    {
        base.OnClickRetryBtn();
        Time.timeScale = 1;
    }
    public void OnClickNextStageBtn()
    {
        GameManager.instance.stageIndex++;
        LoadManager.LoadScene("InGamePackage");
        Time.timeScale = 1;

    }


}
