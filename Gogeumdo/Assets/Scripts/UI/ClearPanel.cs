using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearPanel : PanelScript
{
    public Button homeBtn;
    public Button retryBtn;
    public Button nextStageBtn;
    public Text stageIdxText;

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
    }

    public override void Close()
    {
        base.Close();
    }

    public override void OnClickHomeBtn()
    {
        base.OnClickHomeBtn();
    }
    public override void OnClickRetryBtn()
    {
        base.OnClickRetryBtn();
    }
    public void OnClickNextStageBtn()
    {
        GameManager.instance.stageIndex++;
        base.OnClickRetryBtn();

    }


}
