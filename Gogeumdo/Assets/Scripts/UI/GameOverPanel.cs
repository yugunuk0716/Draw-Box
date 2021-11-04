using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : PanelScript
{
    public Button homeBtn;
    public Button retryBtn;
    public Text stageIdxText;

    CanvasGroup cv;

    protected override void Awake()
    {
        base.Awake();
      
    }

    private void Start()
    {

        cv = GetComponent<CanvasGroup>();

        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        homeBtn.onClick.AddListener(() => 
        {
            OnClickHomeBtn("Stage"); 
        });

        stageIdxText.text = $"{GameManager.instance.stageIndex} 스테이지";
    }

    public override void OnClickHomeBtn(string str)
    {
        
        base.OnClickHomeBtn(str);
    }
}
