using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankClearPanel : PanelScript
{
    public Button homeBtn;
    public Button retryBtn;
    public Button rankBtn;
    public Button backBtn;

    public Text curScoreText;
    public Text highScoreText;

    public CanvasGroup rankListPanel;
    public Transform content;
    public ScoreRank rankPrefab;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        homeBtn.onClick.AddListener(() => OnClickHomeBtn());
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        rankBtn.onClick.AddListener(() => OnClickRankBtn());
        backBtn.onClick.AddListener(() => RankListPanel(false));
    }
    public void RankListPanel(bool on)
    {
        rankListPanel.alpha = on ? 1 : 0;
        rankListPanel.blocksRaycasts = on;
        rankListPanel.interactable = on;
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
        NetworkManager.instance.SendGetRequest("ranklist", "",json =>
        {
            Transform[] childs = content.GetComponentsInChildren<Transform>();
            ResponseVO res = JsonUtility.FromJson<ResponseVO>(json);

            if(res.result)
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
}
