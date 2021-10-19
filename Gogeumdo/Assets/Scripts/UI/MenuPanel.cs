using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : PanelScript
{
    public Button continueBtn;
    public Button retryBtn;
    public Button homeBtn;
    

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        continueBtn.onClick.AddListener(() => Close());
        homeBtn.onClick.AddListener(() => OnClickHomeBtn());
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public override void SetAlpha(bool on)
    {
        base.SetAlpha(on);
    }

    public void OnClickHomeBtn() 
    {
        Close();
        LoadManager.LoadScene("Stage");
    }

    public void OnClickRetryBtn() 
    {
        GameManager.instance.Init();
        Close();
        LoadManager.LoadScene("InGame");
    }
}
