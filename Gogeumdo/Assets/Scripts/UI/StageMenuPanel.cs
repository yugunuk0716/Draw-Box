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


    protected override void Awake()
    {
        base.Awake();
    }


    private void Start()
    {
        continueBtn.onClick.AddListener(() => 
        {
            Close();
            Time.timeScale = 1;
        });
        homeBtn.onClick.AddListener(() => OnClickHomeBtn());
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
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
}
