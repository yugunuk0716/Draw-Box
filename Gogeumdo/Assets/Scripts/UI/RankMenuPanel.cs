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


    public CanvasGroup rankListPanel;
    public Transform content;
    public ScoreRank rankPrefab;


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
        homeBtn.onClick.AddListener(() => OnClickHomeBtn("Main"));
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        rankBtn.onClick.AddListener(() => OnClickRankBtn());
        backBtn.onClick.AddListener(() => RankListPanel(false));
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

    public void OnClickRankBtn()
    {
        //여기서 하시면 댐
        NetworkManager.instance.SendGetRequest("ranklist", "", json =>
        {
            Transform[] childs = content.GetComponentsInChildren<Transform>();
            ResponseVO res = JsonUtility.FromJson<ResponseVO>(json);

            if (res.result)
            {
                ScoreListVO vo = JsonUtility.FromJson<ScoreListVO>(res.payload);
                for (int i = 1; i < childs.Length; i++)
                {
                    Destroy(childs[i].gameObject);
                }

                for (int i = 0; i < vo.list.Count; i++)
                {
                    ScoreRank obj = Instantiate(rankPrefab, content);
                    obj.SetData(i + 1, vo.list[i]);
                }
                RankListPanel(true);
            }
            else
            {
                //오류 발생 시
            }
        });

    }


    public void RankListPanel(bool on)
    {
        rankListPanel.alpha = on ? 1 : 0;
        rankListPanel.blocksRaycasts = on;
        rankListPanel.interactable = on;
    }



}
