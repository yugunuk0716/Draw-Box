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


}
