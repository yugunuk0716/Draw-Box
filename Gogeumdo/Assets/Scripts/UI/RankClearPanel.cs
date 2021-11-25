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

    public CanvasGroup rankListPanel;
    public Transform content;
    public ScoreRank rankPrefab;

    CanvasGroup cv;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        cv = GetComponent<CanvasGroup>();
        homeBtn.onClick.AddListener(() => OnClickHomeBtn("Main"));
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        rankBtn.onClick.AddListener(() => OnClickRankBtn());
        backBtn.onClick.AddListener(() => RankListPanel(false));
    }

    public override void Open(object data = null, int closeCount = 1) // UI 활성화 함수
    {
        base.Open(data, closeCount);
        curScoreText.text = $"점수 : {GameManager.instance.boxCount * 100}";
        Time.timeScale = 0f;

    }

    public void OnClickRankBtn() // 랭킹 보드 활성화 함수
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
                UIManager.instance.OpenPanel("alert",res.payload);
            }
        });

    }


    public void RankListPanel(bool on) // 랭크리스트 패널 활성화 및 비활성화 함수
    {
        rankListPanel.alpha = on ? 1 : 0;
        rankListPanel.blocksRaycasts = on;
        rankListPanel.interactable = on;
    }

    public override void SetAlpha(bool on) // 랭크 클리어 패널 활성화 및 비활성화 함수
    {
        cv.alpha = on ? 1f : 0f;
        cv.interactable = on;
        cv.blocksRaycasts = on;
    }
}
