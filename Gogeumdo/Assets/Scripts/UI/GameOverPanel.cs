using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : PanelScript
{
    public Button homeBtn;
    public Button retryBtn;
    public Text stageIdxText;

    protected override void Awake()
    {
        base.Awake();
      
    }

    private void Start()
    {
        homeBtn.onClick.AddListener(() => OnClickHomeBtn("Stage"));
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());

        stageIdxText.text = $"{GameManager.instance.stageIndex} 스테이지";
    }
}
