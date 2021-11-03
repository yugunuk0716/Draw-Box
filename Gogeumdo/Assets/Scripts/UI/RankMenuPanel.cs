using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankMenuPanel : PanelScript
{

    public Button continueBtn;
    public Button retryBtn;
    public Button homeBtn;
    public Button rankBtn;
    public Button backBtn;



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
        rankBtn.onClick.AddListener(() => OnClickRankBtn());
        backBtn.onClick.AddListener(() => RankListPanel(false));
        if (GameManager.instance.isStage)
        {
            stageIdxText.text = $"Stage {GameManager.instance.stageIndex}";
        }

    }

    


}
