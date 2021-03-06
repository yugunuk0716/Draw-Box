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
    public Button creditBtn;
    public Button rankbackBtn;
    public Button creditBackBtn;


    public CanvasGroup rankListPanel;
    public CanvasGroup creditPanel;
    public Transform content;
    public ScoreRank rankPrefab;


    public Text stageIdxText;

    CanvasGroup cv;

    protected override void Awake()
    {
        base.Awake();
    }


    private void Start()
    {
        cv = GetComponent<CanvasGroup>();
        continueBtn.onClick.AddListener(() =>
        {
            Close();
            Time.timeScale = 1;
        });
        homeBtn.onClick.AddListener(() => OnClickHomeBtn("Main"));
        retryBtn.onClick.AddListener(() => OnClickRetryBtn());
        rankBtn.onClick.AddListener(() => OnClickRankBtn());
        rankbackBtn.onClick.AddListener(() => RankListPanel(false));
        creditBtn.onClick.AddListener(() => CreditPanel(true));
        creditBackBtn.onClick.AddListener(() => CreditPanel(false));
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
                UIManager.instance.OpenPanel("alert", res.payload);
            }
        });

    }


    public void RankListPanel(bool on)  // 랭크 리스트 패널 활성화 및 비활성화 함수
    {
        rankListPanel.alpha = on ? 1 : 0;
        rankListPanel.blocksRaycasts = on;
        rankListPanel.interactable = on;
    }
    public void CreditPanel(bool on) // 크레딧 패널 활성화 및 비활성화 함수
    {
        creditPanel.alpha = on ? 1 : 0;
        creditPanel.interactable = on;
        creditPanel.blocksRaycasts = on;
    }

    public override void SetAlpha(bool on) // 랭크 메뉴 패널 활성화 및 비활성화 함수
    {
        cv.alpha = on ? 1f : 0f;
        cv.interactable = on;
        cv.blocksRaycasts = on;
    }

}
