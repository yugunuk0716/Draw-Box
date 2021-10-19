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
        continueBtn.onClick.AddListener(() => 
        {
            Close();
            Time.timeScale = 1;
        });
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
