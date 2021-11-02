using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankClearPanel : PanelScript
{
    public Button homeBtn;
    public Button retryBtn;
    public Button rankBtn;

    public Text curScoreText;
    public Text highScoreText;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        homeBtn.onClick.AddListener(() => OnClickHomeBtn());
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        rankBtn.onClick.AddListener(() => OnClickRankBtn());    
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

    public void OnClickRankBtn() 
    {
        //여기서 하시면 댐
    }
}
